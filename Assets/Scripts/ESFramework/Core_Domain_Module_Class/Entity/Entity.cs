using DG.Tweening;
using ES;
using ES.EvPointer;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES
{
    public sealed class Entity : ESObject
        , IReceiveAnyLink, IWithSharedAndVariableData<ESEntitySharedData, ESEntityVariableData>
        
    {
        #region 专属扩展域

        [TabGroup("常规域", TextColor = "@Editor_DomainTabColor(NormalDomain)")]
        [SerializeReference, InlineProperty, HideLabel]
        public NormalDomainForEntity NormalDomain;
        [TabGroup("标准状态机域", TextColor = "@Editor_DomainTabColor(StateMachineDomain)")]
        [SerializeReference, InlineProperty, HideLabel]
        public StateMachineDomainForEntity StateMachineDomain;
        [TabGroup("AI域", TextColor = "@Editor_DomainTabColor(AIDomain)")]
        [SerializeReference, InlineProperty, HideLabel]
        public AIDomainForEntity AIDomain;
        [TabGroup("Buff域", TextColor = "@Editor_DomainTabColor(BuffDomain)")]
        [SerializeReference, InlineProperty, HideLabel]
        public BuffDomainForEntity BuffDomain;

        #endregion

        #region 继承重写等
        //先--组件获取
        protected override void OnBeforeAwakeRegister()
        {
            Anim = GetComponent<Animator>();
            CharacterController = GetComponent<CharacterController>();
            base.OnBeforeAwakeRegister();
        }
        //仅注册扩展域
        protected override void OnAwakeRegisterOnly()
        {
            RegisterAllDomains(NormalDomain, StateMachineDomain, AIDomain, BuffDomain);
        }

        //注册完成--》 初始化
        protected override void OnAfterAwakeRegister()
        {
            if (dataInfo != null)
                KeyValueMatchingUtility.DataApply.
                    CopyToClassSameType(dataInfo, this);

            base.OnAfterAwakeRegister();
        }
        //帧更新
        protected override void Update()
        {
            base.Update();
            return;
            Transform t = transform;

            if (GroundCheck != null)
            {
                t = GroundCheck;
            }
            if (YSpeed <= 0)
            {
                var back = Physics.Raycast(t.position + Vector3.up * 0.25f, Vector3.down, 0.5f, ESEditorRuntimePartMaster.LayerMaskGround);

                if (back)
                {
                    if (YSpeed > -0.1f) YSpeed = 0;
                    else YSpeed = Mathf.Lerp(YSpeed, 0, Time.deltaTime * 3);
                    IsGrounded = true;
                }
                else
                {
                    YSpeed -= Time.deltaTime * 9;
                    IsGrounded = false;
                }
            }
            else
            {
                YSpeed -= Time.deltaTime * 9;
            }


            base.Update();

        }
        //入表
        public sealed override void _InTable()
        {
            GameCenterManager.EntityIDPool.Add(ID, this);
        }
        //出表
        public sealed override void _OutTable()
        {
            GameCenterManager.EntityIDPool.Remove(ID);
        }
        #endregion

        #region Motion配置
        [TabGroup("属性")]
        public CharacterController CharacterController;
        [TabGroup("属性")] public float YSpeed = 0;
        [TabGroup("属性")] public Transform GroundCheck;
        [TabGroup("属性")] public bool IsGrounded = false;

        #endregion

        #region 基础属性
        [TabGroup("属性")]
        [LabelText("实体共享数据(等待引用)")]
        public ActorDataInfo dataInfo;
        [TabGroup("属性")]
        [LabelText("实体共享数据(等待引用)"), SerializeReference]
        public ESEntitySharedData entitySharedData = null;
        [TabGroup("属性")]
        [LabelText("实体变量数据"), SerializeReference]
        public ESEntityVariableData entityVariableData;
        #endregion


        [FoldoutGroup("事件收发选通")]
        public LinkReceiveChannelList<Channel_EntityAttackLink, Link_EntityAttack>
            LinksForTest = new();
       
        void test()
        {
            LinksForTest.SendLink(Channel_EntityAttackLink.TryAttack,new Link_EntityAttack ());
            LinksForTest.SendLink(Channel_EntityAttackLink.TryBeAttack, new Link_EntityAttack());
            LinksForTest.SendLink(Channel_EntityAttackLink.TrulyAttack, new Link_EntityAttack());
            LinksForTest.SendLink(Channel_EntityAttackLink.TryBeAttack, new Link_EntityAttack());
        }

        [FoldoutGroup("委托"), LabelText("尝试被攻击时")] public Action<Entity, Damage> OnTryBeAttack = (a, b) => { };
        [FoldoutGroup("委托"), LabelText("尝试攻击时")] public Action<Entity, Damage> OnTryAttack = (a, b) => { };
        [FoldoutGroup("委托"), LabelText("真的被攻击时")] public Action<Entity, Damage> OnTruelyBeAttack = (a, b) => { };
        [FoldoutGroup("委托"), LabelText("真的攻击时")] public Action<Entity, Damage> OnTruelyAttack = (a, b) => { };
        [FoldoutGroup("委托"), LabelText("拿起道具")] public Action<Entity, Damage> OnTestOnly_TakeAObject = (a, b) => { };
        [FoldoutGroup("委托"), LabelText("击杀敌人")] public Action<Entity, Damage> OnTestOnly_Kill = (a, b) => { };

        [TabGroup("测试"), Button("模拟尝试被攻击", ButtonHeight = 20)]
        public void TestTryBeAttack()
        {
            transform.DOMove(default, 1).onComplete(); ;
            OnTryBeAttack?.Invoke(this, null);
        }
        [TabGroup("测试"), Button("模拟尝试攻击他人", ButtonHeight = 20)]
        public void TestTryAttack()
        {
            OnTryAttack?.Invoke(this, null);
        }
        [TabGroup("测试"), Button("模拟真实被攻击", ButtonHeight = 20)]
        public void TestTruelyBeAttack()
        {
            OnTruelyBeAttack?.Invoke(this, null);
        }
        [TabGroup("测试"), Button("模拟真实攻击他人", ButtonHeight = 20)]
        public void TestTruelyAttack()
        {
            OnTruelyAttack?.Invoke(this, null);
        }
        [TabGroup("测试"), Button("拿起来道具", ButtonHeight = 20)]
        public void TestTakeObject()
        {
            OnTestOnly_TakeAObject?.Invoke(this, null);
        }
        [TabGroup("测试"), Button("击杀", ButtonHeight = 20)]
        public void TestKill()
        {
            OnTestOnly_Kill?.Invoke(this, null);
        }
        [TabGroup("测试")]
        public BuffSoInfo info_1;
        [TabGroup("测试")]
        public BuffSoInfo info_2;
        [TabGroup("测试")]
        public BuffSoInfo info_3;
        [TabGroup("测试"), Button("加Buff", ButtonHeight = 40)]
        public void AddBuff3()
        {
            AddThisBuff(info_1);
            AddThisBuff(info_2);
            AddThisBuff(info_3);
        }
        public void AddThisBuff(BuffSoInfo info)
        {
            if (info != null)
            {
                var machine = StateMachineDomain.StateMachine;
                string key = info.DataKey.str_direc;
                var has = machine.GetStateByKey(key);
                if (has == null)
                {
                    var buffState = new EntityState_Buff() { buffSharedData = info.buffSharedData, sharedData = info.bindingState.stateSharedData };
                    machine.RegisterNewState(key, has = buffState);
                }
                if (has != null)
                {
                    if (has is EntityState_Buff buff)
                    {
                        if (buff.RunningStatus == EnumStateRunningStatus.StateUpdate)
                        {
                            //正在巡行
                            buff.OnStateExit();
                        }
                        buff.buffSharedData = info.buffSharedData;
                        buff.buffVariableData.timeRemain = 10;
                        buff.OnStatePrepare();
                    }
                }
            }
        }

        public ESEntitySharedData SharedData { get => entitySharedData; set => entitySharedData = value; }
        public ESEntityVariableData VariableData { get => entityVariableData; set => entityVariableData = value; }


        public void OnLink(ILink link)
        {


        }
        public void Invoke_TryAttackEntityCalculate(Entity who, Damage damage)
        {
            Debug.Log("尝试攻击");
            OnTryAttack?.Invoke(who, damage);
        }
        public void Invoke_BeAttackByEntityCalculate(Entity who, Damage damage)
        {
            OnTryBeAttack?.Invoke(who, damage);
            this.YSpeed = 0.25f;

        }
        public void Invoke_TrulyBeAttack(Entity who, Damage damage)
        {
            OnTruelyBeAttack?.Invoke(who, damage);
            this.VariableData.Health -= damage.damage;
        }
        public void Invoke_TrulyAttack(Entity who, Damage damage)
        {
            OnTruelyAttack?.Invoke(who, damage);
        }


    }
}
