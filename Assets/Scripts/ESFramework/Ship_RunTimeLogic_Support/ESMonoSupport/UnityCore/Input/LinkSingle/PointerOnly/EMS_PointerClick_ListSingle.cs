using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ES {
    [AddComponentMenu("<ES>ESM支持/输入/光标点击-指定接收目标")]
    public class EMS_PointerClick_LinkSingle : EMS_Abstract_LinkSingle<Link_EMS_PointerClick>, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            OnLink(new Link_EMS_PointerClick() { eventData=eventData });
        }
    }
}
