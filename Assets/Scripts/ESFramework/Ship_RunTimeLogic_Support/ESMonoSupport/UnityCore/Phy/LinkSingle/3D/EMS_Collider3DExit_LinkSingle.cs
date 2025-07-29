using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES {
    [AddComponentMenu("<ES>ESM支持/物理/3D碰撞出-指定接收目标")]
    public class EMS_Collider3DExit_LinkSingle : EMS_Coliider_Abstract_LinkSingle<Link_EMS_Collider3DExit>
    {
        private void OnCollisionExit3D(Collision collision)
        {
           OnLink(new Link_EMS_Collider3DExit() { collider = collision.collider, posAT = collision.contacts[0].point });
        }
    }
}
