using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ES
{
    [AddComponentMenu("<ES>ESM支持/输入/滚轮滚动-指定接收目标")]
    public class EMS_Scroll_LinkList : EMS_Abstract_LinkList<Link_EMS_Scroll>, IScrollHandler
    {
        public void OnScroll(PointerEventData eventData)
        {
            SendLink(new Link_EMS_Scroll() { eventData = eventData });
        }
    }
}
