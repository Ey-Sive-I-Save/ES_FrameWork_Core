using FishNet.Connection;
using FishNet.Object;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES
{

    public partial class ESNetBehaviour : NetworkBehaviour
    {
        public ESObject ConnectedObject;
        public bool StartSpawn = false;
        #region 默认支持
        [FoldoutGroup("ES默认功能"), LabelText("仅本地可运转")]
        public List<Transform> AllOnlyOwmer = new List<Transform>();

        #endregion
        [ShowInInspector]
        public Queue<ILink> WaitingSendLinkAtServer = new Queue<ILink>();
        [ShowInInspector]
        public Queue<ILink> WaitingSendLinkAtClient = new Queue<ILink>();
        [ShowInInspector]
        public Queue<Action> WaitingTaskAtServer = new Queue<Action>();
        [ShowInInspector]
        public Queue<Action> WaitingTaskAtClient = new Queue<Action>();
        private float maxWait = 2;
        private float startClientTime;
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
            startClientTime = Time.time;
            base.OnStartClient();
            if (IsOwner&& StartSpawn)
            {
                SpawnThis(gameObject,LocalConnection);
               
            }
            StartCoroutine(RunSelfClientWaitingRPC());
            StartCoroutine(RunTaskClientWaiting());
            if (IsOwner)
            {

                Debug.Log("客户端Owner"+gameObject.name);
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
            StartCoroutine(RunTaskServerWaiting());

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
        public void DoSelfRPCLinkAtClient(ILink link)
        {
            Debug.Log("Client DO" + link);
            if (link is Link_IDSet set)
            {
                ConnectedObject.ID = set.id;
            }
        }

        public void DoSelfRPCLinkAtServer(ILink link)
        {
            Debug.Log("Server DO" + link);
            if (link is Link_IDRequest request)
            {
                int ID = GameCenterManager.NetIDCount;
                SendSelfLinkToClient(new Link_IDSet() { id = ID });
            }
        }
        public IEnumerator RunSelfServerWaitingRPC()
        {
            while (WaitingSendLinkAtServer.Count > 0)
            {
                var next = WaitingSendLinkAtServer.Dequeue();
                DoSelfRPCLinkAtServer(next);
                yield return null;
            }
            yield return null;
        }
        public IEnumerator RunSelfClientWaitingRPC()
        {
            while (Time.time-startClientTime<maxWait|| WaitingTaskAtServer.Count > 0)
            {
                if (WaitingSendLinkAtClient.Count > 0)
                {
                    var next = WaitingSendLinkAtClient.Dequeue();
                    DoSelfRPCLinkAtClient(next);
                }
                yield return null;
            }
            yield return null;
        }

        public IEnumerator RunTaskServerWaiting()
        {
            while (Time.time - startClientTime < maxWait|| WaitingTaskAtServer.Count > 0)
            {
                if (WaitingTaskAtServer.Count > 0)
                {
                    var next = WaitingTaskAtServer.Dequeue();
                    next?.Invoke();
                }
                yield return null;
            }
            yield return null;
        }
        public IEnumerator RunTaskClientWaiting()
        {
            
            while (WaitingTaskAtClient.Count > 0)
            {
                var next = WaitingTaskAtClient.Dequeue();
                next?.Invoke();
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

       [ServerRpc(RequireOwnership = false)]
        public void SendSelfLinkToServer(Link_IDRequest link)
        {
            if (IsServerInitialized)
                DoSelfRPCLinkAtServer(link);
            else WaitingSendLinkAtServer.Enqueue(link);
        }

        [ObserversRpc(BufferLast =true)]
        public void SendSelfLinkToClient(Link_IDSet link)
        {
            if (IsServerInitialized)
                DoSelfRPCLinkAtClient(link);
            else WaitingSendLinkAtClient.Enqueue(link);
        }

       

        [ServerRpc]
        public void SpawnThis(GameObject g,NetworkConnection connection=null)
        {
            ServerManager.Spawn(g,connection);
        }
    }
}

