using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ES {
    [AddComponentMenu("<ES>ESM支持/输入/拖动中-指定接收目标")]
    [HideMonoScript]
    public class EMS_Drag_LinkSingle : EMS_InputPointerEvent_LinkSingle_Abstarct, IDragHandler
    {
        
#if UNITY_EDITOR
        public static string des = "使用 Link_PointerData 传递事件信息";
        [LabelText("使用支持<拖动中>"),InlineProperty]
        public Tool_ESReadMeClass readme = new Tool_ESReadMeClass() {
            edit = false,
            readMeIn= des,
            ReadMeOut= des
        };

#endif
        public void OnDrag(PointerEventData eventData)
        {
            Link_?.OnLink(Channel_InputPointerEvent.Drag, new Link_InputPointerEvent() { eventData = eventData });
        }
    }
}
