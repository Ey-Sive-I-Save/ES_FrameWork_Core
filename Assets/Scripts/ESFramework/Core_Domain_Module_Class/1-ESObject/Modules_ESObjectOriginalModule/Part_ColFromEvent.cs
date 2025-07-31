using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES
{
    [Serializable, TypeRegistryItem("原始模块-碰撞来源集")]
    public class ModuleOR_Collider : ESObjectOriginalModule
    {
        #region 定义源

        [Serializable, TypeRegistryItem("选通碰撞源-指定目标")]
        public  class ToColChannelLink_Single : IReceiveChannelLink<Channel_ColEvent, Link_ColEvent_3D>
        {
            [HideInInspector]
            public ESObject ob;
            [LabelText("链接支持")]
            public EMS_ColEvent_3D_LinkSingle_Abstract ems;
            [LabelText("选通")]
            public Channel_ColliderFrom channel = Channel_ColliderFrom.Body;
            public void OnLink(Channel_ColEvent channel, Link_ColEvent_3D link)
            {
                ob.LinkReceiveChannel_Channel_ColliderFrom.SendLink(this.channel, link);
            }
        }

        #endregion

        [LabelText("全部接收源")]
        public List<ToColChannelLink_Single> ToReSource = new List<ToColChannelLink_Single>();
        protected override void OnEnable()
        {
            foreach(var i in ToReSource)
            {
                i.ob = Core;
                i.ems?.AddRecieve(i);
            }
            base.OnEnable();
        }
        protected override void OnDisable()
        {
            foreach (var i in ToReSource)
            {
                i.ob = Core;
                i.ems?.RemoveRecieve(i);
            }
        }
    }
}
