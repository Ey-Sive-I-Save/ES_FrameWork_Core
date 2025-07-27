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
using static ES.ModuleStateMachine_CrashDodge;
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
    [Serializable, TypeRegistryItem("实体标准技能状态")]
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
                MyEntites = KeyValueMatchingUtility.Sorter.SortAny(MyEntites, (f) => f.transform.position, Module.sortType, Entity.transform.position, Entity.transform);
            }


            if (!Module.UseTimeDis)
            {
                //造成效果
                foreach (var e in MyEntites)
                {
                    if (e == null) continue;

                    foreach (var handle in Module.Applier.handles)
                    {
                        /* Debug.Log(MyEntites.Count + "/" + Module.name + "/" + Module.Applier.handles_.Count + "/" + handle);*/
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
    }

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
                buffSharedData.opeationForOnOff.TryOpeation(Entity, from, this);
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
                    buffSharedData.opeationForTimeDis.TryOpeation(Entity, from, this);
                }
            }
            //缓冲支持更迭
            if (buffSharedData.EnableBuffer)
            {
                CahceBuffer.TryApplyBuffers();
                var keys = CahceBuffer.Groups.Keys.ToArray();
                foreach (var i in keys)
                {
                    var group= CahceBuffer.GetGroup(i);
                    foreach(var buffer in group)
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
            motion = Entity.NormalDomain.Module_3DMotion;
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
        private ModuleBase_AB_3DMotion motion;
        [NonSerialized] public ModuleStateMachine_CrashDodge crashDodge;
        [NonSerialized] public Applyable_CrashDodge data;
        private float HasIn = 0;
        private Vector3 startPos;
        private Quaternion startDirec;
        private Vector3 applyVector;
        private Tween withTween = null;
        private bool useRIGID = false;
        public void Setup(ModuleStateMachine_CrashDodge _CrashDodge)
        {
            crashDodge = _CrashDodge;
        }
        public void SetData(ref Applyable_CrashDodge data_)
        {
            data = data_;
        }
        protected override void RunStatePreparedLogic()
        {
            base.RunStatePreparedLogic();
            motion = Entity.NormalDomain.Module_3DMotion;
            HasIn = 0;
            motion = Entity.NormalDomain.Module_AB_Motion;
            useRIGID = motion?.BaseOnRigid ?? false;
            startPos = Entity.transform.position;
            startDirec = Entity.transform.rotation;
            applyVector = Entity.transform.TransformDirection(data.vector);
            data.duration = Mathf.Max(0.1f, data.duration);
            if (data.baseOn == EnumCollect.ToDestionationBaseOn.DotweenDoMove)
            {
                if (data.pathType == EnumCollect.ToDestinationPathType.Direct)
                {
                    if (data.vectorHandle == EnumCollect.TransformHandleSpaceWithTarget.LerpLookAtSpace)
                    {
                        if (useRIGID) withTween = Entity.Rigid.DOMove(data.vector, data.duration);
                        else withTween = Entity.transform.DOMove(data.vector, data.duration);
                    }
                    else if (data.vectorHandle == EnumCollect.TransformHandleSpaceWithTarget.WorldSpace)
                    {
                        if (useRIGID) withTween = Entity.Rigid.DOMove(data.vector, data.duration).SetRelative();
                        else withTween = Entity.transform.DOMove(data.vector, data.duration).SetRelative();
                    }
                    else if (data.vectorHandle == EnumCollect.TransformHandleSpaceWithTarget.SelfSpace)
                    {
                        if (useRIGID) withTween = Entity.Rigid.DOMove(applyVector, data.duration).SetRelative();
                        else withTween = Entity.transform.DOMove(applyVector, data.duration).SetRelative();
                    }
                }
                else if (data.pathType == EnumCollect.ToDestinationPathType.JumpAndDown)
                {
                    if (data.vectorHandle == EnumCollect.TransformHandleSpaceWithTarget.LerpLookAtSpace)
                    {
                        if (useRIGID) withTween = Entity.Rigid.DOMove(data.vector, data.duration);
                        else withTween = Entity.transform.DOMove(data.vector, data.duration);
                    }
                    else if (data.vectorHandle == EnumCollect.TransformHandleSpaceWithTarget.WorldSpace)
                    {
                        if (useRIGID) withTween = Entity.Rigid.DOMove(data.vector, data.duration).SetRelative();
                        else withTween = Entity.transform.DOMove(data.vector, data.duration).SetRelative();
                    }
                    else if (data.vectorHandle == EnumCollect.TransformHandleSpaceWithTarget.SelfSpace)
                    {
                        if (useRIGID) withTween = Entity.Rigid.DOMove(Entity.transform.TransformDirection(data.vector), data.duration);
                        else withTween = Entity.transform.DOMove(Entity.transform.TransformDirection(data.vector), data.duration);
                    }
                }
                else if (data.pathType == EnumCollect.ToDestinationPathType.Rad)
                {
                    if (data.vectorHandle == EnumCollect.TransformHandleSpaceWithTarget.LerpLookAtSpace)
                    {
                        if (useRIGID) withTween = Entity.Rigid.DOMove(data.vector, data.duration);
                        else withTween = Entity.transform.DOMove(data.vector, data.duration);
                    }
                    else if (data.vectorHandle == EnumCollect.TransformHandleSpaceWithTarget.WorldSpace)
                    {
                        if (useRIGID) withTween = Entity.Rigid.DOMove(data.vector, data.duration).SetRelative();
                        else withTween = Entity.transform.DOMove(data.vector, data.duration).SetRelative();
                    }
                    else if (data.vectorHandle == EnumCollect.TransformHandleSpaceWithTarget.SelfSpace)
                    {
                        if (useRIGID) withTween = Entity.Rigid.DOMove(Entity.transform.TransformDirection(data.vector), data.duration);
                        else withTween = Entity.transform.DOMove(Entity.transform.TransformDirection(data.vector), data.duration);
                    }
                }
                else if (data.pathType == EnumCollect.ToDestinationPathType.AIPath)
                {
                    if (data.vectorHandle == EnumCollect.TransformHandleSpaceWithTarget.LerpLookAtSpace)
                    {
                        if (useRIGID) withTween = Entity.Rigid.DOMove(data.vector, data.duration);
                        else withTween = Entity.transform.DOMove(data.vector, data.duration);
                    }
                    else if (data.vectorHandle == EnumCollect.TransformHandleSpaceWithTarget.WorldSpace)
                    {
                        if (useRIGID) withTween = Entity.Rigid.DOMove(data.vector, data.duration).SetRelative();
                        else withTween = Entity.transform.DOMove(data.vector, data.duration).SetRelative();
                    }
                    else if (data.vectorHandle == EnumCollect.TransformHandleSpaceWithTarget.SelfSpace)
                    {
                        if (useRIGID) withTween = Entity.Rigid.DOMove(Entity.transform.TransformDirection(data.vector), data.duration);
                        else withTween = Entity.transform.DOMove(Entity.transform.TransformDirection(data.vector), data.duration);
                    }
                }
            }
        }
        protected override void RunStateUpdateLogic()
        {
            base.RunStateUpdateLogic();
            //超时
            HasIn += Time.deltaTime;
            if (HasIn > data.duration || HasIn > 1)
            {
                OnStateExit();
            }
            if (data.baseOn == EnumCollect.ToDestionationBaseOn.ESCurve)
            {
                if (data.pathType == EnumCollect.ToDestinationPathType.Direct)
                {
                    if (data.vectorHandle == EnumCollect.TransformHandleSpaceWithTarget.LerpLookAtSpace)
                    {
                        if (useRIGID) Entity.Rigid.position = Vector3.MoveTowards(Entity.Rigid.position, data.vector, crashDodge.MaxSpeed * Time.deltaTime);
                        else
                        {
                            Vector3 vv = data.vector - Entity.Rigid.position;
                            Entity.NormalDomain.Module_AB_Motion.Move((vv).normalized * crashDodge.MaxSpeed * Time.deltaTime);
                            Entity.YV = vv.y * (1 - HasIn / data.duration);
                        }
                        if (Vector3.Distance(Entity.Rigid.position, data.vector) < crashDodge.EndDisSuit)
                        {
                            OnStateExit();
                        }
                    }
                    else if (data.vectorHandle == EnumCollect.TransformHandleSpaceWithTarget.WorldSpace)
                    {
                        if (useRIGID) Entity.Rigid.position += data.vector * Time.deltaTime / data.duration;
                        else
                        {
                            Entity.NormalDomain.Module_AB_Motion.Move((data.vector) * Time.deltaTime / data.duration);
                            Entity.YV = applyVector.y * (1 - HasIn / data.duration);
                        }
                    }
                    else if (data.vectorHandle == EnumCollect.TransformHandleSpaceWithTarget.SelfSpace)
                    {
                        if (useRIGID) Entity.Rigid.position += (applyVector) * Time.deltaTime / data.duration;
                        else/* Entity.transform.position += (applyVector) * Time.deltaTime / data.duration;*/
                        {
                            Entity.NormalDomain.Module_AB_Motion.Move((applyVector) * Time.deltaTime / data.duration);
                            Entity.YV = applyVector.y * (1 - HasIn / data.duration);
                        }
                    }
                }
                else if (data.pathType == EnumCollect.ToDestinationPathType.JumpAndDown)
                {
                    if (data.vectorHandle == EnumCollect.TransformHandleSpaceWithTarget.LerpLookAtSpace)
                    {
                        if (useRIGID) withTween = Entity.Rigid.DOMove(data.vector, data.duration);
                        else withTween = Entity.transform.DOMove(data.vector, data.duration);
                    }
                    else if (data.vectorHandle == EnumCollect.TransformHandleSpaceWithTarget.WorldSpace)
                    {
                        if (useRIGID) withTween = Entity.Rigid.DOMove(data.vector, data.duration).SetRelative();
                        else withTween = Entity.transform.DOMove(data.vector, data.duration).SetRelative();
                    }
                    else if (data.vectorHandle == EnumCollect.TransformHandleSpaceWithTarget.SelfSpace)
                    {
                        if (useRIGID) withTween = Entity.Rigid.DOMove(Entity.transform.TransformDirection(data.vector), data.duration);
                        else withTween = Entity.transform.DOMove(Entity.transform.TransformDirection(data.vector), data.duration);
                    }
                }
                else if (data.pathType == EnumCollect.ToDestinationPathType.Rad)
                {
                    if (data.vectorHandle == EnumCollect.TransformHandleSpaceWithTarget.LerpLookAtSpace)
                    {
                        if (useRIGID) withTween = Entity.Rigid.DOMove(data.vector, data.duration);
                    }
                    else if (data.vectorHandle == EnumCollect.TransformHandleSpaceWithTarget.WorldSpace)
                    {
                        if (useRIGID) withTween = Entity.Rigid.DOMove(data.vector, data.duration).SetRelative();
                    }
                    else if (data.vectorHandle == EnumCollect.TransformHandleSpaceWithTarget.SelfSpace)
                    {
                        if (useRIGID) withTween = Entity.Rigid.DOMove(Entity.transform.TransformDirection(data.vector), data.duration);
                    }
                }
                else if (data.pathType == EnumCollect.ToDestinationPathType.AIPath)
                {
                    if (data.vectorHandle == EnumCollect.TransformHandleSpaceWithTarget.LerpLookAtSpace)
                    {
                        if (useRIGID) withTween = Entity.Rigid.DOMove(data.vector, data.duration);
                    }
                    else if (data.vectorHandle == EnumCollect.TransformHandleSpaceWithTarget.WorldSpace)
                    {
                        if (useRIGID) withTween = Entity.Rigid.DOMove(data.vector, data.duration).SetRelative();
                    }
                    else if (data.vectorHandle == EnumCollect.TransformHandleSpaceWithTarget.SelfSpace)
                    {
                        if (useRIGID) withTween = Entity.Rigid.DOMove(Entity.transform.TransformDirection(data.vector), data.duration);
                    }
                }
            }

        }

        protected override void RunStateExitLogic()
        {
            base.RunStateExitLogic();
            if (withTween != null)
            {
                withTween.Kill();
            }
            crashDodge.CoolDownNext = Mathf.Min(data.CoolDownNext, crashDodge.CoolDownNext);
        }
    }

    #region AI状态
    [Serializable, TypeRegistryItem("实体AI游荡状态")]
    public class EntityState_AIPatrol : EntityState
    {
        public float nextMaxTargetTimeCount = 3;
        private float nextHasTargetPosTimeCount = 2;
        public float nextSeeTargetTimeCount = 0.5f;
        private float nextHasSeeTargetTimeCount = 2;
        [NonSerialized] public ModuleAI_AB_Target ReferModule_Target;
        [NonSerialized] public ModuleBase_AB_Vision Refer_Vision;
        public float procedurallyNormalR = 10;
        public EnumCollect.EntityMotionTargetPosSelect targetSelectType = EnumCollect.EntityMotionTargetPosSelect.ProcedurallyWaypoints;
        [NonSerialized] public ESEntitySharedData ESEntityShared;

        protected override void RunStatePreparedLogic()
        {
            base.RunStatePreparedLogic();
            /* ReferModule_Target = Entity.AIDomain.Module_AB_AITarget;
             Refer_Vision = Entity.NormalDomain.Module_AB_Vision;
             if (ReferModule_Target == null || Refer_Vision == null) { OnStateExit(); }
             ESEntityShared = Entity.SharedData ?? new ESEntitySharedData();
             Entity.NormalDomain.Module_AB_Motion.StandardSpeed.y = ESEntityShared.PatrolSpeed;*/
        }
        protected override void RunStateUpdateLogic()
        {
            base.RunStateUpdateLogic();
            /* if (ReferModule_Target.Target != null)
             {
                 TheEntityStateMachine.TryActiveStateByKey("追击状态");
             }
             nextHasTargetPosTimeCount -= Time.deltaTime;
             nextHasSeeTargetTimeCount -= Time.deltaTime;
             if (nextHasSeeTargetTimeCount < 0)
             {
                 nextHasSeeTargetTimeCount = nextSeeTargetTimeCount;
                 Refer_Vision.TrySee();
                 Refer_Vision.MakeSeeAsTarget();
             }
             if (nextHasTargetPosTimeCount < 0)
             {
                 nextHasTargetPosTimeCount = UnityEngine.Random.Range(nextMaxTargetTimeCount / 2, nextMaxTargetTimeCount);
                 if (targetSelectType == EnumCollect.TargetSelectType.Numerically)
                 {

                 }
                 else if (targetSelectType == EnumCollect.TargetSelectType.Random)
                 {

                 }
                 else if (targetSelectType == EnumCollect.TargetSelectType.ProcedurallyWaypoints)
                 {
                     ReferModule_Target.nextWayPointPosition = RandomNavmeshLocation(procedurallyNormalR);
                 }
             }
             if (targetSelectType == EnumCollect.TargetSelectType.PlayerTarget)
             {

                 ReferModule_Target.nextWayPointPosition = GameCenterManager.Instance.transform.parent.position;
             }*/
        }
        public Vector3 RandomNavmeshLocation(float radius)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * radius;
            randomDirection += Entity.transform.position;

            NavMeshHit hit;
            Vector3 finalPosition = Vector3.zero;


            if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
            {
                finalPosition = hit.position;
            }
            return finalPosition;
        }
    }
    [Serializable, TypeRegistryItem("实体AI追击状态")]
    public class EntityState_AIChase : EntityState
    {
        [NonSerialized] public ModuleAI_AB_Target ReferModule_Target;
        [NonSerialized] public ModuleAI_AB_EnemyAttackControl ReferModule_Attack;
        [NonSerialized] public ESEntitySharedData ESEntityShared;
        protected override void RunStatePreparedLogic()
        {
            base.RunStatePreparedLogic();
            ReferModule_Target = Entity.AIDomain.Module_AB_AITarget;
            ReferModule_Attack = Entity.AIDomain.Module_AB_AttackControl;
            ESEntityShared = Entity.SharedData ?? new ESEntitySharedData();
            Entity.NormalDomain.Module_AB_Motion.StandardSpeed.y = ESEntityShared.ChaseSpeed;

            if (ReferModule_Target == null) OnStateExit();
        }
        protected override void RunStateUpdateLogic()
        {
            base.RunStateUpdateLogic();




            float remainDis = ReferModule_Target.Target != null ?
                Vector3.Distance(Entity.transform.position, ReferModule_Target.Target.transform.position)
                : Vector3.Distance(Entity.transform.position, ReferModule_Target.EndPathVS.Length > 0 ? ReferModule_Target.EndPathVS[0] : default);
            float ffNext = 1 / (Entity.entitySharedData.Attacks_.Count + 1);
            ReferModule_Attack.attackableNow = null;
            foreach (var i in Entity.entitySharedData.Attacks_)
            {

                if (remainDis < i.AttackRangeDis)
                {
                    ReferModule_Attack.attackableNow = i;
                    if (UnityEngine.Random.value < ffNext * 1.5f)
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                else if (remainDis > Entity.entitySharedData.VisionRangeDis)
                {
                    ReferModule_Target.Target = null;
                    TheEntityStateMachine.TryActiveStateByKey("巡逻状态");
                }

            }
            if (ReferModule_Attack.attackableNow != null)
            {

                bool b = TheEntityStateMachine.TryActiveStateByKey("攻击状态");

            }
        }
    }
    [Serializable, TypeRegistryItem("实体AI警报状态")]
    public class EntityState_AIAlert : EntityState
    {
        private float timerHasGo = 0;
        protected override void RunStateUpdateLogic()
        {
            base.RunStateUpdateLogic();
            timerHasGo += Time.deltaTime;
            //还没写特殊的
            if (timerHasGo >= Entity.entitySharedData.AlertTime)
            {
                timerHasGo = 0;
                //进入游荡
            }
        }
    }
    [Serializable, TypeRegistryItem("实体AI攻击状态")]
    public class EntityState_AIAttack : EntityState
    {
        private float preDelay = 0.25f;
        private float toExit = 1;
        public bool hasAttack = false;
        [NonSerialized] public ModuleAI_AB_EnemyAttackControl attackControl;
        protected override void RunStateEnterLogic()
        {
            base.RunStateEnterLogic();
            Entity.NormalDomain.Module_AB_Motion.StandardSpeed.y = 0.2f;

            attackControl = Entity.AIDomain.Module_AB_AttackControl;
            hasAttack = false;
            if (attackControl == null || attackControl.attackableNow == null) OnStateExit();
            toExit = attackControl.attackableNow.attackExit_;
            preDelay = attackControl.attackableNow.preDelay;
        }
        protected override void RunStateUpdateLogic()
        {
            base.RunStateUpdateLogic();
            if (hasAttack) { }
            else
            {
                preDelay -= Time.deltaTime;
                if (preDelay < 0)
                {
                    hasAttack = true;
                    attackControl.attackableNow.TryAttackOn(Entity);
                }
            }
            toExit -= Time.deltaTime;
            if (toExit < 0)
            {
                OnStateExit();
            }
        }
        /*   void Attack(bool shot)
           {
               // Melle Attack
               if (shot == false)
               {
                   if (enemy == null) return;
                   enemy.chaseTarget.SendMessage("GetDamage", new DamageClass(enemy.ActualMelleAttack.meleeDamage, DamageType.Melle, enemy.GetComponent<Enemy>()), SendMessageOptions.DontRequireReceiver);
                   enemy.chaseTarget.SendMessage("DamageIndicatorFunction", enemy.transform, SendMessageOptions.DontRequireReceiver);
                   enemy.GetComponent<Enemy>().source.PlayOneShot(enemy.GetComponent<Enemy>().MelleAttackSound[Random.Range(0, enemy.GetComponent<Enemy>().MelleAttackSound.Length)]);
               }
               // Range Attack
               else if (shot == true)
               {
                   Vector3 SpawnPoint = enemy.vision.position;
                   GameObject missile = GameObject.Instantiate(enemy.ActualRangeAttack.missile, SpawnPoint, enemy.vision.rotation);

                   // AddInfo Colliders

                   missile.GetComponent<Missile>().EnemyColli = enemy.transform.GetComponentsInChildren<Collider>();
                   missile.GetComponent<Missile>().Guided = enemy.ActualRangeAttack.AutoAim;
                   missile.GetComponent<Missile>().speed = enemy.ActualRangeAttack.missileSpeed;
                   missile.GetComponent<Missile>().ApplyDamage = enemy.ActualRangeAttack.missileDamage;
                   missile.GetComponent<Missile>().source = enemy.transform;
                   enemy.GetComponent<Enemy>().source.PlayOneShot(enemy.GetComponent<Enemy>().RangeAttackSound[Random.Range(0, enemy.GetComponent<Enemy>().RangeAttackSound.Length)]);
               }
           }
           public void UpdateActions()
           {
               timer += Time.deltaTime;
               float distance = Vector3.Distance(enemy.chaseTarget.transform.position, enemy.transform.position);

               Watch();

               // Chase Detect
               if (enemy.attackType == EnemyAttackType.Melle && distance > enemy.ActualMelleAttack.attackRange)
               {
                   ToChaseState();

               }
               if (enemy.attackType == EnemyAttackType.Range && distance > enemy.ActualRangeAttack.shootRange)
               {
                   ToChaseState();
               }

               float biggerRange = enemy.ActualRangeAttack.shootRange;
               if (biggerRange < enemy.ActualMelleAttack.attackRange) { biggerRange = enemy.ActualMelleAttack.attackRange; }

               if (enemy.attackType == EnemyAttackType.MelleAndRange && distance > biggerRange)
               {
                   ToChaseState();
               }

               // Do Attack

               // When Melle and Range
               if (distance <= enemy.ActualRangeAttack.shootRange && distance > enemy.ActualMelleAttack.attackRange && timer >= enemy.ActualRangeAttack.Delay & enemy.attackType == EnemyAttackType.MelleAndRange)
               {
                   Attack(true);
                   timer = 0;

                   enemy.GetNextRangeAttack();
               }
               // When Only Range
               if (distance <= enemy.ActualRangeAttack.shootRange && timer >= enemy.ActualRangeAttack.Delay & enemy.attackType == EnemyAttackType.Range)
               {
                   Attack(true);
                   timer = 0;

                   enemy.GetNextRangeAttack();
               }
               // When Melle
               if (distance <= enemy.ActualMelleAttack.attackRange && timer >= enemy.ActualMelleAttack.Delay & enemy.attackType != EnemyAttackType.Range)
               {
                   Attack(false);
                   timer = 0;

                   enemy.GetNextMelleAttack();
               }
           }*/
    }

    #endregion
}

