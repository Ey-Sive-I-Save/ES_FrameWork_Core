using ES;
using ES.MonoTool;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
        void TryRemoveNullModules(bool rightnow = false);

    }
    public interface IDomain<Core_> : IDomain
    {
        //给Module抽象定义用
        Core_ Core { get; }
        public void AddModuleWithoutTypeMatch(IModule Module);
        public void RemoveModuleWithoutTypeMatch(IModule Module);
    }
    [Serializable]
    public abstract class Domain<Core_, Module_> : IESHosting<Module_>, IDomain<Core_> where Core_ : Core where Module_ : class, IModule, IESModule
    {
        #region 总重要信息
#if UNITY_EDITOR //只在编辑器下有用
        [FoldoutGroup("扩展域固有"), LabelText("域功能解释", icon: SdfIconType.Palette), GUIColor("Editor_ColorGetter"), ShowInInspector, PropertyOrder(-100), SerializeField]
        private Tool_ESReadMeClass readMe = new Tool_ESReadMeClass() { readMe = "这是一个扩展区域" };
#endif
        [HideInInspector]
        public Core_ core;

        [FoldoutGroup("扩展域固有"), LabelText("全部模块"), OdinSerialize]
        public SafeLoopList<Module_> Modules = new SafeLoopList<Module_>();

        #endregion


        #region 只读便捷属性

        //模块的IEnumable
        [HideInInspector]
        public IEnumerable<Module_> ModulesIEnumable
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Modules.ValuesNow; }
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
        protected virtual Color Editor_ColorGetter()
        {
            return Color.green;
        }
        public void TryRemoveFromIEnumableOnly(IESModule module)
        {
            if (module is Module_ m)
            {
                _RemoveModuleFromList(m);
            }
        }
        //单纯从列表中移除
        public void _RemoveModuleFromList(Module_ use)
        {
            Modules.TryRemove(use);
        }
        public void TryAddToIEnumableOnly(IESModule module)
        {
            if (module is Module_ m)
            {
                _RemoveModuleFromList(m);
            }
        }
        //尝试移除全部的空模块(一般在编辑器使用)--(立刻清理还是留到)
        public void TryRemoveNullModules(bool rightNow = false)
        {
            foreach (var i in Modules.ValuesNow)
            {
                if (i == null)
                {
                    Modules.TryRemove(i);
                }
            }
            if (rightNow) Modules.ApplyBuffers();
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
            Modules.ApplyBuffersNoForce();
            if (Modules != null)
            {
                for (int i = 0; i < Modules.ValuesNow.Count; i++)
                {
                    var use = Modules.ValuesNow[i];
                    use.TryUpdateSelf();
                }
            }
        }

        //启用子模块
        public virtual void EnableAsHosting()
        {
            if (Modules.ValuesNow != null)
            {
                foreach (var i in Modules.ValuesNow)
                {
                    i._TryActiveAndEnable();
                }
            }
        }

        //禁用子模块
        public virtual void DisableAsHosting()
        {
            if (Modules.ValuesNow != null)
            {
                foreach (var i in Modules.ValuesNow)
                {
                    i._TryInActiveAndDisable();
                }
            }
        }

        public virtual void DestroyAsHosting()
        {

        }
        #endregion

        #region 生命周期(一般不需要调用)

        //正在活动的标识
        public bool Signal_IsActiveAndEnable
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TryDestroySelf()
        {
            OnDestroy();
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

        protected virtual void OnDestroy()
        {
            foreach (var i in Modules.ValuesNow)
            {
                i.TryDestroySelf();//从列表中移除---不过这个Domain都销毁了，无其他意义
            }
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
        public void AddModuleWithoutTypeMatch(IModule Module)
        {
            if (Module is Module_ use && !Modules.ValuesNow.Contains(use))
            {
                Modules.TryAdd(use);
                use.Signal_HasSubmit = true;
            }
        }

        //移除模块
        public void RemoveModuleWithoutTypeMatch(IModule Module)
        {
            if (Module is Module_ use)
            {
                use.Signal_HasSubmit = false;//自己包含移除
            }
        }
        public void AddModule(Module_ module)
        {
            if (module != null)
            {
                if (!Modules.ValuesNow.Contains(module))
                {
                    Modules.TryAdd(module);
                    module._TryStartWithHost(this);
                }
            }
        }

        //移除模块
        public void RemoveModule(Module_ module)
        {
            if (module != null) module.Signal_HasSubmit = false;//自己包含移除
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