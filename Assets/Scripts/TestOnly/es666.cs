using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES
{

    public class es666 : MonoBehaviour
    {
        const int Count = 400_000;
        public Dictionary<aaa, int> pairs = new Dictionary<aaa, int>();
        public List<aaa> aaasss = new List<aaa>();
        private void Start()
        {
            for (int i = 0; i < Count; i++)
            {
                var use = new aaa() { b=i };
                pairs.Add(use,i);
                aaasss.Add(use);
            }
           
        }
        private void Update()
        {
            /*aaasss = new List<aaa>(Count);*/
            aaa eee = new aaa() { b2 = 7 };
            for (int i = 0; i < Count; i++)
            {
               
            }
            aaasss.Clear();
        }
        public void ttt( aaa a)
        {
            
        }
        [Serializable]
        public struct aaa
        {
            public float f;
            public float a;
           public int b;
            public float e;
            public float p;
            public float sf;
            public float a1;
            public float b2;
            public float e3;
            public float p4;

            public override int GetHashCode()
            {
                return b;
            }
        }
    }
}

