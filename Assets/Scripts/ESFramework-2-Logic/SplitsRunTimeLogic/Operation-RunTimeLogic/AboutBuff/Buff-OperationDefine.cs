using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
namespace ES {

    [Serializable]
    public abstract class OutputOpeationDelegate_EEB<MakeAction> : OutputOpeationDelegate<Entity, Entity, EntityState_Buff, MakeAction>, IOutputOperationEEB where MakeAction : Delegate
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
                WhenHappen.TryOperation(on, from, with);
                if (WhenHappen is IOutputOperationFlag_MustCancel) OnCancel += WhenHappen.TryCancel;
            }
            SetWhenActionHappenCountChange(on, from, with);
            Debug.Log("COUNT1     " + "dfsd");
        }

    }



    [Serializable, TypeRegistryItem("委托输出-输出操作-尝试攻击他人时")]
    public class OnTryAttack : OutputOperationDelegate_EEB_BaseOutput<Action<Entity, Damage>>
    {
        public override void TryOperation(Entity on, Entity from, EntityState_Buff with)
        {

        }
        public override void TryCancel(Entity on, Entity from, EntityState_Buff with)
        {
            /* on.OnTryAttack -= GetActionOnDisableExpand(on, from, with);*/
            base.TryCancel(on, from, with);
        }
        protected override Action<Entity, Damage> MakeTheAction(Entity on, Entity from, EntityState_Buff with)
        {
            return (a, b) => GetDelegateHappenExpand(on, from, with);
        }
    }

    
    #region 演示 
    [Serializable,TypeRegistryItem("值变更输出-自定义组合-导向")]
    public class OutputOperationFloatValue_CompositeWithSupportAndTarget_EEB : 
        OutputOperationValue_CompositeWithSupportAndTarget<Entity, Entity, EntityState_Buff, float, float, OperationOptionsForFloat, SupportOperation_EEB_DirectFloat, TargetFloatOperation_EEBuff>,IOutputOperationEEB
    {

    }
    #endregion


    public interface IOutputOperationEEB : IOutputOperation<Entity, Entity, EntityState_Buff>
    {
        
    }


    [Serializable, TypeRegistryItem("执行多个")]
    public class OutputOperationEEB_HandleList : IOutputOperationEEB
    {
        [LabelText("执行列表"), SerializeReference]
        public List<IOutputOperationEEB> handles = new List<IOutputOperationEEB>();
        public void TryCancel(Entity on, Entity from, EntityState_Buff with)
        {
            foreach (var i in handles)
            {
                i.TryOperation(on, from, with);
            }
        }

        public void TryOperation(Entity on, Entity from, EntityState_Buff with)
        {
            foreach (var i in handles)
            {
                i.TryCancel(on, from, with);
            }
        }
    }
    [Serializable]
    public abstract class OutputOpeationSettleFloatEEB : OutputOperationSettle<Entity, Entity, EntityState_Buff, float>, IOutputOperationEEB
    {

    }
    [Serializable/*Buff结算浮点专用操作*/]
    public abstract class OutputOpeationFloat_EEB : OutputOpeationSettleFloatEEB
    {
        [LabelText("强制不渐变")] public bool ForceNormal = false;
        [LabelText("结算类型")] public OperationOptionsForFloat settleType = OperationOptionsForFloat.Add;
        [LabelText("效果类型")] public SettleSelfTypeForFloat selfType = SettleSelfTypeForFloat.None;
        [LabelText("优先级")] public int Priority = 0;
        [LabelText("基值")] public float baseValue = 1;
        [LabelText("每级偏移")] public float offsetPerLevel = 0.1f;
        public sealed override void TryOperation(Entity on, Entity from, EntityState_Buff with)
        {
            var settle = GetSettlement(on, from, with);
            var operation = GetEnableSettle(on, from, with);
            operation.TryStart(settle);
        }
        public sealed override void TryCancel(Entity on, Entity from, EntityState_Buff with)
        {
            var settle = GetSettlement(on, from, with);
            var operation = GetDisableSettle(on, from, with);
            operation.TryStop(settle);
        }
        public void MakeDirty_Output(Entity on, Entity from, EntityState_Buff with)
        {
            if (with.CacheSettle.TryGetValue(this, out var use))
            {
                use.SetValue(baseValue + with.Level * offsetPerLevel);
                var settle = GetSettlement(on, from, with);
                settle.MakeDirty();
            }
        }
        public abstract SettlementFloat GetSettlement(Entity on, Entity from, EntityState_Buff with);
        public SettleOperationFloat GetEnableSettle(Entity on, Entity from, EntityState_Buff with)
        {
            var use = MakeTheOpeation(on, from, with);
            with.CacheSettle.Add(this, use);
            return use;
        }
        public SettleOperationFloat GetDisableSettle(Entity on, Entity from, EntityState_Buff with)
        {
            if (with.CacheSettle.TryGetValue(this, out var use))
            {
                with.CacheSettle.Remove(this);
                return use as SettleOperationFloat;
            }
            return default;
        }
        protected virtual SettleOperationFloat MakeTheOpeation(Entity on, Entity from, EntityState_Buff with)
        {
            var use = ESValueMaster.Instance.floatSettleOpsPool.GetInPool();
            use.selfType = selfType;
            use.settleType = settleType;
            use.Priority = Priority;
            use.Value = baseValue + with.Level * offsetPerLevel;
            return use;
        }

    }
    [Serializable]
    //定义类型，Entity,Entity,Buff
    public abstract class TargetFloatOperation_EEBuff : TargetFloatOperation<Entity, Entity, EntityState_Buff>, ITargetOperationFloatEEB
    {

    }

    [Serializable, TypeRegistryItem("血量修改导向")]
    public class TargetOP_Helath : TargetFloatOperation_EEBuff
    {
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public sealed override void TargetOpeation(Entity on, Entity from, EntityState_Buff with, float Opeation_, OperationOptionsForFloat SelectType_)
        {
            ExpandOperation(ref on.VariableData.Health, Opeation_, SelectType_);
        }
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public sealed override void TargetCancel(Entity on, Entity from, EntityState_Buff with, float Opeation_, OperationOptionsForFloat SelectType_)
        {
            ExpandCancel(ref on.VariableData.Health, Opeation_, SelectType_);
        }
    }

    [Serializable, TypeRegistryItem("伤害增益导向")]
    public class TargetOP_DamageAdd : TargetFloatOperation_EEBuff
    {
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public sealed override void TargetOpeation(Entity on, Entity from, EntityState_Buff with, float Opeation_, OperationOptionsForFloat SelectType_)
        {
            ExpandOperation(ref on.VariableData.DamageAdd, Opeation_, SelectType_);
        }
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public sealed override void TargetCancel(Entity on, Entity from, EntityState_Buff with, float Opeation_, OperationOptionsForFloat SelectType_)
        {
            ExpandCancel(ref on.VariableData.DamageAdd, Opeation_, SelectType_);
        }
    }
    [Serializable, TypeRegistryItem("攻击速度导向")]
    public class TargetOP_AttackSpeed : TargetFloatOperation_EEBuff
    {
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public sealed override void TargetOpeation(Entity on, Entity from, EntityState_Buff with, float Opeation_, OperationOptionsForFloat SelectType_)
        {
            ExpandOperation(ref on.VariableData.AttackSpeed, Opeation_, SelectType_);
        }
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public sealed override void TargetCancel(Entity on, Entity from, EntityState_Buff with, float Opeation_, OperationOptionsForFloat SelectType_)
        {
            ExpandCancel(ref on.VariableData.AttackSpeed, Opeation_, SelectType_);
        }
    }
    [Serializable, TypeRegistryItem("移动速度增益导向")]
    public class TargetOP_MoveSpeed : TargetFloatOperation_EEBuff
    {
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public sealed override void TargetOpeation(Entity on, Entity from, EntityState_Buff with, float Opeation_, OperationOptionsForFloat SelectType_)
        {
            ExpandOperation(ref on.VariableData.SpeedPerUp, Opeation_, SelectType_);
        }
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public sealed override void TargetCancel(Entity on, Entity from, EntityState_Buff with, float Opeation_, OperationOptionsForFloat SelectType_)
        {
            ExpandCancel(ref on.VariableData.SpeedPerUp, Opeation_, SelectType_);
            
        }
    }
    [Serializable, TypeRegistryItem("浮点数操作支持-直接输入")]
    public class SupportOperation_EEB_DirectFloat : SupportOperation_DirectInput<Entity, Entity, EntityState_Buff, float, OperationOptionsForFloat>
    {

    }
    #region 接口实现
    public interface ITargetOperationFloatEEB : ITargetOperation<Entity, Entity, EntityState_Buff, float, float, OperationOptionsForFloat>
    {

    }

    #endregion
}
