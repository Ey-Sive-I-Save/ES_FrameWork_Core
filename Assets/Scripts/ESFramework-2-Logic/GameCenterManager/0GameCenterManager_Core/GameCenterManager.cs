using ES.Pointer;
using FishNet.Broadcast;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace ES
{
    public struct aa : IBroadcast
    {

    }
    [DefaultExecutionOrder(-8)]
    [TypeRegistryItem("游戏核心管理器")]
    public partial class GameCenterManager : SingletonAsCore<GameCenterManager>,IReceiveLink<aa>
    {


        [NonSerialized]public bool NetSupport = false;
        [TabGroup("常规", "独立管理器"), LabelText("音效管理器")] public ESAudioMaster AudioMaster;
        protected override void OnBeforeAwakeRegister()
        {
           
            base.OnBeforeAwakeRegister();

          

            SceneManager.sceneUnloaded += (a) => { this.StopAllCoroutines(); };
        }
        
        [Button("发送任意Link")]
        public void SendLink<Link>(Link link) where Link:ILink
        {
         
        }

        public void OnLink(aa link)
        {
            throw new NotImplementedException();
        }


        #region 生成相关

        #endregion

    }
}
