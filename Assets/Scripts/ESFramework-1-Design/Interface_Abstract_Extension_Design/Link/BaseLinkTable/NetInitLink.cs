using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES
{

    [Serializable]
    public struct Link_IDRequest : ILink
    {
        
    }
    [Serializable]
    public struct Link_IDSet : ILink
    {
        public int id;
    }
}

