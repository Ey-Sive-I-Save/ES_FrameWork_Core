using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ES {
    [AddComponentMenu("<ES>ESM支持/输入/开始拖动-可接收列表")]
    public class EMS_BeginDrag_LinkSingle : EMS_Abstract_LinkSingle<Link_EMS_Drag>, IBeginDragHandler
    {
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            OnLink(new Link_EMS_Drag() { eventData = eventData });
        }
    }
}
