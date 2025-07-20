using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES
{
    public partial class TestCore : Core
    {
        [TabGroup("基本域", TextColor = "@Editor_DomainTabColor(Domain_Normal)")]
        [SerializeReference,InlineProperty, HideLabel] 
        public TestCoreNormalDomain Domain_Normal;
    }
    [Serializable,TypeRegistryItem("TestCore扩展域-常规")]
    public partial class TestCoreNormalDomain : Domain<TestCore, TestCoreNormalModule>
    {
        [NonSerialized] public TestCoreNormalModule_MyModule Module_MyModule;
         
    }
    [Serializable, TypeRegistryItem("TestCore常规扩展模块")]
    public abstract class TestCoreNormalModule : Module<TestCore, TestCoreNormalDomain>
    {

    }
    [Serializable, TypeRegistryItem("扩展模块_XX")]
    public class TestCoreNormalModule_MyModule : TestCoreNormalModule
    {
        protected override void CreateRelationshipOnly()
        {
            Domain.Module_MyModule = this;
            base.CreateRelationshipOnly();
        }
    }
}

