using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES {
    [AddComponentMenu("<ES>ESM支持/物理/3D碰撞入-可接收列表")]
    public class EMS_Collider3DEnter_LinkList : EMS_Coliider_Abstract_LinkList<Link_EMS_Collider3DEnter>
    {
        private void OnCollisionEnter3D(Collision collision)
        {
            Links.SendLink(new Link_EMS_Collider3DEnter() { collider = collision.collider, posAT = collision.contacts[0].point });
        }
    }
}
