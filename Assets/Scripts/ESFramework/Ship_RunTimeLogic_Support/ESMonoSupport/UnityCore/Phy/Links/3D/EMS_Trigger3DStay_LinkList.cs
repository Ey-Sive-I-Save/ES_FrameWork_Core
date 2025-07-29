using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES
{
    [AddComponentMenu("<ES>ESM支持/物理/3D触发中-可接收列表")]
    public class EMS_Trigger3DStay_LinkList : EMS_Coliider_Abstract_LinkList<Link_EMS_Trigger3DStay>
    {
        private void OnTriggerStay3D(Collider collider)
        {
            SendLink(new Link_EMS_Trigger3DStay() { collider = collider, posAT = collider.ClosestPoint(transform.position) }); ;
        }
    }
}
