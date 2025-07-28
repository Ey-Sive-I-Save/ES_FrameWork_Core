using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES {
    [Serializable,TypeRegistryItem("原始扩展域")]
    public class ESObjectOriginalDomain : Domain<ESObject, ESObjectOriginalModule>
    {
          
    }
    [Serializable, TypeRegistryItem("原始模块")]
    public abstract class ESObjectOriginalModule : Module<ESObject, ESObjectOriginalDomain>
    {

    }
}
