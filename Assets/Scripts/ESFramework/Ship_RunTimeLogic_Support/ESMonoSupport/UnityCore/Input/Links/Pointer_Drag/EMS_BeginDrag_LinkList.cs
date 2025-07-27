using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ES
{
    [AddComponentMenu("<ES>ESM支持/输入/开始拖动-可接收列表")]
    public class EMS_BeginDrag_LinkList : EMS_Abstract_LinkList<Link_EMS_BeginDrag>, IBeginDragHandler
    {
        public void OnBeginDrag(PointerEventData eventData)
        {
            SendLink(new Link_EMS_BeginDrag() { eventData = eventData });
        }
    }
}
