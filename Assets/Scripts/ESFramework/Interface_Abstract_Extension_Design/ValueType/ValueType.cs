using ES.EvPointer;
using ES;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.Rendering.DebugUI;
using System.Runtime.CompilerServices;

namespace ES
{
    

    [Serializable/*原型全匹配安全列表IOC*/]
    public sealed class ArchutectureTypeMatchSafeListIOC : SafeTypeMatchKeyGroup<IArchitecture>
    {
        
    }
 

   
    



  
    #region 特殊IOC —— 原型参数键池专属参数集合
    [Serializable, TypeRegistryItem("原型参数IOC")/*技能树参数分类-*/]
    public class ArchitectureKeyValuePoolTypeListIOC_OBSULUTE : KeyGroup<EnumCollect.ArchitectureValueType, IArchitectureValue>
    {
        public Action<EnumCollect.ArchitectureValueType, IArchitectureValue> OnHandle = (at, who) => { };
    }

    #endregion



}