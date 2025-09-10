using DG.Tweening;
using DG.Tweening.Core.Easing;
using ES;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using UnityEditorInternal;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;
using Sirenix.Serialization;
using static UnityEngine.GraphicsBuffer;


namespace ES
{
    using EEBOOPBuffer = OutputOperationBufferFloat_TargetAndDirectInput<Entity, Entity, EntityState_Buff, ITargetOperationFloatEEB>;

    [Serializable, TypeRegistryItem("实体标准状态机")]
    public class EntityStateMachine : ESStandardStateMachine_StringKey
    {
        [HideInInspector] public Entity entity;
        [HideInInspector, NonSerialized] public StateMachineDomainForEntity StateDomain;
        [LabelText("技能子状态机")] public ESStandardStateMachine_StringKey SkillMachine = new ESStandardStateMachine_StringKey();
        [LabelText("Buff子状态机")] public ESStandardStateMachine_StringKey BuffMachine = new ESStandardStateMachine_StringKey();
        [LabelText("交互子状态机")] public ESStandardStateMachine_StringKey InteractionMachine = new ESStandardStateMachine_StringKey();


        public bool TryActiveSkill(string name)
        {
            return SkillMachine.TryActiveStateByKey(name);
        }
        public bool TryActiveBuff(string name)
        {
            return BuffMachine.TryActiveStateByKey(name);
        }
        public bool TryActiveInteraction(IESNanoState state)
        {
            return InteractionMachine.TryActiveState(state);
        }
        public bool TryActiveSkill(IESNanoState state)
        {
            return SkillMachine.TryActiveState(state);
        }
        public bool TryActiveBuff(IESNanoState state)
        {
            return BuffMachine.TryActiveState(state);
        }
        public bool TryActiveInteraction(string name)
        {
            return InteractionMachine.TryActiveStateByKey(name);
        }
        public void CreateRelationShip(Entity e, StateMachineDomainForEntity stateMachineDomain)
        {
            if (e != null)
            {
                entity = e;
                StateDomain = stateMachineDomain;
                defaultStateKey = "静止";
                this.RegisterNewState("技能", SkillMachine);
                this.RegisterNewState("Buff", BuffMachine);
                this.RegisterNewState("交互", InteractionMachine);

                //
            }
        }
        protected override void Update()
        {
            base.Update();

        }
        protected override void OnEnable()
        {
            base.OnEnable();

        }
        protected override void OnDisable()
        {
            base.OnDisable();

        }
        protected override void OnStateEnter()
        {
            base.OnStateEnter();

        }
        public override void OnStateExit()
        {
            base.OnStateExit();

        }
    }

