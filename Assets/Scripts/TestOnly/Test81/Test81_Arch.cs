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
        [Button]
        public void aa()
        {
            unityEvent.Invoke();
        }
        [ESDragToFieldSolver(ESDragToFieldSolverOptions.UnityEventNewInvoke)]
        public UnityEvent unityEvent;
        private ArchPool pool = new ArchPool();
        [LabelText("拖入资产-自动解析AB查询键")]
        [ESDragToFieldSolver]
        public string Asset;
        [ESDragToFieldSolver]
        [LabelText("拖入资产-自动解析AB查询键")]
        public string Asset1;
        [ESDragToFieldSolver]
        [LabelText("拖入资产-自动解析AB查询键")]
        public string Asset2;
        [ESDragToFieldSolver]

        [LabelText("拖入资产-自动解析AB查询键")]
        public string Asset3;
        private float fff2;
        private void Awake()
        {
            pool.Init();
        }
        private void OnEnable()
        {
            pool.Enable();
            pool.LinkRCL_Float.AddReceive("float7",this);
        }
        private void OnDisable()
        {
            pool.Disable();
            pool.LinkRCL_Float.RemoveReceive("float7",this);
        }
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
        public int i2= 0;
        private void Update()
        {
            for (int i = 0; i < 1000_0000; i++)
            {
                ASS(3,2);
                /*i2++;
                field = typeof(Test81_Arch).GetField("fff2");
                field.SetValue(this,i2);*/
                // pool.SetFloatDirect("float1", i2);
            }
        }
        public void ASS()
        {
            i2++;
        }
        public void ASS(float f=0)
        {
            i2++;
        }
        public Entity s;
        public object ASS(float f = 0,int a=5)
        {
            i2++;
            return s;
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
