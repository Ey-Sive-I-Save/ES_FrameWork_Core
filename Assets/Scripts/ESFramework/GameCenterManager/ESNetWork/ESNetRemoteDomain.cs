using ES;
using FishNet.Managing;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES
{
    
    public class  ESNetRemoteDomain : Domain<ESNetManager, RemoteModuleForESNetManager>
    {
    }
    [Serializable]
    public abstract class RemoteModuleForESNetManager : Module<ESNetManager, ESNetRemoteDomain>
    {

    }


    [Serializable, TypeRegistryItem("房间支持：客户端发送验证消息")]
    public class ModuleRemote_ClientSendThisAuthentiacationMessage : RemoteModuleForESNetManager
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            ESNet.OnSelfClientStart += SendMessage;
        }
        private void SendMessage()
        {
            Debug.Log("发送哈");
            /*ESNet.NetBehaviourCore.Function_ClientSendStartWithPlayerAtServer(ESNet.SelfConnection, ESNet.NetPlayer);*/
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            ESNet.OnSelfClientStart -= SendMessage;
        }
        protected override void Update()
        {

        }
        protected override void CreateRelationshipOnly()
        {
            base.CreateRelationshipOnly();
            //Domain.Module_XXX = this;  显性引用
        }
    }

}
