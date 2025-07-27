using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES {
    public class Test727_0 : MonoBehaviour, IReceiveLink<Link_EMS_Drag>
    {
        private void OnEnable()
        {
            this.GetComponent<EMS_Drag_LinkSingle>().Link_ = this;
        }
        public void OnLink(Link_EMS_Drag link)
        {
            transform.position = link.eventData.position;
        }
    }
}
