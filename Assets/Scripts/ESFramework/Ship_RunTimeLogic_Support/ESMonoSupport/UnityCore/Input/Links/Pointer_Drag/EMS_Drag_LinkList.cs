using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ES {
    [AddComponentMenu("<ES>ESM支持/输入/拖动中-可接收列表")]
    public class EMS_Drag_LinkList : EMS_Abstract_LinkList<Link_EMS_Drag>, IDragHandler
    {
        public void OnDrag(PointerEventData eventData)
        {
            SendLink(new Link_EMS_Drag() { eventData = eventData });
        }
    }
}
