using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


namespace ES {

    public abstract class EMS_Abstract : MonoBehaviour
    {
       
    }
    [HideMonoScript]
    public abstract class EMS_Abstract_LinkList<Link> : EMS_Abstract where Link:ILink
    {
        public LinkReceiveList<Link> Links = new LinkReceiveList<Link>();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SendLink(Link link)
        {
            Links.SendLink(link);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddRecieve(IReceiveLink<Link> t) 
        {
            Links.AddReceive(t);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveRecieve(IReceiveLink<Link> t)
        {
            Links.RemoveReceive(t);
        }
    }
    [HideMonoScript]
    public abstract class EMS_Abstract_LinkSingle<Link> : EMS_Abstract where Link : ILink
    {
        public IReceiveLink<Link> Link_;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SendLink(Link link)
        {
            Link_.OnLink(link);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnLink(Link link)
        {
            Link_.OnLink(link);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddRecieve(IReceiveLink<Link> t)
        {
            Link_ = t;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveRecieve(IReceiveLink<Link> t)
        {
            if (Link_ == t) Link_ = null;
        }
    }
}
