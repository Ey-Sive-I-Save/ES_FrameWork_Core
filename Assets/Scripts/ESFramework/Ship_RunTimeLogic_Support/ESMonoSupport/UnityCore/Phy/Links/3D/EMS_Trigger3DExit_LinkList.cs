using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES {
    [AddComponentMenu("<ES>ESM支持/物理/3D触发出-可接收列表")]
    public class EMS_Trigger3DExit_LinkList : EMS_Coliider_Abstract_LinkList<Link_EMS_Trigger3DExit>
    {
        private void OnTriggerExit3D(Collider collider)
        {
            SendLink(new Link_EMS_Trigger3DExit() { collider = collider, posAT = collider.ClosestPoint(transform.position) }); ;
        }
    }
}
