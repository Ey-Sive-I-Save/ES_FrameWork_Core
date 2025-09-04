using ES;
using ES.Pointer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES
{

    [Serializable/*为原型参数键值池准备的专用针*/]
    public abstract class PointerOnArch : IPointer<object, ArchPool, object, object>
    {
        public object Pick(ArchPool on = null, object from = null, object with = null)
        {
            throw new NotImplementedException();
        }

        public object Pick(object a = null, object b = null, object c = null)
        {
            return Pick(a as ArchPool, b,c);
        }
    }
}

