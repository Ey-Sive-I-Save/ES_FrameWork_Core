using ES;
using ES.EvPointer;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ES.KeyValueMatchingUtility;
namespace ES
{
    public class TestRefer : MonoBehaviour
    {
        public ESCodeTreeTarget Target = new ESCodeTreeTarget();
        public ESCodeNode_Field field = new ESCodeNode_Field();
        public ESCodeNode_Comment comment = new ESCodeNode_Comment();
        public ESCodeNode_Method method = new ESCodeNode_Method();
        public ESCodeTree_Class Class = new ESCodeTree_Class();
        public int a(int a)
        {
            return 1;
        }
    }
}
