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


namespace ES {
    /*ES Message 用于非字段或者枚举(尤其)添加额外信息，减少使用键值匹配方法的手动书写
     配合扩展方法 _AT_ESMessage 直接取出信息
     */
    #region 信息
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ESMessage : Attribute
    {
        public string message;

        public int message_int=0;

        public float message_float=0;
        public ESMessage()
        {
            
            message = "";
        }
        // 支持多种构造方式
        public ESMessage(string s)
        {
            message = s;
        }
        public ESMessage(int i)
        {
            message_int = i;
        }
        public ESMessage(float f)
        {
            message_float = f;
        }
        public ESMessage(string s,int i,float f)
        {
            message = s;
            message_int = i;
            message_float = f;
        }
    }

    #endregion
}
