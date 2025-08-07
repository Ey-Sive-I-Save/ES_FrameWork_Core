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
    public abstract class EMS_Abstarct_Define <Channel, Link> : MonoBehaviour where Link : ILink
    {
        public abstract void SendLink(Channel channel, Link link);
        public abstract void AddRecieve(IReceiveChannelLink<Channel, Link> ir);
        public abstract void RemoveRecieve(IReceiveChannelLink<Channel, Link> ir);

    }
    #region 抽象定义EMS
    [HideMonoScript]
    public abstract class EMS_Abstract_ChannelLinkList<Channel,Link> : EMS_Abstarct_Define<Channel, Link> where Link:ILink
    {
        public LinkReceiveList<Channel, Link> Links = new LinkReceiveList<Channel, Link>();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sealed override void SendLink(Channel channel, Link link)
        {
            Links.SendLink(channel,link);
            Debug.Log("Hanppen："+channel+" by "+link);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sealed override void AddRecieve(IReceiveChannelLink<Channel,Link> t) 
        {
            Links.AddReceive(t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sealed override void RemoveRecieve(IReceiveChannelLink<Channel, Link> t)
        {
            Links.RemoveReceive(t);
        }
    }
    [HideMonoScript]
    public abstract class EMS_Abstract_ChannelLinkSingle<Channel,Link> : EMS_Abstarct_Define<Channel, Link> where Link : ILink
    {
        public IReceiveChannelLink<Channel, Link> Link_;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sealed override void SendLink(Channel channel,Link link)
        {
            Debug.Log("Hanppen：" + channel + " by " + link);
            Link_.OnLink(channel,link);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnLink(Channel channel, Link link)
        {
            Debug.Log("Hanppen：" + channel + " by " + link);
            Link_.OnLink(channel, link);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sealed override void AddRecieve(IReceiveChannelLink<Channel,Link> t)
        {
            Link_ = t;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sealed override void RemoveRecieve(IReceiveChannelLink<Channel, Link> t)
        {
            if (Link_ == t) Link_ = null;
        }
    }
    #endregion

    #region 定义触发事件


    #endregion

}
