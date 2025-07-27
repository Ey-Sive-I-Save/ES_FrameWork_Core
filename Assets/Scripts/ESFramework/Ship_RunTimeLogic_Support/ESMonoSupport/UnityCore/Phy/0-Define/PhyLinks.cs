using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES
{
    /*此处针对碰撞事件制造Link 声明通用的Link*/
    public struct Link_EMS_Collider3DEnter : ILink
    {
        public Collider collider;
        public Vector3 posAT;
    }
    public struct Link_EMS_Collider3DStay : ILink
    {
        public Collider collider;
        public Vector3 posAT;
    }
    public struct Link_EMS_Collider3DExit : ILink
    {
        public Collider collider;
        public Vector3 posAT;
    }
    public struct Link_EMS_Collider2DEnter : ILink
    {
        public Collider2D Collider2D;
        public Vector3 posAT;
    }
    public struct Link_EMS_Collider2DStay : ILink
    {
        public Collider2D Collider2D;
        public Vector3 posAT;
    }
    public struct Link_EMS_Collider2DExit : ILink
    {
        public Collider2D Collider2D;
        public Vector3 posAT;
    }
    public struct Link_EMS_Trigger3DEnter : ILink
    {
        public Collider collider;
        public Vector3 posAT;
    }
    public struct Link_EMS_Trigger3DStay : ILink
    {
        public Collider collider;
        public Vector3 posAT;
    }
    public struct Link_EMS_Trigger3DExit : ILink
    {
        public Collider collider;
        public Vector3 posAT;
    }
    public struct Link_EMS_Trigger2DEnter : ILink
    {
        public Collider2D Trigger2D;
        public Vector3 posAT;
    }
    public struct Link_EMS_Trigger2DStay : ILink
    {
        public Collider2D Trigger2D;
        public Vector3 posAT;
    }
    public struct Link_EMS_Trigger2DExit : ILink
    {
        public Collider2D Trigger2D;
        public Vector3 posAT;
    }
   
}
