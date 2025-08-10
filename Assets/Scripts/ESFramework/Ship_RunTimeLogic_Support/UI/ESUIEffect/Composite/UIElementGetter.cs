using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES {
    public class ESUIElementGetter_DirectHandRefer : ESUIElement_ON_Getter
    {
        [LabelText("手动引用")]
        public ESUIElementCore element;
        public override ESUIElementCore Get(ESUIElementCore on, ESUIElementCore from)
        {
            return element;
        }
    }
}
