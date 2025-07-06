using ES;
using ES.EvPointer;
using FishNet;

using FishNet.Connection;
using FishNet.Managing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ES.EnumCollect;

namespace ES
{
    /* Link管理部分 管理部分，对性能要求极高，
    * 这里会使用大量接口和显式 数据结构 避免 as/强转 和 a.b.c 的出现
     
    */
    public partial class GameCenterManager
    {
        public static LinkTaskPool LinkTaskPool = new LinkTaskPool();

        //仅游戏核心
        public static LinkReceivePool LinkReceivePoolGameCenter = new LinkReceivePool();
        //
        public static LinkReceivePoolNet LinkReceivePoolAsServer = new LinkReceivePoolNet();

        public static LinkReceivePoolNet LinkReceivePoolAsClient = new LinkReceivePoolNet();

        public static LinkReceivePool LinkReceivePoolAsTarget = new LinkReceivePool();
        public static void SendLinkTo<Link>(LinkTaskChannel channel,Link link,NetworkConnection connection=null) where Link: struct,ILink 
        {
            if (channel == LinkTaskChannel.None) return;
            if(channel.HasFlag(LinkTaskChannel.GameCenter))
            {
                LinkReceivePoolGameCenter.SendLink(link);
            }
            if (channel.HasFlag(LinkTaskChannel.ClientToServer))
            {
                ESNetManager.ClientManager.Broadcast(link);
            }
            if (channel.HasFlag(LinkTaskChannel.ServerToClients))
            {
                ESNetManager.ServerManager.Broadcast(link);
            }
            if (channel.HasFlag(LinkTaskChannel.ServerToTarget))
            {
                if (connection != null)
                {
                    ESNetManager.Instance.ESNetRPC.SendLinkToTarget(link,connection);
                }
            }
        }
        

        public void Part_Link_OnBeforeAwakeRegister()
        {
            /*LinkReceivePoolAsServer.OnNewListCreate = (t) => { ESNetManager.ServerManager.RegisterBroadcast(t); };
            ESNetManager.ClientManager.re*/
        }
    }
}
