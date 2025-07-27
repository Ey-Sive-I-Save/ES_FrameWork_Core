using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ES
{
    [AddComponentMenu("<ES>ESM支持/输入/取消-指定接收目标")]
    public class EMS_Cancel_ListSingle : MonoBehaviour, ICancelHandler
    {
        public IReceiveLink<Link_BaseData> Link;

        public void OnCancel(BaseEventData eventData)
        {
            Link?.OnLink(new Link_BaseData() { eventData = eventData });
        }
    }
}
