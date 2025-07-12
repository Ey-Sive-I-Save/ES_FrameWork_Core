using ES;
using FishNet.Connection;
using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES
{
    public class ESNetRPCMaster : ESNetBehaviour
    {
        public ESNetObject NetObject;
        public bool IsNetWorked = false;
        #region 声明
        private void Awake()
        {
            if (NetObject == null)
            {
                NetObject = GetComponentInParent<ESNetObject>();
                if (NetObject == null)
                {
                    NetObject = gameObject.AddComponent<ESNetObject>();
                }
            }
            ESNetManager.Instance.ESNetRPC = this;
        }
        #endregion

        public override void OnStartClient()
        {
            base.OnStartClient();
            ESNetManager.Instance.ESNetRPC = this;
            ServerManager.Spawn(gameObject);
        }
        public override void OnStopClient()
        {
            base.OnStopClient();
            ServerManager.Despawn(gameObject);
        }

        public void SendLinkToTarget<Link>(Link link, NetworkConnection connection) where Link : ILink
        {
            if (connection == this.LocalConnection)
            {
                Debug.Log("发送到目标");
                GameCenterManager.LinkReceivePoolAsServer.SendLink(link);
            }
        }


        public void SendLinkToServerT<Link>(Link link, NetworkConnection connection) where Link : ILink
        {

            Debug.Log("发送到服务器");
            if (link is Link_IDRequest request)
            {
                SendLinkToServerT(request);
            }
        }

        public void SendLinkToClientsT<Link>(Link link, NetworkConnection connection) where Link : ILink
        {
            Debug.Log("发送到客户端");
            if (link is Link_IDSet request)
            {
                SendLinkToClientsT(request);
            }
            GameCenterManager.LinkReceivePoolAsClient.SendLink(link);
        }
        [ServerRpc]
        public void SendLinkToServerT(Link_IDRequest link)
        {
            GameCenterManager.LinkReceivePoolAsServer.SendLink(link);
        }
        [ObserversRpc]
        public void SendLinkToClientsT(Link_IDSet link)
        {
            GameCenterManager.LinkReceivePoolAsClient.SendLink(link);
        }

        [ServerRpc]
        public void ServerSpawnPrefab(GameObject prefab,NetworkConnection connection=null)
        {
            ServerManager.Spawn(Instantiate(prefab),connection);
        }
    }
}
