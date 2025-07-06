using ES;
using FishNet.Connection;
using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES {
    public class ESNetRPCMaster : NetworkBehaviour
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

      
        public void SendLinkToTarget<Link>(Link link,NetworkConnection connection) where Link:ILink
        {
            if (connection == this.LocalConnection)
            {
                Debug.Log("发送到目标");
                GameCenterManager.LinkReceivePoolGameCenter.SendLink(link);
            }
        }
    }
}
