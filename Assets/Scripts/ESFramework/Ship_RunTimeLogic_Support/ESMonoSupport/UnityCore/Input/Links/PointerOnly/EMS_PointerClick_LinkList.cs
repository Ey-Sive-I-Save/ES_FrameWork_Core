using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ES {
    [AddComponentMenu("<ES>ESM支持/输入/光标点击-可接收列表")]
    public class EMS_PointerClick_LinkList : EMS_Abstract_LinkList<Link_EMS_PointerClick>, IPointerClickHandler
    {
       
        public void OnPointerClick(PointerEventData eventData)
        {
            SendLink(new Link_EMS_PointerClick() { eventData=eventData });
        }
    }
}
