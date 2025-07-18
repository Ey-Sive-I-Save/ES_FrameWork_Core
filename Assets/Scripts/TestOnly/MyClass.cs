
using UnityEngine;
using ES.EvPointer;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
    namespace ES{   
        [Serializable]
        public   class MyClass :IPointer
        {
            /*
 测试生成代码
*/         [LabelText("哈哈")] 
            public List<int> ints =new List<int>();

          
            public static string iii ;


                [Button]
                public   object Pick  (object a = null,object b = null, object c = null)
                {
                        
                if (a is int i)  
                {
                    ints.Add(i);
                }
                
            return a;
                }
                

        }
    }