    [Serializable, TypeRegistryItem("实体标准常见状态")]
    public class EntityState : BaseESStandardStateRunTimeLogic_StringKey
    {
        [NonSerialized]
        public EntityStateMachine TheEntityStateMachine;
        [NonSerialized] public Entity Entity;
        protected override void RunStatePreparedLogic()
        {
            TheEntityStateMachine = host as EntityStateMachine;
            Entity = TheEntityStateMachine?.StateDomain.core;
            Debug.Log("Statemachine" + TheEntityStateMachine);
            Debug.Log("Statemachine2" + TheEntityStateMachine.StateDomain);
            Debug.Log("Statemachine3" + TheEntityStateMachine.StateDomain.core);
            base.RunStatePreparedLogic();

        }
    }
    /* [Serializable, TypeRegistryItem("实体标准技能状态")]
     public class EntityState_Skill : EntityState
     {
         #region 技能数据
         [NonSerialized]
         public ReleasableSkillsSequence Sequence;
         [FoldoutGroup("技能专属")][LabelText("技能片段序列")] public Queue<ReleaseableSkillModule> SkillModules;
         [FoldoutGroup("技能专属")][LabelText("上一个片段获得的实体列表")] public List<Entity> LastModuleSelectorEntites = new List<Entity>();
         [FoldoutGroup("技能专属")][LabelText("自己的缓冲实体")] public HashSet<Entity> SelfCacheEntites = new HashSet<Entity>();
         [FoldoutGroup("技能专属")][LabelText("自己的缓冲坐标"), ShowInInspector] public HashSet<Vector3> SelfCacheVector3 = new HashSet<Vector3>();
         [FoldoutGroup("技能专属")][LabelText("自己的缓冲模块")] public List<ESModule_WithDelegate> SelfModule = new List<ESModule_WithDelegate>();
         [FoldoutGroup("技能专属")][LabelText("退出委托")] public Action<float> OnExit = (f) => { };//float--已经开始的时间
         [FoldoutGroup("技能专属")][LabelText("运行时委托")] public Action<float> OnUpdate = (progress) => { };
         #endregion

         public void Setup(ReleasableSkillsSequence releasable)
         {
             Sequence = releasable;
         }
         //进入逻辑
         protected override void RunStatePreparedLogic()
         {
             TheEntityStateMachine = host as EntityStateMachine;
             base.RunStatePreparedLogic();
             variableData.hasEnterTime = 0;
             if (Sequence == null) return;
             //开始准备
             Debug.Log("进入");
             OnExit = (f) => { };
             PrivateMethod_ClearCache();
             PrivateMethod_PrepareSkillModules();

         }
         //更新逻辑
         protected override void RunStateUpdateLogic()
         {
             base.RunStateUpdateLogic();
             variableData.hasEnterTime += Time.deltaTime;
             if (SkillModules != null)
             {
                 if (SkillModules.Count != 0)
                 {

                     var next = SkillModules.Peek();

                     if (next == null) SkillModules.Dequeue();

                     if (variableData.hasEnterTime > next.triggerAtTime)
                     {
                         next = SkillModules.Dequeue();//出列！

                         PrivateMethod_TriggerSkillModule(next);
                     }
                 }

             }
             OnUpdate?.Invoke(variableData.hasEnterTime / Sequence.skillDuration);
             if (variableData.hasEnterTime >= Sequence.skillDuration) OnStateExit();//截止了

         }
         //退出逻辑
         protected override void RunStateExitLogic()
         {
             base.RunStateExitLogic();
             PrivateMethod_ClearCache();
             OnExit?.Invoke(variableData.hasEnterTime);
         }


         #region 私有方法
         //-------准备切片
         private void PrivateMethod_PrepareSkillModules()
         {
             //
             Debug.Log("Heelpw" + Sequence.AllModules.Count);
             SkillModules = new Queue<ReleaseableSkillModule>();
             foreach (var i in Sequence.AllModules)
             {
                 SkillModules.Enqueue(i);
             }
             LastModuleSelectorEntites = new List<Entity>();//初始化
         }
         //清理缓存
         private void PrivateMethod_ClearCache()
         {
             SelfCacheEntites.Clear();
             SelfCacheVector3.Clear();
             foreach (var i in SelfModule)
             {
                 if (i != null)
                 {
                     i.TryDestroySelf();
                 }
             }
             SelfModule.Clear();
         }
         //使用切片
         private void PrivateMethod_TriggerSkillModule(ReleaseableSkillModule Module)
         {
             //动画器相关
             if (Module.useStateSwitch)
                 Entity.Anim?.CrossFade(Module.st, Module.crossFade?.Pick() ?? 0.2f, Module.layer);
             //开始筛选
             List<Entity> MyEntites = new List<Entity>();
             var overrideOption = Module.optionForOverrideLast;
             //--------直接使用上次的
             if (overrideOption == ReleaseableSkillModule.SelectorOverrideOptions.DirectUse && LastModuleSelectorEntites.Count > 0)
             {
                 MyEntites = LastModuleSelectorEntites;
             }
             //--------跳过头部，再次筛选
             else if (overrideOption == ReleaseableSkillModule.SelectorOverrideOptions.IgnoreHeadAndReSelect && LastModuleSelectorEntites.Count > 0)
             {
                 if (Module.Selector is SomeEntitySelectorFromSelf chainSelector)
                 {
                     MyEntites = chainSelector.PickAfterHead(LastModuleSelectorEntites, Entity);
                 }
             }
             //--------完全更新
             else
             {
                 if (Module.Selector is SomeEntitySelectorFromSelf chainSelector)
                 {

                     MyEntites = chainSelector.Pick(Entity, Entity, this);
                 }
             }
             if (Module.sortType == EnumCollect.PathSortType.NoneSort)
             {

             }
             else
             {
                 MyEntites = MyEntites.Where((w) => w != null).ToList();
                 MyEntites = ESStaticUtility.Sorter.SortAny(MyEntites, (f) => f.transform.position, Module.sortType, Entity.transform.position, Entity.transform);
             }


             if (!Module.UseTimeDis)
             {
                 //造成效果
                 foreach (var e in MyEntites)
                 {
                     if (e == null) continue;

                     foreach (var handle in Module.Applier.handles)
                     {
                         *//* Debug.Log(MyEntites.Count + "/" + Module.name + "/" + Module.Applier.handles_.Count + "/" + handle);*//*
                         handle.Pick(e, Entity, this);
                     }
                 }
             }
             else
             {

                 var se = DOTween.Sequence();
                 foreach (var e in MyEntites)
                 {
                     if (e == null) continue;
                     se.AppendCallback(() =>
                     {
                         if (e != null && Entity != null)
                             foreach (var handle in Module.Applier.handles)
                             {

                                 handle.Pick(e, Entity, this);
                             }
                     });
                     se.AppendInterval(Mathf.Max(Module.TriggerTimeDis_?.Pick() ?? 0.2f, 0.2f));
                 }
                 OnExit += (f) => { se.Kill(); };
             }

             //应用到下次
             var nextOption = Module.optionForNext;
             //-------更新
             if (nextOption == ReleaseableSkillModule.SelectorNextOptions.UpdateAll)
             {
                 LastModuleSelectorEntites = MyEntites;
             }
             //------不关心
             else if (nextOption == ReleaseableSkillModule.SelectorNextOptions.DontEffectNext)
             {
                 //不影响
                 MyEntites.Clear();
             }
             //----------------添加到
             else if (nextOption == ReleaseableSkillModule.SelectorNextOptions.AddTo)
             {
                 LastModuleSelectorEntites.AddRange(MyEntites);
                 MyEntites.Clear();
             }
             //-------------从中移除
             else if (nextOption == ReleaseableSkillModule.SelectorNextOptions.RemoveFrom)
             {
                 foreach (var i in MyEntites)
                 {
                     LastModuleSelectorEntites.Remove(i);
                 }
                 MyEntites.Clear();
             }
             //--------------全部清除
             else if (nextOption == ReleaseableSkillModule.SelectorNextOptions.ClearAll)
             {
                 LastModuleSelectorEntites.Clear();
                 MyEntites.Clear();
             }
         }
         #endregion
     }*/

