using FishNet.Connection;
using FishNet.Object;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES
{
    /*
     *  继承自NetWorkBehaviour,拥有网络周期
        
     
     */
    public partial class ESNetBehaviour : NetworkBehaviour
    {
        public ESObject ConnectedObject;
        public bool StartSpawn = false;
        #region 默认支持
        #endregion
        private void Awake()
        {

        }
        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
        }
        public override void OnStartClient()
        {
            base.OnStartClient();
        }
        public override void OnStopClient()
        {
            base.OnStopClient();
        }
        public override void OnStartServer()
        {
            base.OnStartServer();
        }
        public override void OnStopServer()
        {
            base.OnStopServer();
        }
        public void Function_ClientSendStartWithPlayerAtServer(NetworkConnection connection, ESNetPlayer player)
        {
            if (ESNetManager.Instance.Need_RoomIDMatch)
            {
                if (player.RoomNumber == ESNetManager.Instance.NetPlayer.RoomNumber)
                {
                    Debug.Log("房间号匹配！" + player.RoomNumber);
                }
                else
                {
                    Debug.LogWarning("房间号不对！" + "试图加入" + player.RoomNumber + "实际上这里是" + ESNetManager.Instance.NetPlayer.RoomNumber);
                    connection.Kick(FishNet.Managing.Server.KickReason.ExploitExcessiveData);
                }
            }
        }
        [ServerRpc]
        public void SpawnThis(GameObject g,NetworkConnection connection=null)
        {
            ServerManager.Spawn(g,connection);
        }
    }
}

