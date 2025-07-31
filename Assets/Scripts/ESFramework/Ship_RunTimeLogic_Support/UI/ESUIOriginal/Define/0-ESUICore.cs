using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES
{
    [DefaultExecutionOrder(-2)]//顺序在前
    public abstract class ESUICore_Original : SerializedMonoBehaviour, ICore
    {
        #region 原始通用域
        [LabelText("ESUI原始扩展域"), TabGroup("【固有】")]
        public ESUIOriginalDomain OriginalDomain;

        #endregion
        [LabelText("正在加载")] public bool IsLoading = false;
        public void TryOpen(ILink link = default)
        {
            if (enabled) return;//还在使用呢
            this.enabled = true;  //可见不一定可用把
            OnOpen(link);
            gameObject.SetActive(true);//打开必可见
        }
        public void TryClose(ILink link = default)
        {
            if (!enabled) return;//已经禁用了哈
            this.enabled = false;
            OnClose(link);
            //关闭不一定可见--或者需要等待不可见
        }
        protected virtual void OnOpen(ILink link)
        {

        }
        protected virtual void OnClose(ILink link)
        {

        }
        //注册和注销
        #region 检查器专属

        //域颜色赋予
        public Color Editor_DomainTabColor(IDomain domain)
        {
            if (domain == null) return Color.gray;
            else return Color.yellow;
        }



        //编辑器模式下的临时关联
#if UNITY_EDITOR
        [ContextMenu("<ES>创建临时关系")]
        public void CreateCacheRelationship()
        {
            var all = GetComponentsInChildren<IDomain>();
            foreach (var i in all)
            {
                i.RegisterAllButOnlyCreateRelationship(this);
            }
        }
#endif

        #endregion

        #region 补充信息

        //获取特定域
        private List<IDomain> domains = new List<IDomain>(3);



        #endregion

        #region Awake流程
        //Awake回调
        protected virtual void Awake()
        {
            _DoAwake();
        }
        public void _DoAwake()
        {
            OnBeforeAwakeRegister();
            OnAwakeRegisterOnly();
            OnAfterAwakeRegister();
        }

        //注册扩展Domain前发生前
        protected virtual void OnBeforeAwakeRegister()
        {

        }
        //仅用于手动注册
        protected virtual void OnAwakeRegisterOnly()
        {
            /* 演示 一句完成
             * RegisterDomains(domain1,domain2,...);*/
        }
        //注册扩展Domain发生的事
        protected virtual void OnAfterAwakeRegister()
        {
            /*RegisterDomains*/
        }

        #endregion

        #region 重写逻辑

        protected virtual void Update()
        {
            for (int i = 0; i < domains.Count; i++)
            {
                domains[i].TryUpdateSelf();
            }
        }

        protected virtual void OnEnable()
        {
            for (int i = 0; i < domains.Count; i++)
            {
                domains[i].TryEnableSelf();
            }
        }

        protected virtual void OnDisable()
        {
#if UNITY_EDITOR
            if (ESEditorRuntimePartMaster.IsQuit) return;
#endif
            for (int i = 0; i < domains.Count; i++)
            {
                domains[i].TryDisableSelf();
            }
        }

        protected virtual void OnDestroy()
        {
#if UNITY_EDITOR
            if (ESEditorRuntimePartMaster.IsQuit) return;
#endif
            for (int i = 0; i < domains.Count; i++)
            {
                domains[i].TryDestroySelf();
            }
        }

        #endregion

        #region 常用功能

        //手动注册
        public virtual void RegisterAllDomains(params IDomain[] rgdomains)
        {
            if (OriginalDomain != null) OriginalDomain.RegisterAllWithCore(this);
            foreach (var i in rgdomains)
            {
                if (i == null) continue;
                i.RegisterAllWithCore(this);
                domains.Add(i);
            }
        }

        #endregion

    }

}
