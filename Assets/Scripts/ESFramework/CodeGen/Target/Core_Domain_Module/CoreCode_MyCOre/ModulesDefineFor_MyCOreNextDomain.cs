using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 namespace ES{ 
     /*MyCOreNextDomain （新核心下一个扩展域）的*/
     [Serializable, TypeRegistryItem("扩展模块_特殊")]
     public class MyCOreNextModule_Special :MyCOreNextModule
     {
         
         }
     
    
     [Serializable, TypeRegistryItem("扩展模块_网络")]
     public class MyCOreNextModule_Net :MyCOreNextModule
     {
         protected override void CreateRelationshipOnly()
         {
             Domain.Module_Net = this;
             base.CreateRelationshipOnly();
             }
         }
     
    
     }

//ES已修正