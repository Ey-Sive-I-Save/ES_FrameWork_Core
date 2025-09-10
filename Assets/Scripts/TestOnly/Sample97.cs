using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace ES
{
    public class Sample97 : MonoBehaviour
    {
        ParticleSystem system;
        ParticleSystem.Particle[] particles;
        void test()
        {
            system.GetParticles(particles);
            // DO Change On <particles>
            system.SetParticles(particles);
        }
       /* public class MenagedCLass
        {
            public ushort Value = 0xBEEF;
        }
        [Button("DO")]
        public unsafe void Test()
        {
            var m =new MenagedCLass();
            fixed(ushort* fielfPTR = &m.Value)
            {
                MenagedCLass* mPTR = &m;
                Debug.Log("Ref Address" + (nuint)mPTR );
                Debug.Log("OBJ Address" + (nuint)(*(nuint**)mPTR));
                Debug.Log("Ref Address" + (nuint)fielfPTR);
                Debug.Log("Ref Address" + (nuint)((byte*)fielfPTR-*(byte**)mPTR));
            }
        }*/

    }
}
