using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES {
    [AddComponentMenu("<ES>ESM支持/物理/3D触发入-可接收列表")]
    public class EMS_Trigger3DEnter_LinkList : EMS_Abstract_LinkList<Link_EMS_Trigger3DEnter>
    {
        private void OnTriggerEnter3D(Collider collider)
        {
            Links.SendLink(new Link_EMS_Trigger3DEnter() { collider = collider, posAT = collider.ClosestPoint(transform.position) }); ;
        }
    }
}
