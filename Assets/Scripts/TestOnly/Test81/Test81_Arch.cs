using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace ES {
    public class Test81_Arch : MonoBehaviour,IReceiveLink<Link_ArchEvent_FloatChange>,IReceiveChannelLink<string, Link_ArchEvent_FloatChange>
    {
       /* [Button]
        public void aa()
        {
            unityEvent.Invoke();
        }
        [ESDragToFieldSolver(ESDragToFieldSolverOptions.UnityEventNewInvoke)]
        public UnityEvent unityEvent;*/
        public ArchPool pool = new ArchPool();
        

        private void Awake()
        {
            pool.Init();
        }
        private void OnEnable()
        {
            pool.Enable();
        }
        private void OnDisable()
        {
            pool.Disable();
        }
        public int i2 = 0;
        private void Start()
        {
            pool.TryAddNewArchValueBySplits(EnumCollect.ArchitectureValueType.FloatValue, "float1", 5);
            pool.TryAddNewArchValueBySplits(EnumCollect.ArchitectureValueType.FloatValue, "float2", 5);
            pool.TryAddNewArchValueBySplits(EnumCollect.ArchitectureValueType.FloatValue, "float3", 5);
            pool.TryAddNewArchValueBySplits(EnumCollect.ArchitectureValueType.FloatValue, "float5", 5);
            pool.TryAddNewArchValueBySplits(EnumCollect.ArchitectureValueType.FloatValue, "float7", 5,true);
            field = typeof(Test81_Arch).GetField("fff2");
        }
        FieldInfo field;
        private int count;

        public float fff2;
        private void Update()
        {
            for (int i = 0; i < 100_0000; i++)
            {
                count++;
               // field.SetValue(this, count);
                pool.SetFloatDirect("float3",count);
            }
        }
       
        public void OnLink(Link_ArchEvent_FloatChange link)
        {
            Debug.Log("从"+link.Value_Pre+"到"+link.Value_Now);
        }

        public void OnLink(string channel, Link_ArchEvent_FloatChange link)
        {
            Debug.Log(channel+"从" + link.Value_Pre + "到" + link.Value_Now);
        }
    }
}
