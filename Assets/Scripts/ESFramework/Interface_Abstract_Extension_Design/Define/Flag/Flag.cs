using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES
{ 
    /*Flag 为每类可能重复的同名行为定义了无损失的参数支持，让重载得以成立*/
    public class Flag<This> where This : Flag<This>
    {
        public static This flag;
    }
}

