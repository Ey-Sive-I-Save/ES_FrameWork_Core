using ES;
using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES
{
    public abstract class NetHandler<Base, HandleTargeter, Link_ServerStart, Link_ClientReceive>
        where HandleTargeter : INetHandlerTargeter<Base, Link_ServerStart, Link_ClientReceive, HandleTargeter>
    {
        private bool init = false;
        public virtual NetHanlderSyncOption SyncOption => NetHanlderSyncOption.Normal;
        public void Handle(Base baseOn, HandleTargeter targeter, Link_ServerStart link_Server, bool IsNet = true,bool fromClient=true)
        {
            if (fromClient)
            {
                if (IsNet)
                {
                    if (SyncOption.HasFlag(NetHanlderSyncOption.ClientPre))
                        ClientHandle(baseOn, ServerHandle(baseOn, link_Server));
                    targeter.ServerRPCHandle(link_Server);
                }
                else if (!SyncOption.HasFlag(NetHanlderSyncOption.MustNet))
                {
                    ClientHandle(baseOn, ServerHandle(baseOn, link_Server));
                }
            }
            else
            {
                //来自服务器的话
                if (IsNet)
                {
                    targeter.ClientRPCHandle(ServerHandle(baseOn, link_Server));
                }
            }
        }
        public virtual Link_ClientReceive ServerHandle(Base baseOn, Link_ServerStart link_server)
        {
            return default;
        }
        public virtual void ClientHandle(Base baseOn, Link_ClientReceive link_client)
        {

        }
    }

    public interface INetHandlerTargeter<Base, Link_ServerStart, Link_ClientReceive, This>
        where This : INetHandlerTargeter<Base, Link_ServerStart, Link_ClientReceive, This>
    {
        public NetHandler<Base, This, Link_ServerStart, Link_ClientReceive> GetHandler();
        public void ServerRPCHandle(Link_ServerStart serverStart);
        public void ClientRPCHandle(Link_ClientReceive clientReceive);
        /// <summary>
        /// 案例模拟
        /// </summary>
        /// <param name="serverStart"></param>
        //-  写一个ServerRPC [ServerRpc]
        public void ONLY_MONITOR_ServerRPCHandle(Link_ServerStart serverStart)
        {
            ONLY_MONITOR_ClientRPCHandle(GetHandler().ServerHandle(default(Base), serverStart));
        }

        /// <summary>
        /// 案例模拟
        /// </summary>
        /// <param name="clientReceive"></param>
        //-  写一个ServerRPC [ServerRpc]
        public void ONLY_MONITOR_ClientRPCHandle(Link_ClientReceive clientReceive)
        {
            GetHandler().ClientHandle(default(Base), clientReceive);
        }
    }
    [Flags]
    public enum NetHanlderSyncOption
    {
        Normal = 0,
        ClientPre = 1,
        MustNet = 2
    }
}
