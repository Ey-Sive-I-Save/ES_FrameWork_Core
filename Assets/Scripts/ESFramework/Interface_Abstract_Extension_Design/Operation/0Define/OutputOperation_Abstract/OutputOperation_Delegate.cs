using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;


namespace ES
{
    public class OutputOpeationDelegateFlag : OverLoadFlag<OutputOpeationDelegateFlag>
    {

    }
    public class DeleAndCount
    {
        public Delegate dele;
        public int count = 0;
    }
    [Serializable]
    public abstract class OutputOpeationDelegate<On, From, With, MakeAction> :
        OutputOperation_Abstract<On, From, With>, IOutputOperationFlag_MustCancel
        where MakeAction : Delegate
        where With : ICacheKeyValueForOutputOpeation<IOperation, DeleAndCount, OutputOpeationDelegateFlag>
    {
       
        
        [LabelText("给与触发次数")]
        public int GiveCount = 99;
        public MakeAction GetActionOnEnableExpand(On on, From from, With with)
        {
            MakeAction make = null;
            var cache = with.GetCache(OutputOpeationDelegateFlag.flag);
            if(cache.TryGetValue(this, out var value))
            {
                make = value.dele as MakeAction;
                value.count += GiveCount;
            }
            else
            {
                make = MakeTheAction(on, from, with);
                cache.Add(this,new DeleAndCount() { dele= make, count=GiveCount });
            }
            return make;
        }
        public MakeAction GetActionOnDisableExpand(On on, From from, With with)
        {
            var cacher = with.GetCache(OutputOpeationDelegateFlag.flag);
            if (cacher.TryGetValue(this, out var use))
            {
                cacher.Remove(this);
                return use.dele as MakeAction;
            }
            return default;
        }
        public void SetWhenActionHappenCountChange(On on, From from, With with)
        {
            var cacher = with.GetCache(OutputOpeationDelegateFlag.flag);
            if (cacher.TryGetValue(this, out var use))
            {
                Debug.Log("COUNT2     "+ use.count);
                use.count--;
                if (use.count <= 0)
                {
                    //提前退出
                    Debug.Log("COUNT3");
                    TryCancel(on,from,with);
                }
            }
        }
        protected abstract MakeAction MakeTheAction(On on, From from, With with);
        
    }
    #region 演示

    [Serializable]
    public abstract class OutputOpeationDelegate_EEB<MakeAction> : OutputOpeationDelegate<Entity ,Entity,EntityState_Buff, MakeAction>,IOutputOperationEEB where MakeAction : Delegate
    {
        protected override MakeAction MakeTheAction(Entity on, Entity from, EntityState_Buff with)
        {
            /*委托触发可能来自任意情况 直接写一个outputoperation的操作比较常见*/
            return (MakeAction)new object();
        }
    }
    [Serializable]
    public abstract class OutputOperationDelegate_EEB_BaseOutput<MakeAction> : OutputOpeationDelegate_EEB<MakeAction> where MakeAction : Delegate
    {
        [SerializeReference, LabelText("委托触发时")]
        public IOutputOperationEEB WhenHappen;
        protected void GetDelegateHappenExpand(Entity on, Entity from, EntityState_Buff with)
        {
            if (WhenHappen != null)
            {
                WhenHappen.TryOpeation(on, from, with);
                if(WhenHappen is IOutputOperationFlag_MustCancel) OnCancel += WhenHappen.TryCancel;
            }
            SetWhenActionHappenCountChange(on, from, with);
            Debug.Log("COUNT1     " + "dfsd");
        }
        
    }

