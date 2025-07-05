using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ES
{
public class monoLifeTest : MonoBehaviour
{
        private void Awake()
        {
            Debug.Log("A");
        }

        private void OnEnable()
        {
            Debug.Log("En");
        }
        void Start()
    {
            Debug.Log("Start");
    }

    
    void Update()
    {
        
    }
}
}
