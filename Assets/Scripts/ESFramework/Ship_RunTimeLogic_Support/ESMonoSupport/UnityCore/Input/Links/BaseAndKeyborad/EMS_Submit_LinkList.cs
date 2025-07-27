using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ES {
    [AddComponentMenu("<ES>ESM支持/输入/确定提交-可接收列表")]
    public class EMS_Submit_LinkList : MonoBehaviour, ISubmitHandler
    {
        public LinkReceiveList<Link_BaseData> Links = new LinkReceiveList<Link_BaseData>();

        public void OnSubmit(BaseEventData eventData)
        {
            Links.SendLink(new Link_BaseData() { eventData = eventData });
        }
    }
}
