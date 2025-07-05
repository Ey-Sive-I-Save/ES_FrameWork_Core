#define TEST

using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

//接口--可更新
namespace ES
{
    public interface IESOriginalModule
    {

    }
    public interface IESOriginalModule<in Host> : IESOriginalModule where Host : IESOringinHosting
    {
        bool Start(Host host);
    }
    //生命周期接口--纯Host和模块和Host且模块都有
    public interface IESWithLife
    {
        #region 生命周期接口
        bool Signal_IsActiveAndEnable { get; set; }
        bool CanUpdating { get; }
        public void TryDisableSelf();
        public void TryEnableSelf();
        public void TryUpdateSelf();
        public void TryDestroySelf();
        #endregion
    }
    //可更新的
    public interface IESModule : IESOriginalModule, IESWithLife
    {
        //这个是模块专属哈
        #region 模块专属功能区
        bool EnabledSelf { get; set; }

        void _TryActiveAndEnable();//带条件尝试启用

        void _TryInActiveAndDisable();//带条件尝试禁用
        bool _TryStartWithHost(IESOringinHosting host);//带条件尝试开始

        bool Signal_HasSubmit { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; set; }
        void _SetHost(IESOringinHosting host);
        #endregion 
    }

    public interface IESModule<Host> : IESModule where Host : class, IESOringinHosting
    {
        #region 托管声明
        Host GetHost { get; }
        bool _TryStartWithHost(Host host);
        void _SetHost(Host host);

        bool IESModule._TryStartWithHost(IESOringinHosting host)
        {
            if (Signal_HasSubmit) return true;
            return _TryStartWithHost(host as Host);
        }
        void IESModule._SetHost(IESOringinHosting host)
        {
            _SetHost(host as Host);
        }
        #endregion
    }

    public abstract class BaseESModule : IESModule
    {
        #region 显示控制状态
        [ShowInInspector, LabelText("控制自身启用状态"), PropertyOrder(-1)] public bool EnabledSelfControl { get => EnabledSelf; set { if (value) TryEnableSelf(); else TryDisableSelf(); } }
        [ShowInInspector, LabelText("显示活动状态"), GUIColor("@KeyValueMatchingUtility.ColorSelector.ColorForUpdating")]
        public bool IsActiveAndEnableShow { get => Signal_IsActiveAndEnable; }
        #endregion

        #region 重写逻辑
        //启用时逻辑
        public virtual bool CanUpdating => true;
        protected virtual void OnEnable() {
#if TEST
            Debug.Log("Enable");
#endif
        }
        //禁用时逻辑
        protected virtual void OnDisable() {
#if TEST
            Debug.Log("Disable");
#endif
        }
        //更新时逻辑
        protected virtual void Update() {
#if TEST
            Debug.Log("ApplyBuffers");
#endif
        }
        protected virtual void Start() {
#if TEST
            Debug.Log("Start");
#endif
        }
        protected virtual void OnDestroy() {
#if TEST
            Debug.Log("Destroy");
#endif
        }
        #endregion

