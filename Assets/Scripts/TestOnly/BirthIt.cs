using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ES
{
    public class BirthIt : MonoBehaviour
    {
        public GameObject use;
        public int once = 10;
        public LinkMono LinkMono;
        void Start()
        {

        }

        public NormalDomainForEntity domain;
        void Update()
        {
           /* for (int i = 0; i < 1_0000_0000;)
            {
                if (LinkMono.next.next.value > -1) i++; 
            }*/
/*            if (Keyboard.current.pKey.isPressed)
            {
                Instantiate(use);
                ESDebugMaster.entityNum += once;
            }*/
        }
    }
}
