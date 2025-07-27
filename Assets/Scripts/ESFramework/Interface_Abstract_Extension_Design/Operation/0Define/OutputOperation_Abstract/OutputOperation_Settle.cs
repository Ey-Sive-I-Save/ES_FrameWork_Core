using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES {
    public class OutputOpeationSettleFlag : OverLoadFlag<OutputOpeationSettleFlag>
    {

    }
    [TypeRegistryItem("布尔结算结果值"), Serializable]
    public class SettlementBool : Settlement<bool, SettleOperationBool, SettlementBool>
    {

    }
    public abstract class OutputOperationSettle<On, From, With, ValueType_> : IOutputOperation<On, From, With>
    {
        public abstract void TryOpeation(On on, From from, With with);
        public abstract void TryCancel(On on, From from, With with);
    }
    [Serializable]
    public abstract class OutputOpeationSettleFloatEEB : OutputOperationSettle<Entity, Entity, EntityState_Buff, float> ,IOutputOperationEEB
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
        public sealed override void TryOpeation(Entity on, Entity from, EntityState_Buff with)
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

    

    [Serializable, TypeRegistryItem("结算输出-测试专属-攻击力")]
    public class OutputOpeationBuffSettle_Test_Attack : OutputOpeationFloat_EEB
    {
        public override SettlementFloat GetSettlement(Entity on, Entity from, EntityState_Buff with)
        {
            return on.VariableData.Attack;
        }
    }
    [Serializable, TypeRegistryItem("结算输出-测试专属-暴击率")]
    public class OutputOpeationBuffSettle_Test_Cri : OutputOpeationFloat_EEB
    {
        public override SettlementFloat GetSettlement(Entity on, Entity from, EntityState_Buff with)
        {
            return on.VariableData.Cri;
        }
    }
    [Serializable, TypeRegistryItem("结算输出-测试专属-防御力")]
    public class OutputOpeationBuffSettle_Test_Defend : OutputOpeationFloat_EEB
    {
        public override SettlementFloat GetSettlement(Entity on, Entity from, EntityState_Buff with)
        {
            return on.VariableData.Defend;
        }
    }
}
