using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ES
{
    partial class EntityNetBehaviour : INetHandlerTargeter<Entity, Link_HurtFrom, Link_HurtOn, EntityNetBehaviour>
    {

        [ServerRpc]
        public void ServerRPCHandle(Link_HurtFrom a)
        {
            var baseon = GetBaseOn4();
            //   Debug.Log("SERVER Start");
            ClientRPCHandle(((INetHandlerTargeter<Entity, Link_HurtFrom, Link_HurtOn, EntityNetBehaviour>)
 (this)).GetHandler().ServerHandle(baseon, a));
        }
        [ObserversRpc]
        public void ClientRPCHandle(Link_HurtOn clientReceive)
        {
            var baseon = GetBaseOn4();
            ((INetHandlerTargeter<Entity, Link_HurtFrom, Link_HurtOn, EntityNetBehaviour>)
(this)).GetHandler().ClientHandle(baseon, clientReceive);
        }


        public Entity GetBaseOn4()
        {
            return ConnectedCoreObject;
        }

        NetHandler<Entity, EntityNetBehaviour, Link_HurtFrom, Link_HurtOn> INetHandlerTargeter<Entity, Link_HurtFrom, Link_HurtOn, EntityNetBehaviour>.GetHandler()
        {
            return GetBaseOn4().hurt;
        }


    }
    [Serializable]
    public struct Link_HurtFrom
    {
        public float baseDamage;
        public int itemID;
    }


    [Serializable]
    public struct Link_HurtOn
    {
        public float damage;
    }


    public class Handler_Hurt : NetHandler<Entity, EntityNetBehaviour, Link_HurtFrom, Link_HurtOn>
    {
        public override Link_HurtOn ServerHandle(Entity baseOn, Link_HurtFrom link_server)
        {
            var Target = new Link_HurtOn();
            Target.damage = link_server.baseDamage + GameCenterManager.ItemIDPool[link_server.itemID].col.bounds.max.y;
            return Target;
        }
        public override void ClientHandle(Entity baseOn, Link_HurtOn link_client)
        {
            baseOn.AttackOn(link_client.damage);
            //Core.Rigid.rotation = Quaternion.Euler(0, link_client.rotYEnd, 0);
             Debug.Log("Hurt Local" + link_client.damage);
        }
    }
}
