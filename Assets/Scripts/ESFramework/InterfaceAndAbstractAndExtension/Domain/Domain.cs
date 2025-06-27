using ES;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;


namespace ES
{
    public interface IDomain : IESHosting
    {
        //编辑器情况下的链接创建
        void RegisterAllButOnlyCreateRelationship(ICore core_);
        void RegisterAllWithCore(ICore core);
        void TryRemoveNullClips();
    }
    public interface IDomain<Core_> : IDomain
    {
        //给Clip抽象定义用
        Core_ Core { get; }
        public void AddClip(IClip clip, bool selfInvoke = true);
        public void RemoveClip(IClip clip, bool selfInvoke = true);
    }
    [DefaultExecutionOrder(-1)]
    public abstract class Domain<Core_, Clip> : ESHostingMono<Clip>, IDomain<Core_> where Core_ : Core where Clip : class, IClip, IESModule
    {
        #region 常规属性字段
        //剪影就是我的常规列表
        public sealed override IEnumerable<Clip> Modules => Clips.valuesNow_;
        public Core_ Core => core;
        [FoldoutGroup("扩展域原始"), LabelText("域功能解释", icon: SdfIconType.Palette), GUIColor("ColorGetter"), ShowInInspector, PropertyOrder(-100)] public ESReadMeClass readMe = new ESReadMeClass() { readMe = "这是一个扩展区域" };
        [FoldoutGroup("扩展域原始"), LabelText("链接的核", icon: SdfIconType.Water), ReadOnly, GUIColor("ColorGetter")] public Core_ core;
        [FoldoutGroup("扩展域原始"), LabelText("全部剪影"), OdinSerialize] public SafeUpdateList_EasyQueue_SeriNot_Dirty<Clip> Clips = new SafeUpdateList_EasyQueue_SeriNot_Dirty<Clip>();
        #endregion


        #region 初始化
        public void RegisterAllWithCore(ICore core)
        {
            if (core is Core_ use)
            {
                this.core = use;
                CreatRelationship();
                AwakeRegisterAllClips();
            }
        }
        //显性引用时
        protected virtual void CreatRelationship()
        {

        }
        //初始化注册时(一般不用改
        public virtual void AwakeRegisterAllClips()
        {
            foreach (var i in Modules)
            {
                i._TryStartWithHost(this);
            }
        }
        #endregion

        protected override void Update()
        {
            if (Core != null)
            {
                Clips.Update();
                base.Update();
            }
        }

        public Action OnFixedUpdate = () => { };
        protected virtual void FixedUpdate()
        {
            if (Core != null)
            {
                OnFixedUpdate?.Invoke();
            }
        }
        #region 辅助方法
        public void RegisterAllButOnlyCreateRelationship(ICore core_)
        {
            if (core_ is Core_ use)
            {
                this.core = use;
                CreatRelationship();
                foreach (var i in Modules)
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
        #endregion


        #region 常用功能
        public void AddClip(IClip clip, bool selfInvoke = true)
        {
            if (clip is Clip use && !Clips.valuesNow_.Contains(use))
            {
                Clips.TryAdd(use);
                if (selfInvoke) clip._SetDomainAndCreateRelationshipOnly(this);
            }
        }

        public void RemoveClip(IClip clip, bool selfInvoke = true)
        {
            if (clip is Clip use)
            {
                use.TryDisableSelf();
                /* Clips.TryRemove(use);*/
                if (selfInvoke) clip._TryStartWithHost(this);
            }
        }
        public override void TryRemoveModule(Clip use)
        {
            Clips.TryRemove(use);
        }
        public void TryRemoveNullClips()
        {
            foreach(var i in Clips.valuesNow_)
            {
                if (i == null)
                {
                    Clips.TryRemove(i);
                }
            }
            Clips.Update();
        }
        public T GetClip<T>()
        {
            foreach (var i in Modules)
            {
                if (i is T t) return t;
            }
            return default;
        }
        #endregion

    }
}