using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ES {
    [AddComponentMenu("<ES>ESM支持/输入/光标进入-指定接收目标")]
    public class EMS_PointerEnter_LinkSingle : EMS_Abstract_LinkSingle<Link_EMS_PointerEnter>, IPointerEnterHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            OnLink(new Link_EMS_PointerEnter() { eventData = eventData });
        }
    }
}