        #region 关于开关逻辑与运行状态
        public bool Signal_IsActiveAndEnable
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return _isActiveAndEnable; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (value != _isActiveAndEnable)
                {
                    _isActiveAndEnable = value;
                    if (value) OnEnable();
                    else OnDisable();
                }
            }
        }
        private bool _isActiveAndEnable = false;
        public bool EnabledSelf { get => _enableSelf; set { _enableSelf = value; StateTestForSelfEnable(); } }

        private void StateTestForSelfEnable()
        {
            if (_hasSubmit)//只有在Submit时，才有权造成因为Self引起的状态改变
            {
                if (_isActiveAndEnable && !_enableSelf) _TryInActiveAndDisable();
                else if (!_isActiveAndEnable && _enableSelf) _TryActiveAndEnable();
            }
        }

        private void StateTestForSelfSubmit()
        {
            if (_isActiveAndEnable && !_hasSubmit) _TryInActiveAndDisable();
            else if (!_isActiveAndEnable && _hasSubmit) _TryActiveAndEnable();
        }

        [SerializeField, HideInInspector] private bool _enableSelf = true;
        public virtual void TryEnableSelf()
        {
            EnabledSelf = true;
        }
        public virtual void TryDisableSelf()
        {
            EnabledSelf = false;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void _TryActiveAndEnable()
        {
            if (Signal_IsActiveAndEnable || !EnabledSelf) return;//打开需要限制
            Signal_IsActiveAndEnable = true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void _TryInActiveAndDisable()
        {
            Signal_IsActiveAndEnable = false;//关闭肯定能关
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TryUpdateSelf()
        {
            if (CanUpdating && Signal_IsActiveAndEnable)
            {
                Update();
            }
        }

        public void HardUpdate()
        {
            //无条件
            Update();
        }
        #endregion

        #region 关于提交SubMit

        public bool Signal_HasSubmit
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return _hasSubmit; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set {
                if (value != _hasSubmit)
                {
                    _hasSubmit = value;
                    StateTestForSelfSubmit();
                    if (value) Start();//在OnEnable之后
                    else OnDestroy();//在OnDisable之后
                }
            }
        }
        private bool _hasSubmit = false;

        /*可能会因为对象销毁和自己被移除队列时，触发它*/
        public abstract void TryDestroySelf();//踢出

        public bool _TryStartWithHost(IESOringinHosting host)
        {
            if (Signal_HasSubmit) return true;
            if (host != null)
            {
                _SetHost(host);
                Signal_HasSubmit = true;
                return true;
            }
            return false;
        }

        public virtual void _SetHost(IESOringinHosting host)
        {

        }


        #endregion
    }
    /*[Serializable, TypeRegistryItem("结构体委托模块")]
    public struct ESModuleStruct : IESModule
    {
        #region 委托
        private Action<ESModuleStruct> Action_Enable;
        private Action<ESModuleStruct> Action_Disable;
        private Action<ESModuleStruct> Action_OnUpdate;
        #endregion

        #region 重写逻辑
        //启用时逻辑
        public bool CanUpdating => true;
        #endregion

        #region 关于开关逻辑与运行状态
        public bool IsActiveAndEnable { get; set; }
        public bool EnabledSelf { get => true; set { } }//永true式

        public void TryEnableSelf()
        {
        }
        public void TryDisableSelf()
        {
        }
        public void _TryActiveAndEnable()
        {
            if (IsActiveAndEnable) return;//不要你
            *//* OnEnable();*//*
            IsActiveAndEnable = true; Action_Enable?.Invoke(this);
        }
        public void _TryInActiveAndDisable()
        {
            if (IsActiveAndEnable)
            {
                IsActiveAndEnable = false; Action_Disable?.Invoke(this);
            }
        }
        public void TryUpdate()
        {
            if (IsActiveAndEnable)
            {
                Action_OnUpdate?.Invoke(this);
            }
        }
        #endregion

        #region 关于提交SubMit

        public bool _HasSubmit { get; set; }
        #endregion
        [Tooltip("规定启用时的事件")]
        public ESModuleStruct WithEnable(Action<ESModuleStruct> func)
        {
            Action_Enable = func;
            return this;
        }
        [Tooltip("规定禁用时的事件")]
        public ESModuleStruct WithDisable(Action<ESModuleStruct> func)
        {
            Action_Disable = func;
            return this;
        }
        [Tooltip("规定帧运行时的事件")]
        public ESModuleStruct WithUpdate(Action<ESModuleStruct> func)
        {
            Action_OnUpdate = func;
            return this;
        }


        public void SetOringinHosting(IESOringinHosting host)
        {
            
        }
    }*/
    /*    [Serializable, TypeRegistryItem("模块_无自周期")]
        public class ESModule_NoSelf : IESModule
        {


            #region 重写逻辑
            //启用时逻辑
            public bool CanUpdating => true;
            #endregion

            #region 关于开关逻辑与运行状态
            public bool IsActiveAndEnable { get; set; }
            public bool EnabledSelf { get => true; set { } }//永true式
            protected virtual void OnEnable() { IsActiveAndEnable = true; }
            //禁用时逻辑
            protected virtual void OnDisable() { IsActiveAndEnable = false; }
            //更新时逻辑
            protected virtual void Update() { }
            public void TryEnableSelf()
            {

            }
            public void TryDisableSelf()
            {
            }
            public void _TryActiveAndEnable()
            {
                if (IsActiveAndEnable) return;//不要你
                *//* OnEnable();*//*
                OnEnable();
            }
            public void _TryInActiveAndDisable()
            {
                if (IsActiveAndEnable)
                {
                    OnDisable();
                }
            }
            public void TryUpdate()
            {
                if (IsActiveAndEnable)
                {
                    Update();
                }
            }
            #endregion

            #region 关于提交SubMit

            public bool _HasSubmit { get; set; }
            public bool TrySubmitHosting(IESOringinHosting hosting)
            {
                if (_HasSubmit) return true;
                if (hosting is IESHosting hosting1)
                {
                    *//* hosting1.VirtualBeHosted.TryAdd(this);*//*
                    return _HasSubmit = true;
                }
                return true;
            }
            public bool TryWithDrawHosting(IESOringinHosting hosting, bool asVirtual)
            {
                if (!_HasSubmit) return false;
                if (asVirtual)
                {
                    return _HasSubmit = false;
                }
                return false;
            }
            public void TryWithDrawHostingVirtual()
            {
                TryWithDrawHosting(null, true);
            }

            #endregion
        }*/
    [Serializable, TypeRegistryItem("模块_快速委托")]
    public class ESModule_Quick : IESModule
    {
        #region 委托
        private Action<ESModule_Quick> Action_Enable;
        private Action<ESModule_Quick> Action_Disable;
        private Action<ESModule_Quick> Action_OnUpdate;
        #endregion

        #region 重写逻辑
        //启用时逻辑
        public bool CanUpdating => true;
        #endregion

        #region 关于开关逻辑与运行状态
        public bool Signal_IsActiveAndEnable { get; set; }
        public bool EnabledSelf { get => true; set { } }//永true式

        public void TryEnableSelf()
        {
        }
        public void TryDisableSelf()
        {
        }
        public void _TryActiveAndEnable()
        {
            if (Signal_IsActiveAndEnable) return;//不要你
            /* OnEnable();*/
            Signal_IsActiveAndEnable = true; Action_Enable?.Invoke(this);
        }
        public void _TryInActiveAndDisable()
        {
            if (Signal_IsActiveAndEnable)
            {
                Signal_IsActiveAndEnable = false; Action_Disable?.Invoke(this);
            }
        }
        public void TryUpdateSelf()
        {
            if (Signal_IsActiveAndEnable)
            {
                Action_OnUpdate?.Invoke(this);
            }
        }
        #endregion

        #region 关于提交SubMit

        public bool Signal_HasSubmit { get; set; }
        public bool TrySubmitHosting(IESOringinHosting hosting)
        {
            if (Signal_HasSubmit) return true;
            if (hosting is IESHosting hosting1)
            {
                /* hosting1.VirtualBeHosted.TryAdd(this);*/
                return Signal_HasSubmit = true;
            }
            return true;
        }
        public bool TryWithDrawHosting(IESOringinHosting hosting, bool asVirtual)
        {
            if (!Signal_HasSubmit) return false;
            if (asVirtual)
            {
                return Signal_HasSubmit = false;
            }
            return false;
        }
        public void TryWithDrawHostingVirtual()
        {
            TryWithDrawHosting(null, true);
        }

        #endregion
        [Tooltip("规定启用时的事件")]
        public ESModule_Quick WithEnable(Action<ESModule_Quick> func)
        {
            Action_Enable = func;
            return this;
        }
        [Tooltip("规定禁用时的事件")]
        public ESModule_Quick WithDisable(Action<ESModule_Quick> func)
        {
            Action_Disable = func;
            return this;
        }
        [Tooltip("规定帧运行时的事件")]
        public ESModule_Quick WithUpdate(Action<ESModule_Quick> func)
        {
            Action_OnUpdate = func;
            return this;
        }

        public bool _TryStartWithHost(IESOringinHosting host)
        {
            Signal_HasSubmit = true;
            return true;
        }

        public void _SetHost(IESOringinHosting host)
        {

        }
        public void TryDestroySelf()
        {

        }
    }
    public abstract class BaseESModule<Host> : BaseESModule, IESModule<Host> where Host : class, IESOringinHosting
    {

        #region 与自己的Host关联

        public virtual bool _TryStartWithHost(Host host) { _SetHost(host); Signal_HasSubmit = true; return true; }

        public abstract void _SetHost(Host host);

        public virtual Host GetHost { get; }

        public sealed override void _SetHost(IESOringinHosting host)
        {
            if (host is Host h) _SetHost(h);
        }

        #endregion
    }
    [Serializable, TypeRegistryItem("ES模块_带委托的", "模块")]
    public class ESModule_WithDelegate : BaseESModule
    {
        [FoldoutGroup("默认委托")] private Action<ESModule_WithDelegate> Action_Enable;
        [FoldoutGroup("默认委托")] private Action<ESModule_WithDelegate> Action_Disable;
        [FoldoutGroup("默认委托")] private Action<ESModule_WithDelegate> Action_OnUpdate;

        protected sealed override void OnEnable()
        {
            Action_Enable?.Invoke(this);
            base.OnEnable();

        }

        protected sealed override void OnDisable()
        {
            Action_Disable?.Invoke(this);
            base.OnDisable();

        }

        protected sealed override void Update()
        {
            Action_OnUpdate?.Invoke(this);
            base.Update();
        }

        [Tooltip("规定启用时的事件")]
        public ESModule_WithDelegate WithEnable(Action<ESModule_WithDelegate> func)
        {
            Action_Enable = func;
            return this;
        }

        [Tooltip("规定禁用时的事件")]
        public ESModule_WithDelegate WithDisable(Action<ESModule_WithDelegate> func)
        {
            Action_Disable = func;
            return this;
        }

        [Tooltip("规定帧运行时的事件")]
        public ESModule_WithDelegate WithUpdate(Action<ESModule_WithDelegate> func)
        {
            Action_OnUpdate = func;
            return this;
        }

        public override void TryDestroySelf()
        {
            Signal_HasSubmit = false;
        }
    }
}
