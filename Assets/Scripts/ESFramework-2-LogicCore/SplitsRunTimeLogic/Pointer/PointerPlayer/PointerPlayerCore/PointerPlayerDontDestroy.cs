using ES.Pointer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES
{
    public class PointerPlayerDontDestroy : PointerPlayer
    {
        public override IPointer Pointer => throw new System.NotImplementedException();

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
