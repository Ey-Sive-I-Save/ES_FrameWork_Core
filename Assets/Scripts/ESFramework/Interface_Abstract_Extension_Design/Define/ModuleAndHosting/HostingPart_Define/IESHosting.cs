using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

namespace ES
{
    public interface IESOringinHosting
    {

    }
    //以Hosting声明
    public interface IESHosting : IESOringinHosting, IESWithLife
    {
        #region 托管器专属
        //虚拟的
        /* SafeUpdateSet_EasyQueue_SeriNot_Dirty<IESModule> VirtualBeHosted { get; }*/
        void UpdateAsHosting();
        void EnableAsHosting();
        void DisableAsHosting();
        void TryAddToIEnumableOnly(IESModule module);
        void TryRemoveFromIEnumableOnly(IESModule module);
        #endregion
    }
    public abstract class BaseESHosting : IESHosting
    {

        #region 实现自定义帧更新
        private short UpdateIntervalFrameCount = -1;
        private short SelfTargetModel = -1;
        public void ResetUpdateIntervalFrameCount(short interval = 10)
        {
            UpdateIntervalFrameCount = interval;
            if (UpdateIntervalFrameCount > 0)
            {
                SelfTargetModel = (short)UnityEngine.Random.Range(0, UpdateIntervalFrameCount);
            }
        }
        #endregion

        #region 虚拟列表
        /* public SafeUpdateSet_EasyQueue_SeriNot_Dirty<IESModule> VirtualBeHosted => virtualBeHostedList;
         [LabelText("虚拟托管列表"),FoldoutGroup("虚拟托管")] public SafeUpdateSet_EasyQueue_SeriNot_Dirty<IESModule> virtualBeHostedList = new SafeUpdateSet_EasyQueue_SeriNot_Dirty<IESModule>();
        */
        #endregion

        #region 重写逻辑
        public virtual bool CanUpdating
        {
            get { return true; }
        }

        protected virtual void Update()
        {
            if (UpdateIntervalFrameCount > 0)
            {
                if (SelfTargetModel < 0) ResetUpdateIntervalFrameCount(UpdateIntervalFrameCount);
                if (Time.frameCount % UpdateIntervalFrameCount != SelfTargetModel)
                {
                    return;
                }
            }
            UpdateAsHosting();
            /*virtualBeHostedList.Update();*/
        }

        protected virtual void OnEnable()
        {
            if (UpdateIntervalFrameCount > 0 && SelfTargetModel < 0)
            {
                ResetUpdateIntervalFrameCount(UpdateIntervalFrameCount);
            }
            Signal_IsActiveAndEnable = true;
            EnableAsHosting();
        }

        protected virtual void OnDisable()
        {
            Signal_IsActiveAndEnable = false;
            DisableAsHosting();
        }
        #endregion

        #region 关于开关逻辑与运行状态
        public bool Signal_IsActiveAndEnable { get; set; } = false;


        public virtual void TryEnableSelf()
        {
            if (Signal_IsActiveAndEnable) return;
            OnEnable();
        }


        public virtual void TryDisableSelf()
        {
            if (!Signal_IsActiveAndEnable)
            {
                OnDisable();
            }
        }

        public void TryUpdateSelf()
        {
            if (CanUpdating && Signal_IsActiveAndEnable)
            {
                Update();
            }
        }
        #endregion

        #region 与对子的控制

        public virtual void UpdateAsHosting()
        {
            /* if (VirtualBeHosted != null)
             {
                 foreach (var i in VirtualBeHosted.valuesNow_)
                 {
                         if (!i._HasSubmit)
                         {
                             i._TryInActiveAndDisable();
                             //已经放弃
                             virtualBeHostedList.TryRemove(i);
                             continue;
                         }
                     i.TryUpdate();
                 }
             }*/
        }

        public virtual void EnableAsHosting()
        {
            /*  if (VirtualBeHosted != null)
              {
                  foreach (var i in VirtualBeHosted.valuesNow_)
                  {
                      i._TryActiveAndEnable();
                  }
              }*/
        }

        public virtual void DisableAsHosting()
        {

            /* if (VirtualBeHosted != null)
             {
                 foreach (var i in VirtualBeHosted.valuesNow_)
                 {
                     i._TryInActiveAndDisable();
                 }
             }*/
        }

        public abstract void TryAddToIEnumableOnly(IESModule module);
        public abstract void TryRemoveFromIEnumableOnly(IESModule module);
        public abstract void TryDestroySelf();
        #endregion
    }
    //以泛型声明
    public interface IESHosting<WithModule> : IESHosting where WithModule : class, IESModule
    {
        IEnumerable<WithModule> ModulesIEnumable { get; }

    }
    public abstract class BaseESHosting<With> : BaseESHosting, IESHosting<With> where With : class, IESModule
    {
        #region 对特定类型的托管支持
        public abstract IEnumerable<With> ModulesIEnumable { get; }
        public override void EnableAsHosting()
        {
            if (ModulesIEnumable != null)
            {
                foreach (var i in ModulesIEnumable)
                {
                    i._TryActiveAndEnable();
                }
            }
            base.EnableAsHosting();
        }
        public override void DisableAsHosting()
        {
            if (ModulesIEnumable != null)
            {
                foreach (var i in ModulesIEnumable)
                {
                    i._TryInActiveAndDisable();
                }
            }
            base.DisableAsHosting();
        }
        public override void UpdateAsHosting()
        {
            if (ModulesIEnumable != null)
            {
                foreach (var i in ModulesIEnumable)
                {
                    //性能几乎一致？
                    if (!i.Signal_HasSubmit)
                    {
                        i._TryInActiveAndDisable();
                        //已经放弃

                        continue;
                    }
                    i.TryUpdateSelf();
                }
            }
            base.UpdateAsHosting();
        }

