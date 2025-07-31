using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

namespace ES
{
    public class SkillControllerSourceCore :Core, IWithArchitecture
    {
        [SerializeReference]
        public _OBSULUTE_ArchitecturePool_OBSULUTE SkillControllerArchitecture = new _OBSULUTE_ArchitecturePool_OBSULUTE();
        public IArchitecture GetArchitecture => SkillControllerArchitecture;
        private void Start()
        {
            SkillControllerArchitecture.Init();
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
