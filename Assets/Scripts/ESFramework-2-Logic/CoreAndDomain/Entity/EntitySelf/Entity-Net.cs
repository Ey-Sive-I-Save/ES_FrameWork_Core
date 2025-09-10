using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES {
    public partial class Entity
    {
        [ToggleGroup("IsNet"),LabelText("网络行为支持")]
        public EntityNetBehaviour NetBehaviour;
    }
}
