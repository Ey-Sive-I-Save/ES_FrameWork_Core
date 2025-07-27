using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ES {
    [AddComponentMenu("<ES>ESM支持/输入/光标进入-可接收列表")]
    public class EMS_PointerEnter_LinkList : EMS_Abstract_LinkList<Link_EMS_PointerEnter>, IPointerEnterHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
           SendLink(new Link_EMS_PointerEnter() { eventData = eventData });
        }
    }
}
