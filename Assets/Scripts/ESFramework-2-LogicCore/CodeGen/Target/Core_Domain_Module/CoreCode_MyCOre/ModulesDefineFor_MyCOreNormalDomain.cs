using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 namespace ES{ 
     /*MyCOreNormalDomain （新核心新的域扩展域）的*/
     [Serializable, TypeRegistryItem("扩展模块_跑")]
     public partial class MyCOreNormalModule_Run :MyCOreNormalModule
     {
         protected override void CreateRelationshipOnly()
         {
             Domain.Module_Run = this;
             base.CreateRelationshipOnly();
             }
         }
     
    
     [Serializable, TypeRegistryItem("扩展模块_跑_新的剪影")]
     public partial class MyCOreNormalModule_Run_TestModule :MyCOreNormalModule_Run
     {
         protected override void CreateRelationshipOnly()
         {
             Domain.Module_Run = this;
             base.CreateRelationshipOnly();
             }
         }
     
    
     [Serializable, TypeRegistryItem("扩展模块_跑_实打实")]
     public partial class MyCOreNormalModule_Run_TestModule2 :MyCOreNormalModule_Run
     {
         protected override void CreateRelationshipOnly()
         {
             Domain.Module_Run = this;
             base.CreateRelationshipOnly();
             }
         }
     
    
     [Serializable, TypeRegistryItem("扩展模块_跳")]
     public partial class MyCOreNormalModule_Jump :MyCOreNormalModule
     {
         protected override void CreateRelationshipOnly()
         {
             Domain.Module_Jump = this;
             base.CreateRelationshipOnly();
             }
         }
     
    
     [Serializable, TypeRegistryItem("扩展模块_吃")]
     public partial class MyCOreNormalModule_Eat :MyCOreNormalModule
     {
         
         }
     
    
     }

//ES已修正