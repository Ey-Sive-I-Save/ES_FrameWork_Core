using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ES
{
    [AddComponentMenu("<ES>ESM支持/输入/取消-可接收列表")]
    public class EMS_Cancel_LinkList : MonoBehaviour, ICancelHandler
    {
        public LinkReceiveList<Link_BaseData> Links = new LinkReceiveList<Link_BaseData>();
         
        public void OnCancel(BaseEventData eventData)
        {
            Links.SendLink(new Link_BaseData() { eventData = eventData });
        }
    }
}