    [Serializable, TypeRegistryItem("实体标准Buff状态")]
    public class EntityState_Buff : EntityState,
        ICacheKeyValueForOutputOpeation<IOperation, DeleAndCount, OutputOpeationDelegateFlag>,
         ICacheSafeKeyGroupForOutputOpeation<EEBOOPBuffer, BufferOperationFloat, OutputOpeationBufferFlag>,
          ICacheKeyValueForOutputOpeation<IOperation, ISettleOperation, OutputOpeationSettleFlag>

    {
        #region 常规
        [LabelText("Buff共享数据")] public ESBuffSharedData buffSharedData;
        [LabelText("Buff变量数据")] public ESBuffVariableData buffVariableData = new ESBuffVariableData();
        public Entity from;
        //这个Buff效果要用的
        private float triggerTimer = 0;
        public float Level => buffVariableData.level;
        #endregion
        protected override void RunStateEnterLogic()
        {
            //初始化时间
            triggerTimer = buffSharedData.triggerTimeStart;
            if (buffVariableData.timeRemain < 0)
            {
                buffVariableData.timeRemain = 10;
            }

            //启用操作
            if (buffSharedData.EnableOnOffTrigger)
            {
                buffSharedData.opeationForOnOff.TryOperation(Entity, from, this);
            }
            base.RunStateEnterLogic();
        }
        protected override void RunStateExitLogic()
        {  //清理缓冲
            if (buffSharedData.EnableBuffer)
            {
                var keys = CahceBuffer.Groups.Keys.ToArray();
                foreach (var i in keys)
                {
                    var group = CahceBuffer.GetGroup(i);
                    foreach (var buffer in group)
                    {
                        Debug.Log(Entity.VariableData.Health);
                        i.TryStopTheBuffer(Entity, from, this, buffer);
                    }
                }
            }
            //禁用操作
            if (buffSharedData.EnableOnOffTrigger)
            {
                buffSharedData.opeationForOnOff.TryCancel(Entity, from, this);
            }

            //清除缓存
            CacheDele.Clear();
            CacheSettle.Clear();
            CahceBuffer.Clear();
            base.RunStateExitLogic();
        }

        protected override void RunStateUpdateLogic()
        {
            base.RunStateUpdateLogic();
            //自动退出
            buffVariableData.timeRemain -= Time.deltaTime;
            if (buffVariableData.timeRemain < 0)
            {
                OnStateExit();//退出状态 
            }
            //按时间执行操作
            if (buffSharedData.EnableTimeDisTrigger)
            {
                triggerTimer -= Time.deltaTime;
                if (triggerTimer < 0)
                {
                    triggerTimer = buffSharedData.triggerTimeDis;
                    //执行
                    buffSharedData.opeationForTimeDis.TryOperation(Entity, from, this);
                }
            }
            //缓冲支持更迭
            if (buffSharedData.EnableBuffer)
            {
                CahceBuffer.TryApplyBuffers();
                var keys = CahceBuffer.Groups.Keys.ToArray();
                foreach (var i in keys)
                {
                    var group = CahceBuffer.GetGroup(i);
                    foreach (var buffer in group)
                    {
                        i.TryUpdateTheBuffer(Entity, from, this, buffer);
                    }
                }
            }
        }
        [ShowInInspector]
        public Dictionary<IOperation, DeleAndCount> CacheDele = new Dictionary<IOperation, DeleAndCount>();
        public Dictionary<IOperation, DeleAndCount> GetCache(OutputOpeationDelegateFlag flag = null)
        {
            return CacheDele;
        }
        [ShowInInspector]
        public SafeKeyGroup<EEBOOPBuffer, BufferOperationFloat> CahceBuffer = new SafeKeyGroup<EEBOOPBuffer, BufferOperationFloat>();

        public SafeKeyGroup<EEBOOPBuffer, BufferOperationFloat> GetCache(OutputOpeationBufferFlag flag = null)
        {
            return CahceBuffer;
        }
        [ShowInInspector]
        public Dictionary<IOperation, ISettleOperation> CacheSettle = new Dictionary<IOperation, ISettleOperation>();
        public Dictionary<IOperation, ISettleOperation> GetCache(OutputOpeationSettleFlag flag = null)
        {
            return CacheSettle;
        }
    }

    [Serializable, TypeRegistryItem("实体移动状态")]
    public class EntityState_Move : EntityState
    {
        private ModuleBase_3DStandardMotion motion;
        private float HasIn = 0;
        protected override void RunStatePreparedLogic()
        {
            base.RunStatePreparedLogic();
          
            HasIn = 0;
        }
        protected override void RunStateUpdateLogic()
        {
            base.RunStateUpdateLogic();
            HasIn += Time.deltaTime;
            if (HasIn > 0.1f)
            {
                if (Mathf.Abs(motion.CurrentSpeedMutiplerZ) < 0.05 && Mathf.Abs(motion.CurrentSpeedMutiplerX) < 0.05)
                {
                    OnStateExit();
                }
            }
        }
        protected override void RunStateExitLogic()
        {
            base.RunStateExitLogic();
            motion.Set_TargetVX(0, null);
            motion.Set_TargetVZ(0, null);

        }
    }

    [Serializable, TypeRegistryItem("实体闪身状态")]
    public class EntityState_CrashDodge : EntityState
    {

    }
}


