using ES;
using ES.Pointer;
using FishNet;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ES.EnumCollect;


namespace ES
{
    [Serializable]
    public class HurtableDomainForItem : Domain<Item, HurtableModuleForItem>
    {
        //捡起来！
        #region 模块汇总
        [NonSerialized] public ModuleHurtable_FlyingObject Module_Flying;


        #endregion
    }
    [Serializable]
    public abstract class HurtableModuleForItem : Module<Item, HurtableDomainForItem>
    {


    }
    [Serializable, TypeRegistryItem("可伤害飞行投掷物")]
    public class ModuleHurtable_FlyingObject : HurtableModuleForItem
    {
        [LabelText("来源实体")] public Entity source;
        [NonSerialized] public ESItem_FlyingSharedData flyingData;
        [FoldoutGroup("移动相关")][LabelText("当前方向")] public Vector3 CurrentDirect;
        [FoldoutGroup("移动相关")][LabelText("目标方向")] public Vector3 TargetDirect;
        [NonSerialized] public Entity target;

        [FoldoutGroup("移动相关")]
        [LabelText("设置目标的方向获取")]
        public EnumCollect.SetFlyingTargetAboutDirecOption selfSetTargetOption = EnumCollect.SetFlyingTargetAboutDirecOption.Directly;
        [FoldoutGroup("移动相关")]
        [LabelText("设置移动方式原理基于")]
        public EnumCollect.SetFlyingBaseOn flyBaseOn = EnumCollect.SetFlyingBaseOn.RigidFixUpdate;
        [FoldoutGroup("移动相关")]
        [LabelText("调转速度")]
        public float directChangeSpeedLevel = 5;
        [FoldoutGroup("移动相关")]
        [LabelText("速度加成")]
        public float SpeedPerUp = 0;



        [FoldoutGroup("伤害")]
        [LabelText("伤害加成(1+%)")] public float DamagePerUp = 0;
        [FoldoutGroup("伤害")]
        [LabelText("伤害加成(IsAdd)")] public float DamageAdd = 0;
        [FoldoutGroup("伤害")]
       
        [FoldoutGroup("关于附加效果与生命")]
        [InfoBox("在共享数据中配置碰撞实体时触发的效果")]
        [LabelText("是否是Trigger")] public bool asTrigger = true;
        [LabelText("可损耗生命Layer")] public LayerMask TimesSubLayer = 2 << ESEditorRuntimePartMaster_OB.LayerEntity + 2 << ESEditorRuntimePartMaster_OB.LayerWall;
        private float lifeTimeHasGo = 10;
        private int canColTimes = 2;
        [FoldoutGroup("特殊功能"), LabelText("延迟时间")]
        public float delayTime = 0;

        public override Type TableKeyType =>null;

