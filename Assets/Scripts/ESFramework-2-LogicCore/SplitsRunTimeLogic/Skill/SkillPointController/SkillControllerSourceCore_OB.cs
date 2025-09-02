using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

namespace ES
{
    public class SkillControllerSourceCore_OB :Core, IWithArchitecture
    {
       
        public IArchitecture GetArchitecture => null;
        private void Start()
        {
           
            StartCoroutine(_CoroutineMaker_Obsolete.DelayOneFrameCoroutine(RefreshAll));
        }
        // Start is called before the first frame update
        [Button("刷新测试")]
        public void RefreshAll()
        {
            BroadcastMessage("ApplyArchitectureKeyValuePool", this);
        }
    }
}
