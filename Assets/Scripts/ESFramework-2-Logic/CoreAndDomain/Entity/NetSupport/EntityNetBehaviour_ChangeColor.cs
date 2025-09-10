using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ES
{
    partial class EntityNetBehaviour : INetHandlerTargeter<ModuleBase_Expand3DMotion_FirstMotionControl, Link_ColorChangeStart, Link_ColorChangeSet, EntityNetBehaviour>
    {

        [ServerRpc]
        public void ServerRPCHandle(Link_ColorChangeStart a)
        {
            var baseon = GetBaseOn3();
            //   Debug.Log("SERVER Start");
            ClientRPCHandle(((INetHandlerTargeter<ModuleBase_Expand3DMotion_FirstMotionControl, Link_ColorChangeStart, Link_ColorChangeSet, EntityNetBehaviour>)
 (this)).GetHandler().ServerHandle(baseon, a));
        }
        [ObserversRpc]
        public void ClientRPCHandle(Link_ColorChangeSet clientReceive)
        {
            var baseon = GetBaseOn3();
            ((INetHandlerTargeter<ModuleBase_Expand3DMotion_FirstMotionControl, Link_ColorChangeStart, Link_ColorChangeSet, EntityNetBehaviour>)
(this)).GetHandler().ClientHandle(baseon, clientReceive);
        }


        public ModuleBase_Expand3DMotion_FirstMotionControl GetBaseOn3()
        {
            return ConnectedCoreObject.GetMoudle<ModuleBase_Expand3DMotion_FirstMotionControl>();
        }

        NetHandler<ModuleBase_Expand3DMotion_FirstMotionControl, EntityNetBehaviour, Link_ColorChangeStart, Link_ColorChangeSet> INetHandlerTargeter<ModuleBase_Expand3DMotion_FirstMotionControl, Link_ColorChangeStart, Link_ColorChangeSet, EntityNetBehaviour>.GetHandler()
        {
            return GetBaseOn3().colorChange;
        }


    }
    [Serializable]
    public struct Link_ColorChangeStart
    {
        public float alpha;
    }


    [Serializable]
    public struct Link_ColorChangeSet
    {
        public Color color;
    }


    public class Handler_ColorChange : NetHandler<ModuleBase_Expand3DMotion_FirstMotionControl, EntityNetBehaviour, Link_ColorChangeStart, Link_ColorChangeSet>
    {
        public override Link_ColorChangeSet ServerHandle(ModuleBase_Expand3DMotion_FirstMotionControl baseOn, Link_ColorChangeStart link_server)
        {
            var Target = new Link_ColorChangeSet();
            Color c = UnityEngine.Random.ColorHSV();
            c.a = link_server.alpha;
            Target.color = c;
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            block.SetColor("_BaseColor", c);
            baseOn.Core.selfRenderer.SetPropertyBlock(block);
            return Target;
        }
        public override void ClientHandle(ModuleBase_Expand3DMotion_FirstMotionControl baseOn, Link_ColorChangeSet link_client)
        {
            var Core = baseOn.Core;
            MaterialPropertyBlock block = new MaterialPropertyBlock();
                block.SetColor("_BaseColor", link_client.color);
            Core.selfRenderer.SetPropertyBlock(block);
            //Core.Rigid.rotation = Quaternion.Euler(0, link_client.rotYEnd, 0);
            // Debug.Log("SET Local" + link_client.posEnd+ link_client.rotYEnd);
        }
    }
}
