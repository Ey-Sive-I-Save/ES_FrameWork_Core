using ES;
using ES.EvPointer;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


namespace ES {

    #region 原型值类型
    [Serializable, TypeRegistryItem("原型值_动态标签")]
    public class ArchitectureTypeValue_DynamicTag : ArchitectureValue<bool>
    {
        [LabelText("标签是否生效")] public bool value = true;
        public override EnumCollect.ArchitectureValueType ArchType => EnumCollect.ArchitectureValueType.DynamicTag;
        public override object TheValue { get { return value; } }

       

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sealed override void SetValue(object o)
        {
            if(o is bool b)
            {
                value = b;
            }else if(o is float f)
            {
                value = f > 0;
            }else if(o is string s)
            {
                value = s.IsNullOrWhitespace();
            }else if(o is UnityEngine.Object uo)
            {
                value = uo != null;
            }
        }
    }
    [Serializable, TypeRegistryItem("原型值_浮点值")]
    public class ArchitectureTypeValue_Float : ArchitectureValue<float>
    {
        [LabelText("浮点值")] public float value = 10;
        public override EnumCollect.ArchitectureValueType ArchType => EnumCollect.ArchitectureValueType.FloatValue;
        public override object TheValue { get { return value; } }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sealed override void SetValue(object v)
        {
            if(v is float f)
            {
                value = f;
            }else if(v is bool b)
            {
                value = b ? 1 : 0;
            }
        }
    }
    [Serializable, TypeRegistryItem("原型值_整数值")]
    public class ArchitectureTypeValue_Int : ArchitectureValue<int>
    {
        [LabelText("整数值")] public int value = 10;
        public override EnumCollect.ArchitectureValueType ArchType => EnumCollect.ArchitectureValueType.IntValue;
        public override object TheValue { get { return value; } }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sealed override void SetValue(object v)
        {
            if(v is int i)
            {
                value = i;
            }else if(v is float f)
            {
                value = Mathf.RoundToInt(f);
            }else if(v is bool b)
            {
                value = b ? 1 : 0;
            }else if(v is string s)
            {
                value = s?.Length??0;
            }
        }
    }
    [Serializable, TypeRegistryItem("原型参数值_字符串值")]
    public class ArchitectureTypeValue_String : ArchitectureValue<string>
    {
        [LabelText("字符串值")] public string value = "字符串";
        public override EnumCollect.ArchitectureValueType ArchType => EnumCollect.ArchitectureValueType.StringValue;
        public override object TheValue { get { return value; } }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void SetValue(object v)
        {
            value = v.ToString();
        }
    }
    [Serializable, TypeRegistryItem("原型参数值_布尔值")]
    public class ArchitectureTypeValue_Bool : ArchitectureValue<bool>
    {
        [LabelText("标签是否生效")] public bool value = true;
        public override EnumCollect.ArchitectureValueType ArchType => EnumCollect.ArchitectureValueType.BoolValue;
        public override object TheValue { get { return value; } }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sealed override void SetValue(object o)
        {
            if (o is bool b)
            {
                value = b;
            }
            else if (o is float f)
            {
                value = f > 0;
            }
            else if (o is string s)
            {
                value = s.IsNullOrWhitespace();
            }
            else if (o is UnityEngine.Object uo)
            {
                value = uo != null;
            }
        }
    }
    [Serializable, TypeRegistryItem("原型参数值类型")]
    public abstract class ArchitectureValue<ValueT> : IArchitectureValue
    {
        public IArchitectureValue getArch => this;
        public string key;
        public string TheKey => key;

        public abstract object TheValue { get; }
        public abstract EnumCollect.ArchitectureValueType ArchType { get; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void SetValue(object o);

        public  void DeepCloneFrom(IArchitectureValue t)
        {
            key = t.TheKey;
            SetValue(t.TheValue);
        }
    }
    public interface IArchitectureValue:IDeepClone<IArchitectureValue>
    {
        public abstract IArchitectureValue getArch { get; }
        public string TheKey { get; }
        public object TheValue { get; }
        public void SetValue(object o);
        public EnumCollect.ArchitectureValueType ArchType{get;}

    }
    #endregion
}
