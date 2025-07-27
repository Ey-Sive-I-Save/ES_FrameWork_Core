using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ES {
    [AddComponentMenu("<ES>ESM支持/输入/鼠标按下-指定接收目标")]
    public class EMS_PointerUp_LinkSingle
        : EMS_Abstract_LinkSingle<Link_EMS_PointerUp>, IPointerUpHandler
    {
        public void OnPointerUp(PointerEventData eventData)
        {
            SendLink(new Link_EMS_PointerUp() { eventData = eventData });
        }
    }
}
