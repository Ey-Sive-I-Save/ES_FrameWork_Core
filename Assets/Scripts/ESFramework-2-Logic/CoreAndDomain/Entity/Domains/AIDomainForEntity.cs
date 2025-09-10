using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace ES
{
    [Serializable, TypeRegistryItem("AI扩展域")]
    public class AIDomainForEntity : Domain<Entity, BaseAIModuleForDomainForEntity>
    {
       }
    [Serializable]
    public abstract class BaseAIModuleForDomainForEntity : Module<Entity, AIDomainForEntity>
    {

    }



    [Serializable, TypeRegistryItem("测试模块")]
    public class TestModule3 : BaseAIModuleForDomainForEntity
    {
        public override Type TableKeyType => typeof(TestModule3);
    }
}
