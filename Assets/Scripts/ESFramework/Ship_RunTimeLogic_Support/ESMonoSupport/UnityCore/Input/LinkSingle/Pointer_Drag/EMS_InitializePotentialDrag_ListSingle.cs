using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ES {
    [AddComponentMenu("<ES>ESM支持/输入/初始化拖动-指定接收目标")]
    public class EMS_InitializePotentialDrag_LinkSingle : EMS_Abstract_LinkSingle<Link_EMS_InitalizedPotentialDrag>, IInitializePotentialDragHandler
    {
        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            OnLink(new Link_EMS_InitalizedPotentialDrag() { eventData = eventData });
        }
    }
}
