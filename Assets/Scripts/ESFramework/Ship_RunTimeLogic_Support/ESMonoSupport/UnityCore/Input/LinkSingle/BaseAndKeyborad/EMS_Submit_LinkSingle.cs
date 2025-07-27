using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ES {
    [AddComponentMenu("<ES>ESM支持/输入/确定提交-指定接收目标")]
    public class EMS_Submit_LinkSingle : MonoBehaviour, ISubmitHandler
    {
        public IReceiveLink<Link_BaseData> Link;

        public void OnSubmit(BaseEventData eventData)
        {
            Link?.OnLink(new Link_BaseData() { eventData = eventData });
        }
    }
}
