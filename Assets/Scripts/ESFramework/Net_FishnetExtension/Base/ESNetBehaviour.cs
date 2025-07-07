using ES;
using FishNet.Connection;
using FishNet.Object;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES
{

    public class ESNetBehaviour : NetworkBehaviour
    {
        public ESObject ConnectedObject;
        #region 默认支持
        [FoldoutGroup("ES默认功能"), LabelText("仅本地可运转")]
        public List<Transform> AllOnlyOwmer = new List<Transform>();

        #endregion
        [ShowInInspector]
        public Queue<ILink> WaitingLinkAtServer = new Queue<ILink>();
        [ShowInInspector]
        public Queue<ILink> WaitingLinkAtClient = new Queue<ILink>();

        private void Awake()
        {

        }
        public override void OnStartNetwork()
        {
            Debug.Log("NetWork测试");
            base.OnStartNetwork();

        }
        public override void OnStartClient()
        {
            Debug.Log("客户端测试");
            base.OnStartClient();
            StartCoroutine(RunSelfClientWaitingRPC());
            if (IsOwner)
            {

                Debug.Log("客户端测试");
                /*ESNetManager.Instance.OnSelfClientStart?.Invoke();*/
                ServerManager.Spawn(gameObject);
            }
            else
            {
                /*foreach (var i in AllOnlyOwmer)
                {
                    if (i != null) i.gameObject.SetActive(false);
                }*/
            }

        }
        public override void OnStopClient()
        {
            base.OnStopClient();
            if (IsOwner)
            {
                ESNetManager.Instance.OnSelfClientStop?.Invoke();
            }
        }
        public override void OnStartServer()
        {
            base.OnStartServer();
            StartCoroutine(RunSelfServerWaitingRPC());
            if (IsServerInitialized)
            {
                ESNetManager.Instance.OnSelfServerStart?.Invoke();
            }
        }
        public override void OnStopServer()
        {
            base.OnStopServer();
            if (IsServerInitialized)
            {
                ESNetManager.Instance.OnSelfServerStop?.Invoke();
            }
        }

        public void OnSelfRPCLinkAtClient(ILink link)
        {
            Debug.Log("Client DO"+ link);
            if (link is Link_IDSet set)
            {
                Debug.Log(set.id);
                ConnectedObject.ID = set.id;
            }
        }
        public void OnSelfRPCLinkAtServer(ILink link)
        {
            Debug.Log("Server DO" + link);
            if (link is Link_IDRequest request)
            {
                int ID = GameCenterManager.NetIDCount;
                SendSelfLinkToClient(new Link_IDSet() { id=ID });
            }
        }
        public IEnumerator RunSelfServerWaitingRPC()
        {
            while (WaitingLinkAtServer.Count > 0)
            {
                var next = WaitingLinkAtServer.Dequeue();
                OnSelfRPCLinkAtServer(next);
                yield return null;
            }
            yield return null;
        }
        public IEnumerator RunSelfClientWaitingRPC()
        {
            while (WaitingLinkAtClient.Count > 0)
            {
                var next = WaitingLinkAtClient.Dequeue();
                OnSelfRPCLinkAtClient(next);
                yield return null;
            }
            yield return null;
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

       /* [ServerRpc(RequireOwnership = false)]*/
        public void SendSelfLinkToServer(Link_IDRequest link)
        {
            Debug.Log("ToServer"+ IsServerInitialized);
            if (IsServerInitialized)
                OnSelfRPCLinkAtServer(link);
            else WaitingLinkAtServer.Enqueue(link);
        }

       /* [ObserversRpc()]*/
        public void SendSelfLinkToClient(Link_IDSet link)
        {
            if (IsServerInitialized)
                OnSelfRPCLinkAtClient(link);
            else WaitingLinkAtClient.Enqueue(link);

        }


    }
}

