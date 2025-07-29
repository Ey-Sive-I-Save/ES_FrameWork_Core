using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES {
    [AddComponentMenu("<ES>ESM支持/物理/2D触发出-指定接收目标")]
    public class EMS_Trigger2DExit_LinkSingle : EMS_Coliider2D_Abstract_LinkSingle<Link_EMS_Trigger2DExit>
    {
        private void OnTriggerExit2D(Collider2D collider2D)
        {
            Link_?.OnLink(new Link_EMS_Trigger2DExit() {  collider = collider2D, posAT = collider2D.ClosestPoint(transform.position) }); ;
        }
    }
}