        public abstract void _RemoveModuleFromList(With use);
        #endregion
    }
    public abstract class BaseESHostingAndModule<USE, Host> : BaseESHosting<USE>, IESModule<Host> where Host : class, IESHosting where USE : class, IESModule
    {
        #region 与自己的Host关联
        public Host GetHost { get => host; }
        [LabelText("托管核心", SdfIconType.Bullseye), NonSerialized, ShowInInspector]
        public Host host = null;
        #endregion

        //启用时逻辑

        #region 关于提交SubMit
        public bool Signal_HasSubmit
        {
            get;
            set;
        }
        #endregion

        #region 检查器显示控制与信息
        [ShowInInspector, LabelText("控制自身启用状态"), PropertyOrder(-1)] public bool EnabledSelfControl { get => EnabledSelf; set { if (value) TryEnableSelf(); else TryDisableSelf(); } }
        [ShowInInspector, LabelText("显示活动状态"), PropertyOrder(-1), GUIColor("@KeyValueMatchingUtility.ColorSelector.ColorForUpdating")]
        public bool IsActiveAndEnableShow { get => Signal_IsActiveAndEnable; }
        [ShowInInspector, LabelText("是否已经Submit"), PropertyOrder(-1)] public bool ShowHasSubmit => Signal_HasSubmit;
        #endregion

        #region 开关逻辑


        public bool EnabledSelf
        {
            get;
            set;
        } = true;

        public override void TryEnableSelf()
        {
            if (EnabledSelf) return;
            EnabledSelf = true;
        }
        public override void TryDisableSelf()
        {
            if (EnabledSelf)
            {
                EnabledSelf = false;
            }
        }
        public override void TryDestroySelf()
        {
            Signal_HasSubmit = false;
            host.TryRemoveFromIEnumableOnly(this);
        }

        public void _TryActiveAndEnable()
        {
            if (Signal_IsActiveAndEnable || !EnabledSelf) return;//不要你
            OnEnable();

        }

        public void _TryInActiveAndDisable()
        {
            if (Signal_IsActiveAndEnable)
            {
                OnDisable();
            }
        }

        public bool _TryStartWithHost(Host host)
        {
            if (host == null || Signal_HasSubmit) return false;
            this.host = host;
            Signal_HasSubmit = true;
            return true;
        }

        public void _SetHost(Host host)
        {
            this.host = host;
        }


        #endregion

        #region 实现隐蔽辅助方法

        #endregion




    }
    [Serializable, TypeRegistryItem("ES托管器＋模块_带委托的", "模块")]
    public class ESHostingAndModule_WithDelegate : BaseESHostingAndModule<BaseESModule, BaseESHosting>
    {
        public override IEnumerable<BaseESModule> ModulesIEnumable => null;
        [FoldoutGroup("默认委托")] private Action<ESHostingAndModule_WithDelegate> Action_Enable;
        [FoldoutGroup("默认委托")] private Action<ESHostingAndModule_WithDelegate> Action_Disable;
        [FoldoutGroup("默认委托")] private Action<ESHostingAndModule_WithDelegate> Action_OnUpdate;

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
        public ESHostingAndModule_WithDelegate WithEnable(Action<ESHostingAndModule_WithDelegate> func)
        {
            Action_Enable = func;
            return this;
        }

        [Tooltip("规定禁用时的事件")]
        public ESHostingAndModule_WithDelegate WithDisable(Action<ESHostingAndModule_WithDelegate> func)
        {
            Action_Disable = func;
            return this;
        }

        [Tooltip("规定帧运行时的事件")]
        public ESHostingAndModule_WithDelegate WithUpdate(Action<ESHostingAndModule_WithDelegate> func)
        {
            Action_OnUpdate = func;
            return this;
        }

        public override void _RemoveModuleFromList(BaseESModule use)
        {
            //无事发生
        }

        public override void TryAddToIEnumableOnly(IESModule module)
        {
            throw new NotImplementedException();
        }

        public override void TryRemoveFromIEnumableOnly(IESModule module)
        {
            throw new NotImplementedException();
        }
    }

    #region 原始托管器
    public abstract class BaseESOringinalHostingAndModule<Host> : BaseESModule<Host>, IESOringinHosting where Host : class, IESOringinHosting
    {
        #region 与自己的Host关联
        public override Host GetHost => host;
        [LabelText("托管核心", SdfIconType.Bullseye), NonSerialized, ShowInInspector]
        public Host host = null;
        public sealed override void _SetHost(Host host)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    #endregion

    //初级托管器--IMoudle

    //完整托管器--IDelegatedUpdatable


    //下发委托托管器---无条件

    //可委托的初级托管器

    //可委托的完备托管器


    //下发更新托管器--无条件

}
