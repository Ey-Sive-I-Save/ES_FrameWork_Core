using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ES {
    [AddComponentMenu("<ES>ESM支持/输入/轴输入移动-可接收列表")] 
    public class EMS_Move_LinkList : EMS_Abstract_LinkList<Link_EMS_Move>, IMoveHandler
    {
        
        public void OnMove(AxisEventData eventData)
        {
            SendLink(new Link_EMS_Move() { eventData=eventData });
        }
    }
}
