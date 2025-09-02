using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace ES
{
    public interface IWithSharedAndVariableData
    {
        ISharedData ORSharedData { get; set; }
        IVariableData ORVariableData { get; set; }
    }
    public interface IWithSharedAndVariableData<Shared_, Variable_> : IWithSharedAndVariableData
        where Shared_ : ISharedData
        where Variable_ : IVariableData
    {
        #region 两大属性
        Shared_ SharedData { get; set; }
        Variable_ VariableData { get; set; }

        ISharedData IWithSharedAndVariableData.ORSharedData{
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => SharedData;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => SharedData = (Shared_)value ; }

        IVariableData IWithSharedAndVariableData.ORVariableData
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => VariableData;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => VariableData = (Variable_)value;
        }
        #endregion
    }
    /* public static partial class KeyValueMatchingUtility
     {
          注册到KVMU-->双数据赋予  DataApply
     }*/
}
