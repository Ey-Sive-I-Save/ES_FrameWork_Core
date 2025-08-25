using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES {
    public class 原件 : MonoBehaviour
    {
        [HideLabel]
        public VersionedList<string> source = new VersionedList<string>();
    }
}
