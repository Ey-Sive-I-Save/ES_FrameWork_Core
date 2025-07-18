using ES;
using ES.EvPointer;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ES.KeyValueMatchingUtility;
namespace ES
{
    public class TestRefer : MonoBehaviour
    {
        public int count = 10_0000;


        /*
         获得身上的Collider组件，并且“使用"
         */

        public ESReferLazy<Collider> Refer_col=new ESReferLazy<Collider>();

        public bool has=false;

        private void Start()
        {
            Refer_col.SetValueSourceGetter(()=>GetComponent<Collider>());
        }

        private void Update()
        {
            for (int i = 0; i < count; i++)
            {



                if (Refer_col != null)
                {
                    has = true;
                }



            }
        }
    }
}
