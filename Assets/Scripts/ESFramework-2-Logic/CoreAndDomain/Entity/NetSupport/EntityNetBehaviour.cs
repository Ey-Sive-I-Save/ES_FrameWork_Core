using ES;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES {
    public partial class EntityNetBehaviour : ESNetBehaviour<Entity>
    {
        public override void TryConnectCoreObject(Entity coreObject)
        {
            coreObject.NetBehaviour = this;
        }
        [ServerRpc]
        public void Spawn(GameObject gameObject,Vector3 pos,Vector3 direct,int fromID,NetworkConnection connection)
        {
            GameObject ins = MonoBehaviour.Instantiate(gameObject, pos, Quaternion.LookRotation(direct));
            var item = ins.GetComponent<Item>(); Debug.Log("ITEM" + item);
            var fly = item.HurtableDomain?.Module_Flying;
            if (fly != null)
            {
                fly.source = GameCenterManager.EntityIDPool[fromID];
                fly.TargetDirect = fly.source.transform.forward;
                Physics.IgnoreCollision(item.col, fly.source.col);
            }
            InstanceFinder.ServerManager.Spawn(ins,connection);
        }
    }
}
