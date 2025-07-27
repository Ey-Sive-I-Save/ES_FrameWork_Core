using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES {
    [AddComponentMenu("<ES>ESM支持/物理/2D触发入-指定接收目标")]
    public class EMS_Trigger2DEnter_LinkSingle : EMS_Abstract_LinkSingle<Link_EMS_Trigger2DEnter>
    {
        private void OnTriggerEnter2D(Collider2D collider2D)
        {
            Link_?.OnLink(new Link_EMS_Trigger2DEnter() {    Trigger2D= collider2D, posAT = collider2D.ClosestPoint(transform.position) }); ;
        }
    }
}
