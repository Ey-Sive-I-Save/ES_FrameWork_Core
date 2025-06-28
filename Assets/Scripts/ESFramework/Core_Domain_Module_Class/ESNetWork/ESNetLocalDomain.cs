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
        [NonSerialized] public ModuleLocal_LocalFunctionBlock Modole_BlockFunc;
    }
    [Serializable]
    public abstract class LocalModuleForESNetManager : Module<ESNetManager, ESNetLocalDomain>
    {

    }
    [Serializable,TypeRegistryItem("本地功能拦截")]
    public class ModuleLocal_LocalFunctionBlock  : LocalModuleForESNetManager
    {
        [LabelText("别人的拦截模块")]public ESTypeSelecter ForModuleOtherBan;
        [LabelText("自己的拦截模块")] public ESTypeSelecter ForModuleSelfBan;
        [LabelText("别人的拦截脚本")] public ESTypeSelecter ForESmonoOther;
        protected override string Description_ => "这个功能用于拦截一些非自己掌控的脚本和模块的运转，防止运行问题";
        protected override void CreateRelationshipOnly()
        {
            base.CreateRelationshipOnly();
            Domain.Modole_BlockFunc = this;
        }
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