        protected override void CreateRelationshipOnly()
        {
            base.CreateRelationshipOnly();
            Domain.Module_Flying = this;
            flyingData = Core.sharedData as ESItem_FlyingSharedData ?? new ESItem_FlyingSharedData();
            if (flyingData == null) Domain.RemoveModuleWithoutTypeMatch(this);//没有存在的必要了
            lifeTimeHasGo = 0;
            canColTimes = flyingData.maxTimes;
            if (TargetDirect == default)
            {
                TargetDirect = Core.transform.forward;
            }

        }
        [Button("设置目标测试")]
        public void SetTarget(Entity e, SetFlyingTargetAboutDirecOption setDirecQuick_ = SetFlyingTargetAboutDirecOption.BySelfDefault)
        {
            if (e != null)
            {
                target = e;
                {
                    if (setDirecQuick_ == SetFlyingTargetAboutDirecOption.BySelfDefault)
                    {
                        setDirecQuick_ = selfSetTargetOption;
                    }
                    if (setDirecQuick_ == SetFlyingTargetAboutDirecOption.None)
                    {

                    }
                    else if (setDirecQuick_ == SetFlyingTargetAboutDirecOption.Directly)
                    {
                        TargetDirect = (e.transform.position + Vector3.up - (Core.transform.position)).normalized;
                    }
                    else if (setDirecQuick_ == SetFlyingTargetAboutDirecOption.Parabola)
                    {
                        TargetDirect = (e.transform.position - Core.transform.position + Vector3.up).normalized;
                    }
                    else if (setDirecQuick_ == SetFlyingTargetAboutDirecOption.RadAndFollow)
                    {
                        TargetDirect = (Vector3.Lerp(e.transform.position - Core.transform.position, e.transform.right, UnityEngine.Random.Range(-0.5f, 0.5f))).normalized; ;
                    }
                }
                if (flyBaseOn == SetFlyingBaseOn.RigidVelocityOnce)
                {
                    Core.Rigid.velocity = TargetDirect.normalized * flyingData.speed * (1 + SpeedPerUp);
                }
            }
        }
        protected override void Update()
        {
            delayTime -= Time.deltaTime;
            if (delayTime > 0) return;
            PrivateMethod_Lerp();
            PrivateMethod_LifeTime();
            if (flyBaseOn == SetFlyingBaseOn.TransUpdate)
            {
                Core.transform.position += CurrentDirect * Time.deltaTime * flyingData.speed * (1 + SpeedPerUp);
            }
            base.Update();
        }
        public override void FixedUpdate_MustSelfDelegate()
        {
            base.FixedUpdate_MustSelfDelegate();
            if (delayTime > 0) return;
            if (flyBaseOn == SetFlyingBaseOn.RigidVelocityUpdating)
            {
                Core.Rigid.velocity = CurrentDirect.normalized * flyingData.speed * (1 + SpeedPerUp);
            }
            else if (flyBaseOn == SetFlyingBaseOn.RigidFixUpdate)
            {
                Core.Rigid.position += CurrentDirect.normalized * Time.fixedDeltaTime * flyingData.speed * (1 + SpeedPerUp);
            }
        }
        private void PrivateMethod_Lerp()
        {
            CurrentDirect = Vector3.Lerp(CurrentDirect, TargetDirect, Time.deltaTime * directChangeSpeedLevel);
        }
        private void PrivateMethod_LifeTime()
        {
            lifeTimeHasGo += Time.deltaTime;
            if (lifeTimeHasGo > flyingData.missileLife_)
            {
                /*Core.whyDes = new Link_DestroyWhy() { options = SetFlyingDestroyWhyOption.LifeTime };*/
               /* Core.TryDestroyThisESObject();*/
            }
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            if (asTrigger)
            {
                Core.OnTriEntityHappen += PassiveDelegate_OnTriOrColEntityHandles;
                /* Core.OnTriEntityHappen += PassiveDelegate_OnTriOrColEntityHandles;
                 Core.OnTriHappen += PassiveDelegate_OnTriEvery;*/
            }
            else
            {
              /*  Core.OnColHappen += PassiveDelegate_OnColEvery;
                Core.OnColEntityHappen += PassiveDelegate_OnTriOrColEntityHandles;
         */   }
          /*  Core.OnDestroyHappen += PassiveDelegate_OnDeS;*/
            Domain.OnFixedUpdate += FixedUpdate_MustSelfDelegate;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (asTrigger)
            {
                //Core.OnTriHappen -= PassiveDelegate_OnTriEvery;
                Core.OnTriEntityHappen -= PassiveDelegate_OnTriOrColEntityHandles;
            }
            else
            {
             /*   Core.OnColHappen -= PassiveDelegate_OnColEvery;
                Core.OnColEntityHappen -= PassiveDelegate_OnTriOrColEntityHandles;
           */ }
          /*  Core.OnDestroyHappen -= PassiveDelegate_OnDeS;*/
            Domain.OnFixedUpdate -= FixedUpdate_MustSelfDelegate;
        }
        private void PassiveDelegate_OnTriOrColEntityHandles(Entity who, Vector3 at)
        {
            if (who != source)
            {
                Debug.Log("FIRE");
                who.hurt.Handle(who,who.NetBehaviour,new Link_HurtFrom() { baseDamage=5, itemID=Core.ID },fromClient:false);
                InstanceFinder.ServerManager.Despawn(Core.gameObject);
            }
           /* if (Tags.tagNames.Condtains(who.tag))
            {

                ESStaticLogicUtility.ESProcess.Global.GlobalLink_EntityAttackEntityTryStart
                    (new Link_EntityAttackEntityTryStart()
                    {
                        attacker = source,
                        damage = new Damage() { damage = flyingData.damage + source?.entityVariableData.DamagePerUp ?? 0 },
                        victim = who
                    });
                if (flyingData.entityHandleOfItem != null)
                {
                    var apply = flyingData.entityHandleOfItem.handles_;
                    if (apply != null && apply.Count > 0)
                    {
                        foreach (var i in apply)
                        {
                            i.Pick(who, source, this);
                        }
                    }
                }
            }*/
        }
        private void PassiveDelegate_OnColEvery(Collision who, Vector3 at, bool b)
        {
            if (((2 << who.gameObject.layer) & TimesSubLayer) > 0)
            {
                canColTimes--;
                if (canColTimes <= 0)
                {
                  /*  Core.TryDestroyThisESObject();*/
                }
            }
        }
        private void PassiveDelegate_OnTriEvery(Collider who, Vector3 at, bool b)
        {

            if (((1 << who.gameObject.layer) & TimesSubLayer) > 0)
            {
                canColTimes--;
                if (canColTimes <= 0)
                {
                   /* Core.whyDes.options = SetFlyingDestroyWhyOption.OnTriEntity;*/
                   /* Core.TryDestroyThisESObject();*/
                }
            }
        }
        private void PassiveDelegate_OnDeS(Link_DestroyWhy why)
        {
            Debug.Log("妈耶");
            var handle = flyingData.entityHandleOfItem;
            if (handle.OnDesBirth != null && (handle.optionForDesBirth & why.options) > 0)
            {
                Debug.Log(666);
                ESSpawnMaster.Instance.Ins(handle.OnDesBirth, Core.transform.position, null);
            }

            if (handle.OnDesPlaySound != null && (handle.optionForPlaySound & why.options) > 0)
            {
               /* GameCenterManager.Instance.AudioMaster.PlayDirect_Sound_OneShot(handle.OnDesPlaySound, 0.8f);
         */   }
        }
        #region 预设


        #endregion
    }

    
}