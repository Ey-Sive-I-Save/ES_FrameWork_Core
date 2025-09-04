using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static ES.EnumCollect;

using UnityEngine.Rendering;
using FishNet.Broadcast;
using FishNet.Serializing;

namespace ES
{


    #region 数据结构LINK
    #region 完成支持的结构类型声明
/*    [Serializable, TypeRegistryItem("自定义事件名Link")]
    public struct Link_StringNameEvent : ILink
    {
        [LabelText("事件名称")] public string eventName;
        [LabelText("传输的数据")] public object param;
    }*/

    #endregion

    #region Link原始
    //Link本质上上是一个事件数据(相当于一个Action?.Invoke())，
    //由于类型信息也是独立的，
    //Link也就可以规定一类事件
    public interface ILink 
    {

    }
    public interface fff : IBroadcast
    {

    }
    [Serializable]
    public struct IESTESET : ILink, IBroadcast
    {

    }
    public interface ILinkTest : IBroadcast
    {
        void WriterApply();
        void ReadApply();
    }
    public static class Hepler
    {
        public static void WriteILinkCaster(this Writer writer, ILinkTest value)
        {
            
        }

        // Read and return a Vector2.
        public static ILinkTest ReadILinkCaster(this Reader reader)
        {
            return null;
        }
        public static void Writefff(this Writer writer, fff value)
        {
            writer.WriteInt16(3);
        }

        // Read and return a Vector2.
        public static fff Readfff(this Reader reader)
        {
            return null;
        }
    }

    [Serializable]
    public class aaa : IBroadcast
    {

    }
    //IReceiveLink本质上上是一个接受事件处理,
    //相当于被委托的对象和方法
    public interface IReceiveLink
    {

    }

    #endregion

    #region 常用扩展
    public interface IAutoTaskLink : ILink
    {
        void RunTaskLinkAt(LinkTaskEnvironment e);
    }


    #endregion



    #region 特殊扩展
    public interface IReceiveLink<in Link> : IReceiveLink 
    {
        void OnLink(Link link);
        
    }
    public interface IReceiveChannelLink<in Channel,in Link> : IReceiveLink<Link> 
    {
        void OnLink(Channel channel, Link link);
        void IReceiveLink<Link>.OnLink(Link link)
        {
            OnLink(default,link);
        }
    }

    public interface IReceiveAnyChanneLink<Link> : IReceiveChannelLink<IChannel, Link> 
    {

    }
    public interface IReceiveAnyClassLink : IReceiveAnyChanneLink<object>
    {

    }
    [Serializable]
    //可用Class
    public class ReceiveChannelLink<Channel, Link> : IReceiveChannelLink<Channel, Link>
    {
        public Action<Channel, Link> Action;
        public ReceiveChannelLink(Action<Channel, Link> action)
        {
            Action = action;
        }

        public void OnLink(Channel channel, Link link)
        {
            Action?.Invoke(channel,link);
        }
    }
    [Serializable]
    //可用Class
    public class ReceiveLink<Link> : IReceiveLink<Link> 
    {
        public Action<Link> Action;
        public ReceiveLink(Action<Link> action)
        {
            Action = action;
        }

        public void OnLink(Link link)
        {
            Action?.Invoke(link);
        }
    }
    [Serializable]
    public class ReceiveAnyChannelLink<Link> : IReceiveAnyChanneLink<Link> 
    {
        public Action<IChannel,Link> Action;
        public ReceiveAnyChannelLink(Action<IChannel,Link> action)
        {
            Action = action;
        }

        public void OnLink(IChannel channel,Link link)
        {
            Action?.Invoke(channel,link);
        }
    }
    [Serializable]
    public class ReceiveAnyLink : IReceiveAnyClassLink
    {
        public Action<IChannel, object> Action;
        public ReceiveAnyLink(Action<IChannel, object> action)
        {
            Action = action;
        }
        public void OnLink(IChannel channel, object link)
        {
            Action?.Invoke(channel, link);
        }
    }
    #endregion

    #region Link扩展

   
    #endregion

    #region UnityEvent支持
    [Serializable]
    public class LinkUnityEvent<Link> 
    {

        [ShowInInspector, GUIColor("yellow"), PropertyOrder(-1), LabelText("LinkUnity", SdfIconType.BroadcastPin)] public virtual string description => "Link事件";
        [HorizontalGroup("水平")]
        //不需要
        [FoldoutGroup("水平/事件配置"), LabelText("触发类型", SdfIconType.Signal), GUIColor("@new Color(0.7f,0.8f,0,1)")] public EnumCollect.LinkEventType triggerType;
        //不需要
        [FoldoutGroup("水平/事件配置"), LabelText("默认Link", SdfIconType.Signal), GUIColor("@new Color(0.5f,1f,0,1)"), Indent(-1)] public Link defaultLink;
        [FoldoutGroup("水平/事件配置"), LabelText("UnityEvent", SdfIconType.EarFill)] public UnityEvent<Link> unityEvent;
        /// <summary>
        /// 
        /// </summary>
        /// <param content="architecture">原型针</param>
        [HorizontalGroup("水平", width: 200)]
        [FoldoutGroup("水平/按钮测试")]
        [InfoBox("输入原型>绑定原型>游戏默认原型"), ShowInInspector, PropertyOrder(1), HideLabel]
        public string delta => "测试触发{←默认Link}";
        [FoldoutGroup("水平/按钮测试")]

        [PropertySpace(20)]
        [ShowInInspector]
        [FoldoutGroup("水平/按钮测试"), PropertyOrder(2), HideLabel]
        public string delta2 => "测试触发{自定义Link}";
    }
    #endregion
    #endregion
}

