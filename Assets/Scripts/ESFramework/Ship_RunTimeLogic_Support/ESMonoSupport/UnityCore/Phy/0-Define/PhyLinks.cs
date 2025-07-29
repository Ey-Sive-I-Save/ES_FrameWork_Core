using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES
{
    /*此处针对碰撞事件制造Link 声明通用的Link*/
    public interface ILink_EMS_Collider : ILink
    {
        public Collider collider { get; set; }
        public Vector3 posAT { get; set; }
    }
    public interface ILink_EMS_Collider2D : ILink
    {
        public Collider2D collider { get; set; }
        public Vector3 posAT { get; set; }
    }
    public struct Link_EMS_Collider3DEnter : ILink_EMS_Collider
    {
        public Collider collider { get; set; }
        public Vector3 posAT { get; set; }
    }
    public struct Link_EMS_Collider3DStay : ILink_EMS_Collider
    {
        public Collider collider { get; set; }
        public Vector3 posAT { get; set; }
    }
    public struct Link_EMS_Collider3DExit : ILink_EMS_Collider
    {
        public Collider collider { get; set; }
        public Vector3 posAT { get; set; }
    }
    public struct Link_EMS_Collider2DEnter : ILink_EMS_Collider2D
    {
        public Collider2D collider { get; set; }
        public Vector3 posAT { get; set; }
    }
    public struct Link_EMS_Collider2DStay : ILink_EMS_Collider2D
    {
        public Collider2D collider { get; set; }
        public Vector3 posAT { get; set; }
    }
    public struct Link_EMS_Collider2DExit : ILink_EMS_Collider2D
    {
        public Collider2D collider { get; set; }
        public Vector3 posAT { get; set; }
    }
    public struct Link_EMS_Trigger3DEnter : ILink_EMS_Collider
    {
        public Collider collider { get; set; }
        public Vector3 posAT { get; set; }
    }
    public struct Link_EMS_Trigger3DStay : ILink_EMS_Collider
    {
        public Collider collider { get; set; }
        public Vector3 posAT { get; set; }
    }
    public struct Link_EMS_Trigger3DExit : ILink_EMS_Collider
    {
        public Collider collider { get; set; }
        public Vector3 posAT { get; set; }
    }
    public struct Link_EMS_Trigger2DEnter : ILink_EMS_Collider2D
    {
        public Collider2D collider { get; set; }
        public Vector3 posAT { get; set; }
    }
    public struct Link_EMS_Trigger2DStay : ILink_EMS_Collider2D
    {
        public Collider2D collider { get; set; }
        public Vector3 posAT { get; set; }
    }
    public struct Link_EMS_Trigger2DExit : ILink_EMS_Collider2D
    {
        public Collider2D collider { get; set; }
        public Vector3 posAT { get; set; }
    }
   
}
