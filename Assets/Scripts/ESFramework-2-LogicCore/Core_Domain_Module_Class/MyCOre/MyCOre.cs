using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ES
{


    public partial class MyCOre : Core
    {
        // 新核心新的域扩展域：MyCOreNormalDomain,作为核心的Domain_Normal// 新核心下一个扩展域：MyCOreNextDomain,作为核心的Domain_Next

    }


    //新核心新的域扩展域
    public partial class MyCOreNormalDomain
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


    //新核心下一个扩展域
    public partial class MyCOreNextDomain
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