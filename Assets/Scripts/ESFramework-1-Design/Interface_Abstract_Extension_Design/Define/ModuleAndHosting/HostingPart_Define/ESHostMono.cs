using ES;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES
{
//虚拟托管脚本是一个托管器
//他不支持特定类型，可以容纳任意的模块并且控制他们的生命周期
    [TypeRegistryItem("纯虚拟托管脚本")]
    public abstract class ESHostingMono : MonoBehaviour, IESHosting
    {
        #region 虚拟列表
      /*  public SafeUpdateSet_EasyQueue_SeriNot_Dirty<IESModule> VirtualBeHosted => virtualBeHostedList;
        [LabelText("虚拟托管列表"), FoldoutGroup("作为托管器")] public SafeUpdateSet_EasyQueue_SeriNot_Dirty<IESModule> virtualBeHostedList = new SafeUpdateSet_EasyQueue_SeriNot_Dirty<IESModule>();
     */   
      #endregion

        #region 显示控制和信息
        [ShowInInspector, LabelText("控制自身启用状态"), PropertyOrder(-1), FoldoutGroup("作为托管器")] public bool EnabledSelfControl { get => enabled; set { if (value) TryEnableSelf(); else TryDisableSelf(); } }
        [ShowInInspector, LabelText("显示活动状态"), PropertyOrder(-1), FoldoutGroup("作为托管器"), GUIColor("@KeyValueMatchingUtility.ColorSelector.ColorForUpdating")]
        public bool IsActiveAndEnableShow { get => Signal_IsActiveAndEnable; }
        #endregion

        #region 自定义间隔帧更新
       /* private short UpdateIntervalFrameCount = -1;
        private short SelfTargetModel = -1;
        public void ResetUpdateIntervalFrameCount(short interval = 10)
        {
            UpdateIntervalFrameCount = interval;
            if (UpdateIntervalFrameCount > 0)
            {
                SelfTargetModel = (short)UnityEngine.Random.Range(0, UpdateIntervalFrameCount);
            }
        }*/
        #endregion

        #region 控制子模块
        public virtual void UpdateAsHosting()
        {
           /*  
            if (VirtualBeHosted != null)
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

            /* if (NormalBeHosted != null)
             {
                 foreach (var i in NormalBeHosted)
                 {
                     i._TryActiveAndEnable();
                 }
             }*/
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
            /* if (NormalBeHosted != null)
             {
                 foreach (var i in NormalBeHosted)
                 {
                     i._TryInActiveAndDisable();
                 }
             }*/
            /*if (VirtualBeHosted != null)
            {
                foreach (var i in VirtualBeHosted.valuesNow_)
                {
                    i._TryInActiveAndDisable();
                }
            }*/
        }
        #endregion

        #region 生命周期
        public bool Signal_IsActiveAndEnable { get; set; }

        public void TryEnableSelf()
        {
            if (Signal_IsActiveAndEnable) return;
            enabled = true;
            OnEnable();
        }
        public void TryDisableSelf()
        {
            if (Signal_IsActiveAndEnable)
            {
                OnDisable();
                enabled = false;
            }
        }
        public void TryDestroySelf()
        {
            Destroy(gameObject);
        }

        public void TryUpdateSelf()
        {
            if (CanUpdating && Signal_IsActiveAndEnable)
            {
                Update();
            }
        }
        #endregion

        #region 重写逻辑
        public virtual bool CanUpdating => true;

     

        protected virtual void Update()
        {
            /*if (UpdateIntervalFrameCount > 0)
            {
                if (SelfTargetModel < 0) ResetUpdateIntervalFrameCount(UpdateIntervalFrameCount);
                if (Time.frameCount % UpdateIntervalFrameCount != SelfTargetModel)
                {
                    return;
                }
            }*/
            UpdateAsHosting();
            /*virtualBeHostedList.Update();*/
        }
        protected virtual void OnEnable()
        {
           /* if (UpdateIntervalFrameCount > 0 && SelfTargetModel < 0)
            {
                ResetUpdateIntervalFrameCount(UpdateIntervalFrameCount);
            }*/
            Signal_IsActiveAndEnable = true;
            EnableAsHosting();
        }
        protected virtual void OnDisable()
        {
           
#if UNITY_EDITOR
            if (ESEditorRuntimePartMaster_OB.IsQuit) return;
#endif
           
            Signal_IsActiveAndEnable = false;
            DisableAsHosting();
        }

        public abstract void TryAddToIEnumableOnly(IESModule module);
        public abstract void TryRemoveFromIEnumableOnly(IESModule module);

        #endregion

    }

    [TypeRegistryItem("虚拟+带类型的托管脚本基类")]
    public abstract class ESHostingMono<USE_Module> : ESHostingMono, IESHosting<USE_Module> where USE_Module : class, IESModule
    {
        
        public virtual IEnumerable<USE_Module> ModulesIEnumable { get; }
        #region 重写控制子模块
        public override void UpdateAsHosting()
        {
            if (ModulesIEnumable != null)
            {
                foreach (var i in ModulesIEnumable)
                {
                    if (!i.Signal_HasSubmit)
                    {
                        i._TryInActiveAndDisable();
                        //已经放弃
                        _RemoveModuleFromList(i);
                        continue;
                    }
                    i.TryUpdateSelf();
                }
            }
            base.UpdateAsHosting();
        }
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

        public abstract void _RemoveModuleFromList(USE_Module use);

        #endregion
    }
    [TypeRegistryItem("虚拟的可用托管脚本")]
    public class ESHostingMono_BaseESModule : ESHostingMono
    {
        public override void TryAddToIEnumableOnly(IESModule module)
        {
            
        }

        public override void TryRemoveFromIEnumableOnly(IESModule module)
        {
           
        }
    }
}