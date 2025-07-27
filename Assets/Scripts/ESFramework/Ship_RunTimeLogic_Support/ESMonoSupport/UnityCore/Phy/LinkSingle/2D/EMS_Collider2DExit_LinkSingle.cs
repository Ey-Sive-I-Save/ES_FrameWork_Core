using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES {
    [AddComponentMenu("<ES>ESM支持/物理/2D碰撞出-指定接收目标")]
    public class EMS_Collider2DExit_LinkSingle : EMS_Abstract_LinkSingle<Link_EMS_Collider2DExit>
    {
        private void OnCollisionExit2D(Collision2D collision)
        {
            Link_?.OnLink(new Link_EMS_Collider2DExit() {  Collider2D = collision.collider, posAT = collision.contacts[0].point });
        }
    }
}
