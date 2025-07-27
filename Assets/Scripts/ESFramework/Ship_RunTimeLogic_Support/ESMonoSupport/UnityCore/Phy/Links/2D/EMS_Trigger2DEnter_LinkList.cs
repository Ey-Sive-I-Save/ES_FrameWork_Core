using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES {
    [AddComponentMenu("<ES>ESM支持/物理/2D触发入-可接收列表")]
    public class EMS_Trigger2DEnter_LinkList : EMS_Abstract_LinkList<Link_EMS_Trigger2DEnter>
    {
        private void OnTriggerEnter2D(Collider2D collider)
        {
           SendLink(new Link_EMS_Trigger2DEnter() { Trigger2D = collider, posAT = collider.ClosestPoint(transform.position) }); ;
        }
    }
}
