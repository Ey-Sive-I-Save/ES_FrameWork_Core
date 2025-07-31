using ES;
using ES.EvPointer;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES
{

    [Serializable,TypeRegistryItem("标准原型值池")]
    public class ArchPool : IArchitecture, IInittable
    {
        [LabelText("初始化数据包"),TitleGroup("初始化", alignment:TitleAlignments.Centered)]
        public ArchitectureDataPack applyPack;
        [LabelText("初始化自选数组"), TitleGroup("初始化", alignment: TitleAlignments.Centered)]
        public ArchitectureDataInfo[] applyInfos;
        [LabelText("预定义(仅编辑器)"),HideInPlayMode,SerializeReference, TitleGroup("初始化", alignment: TitleAlignments.Centered)]
        public List<IArchitectureValue> InitValues = new List<IArchitectureValue>();
        [LabelText("原型值池"),ShowInInspector, HideInEditorMode,SerializeReference]
        public Dictionary<string, IArchitectureValue> ArchValues = new Dictionary<string, IArchitectureValue>();


        public void Init(params object[] ps)
        {
            ArchValues ??= new Dictionary<string, IArchitectureValue>();
            foreach(var i in InitValues)
            {
                if (i != null)
                {

                }
            }
        }
        public void AddNewArchValue(EnumCollect.ArchitectureValueType architecture,string key,object value=null)
        {
            if (ArchValues.TryGetValue(key,out var vv))
            {
                vv.SetValue(value);
            }
            else
            {

            }
        }
        public void AddNewArchValue2(EnumCollect.ArchitectureValueType architecture, string key, object value = null)
        {
            if (ArchValues.TryGetValue(key, out var vv))
            {
                vv.SetValue(value);
            }
        }
    }
    public interface IArchitecture
    {

    }


    public interface IWithArchitecture 
    {
        public abstract IArchitecture GetArchitecture { get; }
    }
}



#region 过时
[Serializable, TypeRegistryItem("标准原型-键池-过时")]
public class _OBSULUTE_ArchitecturePool_OBSULUTE : IArchitecture, IInittable
{
    [LabelText("键值参数池"), SerializeReference]
    public ArchitectureKeyValuePoolTypeListIOC_OBSULUTE KeyValueParamIOC = new ArchitectureKeyValuePoolTypeListIOC_OBSULUTE();

    [LabelText("参数初始化数据包")]
    public ArchitectureDataPack applyPack;

    [DetailedInfoBox("为何是预览??", "IOC不支持持久序列化，仅支持运行时，所以已经准备好一个数据包，在Start时载入数据来初始化,这样做的话也有利于一套数据多次使用，并且有望实现减少运行时数据量(只存动态标签即可)", InfoMessageType.Warning)]
    [Button("初始化预览")]
    public void Init(params object[] ps)
    {
        KeyValueParamIOC ??= new ArchitectureKeyValuePoolTypeListIOC_OBSULUTE();
        //
        if (!KeyValueParamIOC.Groups.ContainsKey(EnumCollect.ArchitectureValueType.DynamicTag))
        {
            KeyValueParamIOC.Groups.Add(EnumCollect.ArchitectureValueType.DynamicTag, new List<IArchitectureValue>());
        }
        if (!KeyValueParamIOC.Groups.ContainsKey(EnumCollect.ArchitectureValueType.FloatValue))
        {
            KeyValueParamIOC.Groups.Add(EnumCollect.ArchitectureValueType.FloatValue, new List<IArchitectureValue>());
        }
        if (!KeyValueParamIOC.Groups.ContainsKey(EnumCollect.ArchitectureValueType.IntValue))
        {
            KeyValueParamIOC.Groups.Add(EnumCollect.ArchitectureValueType.IntValue, new List<IArchitectureValue>());
        }
        if (!KeyValueParamIOC.Groups.ContainsKey(EnumCollect.ArchitectureValueType.BoolValue))
        {
            KeyValueParamIOC.Groups.Add(EnumCollect.ArchitectureValueType.BoolValue, new List<IArchitectureValue>());
        }
        if (!KeyValueParamIOC.Groups.ContainsKey(EnumCollect.ArchitectureValueType.StringValue))
        {
            KeyValueParamIOC.Groups.Add(EnumCollect.ArchitectureValueType.StringValue, new List<IArchitectureValue>());
        }
        if (applyPack != null)
        {
            foreach (var i in applyPack.allInfos)
            {
                var arch = i.Value.getArch;
                if (arch is ArchitectureTypeValue_DynamicTag tag)
                {
                    KeyValueParamIOC.TryAdd(EnumCollect.ArchitectureValueType.DynamicTag, tag);
                }
                else if (arch is ArchitectureTypeValue_Float f)
                {
                    KeyValueParamIOC.TryAdd(EnumCollect.ArchitectureValueType.FloatValue, f);
                }
                else if (arch is ArchitectureTypeValue_Int int_)
                {
                    KeyValueParamIOC.TryAdd(EnumCollect.ArchitectureValueType.IntValue, int_);
                }
                else if (arch is ArchitectureTypeValue_String str)
                {
                    KeyValueParamIOC.TryAdd(EnumCollect.ArchitectureValueType.StringValue, str);
                }
                else if (arch is ArchitectureTypeValue_Bool bo)
                {
                    KeyValueParamIOC.TryAdd(EnumCollect.ArchitectureValueType.BoolValue, bo);
                }
                /*  else if (arch is ArchitectureTypeValue_DynamicTag g)
                  {
                      KeyValueParamIOC.AddElement(EnumCollect.ArchitectureKeyValuePoolType.DynamicTag, arch);
                  }*/
            }
        }
    }
}
#endregion

