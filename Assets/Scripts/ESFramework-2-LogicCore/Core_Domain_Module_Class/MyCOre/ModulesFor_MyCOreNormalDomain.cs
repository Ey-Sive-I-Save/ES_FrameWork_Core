using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ES
{
    /*MyCOreNormalDomain （新核心新的域扩展域）的*//* 抽象模块MyCOreNormalModule【新核心新的域域扩展模块】,
   核心:MyCOre【新核心】,]n域:MyCOreNormalDomain 【新核心新的域扩展域】 */

    //扩展模块_跑
    public partial class MyCOreNormalModule_Run : MyCOreNormalModule
    {
        public InputAction hor;
        public InputAction ver;
        protected override void OnEnable()
        {
            base.OnEnable();
            hor.Enable();
            ver.Enable();
        }
        protected override void Update()
        {
            float x = hor.ReadValue<float>();
            float y = ver.ReadValue<float>();
            Core.transform.position += new Vector3(x, 0.1f, y) * Time.deltaTime;
            base.Update();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            hor.Disable();
            ver.Disable();
        }
    }


    //扩展模块_跑_新的剪影
    public partial class MyCOreNormalModule_Run_TestModule : MyCOreNormalModule_Run
    {
        protected override void OnEnable()
        {
            base.OnEnable();
        }
        protected override void Update()
        {
            base.Update();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
        }
    }


    //扩展模块_跑_实打实
    public partial class MyCOreNormalModule_Run_TestModule2 : MyCOreNormalModule_Run
    {
        protected override void OnEnable()
        {
            base.OnEnable();
        }
        protected override void Update()
        {
            base.Update();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
        }
    }


    //扩展模块_跳
    public partial class MyCOreNormalModule_Jump : MyCOreNormalModule
    {
        protected override void OnEnable()
        {
            base.OnEnable();
        }
        protected override void Update()
        {
            base.Update();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
        }
    }


    //扩展模块_吃
    public partial class MyCOreNormalModule_Eat : MyCOreNormalModule
    {
        protected override void OnEnable()
        {
            base.OnEnable();
        }
        protected override void Update()
        {
            base.Update();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
        }
    }


}

//ES已修正