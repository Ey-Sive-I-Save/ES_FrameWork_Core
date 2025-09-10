using ES.Pointer;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ES
{
    [Serializable, TypeRegistryItem("状态机扩展域")]
    public class StateMachineDomainForEntity : Domain<Entity, StateMachineModuleForDomainForEntity>, IWithESMachine
    {
        public BaseOriginalStateMachine TheMachine => throw new NotImplementedException();
    }
    [Serializable]
    public abstract class StateMachineModuleForDomainForEntity : Module<Entity, StateMachineDomainForEntity>
    {
        
    }
  
}