    [Serializable,TypeRegistryItem("委托输出-输出操作-真实攻击他人时")]
    public class OnAttack : OutputOperationDelegate_EEB_BaseOutput<Action<Entity,Damage>>
    {
        public override void TryOpeation(Entity on, Entity from, EntityState_Buff with)
        {
            on.OnTruelyAttack += GetActionOnEnableExpand(on,from,with);
        }
        public override void TryCancel(Entity on, Entity from, EntityState_Buff with)
        {
            on.OnTruelyAttack -= GetActionOnDisableExpand(on, from, with);
            base.TryCancel(on, from, with);
        }
        protected override Action<Entity, Damage> MakeTheAction(Entity on, Entity from, EntityState_Buff with)
        {
            return (a,b)=>GetDelegateHappenExpand(on,from,with);
        }
    }
    [Serializable, TypeRegistryItem("委托输出-输出操作-被真实攻击时")]
    public class OnBeAttack : OutputOperationDelegate_EEB_BaseOutput<Action<Entity, Damage>>
    {
        public override void TryOpeation(Entity on, Entity from, EntityState_Buff with)
        {
            on.OnTruelyBeAttack += GetActionOnEnableExpand(on, from, with);
        }
        public override void TryCancel(Entity on, Entity from, EntityState_Buff with)
        {
            on.OnTruelyBeAttack -= GetActionOnDisableExpand(on, from, with);
            base.TryCancel(on, from, with);
        }
        protected override Action<Entity, Damage> MakeTheAction(Entity on, Entity from, EntityState_Buff with)
        {
            return (a, b) => GetDelegateHappenExpand(on, from, with);
        }
    }
    [Serializable, TypeRegistryItem("委托输出-输出操作-尝试攻击他人时")]
    public class OnTryAttack : OutputOperationDelegate_EEB_BaseOutput<Action<Entity, Damage>>
    {
        public override void TryOpeation(Entity on, Entity from, EntityState_Buff with)
        {
            on.OnTryAttack += GetActionOnEnableExpand(on, from, with);
        }
        public override void TryCancel(Entity on, Entity from, EntityState_Buff with)
        {
            on.OnTryAttack -= GetActionOnDisableExpand(on, from, with);
            base.TryCancel(on, from, with);
        }
        protected override Action<Entity, Damage> MakeTheAction(Entity on, Entity from, EntityState_Buff with)
        {
            return (a, b) => GetDelegateHappenExpand(on, from, with);
        }
    }
    [Serializable, TypeRegistryItem("委托输出-输出操作-被尝试攻击时")]
    public class OnTryBeAttack : OutputOperationDelegate_EEB_BaseOutput<Action<Entity, Damage>>
    {
        public override void TryOpeation(Entity on, Entity from, EntityState_Buff with)
        {
            on.OnTryBeAttack += GetActionOnEnableExpand(on, from, with);
        }
        public override void TryCancel(Entity on, Entity from, EntityState_Buff with)
        {
            on.OnTryBeAttack -= GetActionOnDisableExpand(on, from, with);
            base.TryCancel(on, from, with);
        }
        protected override Action<Entity, Damage> MakeTheAction(Entity on, Entity from, EntityState_Buff with)
        {
            return (a, b) => GetDelegateHappenExpand(on, from, with);
        }
    }

    [Serializable, TypeRegistryItem("委托输出-输出操作-捡起物品")]
    public class OnTryTake: OutputOperationDelegate_EEB_BaseOutput<Action<Entity, Damage>>
    {
        public override void TryOpeation(Entity on, Entity from, EntityState_Buff with)
        {
            on.OnTestOnly_TakeAObject += GetActionOnEnableExpand(on, from, with);
        }
        public override void TryCancel(Entity on, Entity from, EntityState_Buff with)
        {
            on.OnTestOnly_TakeAObject -= GetActionOnDisableExpand(on, from, with);
            base.TryCancel(on, from, with);
        }
        protected override Action<Entity, Damage> MakeTheAction(Entity on, Entity from, EntityState_Buff with)
        {
            return (a, b) => GetDelegateHappenExpand(on, from, with);
        }
    }
    [Serializable, TypeRegistryItem("委托输出-输出操作-击杀时")]
    public class OnKill : OutputOperationDelegate_EEB_BaseOutput<Action<Entity, Damage>>
    {
        public override void TryOpeation(Entity on, Entity from, EntityState_Buff with)
        {
            on.OnTestOnly_Kill += GetActionOnEnableExpand(on, from, with);
        }
        public override void TryCancel(Entity on, Entity from, EntityState_Buff with)
        {
            on.OnTestOnly_Kill -= GetActionOnDisableExpand(on, from, with);
            base.TryCancel(on, from, with);
        }
        protected override Action<Entity, Damage> MakeTheAction(Entity on, Entity from, EntityState_Buff with)
        {
            return (a, b) => GetDelegateHappenExpand(on, from, with);
        }
    }

    #endregion
}

