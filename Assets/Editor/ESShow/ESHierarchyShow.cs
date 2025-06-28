using ES;
using Sirenix.Utilities.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace ES
{

    public class ESHierarchyShow
    {
        [InitializeOnLoad]
        public static class HierarchyIconAdder
        {
            static HierarchyIconAdder()
            {
                EditorApplication.hierarchyWindowItemOnGUI += DrawIconOnHierarchy;
            }

            private static void DrawIconOnHierarchy(int instanceID, Rect selectionRect)
            {
                GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

                if (gameObject != null)
                {
                    var ins = ESEditorOnlyPartMaster.Instance;
                    if (ins == null) return;
                    var ICON = ins.ICON;
                    //右侧图标系
                    if (ICON != null)
                    {
                        float offset = ICON.startOffset;
                        foreach (var i in ICON.HierachySelectorS)
                        {
                            if (i == null) continue;
                            (bool Macth, ESEditorOnlyPartMaster.BackMessage Back) s = i.IsAppliable(gameObject);
                            if (s.Macth)
                            {
                                Rect iconRect = new Rect(selectionRect);
                                iconRect.x = iconRect.xMax + offset;

                                var con = i.contentGetter;
                                if (con != null)
                                {
                                    iconRect.width = ICON.width * con.scale;
                                    GUIContent icon = con.GetContent() ?? EditorGUIUtility.IconContent("d_GameObject Icon");
                                    icon.tooltip = "按下左Alt缓存--------";
                                    GUIHelper.PushColor(i.contentGetter?.color ?? Color.white);
                                    GUI.Label(iconRect, icon);
                                    GUIHelper.PopColor();
                                    offset += ICON.offsetPerTime;
                                    /*Debug.Log(""+ (s.Back.component != null)+(Event.current.type == EventType.KeyDown) + ((Event.current.keyCode == KeyCode.LeftAlt)) + 11 + (iconRect .Contains( Event.current.mousePosition)));*/
                                    if (s.Back.component != null && (Event.current.type == EventType.KeyDown) && (Event.current.keyCode == KeyCode.LeftAlt))
                                    {
                                       
                                        if (true)
                                        {
                                            if (iconRect.Contains(Event.current.mousePosition))
                                            {
                                                ESPreviewWindow.Objects.Add(s.Back.component);
                                                ESPreviewWindow.ES_RefreshWindow();   
                                               /* inlineed
                                                EditorUtility.ob<MonoBehaviour>(s.Back.component, false, "l:script", EditorGUIUtility.GetControlID(FocusType.Passive));
*/
                                            }
                                        }
                                    }
                                
                                }

                            }

                        }
                    }
                    //左侧复选框
                    if (true)
                    {
                        Rect enable = new Rect(selectionRect);
                        enable.x = enable.xMin - 25;
                        enable.width = 16;
                        enable.height = 16;
                        bool pre = gameObject.activeSelf;
                        GUIHelper.PushColor(Color.white.EX_WithAlpha(pre ? 0.75f : 1f).EX_WithRGBMuti(pre ? 0.95f : 0.5f));
                        bool b = GUI.Toggle(enable, pre, "");
                        GUIHelper.PopColor();
                        if (b != pre)
                        {
                            gameObject.SetActive(b);
                        }
                    }
                }
            }
        }
    }
}

