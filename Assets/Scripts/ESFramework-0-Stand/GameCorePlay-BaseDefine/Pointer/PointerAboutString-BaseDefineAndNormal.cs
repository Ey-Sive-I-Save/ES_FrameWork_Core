#if UNITY_EDITOR
using UnityEditorInternal;
#endif

using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
/*
 扩展时 建议创建新的脚本 
 修改文件时 使用 #region + 自己的名字
 格式尽量统一 
 多交流 --- Everey
 */
namespace ES.Pointer
{
    //核心 Ev针支持 关于 字符串 部分
    #region 字符串部分
    #region 字符串接口抽象和包
    public interface IPointerForString_Only : IPointerOnlyBackSingle<string>, IPointerForString<object, object, object>
    {
        object IPointer.Pick(object a, object b, object c)
        {
            return Pick();
        }
    }
    public interface IPointerForString<On, From, With> : IPointer<string, On, From, With>
    {

    }

    public interface IPointerForStringList : IPointerOnlyBackList<String>
    {
        object IPointer.Pick(object a, object b, object c)
        {
            return Pick();
        }
    }

    #endregion
   
   
    [Serializable, TypeRegistryItem("字符串资源文件路径", "单值针/字符串针")]
    public class PointerForString_ResourceDirec : IPointerForString_Only
    {
        [LabelText("直接拖入资源")]
        [FilePath(ParentFolder = "Assets/Resources/", RequireExistingPath = false, IncludeFileExtension = false)]
        public string path;
        public string Pick(object on= null, object from = null, object with = null)
        {
            return path;
        }
    }
   


    [Serializable]
    public class PointerForString_Direc : IPointerForString_Only
    {
        [LabelText("直接输入")] public string string_direc;
        public string Pick(object on= null, object from = null, object with = null)
        {
            return string_direc;
        }
        object IPointer.Pick(object a, object b, object c)
        {
            return (this as IPointerForString_Only)?.Pick();
        }
    }
    [Serializable, TypeRegistryItem("字符串_标签", "单值针/字符串")]
    public class PointerForString_Tag : IPointerForString_Only
    {
        [ValueDropdown("tags"), LabelText("选择标签")]
        public string tagName;
        public string Pick(object on= null, object from = null, object with = null)
        {
            return tagName;
        }

        public string[] tags()
        {
            string[] s = default;
#if UNITY_EDITOR

            s = InternalEditorUtility.tags;
#endif
            return s;
        }
        object IPointer.Pick(object a, object b, object c)
        {
            return (this as IPointerForString_Only)?.Pick();
        }
    }

    [Serializable, TypeRegistryItem("字符串列表_标签", "单值针/字符串")]
    public class PointerForStringList_Tag : IPointerForStringList
    {
        [ValueDropdown("tags"), LabelText("选择一些标签")]
        public List<string> tagNames = new List<string>();
        public List<string> Pick(object on= null, object from = null, object with = null)
        {
            
            return tagNames;
        }
        public string[] tags()
        {
            string[] s = default;
#if UNITY_EDITOR
            s = InternalEditorUtility.tags;
#endif
            return s;
        }
        
    }
    #endregion
}
