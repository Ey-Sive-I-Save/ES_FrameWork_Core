using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace ES
{
    public class c5 : MonoBehaviour
    {
        [MyCustom("")]
        [ESBackGround(0.5f,0.5f,0.5f)]
        public Vector2 vv;
        [ESBackGround(0, 0, 0.5f,0.2f)]
        public Vector3 vvv;
        [ESBackGround("blue",0.25f,0.4f)]
        public Vector2 vv1;
        [ESBackGround("black")]
        public Vector3 vvv2;
       [ESBackGround()]
        public Vector2 vv3;
       [ESBackGround("yellow",0.5f)]
        public NormalDomainForEntity vvv4;
        [PropertySpace()]

        void Start()
        {

        }


        void Update()
        {

        }
      
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
        public class MyCustomAttribute : Attribute
        {
            // 必需的参数，通过构造函数传入
            public MyCustomAttribute(string requiredParam)
            {
                this.RequiredParam = requiredParam;
            }

            // 公共属性，可以作为命名参数（可选参数）使用
            public int OptionalParam { get; set; }

            // 另一个可选参数
            public string AnotherOptionalParam { get; set; }

            // 存储必需参数的属性
            public string RequiredParam { get; private set; }
        }
       
    }
}
