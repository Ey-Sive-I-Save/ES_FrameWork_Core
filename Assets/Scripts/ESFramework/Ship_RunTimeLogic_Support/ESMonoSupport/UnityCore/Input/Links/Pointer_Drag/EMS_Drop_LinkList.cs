using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ES {
    [AddComponentMenu("<ES>ESM支持/输入/可拖动落入此处-可接收列表")]
    public class EMS_Drop_LinkList : EMS_Abstract_LinkList<Link_EMS_Drop>, IDropHandler
    {

        public void OnDrop(PointerEventData eventData)
        {
           /* eventData.pointerDrag*/
            SendLink(new Link_EMS_Drop() { eventData = eventData });
        }
    }
}
