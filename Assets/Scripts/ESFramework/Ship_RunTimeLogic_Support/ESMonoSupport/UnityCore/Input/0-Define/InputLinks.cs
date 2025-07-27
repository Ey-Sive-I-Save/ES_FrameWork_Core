using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ES {
    public struct Link_EMS_Move : ILink
    {
        public AxisEventData eventData;
    }
    public struct Link_EMS_Cancel : ILink
    {
        public BaseEventData eventData;
    }
    public struct Link_EMS_Select : ILink
    {
        public BaseEventData eventData;
    }
    public struct Link_EMS_Submit : ILink
    {
        public BaseEventData eventData;
    }
    public struct Link_EMS_UpdateSelected : ILink
    {
        public BaseEventData eventData;
    }
    public struct Link_BaseData : ILink
    {
        public BaseEventData eventData;
    }
    public struct Link_EMS_BeginDrag : ILink
    {
        public PointerEventData eventData;
    }
    public struct Link_EMS_Drag : ILink
    {
        public PointerEventData eventData;
    }
    public struct Link_EMS_Drop : ILink
    {
        public PointerEventData eventData;
    }
    public struct Link_EMS_EndDrag: ILink
    {
        public PointerEventData eventData;
    }
    public struct Link_EMS_InitalizedPotentialDrag : ILink
    {
        public PointerEventData eventData;
    }
    public struct Link_EMS_PointerClick : ILink
    {
        public PointerEventData eventData;
    }
    public struct Link_EMS_PointerDown : ILink
    {
        public PointerEventData eventData;
    }
    public struct Link_EMS_PointerEnter : ILink
    {
        public PointerEventData eventData;
    }
    public struct Link_EMS_PointerExit : ILink
    {
        public PointerEventData eventData;
    }
    public struct Link_EMS_PointerUp : ILink
    {
        public PointerEventData eventData;
    }
    public struct Link_EMS_Scroll : ILink
    {
        public PointerEventData eventData;
    }
}
