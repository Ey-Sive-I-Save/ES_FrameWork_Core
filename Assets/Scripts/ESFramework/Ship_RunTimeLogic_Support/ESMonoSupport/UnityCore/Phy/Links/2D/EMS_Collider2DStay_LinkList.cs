using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES {
    [AddComponentMenu("<ES>ESM支持/物理/2D碰撞中-可接收列表")]
    public class EMS_Collider2DStay_LinkList : EMS_Coliider2D_Abstract_LinkList<Link_EMS_Collider2DStay>
    {
        private void OnCollisionStay2D(Collision2D collision)
        {
            Links.SendLink(new Link_EMS_Collider2DStay() { collider = collision.collider, posAT = collision.contacts[0].point });
        }
    }
}
