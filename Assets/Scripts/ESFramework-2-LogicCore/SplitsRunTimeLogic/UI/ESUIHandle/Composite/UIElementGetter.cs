using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES {
    [Serializable,TypeRegistryItem("UI元素来源-手动引用")]
    public class ESUIElementGetter_DirectHandRefer : ESUIElementGetter
    {
        [LabelText("手动引用")]
        public ESUIElementCore element;
        public sealed override ESUIElementCore Get(ESUIElementCore on, ESUIElementCore from)
        {
            return element;
        }
    }
    [Serializable, TypeRegistryItem("UI元素来源-自己")]
    public class ESUIElementGetter_Self : ESUIElementGetter
    {
        public sealed  override ESUIElementCore Get(ESUIElementCore on, ESUIElementCore from)
        {
            return on;
        }
    }
    [Serializable, TypeRegistryItem("UI元素来源-来自(默认MyPanel)")]
    public class ESUIElementGetter_From : ESUIElementGetter
    {
        public sealed override ESUIElementCore Get(ESUIElementCore on, ESUIElementCore from)
        {
            return from;
        }
    }
}
