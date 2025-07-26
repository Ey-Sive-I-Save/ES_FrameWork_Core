using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 namespace ES{ 
     /*生成枚举集合SortEnum*/
    /*
     枚举类型名:WeaponType,枚举中文名武器类型
    */[Flags]
    public enum WeaponType{
        [ESMessage("message",55,77)][InspectorName("近战的")]Jin=1<<1,
        [ESMessage("message",888,0)][InspectorName("远程的")]Range=1<<2,
        [InspectorName("特殊的")]Speical=1<<3,
        [ESMessage("message",0,744)][InspectorName("远近兼修的")]MelleAndRange=Jin | Range,
        [ESMessage("message",-5,744)][InspectorName("远近兼修还会魔法的")]MelleAndRangeAndMagic=Jin | Range | MelleAndRange,
        }
    
     }

//ES已修正