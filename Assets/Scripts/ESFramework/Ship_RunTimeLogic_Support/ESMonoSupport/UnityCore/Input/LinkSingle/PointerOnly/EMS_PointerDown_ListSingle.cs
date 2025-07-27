using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ES {
    [AddComponentMenu("<ES>ESM支持/输入/鼠标按下-指定接收目标")]
    public class EMS_PointerDown_LinkSingle : EMS_Abstract_LinkSingle<Link_EMS_PointerDown>, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            OnLink(new Link_EMS_PointerDown() { eventData = eventData });
        }
    }
}
