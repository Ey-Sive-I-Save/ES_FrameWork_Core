using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace ES
{
    
    //有最优先的加载权限
    [DefaultExecutionOrder(-10)]
    /*作为全局数据替代场景单例来获取编辑器时数据*/
    public class GlobalDataSupport<This> : GlobalSerializedScriptableObject where This : GlobalDataSupport<This>
    {
        #region 单例声明
        public static This Instance
        {
            get
            {
                //已经确定
               
                if (_instance != null)
                {
                    return _instance;
                };
                if (AllCaches.Count > 0)
                {
                    foreach (var i in AllCaches)
                    {
                        //先找确定的
                        if (i._IsNotNullAndUseIt()?.HasConfirm ?? false) return _instance = i;
                    }
                    foreach (var i in AllCaches)
                    {
                        //只要能用就行
                        if (i != null) return _instance = i.ConfirmThisOnly();
                    }
                  
                }
                bool b = HasReactiveTable.TryGetValue(typeof(This), out var bo);
                Debug.Log(b + "  "+ bo);
                if (!b || !bo) //未定义或者未呈
                {
                    HasReactiveTable[typeof(This)] = true;
                    if (KeyValueMatchingUtility.SafeEditor.DisplayDialog
                        ("配置缺失，准备创建", "检测不到可用的【" + typeof(This) + "】SO数据,准备新建一个，是否继续"))
                    {
                        string path = KeyValueMatchingUtility.SafeEditor.SelectorFolder(title: "创建数据到：");
                        path = path._KeepAfter("Asset", true);
                        Debug.Log(path+ ""+ KeyValueMatchingUtility.SafeEditor.IsValidFolder(path));
                        if (KeyValueMatchingUtility.SafeEditor.IsValidFolder(path))
                        {
                            var first = KeyValueMatchingUtility.SafeEditor.CreateSO<This>(path, "全局数据" + typeof(This).Name);
                            first.TryConfirmSwitchThis();
                        }
                    }
                }
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        [ShowInInspector]
        private static This _instance;
        [ShowInInspector, LabelText("该类型全部数据"), InlineButton("TryConfirmSwitchThis", "选中当前为主数据")]
        private static HashSet<This> AllCaches = new HashSet<This>();
        private static Action<This> OnConfirmOneSO = (who) => { };


        [LabelText("被选中为主数据"), ReadOnly, PropertyOrder(-3)]
        public bool HasConfirm = false;//选定一个
        private static Dictionary<Type, bool> HasReactiveTable = new Dictionary<Type, bool>();//敏感互动表(自动创建)
        private void OnValidate()
        {
            Refresh();
            Debug.Log("Channel1");
        }
        private void OnEnable()
        {
            Refresh();
            Debug.Log("Channel2");
        }
        private void OnDestroy()
        {
            if (HasConfirm)
            {
                HasReactiveTable[typeof(This)] = false;
                Debug.Log(666);
            }
        }
        private void Refresh()
        {
          
            if (this is This use )
            {
                if (HasConfirm) _instance = use;
                if (!AllCaches.Contains(use)) {
                    AllCaches.Add(use);
                    OnConfirmOneSO += Delegate_OnConfirmOneSO;
                    HasReactiveTable[typeof(This)] = false;
                }
            }
        }
        internal This ConfirmThisOnly()
        {
            HasConfirm = true;
            return this as This;
        }
        internal void TryConfirmSwitchThis()
        {
            if (this is This use)
            {
                if (!HasConfirm)//从关到开
                {

                    HasConfirm = true;
                    _instance = use;
                    OnConfirmOneSO?.Invoke(use);

                }
                else //从开到验证
                {
                    if (_instance != this)
                    {
                        _instance = null;//取消确立
                    }
                }
            }
        }
        private void Delegate_OnConfirmOneSO(This who)
        {
            if (who != this as This)
            {
                HasConfirm = false;
                KeyValueMatchingUtility.SafeEditor.SetDirty(this);
            }
        }
        #endregion
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnAppStart()
        {
            // 创建并初始化 ScriptableObject

        }
    }
   
    public abstract class GlobalSerializedScriptableObject : SerializedScriptableObject
    {
       
    }

#if UNITY_EDITOR
    [InitializeOnLoad]
    [DefaultExecutionOrder(-10)]
    public static class GlobalDataEditorIniter
    {
        static GlobalDataEditorIniter()
        {
            Debug.Log("Init");
            KeyValueMatchingUtility.SafeEditor.InitAsset<GlobalSerializedScriptableObject>();
        }
    }
#endif
    [Serializable]
    public class GlobalDataRefer<This> : ESRefer<This> where This : GlobalDataSupport<This>
    {
        public override This HandleIfNull()
        {
            return GlobalDataSupport<This>.Instance;
        }
    }
}

