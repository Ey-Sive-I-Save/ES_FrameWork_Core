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
    public abstract class ESNetBehaviour<ForObject> : NetworkBehaviour where ForObject:ESObjectBase
    {
        //public ESObject ConnectedObject;
        [LabelText("初始化生成")]
        public bool StartSpawn = false;
        [LabelText("关联核心")]
        public ForObject ConnectedCoreObject;
        private void Awake()
        {
            if (ConnectedCoreObject == null)
            {
                ConnectedCoreObject = GetComponent<ForObject>();
            }
            if (ConnectedCoreObject == null)
            {
                enabled = false;
                StartSpawn = false;
            }
            else
            {

            }
        }
        public abstract void TryConnectCoreObject(ForObject coreObject);
        public override void OnStartClient()
        {
            base.OnStartClient();
            ConnectedCoreObject.ID = this.ObjectId;
        }
        public override void OnStartServer()
        {
            base.OnStartServer();
            ConnectedCoreObject.ID = this.ObjectId;
        }
        public override void OnStopClient()
        {
            ConnectedCoreObject.ID = -1;
            base.OnStopClient();
        }
        public override void OnStopServer()
        {
            ConnectedCoreObject.ID = -1;
            base.OnStopServer();
        }
    }
}

