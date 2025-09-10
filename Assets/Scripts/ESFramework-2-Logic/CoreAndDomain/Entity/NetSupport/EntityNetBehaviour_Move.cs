using ES;
using FishNet.Object;
using FishNet.Transporting;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES
{
    partial class EntityNetBehaviour : INetHandlerTargeter<ModuleBase_3DStandardMotion, Link_MovePosAndRot, Link_EndPosition, EntityNetBehaviour>
    {

        [ServerRpc]
        public void ServerRPCHandle(Link_MovePosAndRot a)
        {
             var baseon = GetBaseOn2();
         //   Debug.Log("SERVER Start");
            ClientRPCHandle(((INetHandlerTargeter<ModuleBase_3DStandardMotion, Link_MovePosAndRot, Link_EndPosition, EntityNetBehaviour>)
 (this)).GetHandler().ServerHandle(baseon, a));
        }
        [ObserversRpc]
        public void ClientRPCHandle(Link_EndPosition clientReceive)
        {
            var baseon = GetBaseOn2();
            ((INetHandlerTargeter<ModuleBase_3DStandardMotion, Link_MovePosAndRot, Link_EndPosition, EntityNetBehaviour>)
(this)).GetHandler().ClientHandle(baseon, clientReceive);
        }


        public ModuleBase_3DStandardMotion GetBaseOn2()
        {
            return ConnectedCoreObject.GetMoudle<ModuleBase_AB_3DMotion, ModuleBase_3DStandardMotion>();
        }

        NetHandler<ModuleBase_3DStandardMotion, EntityNetBehaviour, Link_MovePosAndRot, Link_EndPosition> INetHandlerTargeter<ModuleBase_3DStandardMotion, Link_MovePosAndRot, Link_EndPosition, EntityNetBehaviour>.GetHandler()
        {
            return GetBaseOn2().move;
        }

      
    }
    [Serializable]
    public struct Link_MovePosAndRot
    {
        public Vector3 posOff;
        public float rotYOff;
    }


    [Serializable]
    public struct Link_EndPosition
    {
        public Vector3 posEnd;
        public float rotYEnd;
    }


    public class Handler_Move : NetHandler<ModuleBase_3DStandardMotion, EntityNetBehaviour, Link_MovePosAndRot, Link_EndPosition>
    {
        public override Link_EndPosition ServerHandle(ModuleBase_3DStandardMotion baseOn, Link_MovePosAndRot link_server)
        {
            var Target = new Link_EndPosition();
            var Core = baseOn.Core;
            Target.posEnd = Core.Rigid.position += link_server.posOff;

            Quaternion onlyYOFf = Quaternion.Euler(0, Mathf.Clamp(link_server.rotYOff, -baseOn.MaxRotSpeed_, baseOn.MaxRotSpeed_)* Time.fixedDeltaTime, 0);
         //   Debug.Log("SERVER HANDLE"+onlyYOFf+"  " + link_server.rotYOff +"   "+ Mathf.Clamp(link_server.rotYOff, -baseOn.MaxRotSpeed_, baseOn.MaxRotSpeed_) +"   "+ Time.fixedDeltaTime+ "    "+onlyYOFf.eulerAngles.y);
            Core.Rigid.rotation *= onlyYOFf;
            Target.rotYEnd = Core.Rigid.rotation.eulerAngles.y;
            return Target;
        }
        public override void ClientHandle(ModuleBase_3DStandardMotion baseOn, Link_EndPosition link_client)
        {
            var Core = baseOn.Core;
            baseOn.posTarget = link_client.posEnd;
            baseOn.rotTarget = link_client.rotYEnd;
            baseOn.lerpingTarget = 0.2f;
            //Core.Rigid.rotation = Quaternion.Euler(0, link_client.rotYEnd, 0);
           // Debug.Log("SET Local" + link_client.posEnd+ link_client.rotYEnd);
        }
    }
}
