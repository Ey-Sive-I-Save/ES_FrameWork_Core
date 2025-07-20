using ES;
#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
#endif
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

namespace ES
{
    #region 背景
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property| AttributeTargets.Class| AttributeTargets.Struct, AllowMultiple = false)]
    public class ESBackGroundAttribute : Attribute
    {

        internal Color color;
        public ESBackGroundAttribute()
        {

            color = Color.white;
        }
        // 支持多种构造方式
        public ESBackGroundAttribute(float r, float g, float b, float a)
        {

            color = new Color(r, g, b, a);
        }
        public ESBackGroundAttribute(float r, float g, float b)
        {

            color = new Color(r, g, b, 1);
        }
        public ESBackGroundAttribute(string colorName)
        {
            color = Color.white;
            KeyValueMatchingUtility.ColorSelector.normalColors.TryGetValue(colorName, out color);
        }
        public ESBackGroundAttribute(string colorName, float withMuti = 1)
        {
            KeyValueMatchingUtility.ColorSelector.normalColors.TryGetValue(colorName, out color);
            color *= withMuti;
        }
        public ESBackGroundAttribute(string colorName, float withRGBMuti, float withAMuti)
        {
            KeyValueMatchingUtility.ColorSelector.normalColors.TryGetValue(colorName, out color);
            float a = color.a * withAMuti;
            color = color * withRGBMuti;
            color.a = a;
        }
    }
#if UNITY_EDITOR
    public class BackGroundAttributeDrawer : OdinAttributeDrawer<ESBackGroundAttribute>
    {
        private float lastHeight;
        protected override void DrawPropertyLayout(GUIContent label)
        {
            EditorGUILayout.Space(0);
            var space = GUILayoutUtility.GetLastRect();
            var startY1 = space.yMax;
            SirenixEditorGUI.DrawBorders(space, (int)space.width, 0, (int)lastHeight + 2, 0, this.Attribute.color);
            this.CallNextDrawer(label);
            float startY2 = GUILayoutUtility.GetLastRect().yMax;
            float f = startY2 - startY1;
            lastHeight = f > 0 ? f : lastHeight;
        }

    }
#endif
    #endregion

    #region 键组
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ESKeyStore: Attribute
    {

        
        public ESKeyStore()
        {
        }
        // 支持多种构造方式
    }
#if UNITY_EDITOR
    public class ESKeyStoreDrawer : OdinAttributeDrawer<ESKeyStore>
    {
        private float lastHeight;
        protected override void DrawPropertyLayout(GUIContent label)
        {
            EditorGUILayout.Space(0);
            this.CallNextDrawer(label);
            Rect OR = GUILayoutUtility.GetLastRect();
            Rect buttonRectForAT = OR;
            buttonRectForAT.width = 25f;
            buttonRectForAT.xMin = buttonRectForAT.xMin-25;
            buttonRectForAT.xMax = buttonRectForAT.xMin +25;
            buttonRectForAT.y -= 5;
           

            //GUIHelper.PushColor(Color.white._WithAlpha(0.5f));
            if (GUI.Button(buttonRectForAT, "@"))
            {
                // 弹出菜单
                GenericMenu menu = new GenericMenu();

                // 添加普通选项
                menu.AddItem(new GUIContent("Option1"), false, () => Debug.Log("Selected Option1"));
                menu.AddItem(new GUIContent("Option2"), false, () => Debug.Log("Selected Option2"));

                // 添加带图标的选项（需自定义图标）
                // menu.AddItem(new GUIContent("Option3", someIcon), false, () => {...});

                // 显示菜单（在鼠标位置）
                menu.ShowAsContext();
            }
            //GUIHelper.PopColor();
        }

    }
#endif

    #endregion
}

