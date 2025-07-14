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

namespace ES
{
    #region 背景
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
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
}

