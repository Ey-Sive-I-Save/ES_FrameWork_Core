using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ES
{
    public class ItemNetBehaviour : ESNetBehaviour<Item>
    {
        public override void TryConnectCoreObject(Item coreObject)
        {
            coreObject.NetBehaviour = this;
        }

        
    }
}
