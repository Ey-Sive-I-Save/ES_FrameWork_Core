using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES
{
    [AddComponentMenu("<ES>ESM支持/物理/2D触发中-可接收列表")]
    public class EMS_Trigger2DStay_LinkList : EMS_Abstract_LinkList<Link_EMS_Collider2DStay>
    {
        
        private void OnTriggerStay2D(Collider2D collider)
        {
            Links.SendLink(new Link_EMS_Collider2DStay() { Collider2D = collider, posAT = collider.ClosestPoint(transform.position) }); ;
        }
    }
}
