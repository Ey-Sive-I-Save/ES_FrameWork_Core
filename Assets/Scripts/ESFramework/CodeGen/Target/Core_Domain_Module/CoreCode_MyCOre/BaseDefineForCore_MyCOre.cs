using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 namespace ES{ 
     
     
     public partial class MyCOre :Core
     {
         protected override void OnAwakeRegisterOnly()
         {
             RegisterAllDomains(Domain_Normal,Domain_Next);
             }
         [TabGroup("新的域", TextColor = "@Editor_DomainTabColor(Domain_Normal)")]
         [SerializeReference,InlineProperty, HideLabel] 
         public MyCOreNormalDomain Domain_Normal; [TabGroup("下一个", TextColor = "@Editor_DomainTabColor(Domain_Next)")]
         [SerializeReference,InlineProperty, HideLabel] 
         public MyCOreNextDomain Domain_Next;
         }
     
    
     [Serializable, TypeRegistryItem("新核心新的域扩展域")]
     public partial class MyCOreNormalDomain :Domain<MyCOre, MyCOreNormalModule>
     {
         [NonSerialized] public MyCOreNormalModule_Run Module_Run;[NonSerialized] public MyCOreNormalModule_Jump Module_Jump;
         }
     
    
     [Serializable, TypeRegistryItem("新核心下一个扩展域")]
     public partial class MyCOreNextDomain :Domain<MyCOre, MyCOreNextModule>
     {
         [NonSerialized] public MyCOreNextModule_Net Module_Net;
         }
     
    
     [Serializable, TypeRegistryItem("新核心新的域域扩展模块")]
     public abstract class MyCOreNormalModule :Module<MyCOre, MyCOreNormalDomain>
     {
         /* 抽象模块,核心:MyCOre,域:MyCOreNormalDomain */
         }
     
    
     [Serializable, TypeRegistryItem("新核心下一个域扩展模块")]
     public abstract class MyCOreNextModule :Module<MyCOre, MyCOreNextDomain>
     {
         /* 抽象模块,核心:MyCOre,域:MyCOreNextDomain */
         }
     
    
     }

//ES已修正