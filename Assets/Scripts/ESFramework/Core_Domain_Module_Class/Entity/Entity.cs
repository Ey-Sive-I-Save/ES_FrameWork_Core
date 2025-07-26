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
    public class Entity : ESObject, IReceiveAnyLink, IWithSharedAndVariableData<ESEntitySharedData, ESEntityVariableData>
    {
        [TabGroup("基本域", TextColor = "@Editor_DomainTabColor(NormalDomain)")]
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
        protected override void OnAwakeRegisterOnly()
        {
            Debug.Log("RE1");
            RegisterDomains(NormalDomain, StateMachineDomain, AIDomain, BuffDomain);
        }



        [FoldoutGroup("属性")]
        public CharacterController CharacterController;
        [FoldoutGroup("属性")] public float YV = 0;
        [FoldoutGroup("属性")] public Transform GroundCheck;
        [FoldoutGroup("属性")] public bool IsGrounded = false;


        [FoldoutGroup("属性")]
        [LabelText("实体共享数据(等待引用)")]
        public ActorDataInfo dataInfo;
        [FoldoutGroup("属性")]
        [LabelText("实体共享数据(等待引用)"), SerializeReference]
        public ESEntitySharedData entitySharedData = null;
        [FoldoutGroup("属性")]
        [LabelText("实体变量数据"), SerializeReference]
        public ESEntityVariableData entityVariableData;

        [FoldoutGroup("委托"), LabelText("尝试被攻击时")] public Action<Entity, Damage> OnTryBeAttack = (a, b) => { };
        [FoldoutGroup("委托"), LabelText("尝试攻击时")] public Action<Entity, Damage> OnTryAttack = (a, b) => { };
        [FoldoutGroup("委托"), LabelText("真的被攻击时")] public Action<Entity, Damage> OnTruelyBeAttack = (a, b) => { };
        [FoldoutGroup("委托"), LabelText("真的攻击时")] public Action<Entity, Damage> OnTruelyAttack = (a, b) => { };
        [FoldoutGroup("委托"), LabelText("拿起道具")] public Action<Entity, Damage> OnTestOnly_TakeAObject = (a, b) => { };
        [FoldoutGroup("委托"), LabelText("击杀敌人")] public Action<Entity, Damage> OnTestOnly_Kill = (a, b) => { };

        [FoldoutGroup("模拟委托触发"), Button("模拟尝试被攻击", ButtonHeight = 40)]
        public void TestTryBeAttack()
        {
            OnTryBeAttack?.Invoke(this, null);
        }
        [FoldoutGroup("模拟委托触发"), Button("模拟尝试攻击他人", ButtonHeight = 40)]
        public void TestTryAttack()
        {
            OnTryAttack?.Invoke(this, null);
        }
        [FoldoutGroup("模拟委托触发"), Button("模拟真实被攻击", ButtonHeight = 40)]
        public void TestTruelyBeAttack()
        {
            OnTruelyBeAttack?.Invoke(this, null);
        }
        [FoldoutGroup("模拟委托触发"), Button("模拟真实攻击他人", ButtonHeight = 40)]
        public void TestTruelyAttack()
        {
            OnTruelyAttack?.Invoke(this, null);
        }
        [FoldoutGroup("模拟委托触发"), Button("拿起来道具", ButtonHeight = 40)]
        public void TestTakeObject()
        {
            OnTestOnly_TakeAObject?.Invoke(this, null);
        }
        [FoldoutGroup("模拟委托触发"), Button("击杀", ButtonHeight = 40)]
        public void TestKill()
        {
            OnTestOnly_Kill?.Invoke(this, null);
        }
        [FoldoutGroup("加buff")]
        public BuffSoInfo info;
        [FoldoutGroup("加buff")]
        [Button("加Buff", ButtonHeight = 40)]
        public void AddThisBuff()
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

        protected override void OnAfterAwakeRegister()
        {
            if (dataInfo != null)
                KeyValueMatchingUtility.DataApply.
                    CopyToClassSameType(dataInfo, this);

            base.OnAfterAwakeRegister();
        }
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
            this.YV = 0.25f;

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
        protected override void OnBeforeAwakeRegister()
        {
            Anim = GetComponent<Animator>();
            CharacterController = GetComponent<CharacterController>();
            base.OnBeforeAwakeRegister();
        }
        protected override void Update()
        {
            base.Update();
            return;
            Transform t = transform;

            if (GroundCheck != null)
            {
                t = GroundCheck;
            }
            if (YV <= 0)
            {
                var back = Physics.Raycast(t.position + Vector3.up * 0.25f, Vector3.down, 0.5f, ESEditorRuntimePartMaster.LayerMaskGround);

                if (back)
                {
                    if (YV > -0.1f) YV = 0;
                    else YV = Mathf.Lerp(YV, 0, Time.deltaTime * 3);
                    IsGrounded = true;
                }
                else
                {
                    YV -= Time.deltaTime * 9;
                    IsGrounded = false;
                }
            }
            else
            {
                YV -= Time.deltaTime * 9;
            }


            base.Update();

        }

        public override void _InTable()
        {
            GameCenterManager.EntityIDPool.Add(ID, this);
        }

        public override void _OutTable()
        {
            GameCenterManager.EntityIDPool.Remove(ID);
        }
    }
}
