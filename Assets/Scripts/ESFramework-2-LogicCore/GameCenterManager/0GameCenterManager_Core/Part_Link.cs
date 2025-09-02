using ES;
using ES.Pointer;
using FishNet;
using FishNet.Broadcast;
using FishNet.Connection;
using FishNet.Managing.Server;
using Sirenix.OdinInspector;
/*using FishNet;

using FishNet.Connection;
using FishNet.Managing;*/
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
        [TabGroup("全局事件"),ShowInInspector]
        //仅游戏核心收发
        public static LinkReceivePool LinkReceiveForGameCenter = new LinkReceivePool();
        [TabGroup("全局事件"), ShowInInspector]
        //作为服务器 接受
        public static LinkReceivePoolServer LinkReceivePoolForServer = new LinkReceivePoolServer();
        [TabGroup("全局事件"), ShowInInspector]
        //作为客户端 接受
        public static LinkReceivePoolClient LinkReceivePoolForClient = new LinkReceivePoolClient();
        [TabGroup("全局事件"), ShowInInspector]
        //作为特定接受目标
        public static LinkReceivePool LinkReceivePoolForTarget = new LinkReceivePool();

        public static void SendLinkAtGameCenter<Link>(Link link)
        {
            LinkReceiveForGameCenter.SendLink<Link>(link);
        }
        public static void SendLinkToServer<Link>(Link link) where Link : struct, IBroadcast
        {
            if (InstanceFinder.ServerManager.Started)
                InstanceFinder.ServerManager.Broadcast<Link>(link);
        }
        public static void SendLinkToClient<Link>(Link link) where Link : struct, IBroadcast
        {
            InstanceFinder.ClientManager.Broadcast<Link>(link);
        }
        public static void SendLinkToTarget<Link>(Link link, NetworkConnection connection = null) where Link : struct, IBroadcast
        {
            ESNetManager.Instance.ESNetRPC.SendLinkToTarget<Link>(link, connection);
        }


        public void Part_Link_OnBeforeAwakeRegister()
        {
           
        }
    }
}
