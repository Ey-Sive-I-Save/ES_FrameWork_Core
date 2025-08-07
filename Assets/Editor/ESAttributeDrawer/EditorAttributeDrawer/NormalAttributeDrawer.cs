using ES;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace ES
{

#if UNITY_EDITOR
    #region ESMessage
    public class ESMessageAttributeDrawer : OdinAttributeDrawer<ESMessage>
    {
        private float xAT = 50;
        protected override void DrawPropertyLayout(GUIContent label)
        {
            EditorGUILayout.Space(0);
            this.CallNextDrawer(label);
            Rect OR = GUILayoutUtility.GetLastRect();
            Rect buttonRectForAT = OR;
            buttonRectForAT.width = 20f;
            buttonRectForAT.x = xAT;
            GUIHelper.PushColor(Color.white._WithAlpha(0.5f));
            if (GUI.Button(buttonRectForAT, "?"))
            {
                // 弹出菜单
                GenericMenu menu = new GenericMenu();
                var currentValue = this.Property.ValueEntry.WeakSmartValue;
                Type type = currentValue.GetType();

                // 添加普通选项
                menu.AddItem(new GUIContent("字符串:" + this.Attribute.message), !this.Attribute.message.IsNullOrWhitespace(), () => { });
                menu.AddItem(new GUIContent("整数:" + this.Attribute.message_int), this.Attribute.message_int == 0, () => { });
                menu.AddItem(new GUIContent("浮点数:" + this.Attribute.message_float), this.Attribute.message_float == 0, () => { });
                Vector2 pos = Event.current.mousePosition;
                if (type.IsEnum)
                {
                    Enum enumValue = (Enum)currentValue;
                    var all = Enum.GetValues(type);

                    foreach (var i in all)
                    {
                        FieldInfo field = type.GetField(i.ToString());
                        var att = field.GetCustomAttribute<ESMessage>();
                        if (att != null)
                        {
                            var attIns = field.GetCustomAttribute<InspectorNameAttribute>();
                            menu.AddItem(new GUIContent("枚举/" + (attIns != null ? ("(" + attIns.displayName + ")") : "") + i), i == enumValue, () =>
                            {
                                GenericMenu menu2 = new GenericMenu();
                                menu2.AddDisabledItem(new GUIContent($"=== 枚举<{i}>信息 ==="), false);
                                menu2.AddItem(new GUIContent("检查器显示:" + attIns.displayName), !att.message.IsNullOrWhitespace(), () => { });
                                menu2.AddItem(new GUIContent("字符串:" + att.message), !att.message.IsNullOrWhitespace(), () => { });
                                menu2.AddItem(new GUIContent("整数:" + att.message_int), att.message_int == 0, () => { });
                                menu2.AddItem(new GUIContent("浮点数:" + att.message_float), att.message_float == 0, () => { });
                                menu2.DropDown(new Rect(pos, new Vector2(500, 300)));
                            });
                        }
                    }

                }

                menu.ShowAsContext();
            }
            GUIHelper.PopColor();
            if (Event.current.type == EventType.MouseDrag)
            {
                xAT += 1;
                xAT %= 100;
            }
        }

    }
    #endregion

    #region ESDragResolver

    public class ESDragToFieldSolverAttributeDrawer : OdinAttributeDrawer<ESDragToFieldSolver>
    {
        private float lastHeight;
        private bool drag = false;
        protected override void DrawPropertyLayout(GUIContent label)
        {
            EditorGUILayout.Space(0);
            var space = GUILayoutUtility.GetLastRect();
            var startY1 = space.yMax;

            this.CallNextDrawer(label);
            float startY2 = GUILayoutUtility.GetLastRect().yMax;
            float f = startY2 - startY1;
            lastHeight = f > 0 ? f : lastHeight;

            Rect rect = space.SetYMax(space.yMin + lastHeight);
            var cc = Event.current;
            if (cc.type == EventType.DragExited || cc.type == EventType.MouseUp)
            {
                drag = false;
            }
            if (cc.type == EventType.DragUpdated)
            {
                drag = true;
            }
            if (drag)
            {
                EditorGUI.DrawRect(rect, Color.black._WithAlpha(0.25f));
            }
            if (cc.type == EventType.DragUpdated || cc.type == EventType.DragPerform)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Link;

                if (cc.type == EventType.DragPerform && rect.Contains(cc.mousePosition))
                {
                    DragAndDrop.AcceptDrag();
                    var use = DragAndDrop.objectReferences[0];
                    if (this.Attribute.solverOptions == ESDragToFieldSolverOptions.SimpleAssetToABSearchKey)
                    {

                        string prePath = AssetDatabase.GetAssetPath(use);
                        if (prePath == null) return;
                        string nowPath = prePath;
                        bool start = true;
                        while (nowPath != prePath || start)
                        {
                            start = false;
                            if (nowPath != null && !nowPath.IsNullOrWhitespace())
                            {
                                var ai = AssetImporter.GetAtPath(nowPath);
                                if (ai == null || ai.assetBundleName == null || ai.assetBundleName.IsNullOrWhitespace())
                                {
                                    prePath = nowPath.ToString();
                                    nowPath = nowPath._KeepBeforeByLast("/");
                                }
                                else
                                {
                                    var ab = ai.assetBundleName;
                                    Property.ValueEntry.WeakSmartValue = " AB名: " + ab + " ，资源名 ： " + use.name;
                                    break;
                                }

                            }
                        }
                        cc.Use();
                    }
                    else if (this.Attribute.solverOptions == ESDragToFieldSolverOptions.UnityEventNewInvoke)
                    {
                        if (Property.ValueEntry.WeakSmartValue is UnityEvent ue)
                        {
                            if (use is GameObject gg)
                            {
                                
                                MethodInfo addCall = typeof(UnityEvent).GetMethod("AddCall", BindingFlags.NonPublic | BindingFlags.Instance);
                                MethodInfo dele = typeof(UnityEvent).GetMethod("GetDelegate", BindingFlags.NonPublic | BindingFlags.Instance);

                                MethodInfo StringDELE = typeof(UnityEvent).GetMethod("AddStringPersistentListener", BindingFlags.NonPublic | BindingFlags.Instance);

                                MethodInfo BoolDELE = typeof(UnityEvent).GetMethod("AddBoolPersistentListener", BindingFlags.NonPublic | BindingFlags.Instance);


           /*                     Debug.Log(addCall + " ADD ");
                                Debug.Log(dele + " DELE ");
                                Debug.Log(StringDELE + " String ");*/

                                UnityAction<bool> unityAction = gg.SetActive;
                                var ThisDele = BoolDELE.Invoke(ue, new object[] { unityAction,true });
                                addCall.Invoke(ue, new object[] { ThisDele });
                                
                               /* Activator.CreateInstance(BaseInvokableCall)
                                ue.AddListener*/


                            }
                            else if (use is MonoBehaviour mb)
                            {

                            }
                            else if (use is Component com)
                            {

                            }
                        }
                    }
                }

            }

        }

    }

    #endregion
#endif
}
