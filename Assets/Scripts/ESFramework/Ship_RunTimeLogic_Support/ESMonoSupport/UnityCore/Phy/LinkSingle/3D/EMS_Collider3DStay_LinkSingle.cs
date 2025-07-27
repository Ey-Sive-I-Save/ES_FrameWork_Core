using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES {
    [AddComponentMenu("<ES>ESM支持/物理/3D碰撞中-指定接收目标")]
    public class EMS_Collider3DStay_LinkSingle : EMS_Abstract_LinkSingle<Link_EMS_Collider3DStay>
    {
        private void OnCollisionStay3D(Collision collision)
        {
            OnLink(new Link_EMS_Collider3DStay() { collider = collision.collider, posAT = collision.contacts[0].point });
        }
    }
}
