using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ES {
    [AddComponentMenu("<ES>ESM支持/输入/鼠标按下-可接收列表")]
    public class EMS_PointerUp_LinkList : EMS_Abstract_LinkList<Link_EMS_PointerUp>, IPointerUpHandler
    {
        public void OnPointerUp(PointerEventData eventData)
        {
            SendLink(new Link_EMS_PointerUp() { eventData = eventData });
        }
    }
}
