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


namespace ES
{
#if UNITY_EDITOR
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
#endif
}
