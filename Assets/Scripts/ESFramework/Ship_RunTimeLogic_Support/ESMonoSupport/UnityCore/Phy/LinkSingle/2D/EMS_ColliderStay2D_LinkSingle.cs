using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES {
    [AddComponentMenu("<ES>ESM支持/物理/2D碰撞中-指定接收目标")]
    public class EMS_ColliderStay2D_LinkSingle : EMS_Abstract_LinkSingle<Link_EMS_Collider2DStay>
    {
        private void OnCollisionStay2D(Collision2D collision)
        {
            Link_?.OnLink(new Link_EMS_Collider2DStay() {  Collider2D = collision.collider, posAT = collision.contacts[0].point });
        }
    }
}
