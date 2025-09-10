                                                                         using DG.Tweening;
using ES;
using ES.Pointer;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES
{
    public sealed partial class Entity : ESObjectBase
        , IWithSharedAndVariableData<ESEntitySharedData, ESEntityVariableData>
    {
        #region 专属扩展域

        [TabGroup("扩展域","常规域", TextColor = "@Editor_DomainTabColor(NormalDomain)")]
        [SerializeReference, InlineProperty, HideLabel]
        public NormalDomainForEntity NormalDomain;
        [TabGroup("扩展域", "标准状态机域", TextColor = "@Editor_DomainTabColor(StateMachineDomain)")]
        [SerializeReference, InlineProperty, HideLabel]
        public StateMachineDomainForEntity StateMachineDomain;
        [TabGroup("扩展域", "AI域", TextColor = "@Editor_DomainTabColor(AIDomain)")]
        [SerializeReference, InlineProperty, HideLabel]
        public AIDomainForEntity AIDomain;
        [TabGroup("扩展域", "Buff域", TextColor = "@Editor_DomainTabColor(BuffDomain)")]
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
            RegisterDomain(NormalDomain);
            RegisterDomain(StateMachineDomain);
            RegisterDomain(AIDomain);
            RegisterDomain(BuffDomain);
        }

        //注册完成--》 初始化
        protected override void OnAfterAwakeRegister()
        {
            if (dataInfo != null)
                ESDesignUtility.DataApply.
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
                var back = Physics.Raycast(t.position + Vector3.up * 0.25f, Vector3.down, 0.5f, ESEditorRuntimePartMaster_OB.LayerMaskGround);

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
        [TabGroup("常规","属性")]
        public CharacterController CharacterController;
        [TabGroup("常规","属性")] public float YSpeed = 0;
        [TabGroup("常规","属性")] public Transform GroundCheck;
        [TabGroup("常规","属性")] public bool IsGrounded = false;

        #endregion

        #region 基础属性
        [TabGroup("常规","属性")]
        [LabelText("实体共享数据(等待引用)")]
        public ActorDataInfo dataInfo;
        [TabGroup("常规","属性")]
        [LabelText("实体共享数据(等待引用)"), SerializeReference]
        public ESEntitySharedData entitySharedData = null;
        [TabGroup("常规","属性")]
        [LabelText("实体变量数据"), SerializeReference]
        public ESEntityVariableData entityVariableData=new ESEntityVariableData();
        #endregion
        public Renderer selfRenderer;
        public Collider col;
        [TabGroup("常规","事件收发")]
        public LinkReceiveChannelPool<Channel_EntityAttackLink, Link_EntityAttack>
            LinksForTest = new();

        public ESEntitySharedData SharedData { get => entitySharedData; set => entitySharedData = value; }
        public ESEntityVariableData VariableData { get => entityVariableData; set => entityVariableData = value; }
        public Handler_Hurt hurt = new Handler_Hurt();

        public void AttackOn(float f)
        {
            VariableData.Health -= f;
            Debug.Log("ATTACK");
            LinksForTest.SendLink( Channel_EntityAttackLink.TrulyBeAttack, new Link_EntityAttack() {  f=VariableData.Health });
        }
    }
}
