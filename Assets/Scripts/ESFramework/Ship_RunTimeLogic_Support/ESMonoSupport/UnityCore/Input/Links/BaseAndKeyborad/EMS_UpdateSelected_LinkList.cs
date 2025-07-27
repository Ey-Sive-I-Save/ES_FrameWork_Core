using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ES {
    [AddComponentMenu("<ES>ESM支持/输入/选择中-可接收列表")]
    public class EMS_UpdateSelected_LinkList : EMS_Abstract_LinkList<Link_EMS_UpdateSelected>, IUpdateSelectedHandler
    {
        public void OnUpdateSelected(BaseEventData eventData)
        {
            SendLink(new Link_EMS_UpdateSelected() { eventData = eventData });
        }
    }
}
