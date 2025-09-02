using ES;
using FishNet.Connection;
using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES
{

    public class ESNetGlobalRPCMaster : ESNetBehaviour
    {
        public NetworkObject NetObject;
        public bool IsNetWorked = false;
        #region 声明
        private void Awake()
        {
            if (NetObject == null)
            {
                NetObject = GetComponentInParent<NetworkObject>();
                if (NetObject == null)
                {
                    NetObject = gameObject.AddComponent<NetworkObject>();
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

        public void SendLinkToTarget<Link>(Link link, NetworkConnection connection) 
        {
            if (connection == this.LocalConnection)
            {
                Debug.Log("发送到目标");
                
            }
        }

        [ServerRpc]
        public void ServerSpawnPrefab(GameObject prefab,NetworkConnection connection=null)
        {
            ServerManager.Spawn(Instantiate(prefab),connection);
        }
    }
}
