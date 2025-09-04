using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 namespace ES{ 
     /*MyCOreNextDomain （新核心下一个扩展域）的*/
     [Serializable, TypeRegistryItem("扩展模块_特殊")]
     public partial class MyCOreNextModule_Special :MyCOreNextModule
     {
         
         }
     
    
     [Serializable, TypeRegistryItem("扩展模块_特殊_新的剪影")]
     public partial class MyCOreNextModule_Special_TestModule1ss :MyCOreNextModule_Special
     {
         
         }
     
    
     [Serializable, TypeRegistryItem("扩展模块_特殊_新的剪影")]
     public partial class MyCOreNextModule_Special_TestModule244 :MyCOreNextModule_Special
     {
         
         }
     
    
     [Serializable, TypeRegistryItem("扩展模块_网络")]
     public partial class MyCOreNextModule_Net :MyCOreNextModule
     {
         protected override void CreateRelationshipOnly()
         {
             Domain.Module_Net = this;
             base.CreateRelationshipOnly();
             }
         }
     
    
     [Serializable, TypeRegistryItem("扩展模块_网络_新的剪影")]
     public partial class MyCOreNextModule_Net_TestModule发射 :MyCOreNextModule_Net
     {
         protected override void CreateRelationshipOnly()
         {
             Domain.Module_Net = this;
             base.CreateRelationshipOnly();
             }
         }
     
    
     [Serializable, TypeRegistryItem("扩展模块_网络_新的剪影")]
     public partial class MyCOreNextModule_Net_TestModuless :MyCOreNextModule_Net
     {
         protected override void CreateRelationshipOnly()
         {
             Domain.Module_Net = this;
             base.CreateRelationshipOnly();
             }
         }
     
    
     }

//ES已修正