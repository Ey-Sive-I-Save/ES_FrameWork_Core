using ES;
using FishNet.Broadcast;
using FishNet.Connection;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES {
    [Serializable, TypeRegistryItem("Link收发-服务器")/*Link收发安全组*/]
    public class LinkReceivePoolServer : SafeKeyGroup<Type, IReceiveLink> /**/
    {
        public override string Editor_ShowDes => "Link收发-服务器";
        public Action<Type> OnNewListCreate;

        public void SendLink<Link>(Link link) where Link : struct, IBroadcast
        {
            var links = GetGroupDirectly(typeof(Link));
            foreach (var i in links)
            {
                if (i is IReceiveLink<Link> irl)
                {
                    if (irl is UnityEngine.Object ob)
                    {
                        if (ob != null) irl.OnLink(link);
                        else links.TryRemove(irl);
                    }
                    else if (irl != null) irl.OnLink(link);
                }
                else RemoveReceive<Link>(null);
            }
        }
        public void AddRecieveWithNet<Link>(IReceiveLink<Link> e) where Link : struct, IBroadcast
        {
            if (TryAddAndBackIsNewList(typeof(Link), e))
            {
                ESNetManager.ServerManager.RegisterBroadcast<Link>(SendLink_ToServer_Internal);
            }
            void SendLink_ToServer_Internal(NetworkConnection connection, Link link, FishNet.Transporting.Channel channel)
            {
                SendLink(link);
                var links = GetGroupDirectly(typeof(Link));
                foreach (var i in links)
                {
                    if (i is IReceiveLink<Link> irl)
                    {
                        if (irl is UnityEngine.Object ob)
                        {
                            if (ob != null) irl.OnLink(link);
                            else links.TryRemove(irl);
                        }
                        else if (irl != null) irl.OnLink(link);
                    }
                    else RemoveReceive((IReceiveLink<Link>)null);
                }
            }
        }
        public void RemoveReceive<Link>(IReceiveLink<Link> e)
        {
            TryRemove(typeof(Link), e);
        }
    }
    [Serializable, TypeRegistryItem("Link收发-客户端")/*Link收发安全组*/]
    public class LinkReceivePoolClient : SafeKeyGroup<Type, IReceiveLink> /**/
    {
        public override string Editor_ShowDes => "Link收发-客户端";

        public void SendLink<Link>(Link link) where Link : struct, IBroadcast
        {
            var links = GetGroupDirectly(typeof(Link));
            foreach (var i in links)
            {
                if (i is IReceiveLink<Link> irl)
                {
                    if (irl is UnityEngine.Object ob)
                    {
                        if (ob != null) irl.OnLink(link);
                        else links.TryRemove(irl);
                    }
                    else if (irl != null) irl.OnLink(link);
                }
                else RemoveReceive<Link>(null);
            }
        }
        public void AddRecieveWithNet<Link>(IReceiveLink<Link> e) where Link : struct, IBroadcast
        {
            if (TryAddAndBackIsNewList(typeof(Link), e))
            {
                ESNetManager.ClientManager.RegisterBroadcast<Link>(SendLink_ToClient_Internal);
            }
            void SendLink_ToClient_Internal(Link link, FishNet.Transporting.Channel channel)
            {
                SendLink(link);
            }
        }
        public void RemoveReceive<Link>(IReceiveLink<Link> e)
        {
            TryRemove(typeof(Link), e);
        }
    }
}
