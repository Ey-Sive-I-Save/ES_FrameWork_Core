using ES;
using ES.EvPointer;
using FishNet.Connection;
using FishNet.Transporting;
using Sirenix.OdinInspector;
using Sirenix.Serialization.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ES
{

    [Serializable, TypeRegistryItem("Link收发安全键组")/*Link收发安全组*/]
    public class LinkReceivePool : SafeKeyGroup<Type, IReceiveLink> /**/
    {
        public override string Editor_ShowDes => "Link收发安全键组";
        
        public void SendLink<Link>(Link link) where Link : ILink
        {
            var links = GetGroupDirectly(typeof(Link));
            links.ApplyBuffers();
            int count = links.ValuesNow.Count;
            for(int i=0;i< count; i++)
            {
                if (links.ValuesNow[i] is IReceiveLink<Link> irl)
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
           /* foreach (var i in links)
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
            }*/
        }
        public void AddReceive<Link>(IReceiveLink<Link> e) where Link : ILink
        {
            TryAdd(typeof(Link), e);
        }
        public void RemoveReceive<Link>(IReceiveLink<Link> e) where Link : ILink
        {
            TryRemove(typeof(Link), e);
        }
    }

    [Serializable, TypeRegistryItem("Link收发安全键组")/*Link收发安全组*/]
    public class LinkReceivePoolNet : SafeKeyGroup<Type, IReceiveLink> /**/
    {
        public override string Editor_ShowDes => "Link收发安全键组";
        public Action<Type> OnNewListCreate;
        
        public void SendLink<Link>(Link link) where Link : ILink
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

       
        public void AddReceive<Link>(IReceiveLink<Link> e) where Link : ILink
        {
            TryAdd(typeof(Link), e);
        }
        public void AddRecieveWithNet<Link>(IReceiveLink<Link> e,bool isServer) where Link :struct,ILink
        {
            if(TryAddAndBackIsNewList(typeof(Link), e))
            {
               /* if (isServer) ESNetManager.ServerManager.RegisterBroadcast<Link>(SendLink_ToServer_Internal);
                else ESNetManager.ClientManager.RegisterBroadcast<Link>(SendLink_Internal);*/
            }
            void SendLink_ToServer_Internal(NetworkConnection connection, Link link, FishNet.Transporting.Channel channel)
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
                    else RemoveReceive((IReceiveLink<Link>)null);
                }
            }
            void SendLink_Internal(Link link, FishNet.Transporting.Channel channel)
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
                    else RemoveReceive((IReceiveLink<Link>)null);
                }
            }
        }
        public void RemoveReceive<Link>(IReceiveLink<Link> e) where Link : ILink
        {
            TryRemove(typeof(Link), e);
        }
    }
}

