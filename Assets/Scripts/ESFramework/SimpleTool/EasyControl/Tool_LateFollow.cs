using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ES.MonoTool
{
    [AddComponentMenu("<ES>Tool/LateFollowIt(在LateUpdate跟随)")]
    public class Tool_LateFollow : MonoBehaviour
    {
        public Transform Follow; 
        void Start()
        {

        }


        void Update()
        {

        }
        [Button("测试")]
        private void LateUpdate()
        {
            if (Follow != null)
                transform.position = Follow.position;
            transform.rotation = Follow.rotation;
        }
    }
}
