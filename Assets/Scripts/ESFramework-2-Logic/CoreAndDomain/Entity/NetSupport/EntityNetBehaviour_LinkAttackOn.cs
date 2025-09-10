using ES;
using FishNet.Object;
using HellishBattle;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES
{
    partial class EntityNetBehaviour : INetHandlerTargeter<AttackModule, Link_AttackOnStart, Link_AttackOnReceive, EntityNetBehaviour>
    {

        public NetHandler<AttackModule, EntityNetBehaviour, Link_AttackOnStart, Link_AttackOnReceive> GetHandler()
        {
            return GetBaseOn().attackOn;
        }
        //[ServerRpc]
        public void ServerRPCHandle(Link_AttackOnStart serverStart)
        {
            // var baseon = GetBaseOn();
            //ClientRPCHandle(GetHandler().ServerHandle(baseon, serverStart));
        }
        [ServerRpc]
        public void ServerRPCHandle2(Link_AttackOnStart a)
        {
            // var baseon = GetBaseOn();
            //ClientRPCHandle(GetHandler().ServerHandle(baseon, serverStart));
        }
        [ObserversRpc]
        public void ClientRPCHandle(Link_AttackOnReceive clientReceive)
        {
            var baseon = GetBaseOn();
            GetHandler().ClientHandle(baseon, clientReceive);
        }


        public AttackModule GetBaseOn()
        {
            return ConnectedCoreObject.GetMoudle<AttackModule>();
        }
    }
    [Serializable]
    public struct Link_AttackOnStart
    {
        public int fromID;
        public int toID;
        public vitrulDamage damageType;//伤害特性
    }

    [Serializable]
    public class vitrulDamage { 
        public float damage;
        public float add1;
    }

    
    [Serializable]
    public struct Link_AttackOnReceive
    {
        public int fromID;
        public int toID;
        public float damageValue;//最终伤害
    }
    [Serializable,TypeRegistryItem("攻击模块")]
    public class AttackModule : BaseModuleForEntity
    {
        public override Type TableKeyType => typeof(AttackModule);

        public Handler_AttackOn attackOn = new Handler_AttackOn();

        public void Attack(Entity who)
        {
            attackOn.Handle(
                this,Core.NetBehaviour, 
                new Link_AttackOnStart() { 
                   fromID=Core.ID,
                   toID= who.ID,
                   damageType= new vitrulDamage() { damage = 6 } });
        }

        public class Handler_AttackOn : NetHandler<AttackModule, EntityNetBehaviour, Link_AttackOnStart, Link_AttackOnReceive>
        {
            public override Link_AttackOnReceive ServerHandle(AttackModule baseOn, Link_AttackOnStart link_server)
            {
                float dmageValue = link_server.damageType.damage+666+link_server.fromID;
                return new Link_AttackOnReceive(){ fromID=link_server.fromID,toID=link_server.toID, damageValue=dmageValue };
            }
            public override void ClientHandle(AttackModule baseOn, Link_AttackOnReceive link_client)
            {
                Entity e = /*link_server.toID=>Entity*/ null ;
                e.VariableData.Health -= link_client.damageValue;
            }
        }
    }
}
