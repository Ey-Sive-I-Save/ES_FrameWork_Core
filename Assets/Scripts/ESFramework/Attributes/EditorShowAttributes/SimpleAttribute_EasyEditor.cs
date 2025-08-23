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

//纯编辑器显示 的 特性定义
namespace ES
{
    #region 背景
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public class ESBackGroundAttribute : Attribute
    {
        public Color color;
        public float WithAlpha = 1;
        public ESBackGroundAttribute()
        {
            color = Color.white;
        }
        // 支持多种构造方式
        public ESBackGroundAttribute(float r, float g, float b, float a)
        {

            color = new Color(r, g, b);
            WithAlpha = a;
        }
        public ESBackGroundAttribute(float r, float g, float b)
        {

            color = new Color(r, g, b);
        }
        public ESBackGroundAttribute(string colorName)
        {
            color = Color.white;
            KeyValueMatchingUtility.ColorSelector.normalColors.TryGetValue(colorName, out color);
        }
        public ESBackGroundAttribute(string colorName, float withAlpha = 1)
        {
            KeyValueMatchingUtility.ColorSelector.normalColors.TryGetValue(colorName, out color);
            this.WithAlpha = withAlpha;
            
        }
        public ESBackGroundAttribute(string colorName, float withRGBMuti, float withAlpha)
        {
            KeyValueMatchingUtility.ColorSelector.normalColors.TryGetValue(colorName, out color);
            
            color = color * withRGBMuti;
            WithAlpha = withAlpha;
        }
    }

    #endregion

    #region 键组
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ESKeyStore : Attribute
    {


        public ESKeyStore()
        {
        }
        // 支持多种构造方式
    }

    #endregion

    #region bool两级描述

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ESBoolOption : Attribute
    {
        public string FalseLabel;
        public string TrueLabel;

        public ESBoolOption(string forFalse, string forTrue)
        {
            this.FalseLabel = forFalse; 
            this.TrueLabel = forTrue;
        }
    }


    #endregion


}

