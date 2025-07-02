using ES;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;


namespace ES
{
    public interface IDomain : IESHosting
    {
        //编辑器情况下的链接创建
        void RegisterAllButOnlyCreateRelationship(ICore core_);
        void RegisterAllWithCore(ICore core);
        void TryRemoveNullModules(bool rightnow=false);
    }
    public interface IDomain<Core_> : IDomain
    {
        //给Module抽象定义用
        Core_ Core { get; }
        public void AddModule(IModule Module, bool selfInvoke = true);
        public void RemoveModule(IModule Module, bool selfInvoke = true);
    }
    [Serializable]
    public abstract class Domain<Core_, Module> : IESHosting<Module>, IDomain<Core_> where Core_ : Core where Module : class, IModule, IESModule
    {
        #region 总重要信息
#if UNITY_EDITOR //只在编辑器下有用
        [FoldoutGroup("扩展域固有"), LabelText("域功能解释", icon: SdfIconType.Palette), GUIColor("ColorGetter"), ShowInInspector, PropertyOrder(-100),SerializeField]
        private ESReadMeClass readMe = new ESReadMeClass() { readMe = "这是一个扩展区域" };
#endif
        [HideInInspector]
        public Core_ core;

        [FoldoutGroup("扩展域固有"), LabelText("全部模块"), OdinSerialize]
        public SafeUpdateList_EasyQueue_SeriNot_Dirty<Module> Modules = new SafeUpdateList_EasyQueue_SeriNot_Dirty<Module>();

        #endregion

        #region 只读便捷属性

        //模块的IEnumable
        [HideInInspector]
        public IEnumerable<Module> ModulesIEnumable
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Modules.valuesNow_; }
        }
        [HideInInspector]
        //链接的核心
        public Core_ Core
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => core;
        }
        #endregion

        #region 补充逻辑

        //单纯绑引用（编辑器模式有用）
        public void RegisterAllButOnlyCreateRelationship(ICore core_)
        {
            if (core_ is Core_ use)
            {
                this.core = use;
                foreach (var i in ModulesIEnumable)
                {
                    i._SetDomainAndCreateRelationshipOnly(this);
                }
            }
        }

        //设置显示绑定核心的颜色
        protected virtual Color ColorGetter()
        {
            return Color.green;
        }

        //单纯从列表中移除
        protected void _RemoveModuleFromList(Module use)
        {
            Modules.TryRemove(use);
        }

        //尝试移除全部的空模块(一般在编辑器使用)--(立刻清理还是留到)
        public void TryRemoveNullModules(bool rightNow = false)
        {
            foreach (var i in Modules.valuesNow_)
            {
                if (i == null)
                {
                    Modules.TryRemove(i);
                }
            }
            if (rightNow) Modules.Update();
        }

        #endregion

        #region 初始化

        //注册到核心
        public void RegisterAllWithCore(ICore core)
        {
            if (core is Core_ use)
            {
                this.core = use;
                AwakeRegisterAllModules();
            }
        }

        //注册默认模块(一般不用改
        public virtual void AwakeRegisterAllModules()
        {
            foreach (var i in ModulesIEnumable)
            {
                i._TryStartWithHost(this);
            }
        }

        #endregion

        #region 控制子模块(非必要不重写)

        //更新子模块
        public virtual void UpdateAsHosting()
        {
            Modules.UpdateNoForce();
            if (Modules != null)
            {
                for (int i = 0; i < Modules.valuesNow_.Count; i++)
                {
                    var use = Modules.valuesNow_[i];
                    if (!use._HasSubmit)
                    {
                        use._TryInActiveAndDisable();
                        //已经放弃
                        _RemoveModuleFromList(use);
                        continue;
                    }
                    use.TryUpdateSelf();
                }
            }
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

        //启用子模块
        public virtual void EnableAsHosting()
        {
            if (ModulesIEnumable != null)
            {
                foreach (var i in ModulesIEnumable)
                {
                    i._TryActiveAndEnable();

                }
            }
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

        //禁用子模块
        public virtual void DisableAsHosting()
        {
            if (ModulesIEnumable != null)
            {
                foreach (var i in ModulesIEnumable)
                {
                    i._TryInActiveAndDisable();

                }
            }
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

        #region 生命周期(一般不需要调用)

        //正在活动的标识
        public bool IsActiveAndEnable
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => core.EX_IsNotNullAndUseIt()?.enabled ?? false;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { if (core != null) core.enabled = value; }
        }

        //尝试启用
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TryEnableSelf()
        {
            OnEnable();
        }

        //尝试禁用
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TryDisableSelf()
        {
            OnDisable();
        }

        //尝试更新
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TryUpdateSelf()
        {
            if (CanUpdating)
            {
                Update();
            }
        }

        #endregion

        #region 重写逻辑

        //是否可Update更新
        public virtual bool CanUpdating => true;
        //Start时
        protected virtual void Start() { }

        //Update时
        protected virtual void Update()
        {
            UpdateAsHosting();
        }

        //启用时
        protected virtual void OnEnable()
        {
            /* if (UpdateIntervalFrameCount > 0 && SelfTargetModel < 0)
             {
                 ResetUpdateIntervalFrameCount(UpdateIntervalFrameCount);
             }*/
            EnableAsHosting();
        }

        //禁用时
        protected virtual void OnDisable()
        {
#if UNITY_EDITOR
            if (ESEditorRuntimePartMaster.IsQuit) return;
#endif
            DisableAsHosting();
        }
        #endregion

        #region 手动补充逻辑

        //为了节约性能，采用委托
        public Action OnFixedUpdate = () => { };

        protected virtual void FixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }
        #endregion

        #region 常用功能

        //添加模块
        public void AddModule(IModule Module, bool selfInvoke = true)
        {
            if (Module is Module use && !Modules.valuesNow_.Contains(use))
            {
                Modules.TryAdd(use);
                if (selfInvoke) Module._SetDomainAndCreateRelationshipOnly(this);
            }
        }

        //移除模块
        public void RemoveModule(IModule Module, bool selfInvoke = true)
        {
            if (Module is Module use)
            {
                use.TryDisableSelf();
                /* Modules.TryRemove(use);*/
                if (selfInvoke) Module._TryStartWithHost(this);
            }
        }
        
        public T GetModule<T>()
        {
            foreach (var i in ModulesIEnumable)
            {
                if (i is T t) return t;
            }
            return default;
        }
        #endregion

    }
}