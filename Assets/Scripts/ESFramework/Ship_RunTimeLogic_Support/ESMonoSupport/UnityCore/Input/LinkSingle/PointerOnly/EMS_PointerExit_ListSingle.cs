using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ES {
    [AddComponentMenu("<ES>ESM支持/输入/光标退出-指定接收目标")]
    public class EMS_PointerExit_LinkSingle : EMS_Abstract_LinkSingle<Link_EMS_PointerExit>, IPointerExitHandler
    {
        public void OnPointerExit(PointerEventData eventData)
        {
            OnLink(new Link_EMS_PointerExit() { eventData = eventData });
        }
    }
}
