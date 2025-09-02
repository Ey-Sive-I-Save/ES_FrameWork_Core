using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
 扩展时 建议创建新的脚本 
 修改文件时 使用 #region + 自己的名字
 格式尽量统一 
 多交流 --- Everey
 */
namespace ES.Pointer
{
    //核心 Ev针支持 关于 全局功能 部分
    //一般实现IPoinerNone
    #region 全局功能
    [Serializable, TypeRegistryItem("全局功能_重新加载当前场景")]
    public class PointerPicker_LoadCurrentScene : PointerOnlyAction
    {
       public override object Pick(object on= null, object from = null, object with = null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return base.Pick(on,from,with);
        }
    }
    [Serializable, TypeRegistryItem("全局功能_直接加载场景")]
    public class PointerPicker_LoadScene : PointerOnlyAction
    {
        [LabelText("字符串"), SerializeReference]
        public IPointerForString_Only string_Only = new PointerForString_Direc();

        public override object Pick(object on= null, object from = null, object with = null)
        {
            String s = string_Only?.Pick();
            if (s == null || s == "") return null;
            SceneManager.LoadScene(s);
            return base.Pick(on,from,with);
        }
    }
    [Serializable, TypeRegistryItem("全局功能_异步加载场景")]
    public class PointerPicker_LoadSceneAsync : PointerOnlyAction
    {
        [LabelText("字符串"), SerializeReference]
        public IPointerForString_Only string_Only = new PointerForString_Direc();

        public override object Pick(object on= null, object from = null, object with = null)
        {
            String s = string_Only?.Pick();
            if (s == null || s == "") return null;
            SceneManager.LoadSceneAsync(s);
            return base.Pick(on,from,with);
        }
    }
    [Serializable, TypeRegistryItem("全局功能_叠加加载场景")]
    public class PointerPicker_LoadSceneAdditive : PointerOnlyAction
    {
        [LabelText("字符串"), SerializeReference]
        public IPointerForString_Only string_Only = new PointerForString_Direc();

        public override object Pick(object on= null, object from = null, object with = null)
        {
            String s = string_Only?.Pick();
            if (s == null || s == "") return null;
            SceneManager.LoadScene(s, LoadSceneMode.Additive);
            return base.Pick(on,from,with);
        }
    }
    [Serializable, TypeRegistryItem("全局功能_叠加异步加载场景")]
    public class PointerPicker_LoadSceneAsyncAdditive : PointerOnlyAction
    {
        [LabelText("字符串"), SerializeReference]
        public IPointerForString_Only string_Only = new PointerForString_Direc();

        public override object Pick(object on= null, object from = null, object with = null)
        {
            String s = string_Only?.Pick();
            if (s == null || s == "") return null;
            SceneManager.LoadSceneAsync(s, LoadSceneMode.Additive);
            return base.Pick(on,from,with);
        }
    }
    [Serializable, TypeRegistryItem("全局功能_暂停游戏(时间缩放=0)")]
    public class PointerPicker_SetTimeScaleTo0_PauseGame : PointerOnlyAction
    {
        
        public override object Pick(object on= null, object from = null, object with = null)
        {
            Time.timeScale = 0;
            return null;
        }
    }
    [Serializable, TypeRegistryItem("全局功能_恢复游戏(时间缩放=1)")]
    public class PointerPicker_SetTimeScaleTo1_ResumeGame : PointerOnlyAction
    {
        public override object Pick(object on= null, object from = null, object with = null)
        {
            Time.timeScale = 1;
            return null;
        }
    }
    [Serializable, TypeRegistryItem("全局功能_设置时间缩放")]
    public class PointerPicker_SetTimeScale : PointerOnlyAction
    {
        [LabelText("设置缩放时间"), SerializeReference]
        public IPointerForFloat_Only float_only = new PointerForFloat_Direct();

        public override object Pick(object on= null, object from = null, object with = null)
        {
            float scale = float_only?.Pick()??1;
            Time.timeScale = scale;
            return null;
        }
    }
    #endregion

}
