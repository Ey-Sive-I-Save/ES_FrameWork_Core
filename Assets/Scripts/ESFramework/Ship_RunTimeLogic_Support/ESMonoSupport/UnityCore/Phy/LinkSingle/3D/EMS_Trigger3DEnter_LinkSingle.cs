using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES {
    [AddComponentMenu("<ES>ESM支持/物理/3D触发入-指定接收目标")]
    public class EMS_Trigger3DEnter_LinkSingle : EMS_Coliider_Abstract_LinkSingle<Link_EMS_Trigger3DEnter>
    {
        private void OnTriggerEnter3D(Collider collider3D)
        {
            OnLink(new Link_EMS_Trigger3DEnter() { collider = collider3D, posAT = collider3D.ClosestPoint(transform.position) }); ;
        }
    }
}
