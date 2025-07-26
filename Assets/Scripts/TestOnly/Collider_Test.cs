using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES {
    public class Collider_Test : MonoBehaviour
    {
        public float ff;
        private void OnCollisionEnter(Collision collision)
        {
            ff += 1f;
        }
        private void OnCollisionStay(Collision collision)
        {
            ff += 0.01f;
        }
    }
}
