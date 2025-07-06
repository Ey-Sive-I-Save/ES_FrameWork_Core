using FishNet.Object;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
#endif
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace ES
{
    /*核心功能*/
    public interface ICore
    {
    }
    [DefaultExecutionOrder(-2)]//顺序在前
    public abstract class Core : MonoBehaviour, ICore
    {

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
            for(int i = 0; i < domains.Count; i++)
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
        public void RegisterDomains(params IDomain[] rgdomains)
        {
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
