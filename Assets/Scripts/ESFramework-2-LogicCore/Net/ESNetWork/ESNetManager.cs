using ES;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Managing.Client;
using FishNet.Managing.Server;
using FishNet.Object;
using FishNet.Transporting;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES
{
    [DefaultExecutionOrder(-4)]
    public class ESNetManager : SingletonAsCore<ESNetManager>
    { 

        [FoldoutGroup("网络行为预制件")]
        [LabelText("网络行为核心预制件"), AssetsOnly]
        public ESNetGlobalRPCMaster ESNetRPCMasterPrefab;

        [FoldoutGroup("网络行为预制件")]
        [LabelText("网络行为核心场景预备"), SerializeField, SceneObjectsOnly]
        private ESNetGlobalRPCMaster _NetBehaviourCoreScenes;

        public ESNetGlobalRPCMaster ESNetRPC
        {
            get
            {
                if (_NetBehaviourCoreScenes != null) return _NetBehaviourCoreScenes;
                if (ESNetRPCMasterPrefab != null)
                {
                    _NetBehaviourCoreScenes = Instantiate(ESNetRPCMasterPrefab);
                    ServerManager.Spawn(_NetBehaviourCoreScenes.gameObject);
                    return _NetBehaviourCoreScenes;


                }
                GameObject cache = new GameObject();
                cache.AddComponent<NetworkObject>();
                return _NetBehaviourCoreScenes=cache.AddComponent<ESNetGlobalRPCMaster>();
            }

            set {
                if (_NetBehaviourCoreScenes==null&&value != null)
                {
                    _NetBehaviourCoreScenes = value;
                }
            }
        }

        #region 快捷属性
        public NetworkConnection SelfConnection;

        #endregion

        #region 玩家信息
        [FoldoutGroup("联网基础"), LabelText("玩家信息")]
        public ESNetPlayer NetPlayer = new ESNetPlayer();


        #endregion

        #region 联网核心
        [FoldoutGroup("联网基础"), LabelText("需要房间号匹配")]
        public bool Need_RoomIDMatch = true;
        [FoldoutGroup("联网基础"), LabelText("网络管理器")]
        public NetworkManager TheNetWorkManager;
        [FoldoutGroup("联网基础"), LabelText("服务器状态"), InlineButton("OnClick_Server", "服务器链接开关")]
        public LocalConnectionState ServerState = LocalConnectionState.Stopped;
        [FoldoutGroup("联网基础"), LabelText("客户端状态"), InlineButton("OnClick_Client", "客户端链接开关")]
        public LocalConnectionState ClientState = LocalConnectionState.Stopped;

        public bool IsServer => TheNetWorkManager.IsServerStarted;
        public bool IsClient => TheNetWorkManager.IsClientStarted;
        public bool IsNet => TheNetWorkManager.IsClientStarted || TheNetWorkManager.IsServerStarted;

        public static ServerManager ServerManager { get => InstanceFinder.ServerManager; }
        public static ClientManager ClientManager { get => InstanceFinder.ClientManager; }

        public bool ConnectTest(NetworkConnection connection)
        {
            /* if(connection.CustomData is ESNetPlayer netPlayer)
             {
                 if (Need_RoomIDMatch)
                 {
                     if (netPlayer.RoomNumber == NetPlayer.RoomNumber) return true;
                     else { Debug.LogError("房间号不匹配，我需要"+NetPlayer.RoomNumber+"他却是"+ netPlayer.RoomNumber); return false; }
                 }
             }
             Debug.Log("Debug true"+ connection.CustomData);*/
            return true;
        }



        public void OnClick_Server()
        {
            if (TheNetWorkManager == null)
                return;

            if (ServerState != LocalConnectionState.Stopped)
                TheNetWorkManager.ServerManager.StopConnection(true);
            else Invoke_OnServerStartConnection();


        }


        public void OnClick_Client()
        {
            if (TheNetWorkManager == null)
                return;

            if (ClientState != LocalConnectionState.Stopped)
                TheNetWorkManager.ClientManager.StopConnection();
            else
            {
                Invoke_OnClientStartConnection();
            }

        }
        private void _PassiveDelegate_ClientManager_OnClientConnectionState(ClientConnectionStateArgs obj)
        {
            ClientState = obj.ConnectionState;
            if (ClientState == LocalConnectionState.Started)
            {
                if (_NetBehaviourCoreScenes != null)
                    TheNetWorkManager.ServerManager.Spawn(_NetBehaviourCoreScenes.gameObject);
            }
        }
        private void _PassiveDelegate_ServerManager_OnServerConnectionState(ServerConnectionStateArgs obj)
        {
            ServerState = obj.ConnectionState;

        }
        public void Invoke_OnClientStartConnection()
        {


            TheNetWorkManager.ClientManager.StartConnection();
        }
        public void Invoke_OnServerStartConnection()
        {
            TheNetWorkManager.ServerManager.StartConnection();
        }
        #endregion

        #region 联网委托
        public Action OnSelfClientStart = () => { };
        public Action OnSelfServerStart = () => { };
        public Action OnSelfClientStop = () => { };
        public Action OnSelfServerStop = () => { };
        #endregion

        //显性声明扩展域
        //public NormalDomainForXXX NormalDomain;
        //public 02DomainForXXX StateMachineDomain;
        [FoldoutGroup("扩展域")] public ESNetLocalDomain LocalDomain;
        [FoldoutGroup("扩展域")] public ESNetRemoteDomain RemoteDomain;
        //注册前的操作
        protected override void OnBeforeAwakeRegister()
        {
            base.OnBeforeAwakeRegister();
            GameCenterManager.Instance.NetSupport = true;

            TheNetWorkManager = FindObjectOfType<NetworkManager>();
            if (TheNetWorkManager == null)
            {
                Debug.LogError("NetworkManager未找到！");
                return;
            }
            else
            {
                TheNetWorkManager.ServerManager.OnServerConnectionState += _PassiveDelegate_ServerManager_OnServerConnectionState;
                TheNetWorkManager.ClientManager.OnClientConnectionState += _PassiveDelegate_ClientManager_OnClientConnectionState;
            }
            SelfConnection = TheNetWorkManager.ClientManager.Connection;
        }

        [Button("出")]
        public void TrySpawn(GameObject g)
        {
            ESNetRPC.ServerSpawnPrefab(g,ClientManager.Connection); 
           
        }
    }
    public enum NetWorkUpdateOption
    {
        [InspectorName("总更新")] Always,
        [InspectorName("只有拥有者")] OnlyOwner,
        [InspectorName("只有其他人")] OnlyOther,
        [InspectorName("只有客户端")] OnlyClient,
        [InspectorName("只有服务器")] OnlyServer,
        [InspectorName("只有主机")] OnlyHost
    }

}


