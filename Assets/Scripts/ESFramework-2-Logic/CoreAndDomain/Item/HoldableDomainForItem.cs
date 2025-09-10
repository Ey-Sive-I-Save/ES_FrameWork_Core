using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES
{
    [Serializable]
    public class  HoldableDomainForItem : Domain<Item, HoldableModuleForItem>
    {
        //捡起来！
    }
    [Serializable]
    public abstract class HoldableModuleForItem : Module<Item,HoldableDomainForItem>
    {

    }
}
