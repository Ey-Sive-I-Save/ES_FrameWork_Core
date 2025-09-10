using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES
{
    
    public class  ESNetLocalDomain : Domain<ESNetManager, LocalModuleForESNetManager>
    {
    }
    [Serializable]
    public abstract class LocalModuleForESNetManager : Module<ESNetManager, ESNetLocalDomain>
    {
        
    }
    

    /*
    
     [Serializable, TypeRegistryItem("Module名字")]
    public class TheModule : BaseModuleForXXX
    {
        
        protected override void Update()
        {
            
        }
         protected override void CreateRelationship()
        {
            base.CreateRelationship();
            //Domain.Module_XXX = this;  显性引用
        }
    }
    */
}
