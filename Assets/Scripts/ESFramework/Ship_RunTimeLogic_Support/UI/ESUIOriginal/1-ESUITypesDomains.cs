using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ES
{
    [Serializable, TypeRegistryItem("原始UI扩展域")]
    public class ESUIOriginalDomain : ESUIDomain_Original<ESUIElementCore, ESUIOriginalModule>
    {

    }
    [Serializable, TypeRegistryItem("原始UI模块")]
    public abstract class ESUIOriginalModule : ESUIModule_Orignal<ESUIElementCore, ESUIOriginalDomain>
    {

    }
}
