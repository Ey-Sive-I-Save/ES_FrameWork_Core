using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ES
{
    public partial class TestCore : Core
    {
        public NormalDomain normalDomain;
    }
    [Serializable]
    public partial class NormalDomain : Domain<TestCore, NormalModule>
    {

    }
    [Serializable]
    public partial class NormalModule : Module<TestCore, NormalDomain>
    {

    }
}
