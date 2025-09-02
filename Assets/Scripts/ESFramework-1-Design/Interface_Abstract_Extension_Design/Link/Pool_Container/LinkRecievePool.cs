using ES;
using FishNet.Broadcast;
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
        
        public void SendLink<Link>(Link link)
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
        }
        public void AddReceive<Link>(IReceiveLink<Link> e)
        {
            TryAdd(typeof(Link), e);
        }
        public void RemoveReceive<Link>(IReceiveLink<Link> e) 
        {
            TryRemove(typeof(Link), e);
        }
    }

    
}

