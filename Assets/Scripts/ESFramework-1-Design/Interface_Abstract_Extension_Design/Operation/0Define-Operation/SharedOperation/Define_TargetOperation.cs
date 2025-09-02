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
            or = ESStaticDesignUtility.Function.OpearationFloat_Inline(or, Opeation, SelectType_);
        }
        protected sealed override void ExpandCancel(ref float or, float Opeation, OperationOptionsForFloat SelectType_ = OperationOptionsForFloat.Add)
        {
            or = ESStaticDesignUtility.Function.OpearationFloat_Cancel_Inline(or, Opeation, SelectType_);
        }
    }

  
    #endregion


}
