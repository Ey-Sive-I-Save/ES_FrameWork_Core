using ES;
using ES.EvPointer;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES {

    #region 原型值类型
    [Serializable, TypeRegistryItem("原型参数值_动态标签")]
    public class ArchitectureTypeValue_DynamicTag : ArchitectureKeyValuePoolTypeValue<bool>
    {
        [LabelText("标签是否生效")] public bool IsUseable = true;

        public override void SetValue(bool v)
        {
            IsUseable = v;
        }

        public override bool Value()
        {
            return IsUseable;
        }
    }
    [Serializable, TypeRegistryItem("原型参数值_浮点值")]
    public class ArchitectureTypeValue_Float : ArchitectureKeyValuePoolTypeValue<float>
    {
        [LabelText("当前浮点值")] public float floatValue = 10;

        public override void SetValue(float v)
        {
            Debug.Log("pp" + v);
            floatValue = v;
        }

        public override float Value()
        {
            return floatValue;
        }
    }
    [Serializable, TypeRegistryItem("原型参数值_整数值")]
    public class ArchitectureTypeValue_Int : ArchitectureKeyValuePoolTypeValue<int>
    {
        [LabelText("当前整数值")] public int intValue = 10;

        public override void SetValue(int v)
        {
            intValue = v;
        }

        public override int Value()
        {
            return intValue;
        }
    }
    [Serializable, TypeRegistryItem("原型参数值_字符串值")]
    public class ArchitectureTypeValue_String : ArchitectureKeyValuePoolTypeValue<string>
    {
        [LabelText("当前字符串值")] public string strValue = "字符串";

        public override void SetValue(string v)
        {
            strValue = v;
        }

        public override string Value()
        {
            return strValue;
        }
    }
    [Serializable, TypeRegistryItem("原型参数值_布尔值")]
    public class ArchitectureTypeValue_Bool : ArchitectureKeyValuePoolTypeValue<bool>
    {
        [LabelText("当前布尔值")] public bool boolValue = false;

        public override void SetValue(bool v)
        {
            boolValue = v;
        }

        public override bool Value()
        {
            return boolValue;
        }

    }
    [Serializable, TypeRegistryItem("原型参数值类型")]
    public abstract class ArchitectureKeyValuePoolTypeValue<ValueT> : BaseWithStringKeyValue<ValueT>, IArchitectureKeyValuePoolTypeValue
    {
        public IArchitectureKeyValuePoolTypeValue getArch => this;

        public object TheKey => this.key.Key();

        public object TheValue => this.Value();

        public void SetValue(object o)
        {
            if (o is ValueT vv)
            {
                (this as BaseWithStringKeyValue<ValueT>).SetValue(vv);
            }

        }

    }
    public interface IArchitectureKeyValuePoolTypeValue
    {
        public abstract IArchitectureKeyValuePoolTypeValue getArch { get; }
        public object TheKey { get; }
        public object TheValue { get; }
        public void SetValue(object o);
        public void SetKey(object o);
    }
    #endregion
}
