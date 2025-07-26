using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


namespace ES
{

    /*  TargetOperation
     * 导向操作，他的目的是把一个效果指定到特定的无解析值<值一般有数值和引用两种>上，
         ，经常是和其他内容配合的
    */
    public interface ITargetOperation<On, From, With, TargetType,OpeationType, OpeationOptions> : IOperation<On, From, With>
    {
        public abstract void TargetOpeation(On on, From from, With with, OpeationType Opeation_, OpeationOptions SelectType_);

        public abstract void TargetCancel(On on, From from, With with, OpeationType Opeation_, OpeationOptions SelectType_);
    }

    public abstract class TargetOperation<On, From, With, TargetType, OpeationType, WithSelector> : ITargetOperation<On, From, With, TargetType, OpeationType, WithSelector>
    {
        public abstract void TargetCancel(On on, From from, With with, OpeationType Opeation_, WithSelector SelectType_);
        public abstract void TargetOpeation(On on, From from, With with, OpeationType Opeation_, WithSelector SelectType_);
        protected abstract void ExpandOperation(ref TargetType or, OpeationType Opeation, WithSelector SelectType_);
        protected abstract void ExpandCancel(ref TargetType or, OpeationType Opeation, WithSelector SelectType_);
    }

    #region 演示
    //Float导向
    public abstract class TargetFloatOperation<On, From, With> : TargetOperation<On, From, With, float, float, OperationOptionsForFloat>
    {
        protected sealed override void ExpandOperation(ref float or, float Opeation, OperationOptionsForFloat SelectType_)
        {
            or = KeyValueMatchingUtility.Function.OpearationFloat_Inline(or, Opeation, SelectType_);
        }
        protected sealed override void ExpandCancel(ref float or, float Opeation, OperationOptionsForFloat SelectType_ = OperationOptionsForFloat.Add)
        {
            or = KeyValueMatchingUtility.Function.OpearationFloat_Cancel_Inline(or, Opeation, SelectType_);
        }
    }

    [Serializable]
    //定义类型，Entity,Entity,Buff
    public abstract class TargetFloatOperation_EEBuff : TargetFloatOperation<Entity, Entity, EntityState_Buff> , ITargetOperationFloatEEB
    {
        
    }

    [Serializable,TypeRegistryItem("血量修改导向")]
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
    #endregion

    #region 接口实现
    public interface ITargetOperationFloatEEB : ITargetOperation<Entity,Entity,EntityState_Buff,float,float,OperationOptionsForFloat>
    {

    }

    #endregion
}
