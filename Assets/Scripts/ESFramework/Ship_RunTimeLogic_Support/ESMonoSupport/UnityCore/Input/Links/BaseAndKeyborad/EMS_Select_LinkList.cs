using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ES
{
    [AddComponentMenu("<ES>ESM支持/输入/选择-可接收列表")]
    public class EMS_Select  : MonoBehaviour, ISelectHandler
    {
        public LinkReceiveList<Link_BaseData> Links = new LinkReceiveList<Link_BaseData>();


        public void OnSelect(BaseEventData eventData)
        {
            Links.SendLink(new Link_BaseData() { eventData = eventData });
        }
    }
}
