using ES;
using ES.EvPointer;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.MessageBox;
using UnityEngine.Events;
using static ES.EnumCollect;
using FishNet.Broadcast;
using FishNet.Serializing;
using UnityEngine.Rendering;
using FishNet.Transporting;

namespace ES
{


    #region 数据结构LINK
    #region 完成支持的结构类型声明
    [Serializable, TypeRegistryItem("自定义事件名Link")]
    public struct Link_StringNameEvent : ILink
    {
        [LabelText("事件名称")] public string eventName;
        [LabelText("传输的数据")] public object param;
    }
    public class Link_BuffHandleChangeHappen : ILink
    {
        public Entity who;
        public BuffSoInfo info;
        public bool add;
    }

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
    public class aa : IBroadcast
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
        void RunTask(LinkTaskEnvironment e);
    }


    #endregion



    #region 特殊扩展

    public interface ILinkMatchPin : ILink
    {
    }
    public interface ILink<out On, out From, out With> : ILinkMatchPin
    {
        On On_ { get; }
        From From_ { get; }
        With With_ { get; }
        object UsePointer<Back>(IPointer<Back, With, From, With> pointer)
        {
            return pointer.Pick(On_, From_, With_);
        }

    }

    public interface IReceiveLink<in Link> : IReceiveLink where Link : ILink
    {
        void OnLink(Link link);
      
    }
    public interface IReceiveChannelLink<in Channel,in Link> : IReceiveLink<Link> where Link : ILink
    {
        void OnLink(Channel channel, Link link);
        void IReceiveLink<Link>.OnLink(Link link)
        {
            OnLink(default,link);
        }
    }

    public interface IReceiveAnyChanneLink<Link> : IReceiveChannelLink<IChannel, Link> where Link:ILink
    {

    }
    public interface IReceiveAnyLink : IReceiveAnyChanneLink<ILink>
    {

    }
    #endregion

    #region Link扩展
    [Serializable]
    public class LinkFunc<On, From, With> : ILink<On, From, With>
    {

        [SerializeReference] public IPointerOnlyBack<On> on;
        [SerializeReference] public IPointerOnlyBack<From> from;
        [SerializeReference] public IPointerOnlyBack<With> with;

        public On On_ => on.Pick();

        public From From_ => from.Pick();

        public With With_ => with.Pick();


    }
    [Serializable]
    public class LinkDirect<On, From, With> : ILink<On, From, With>
    {
        [SerializeReference] public On on;
        [SerializeReference] public From from;
        [SerializeReference] public With with;

        public On On_ => on;

        public From From_ => from;

        public With With_ => with;

    }
    #endregion

    #region UnityEvent支持
    [Serializable]
    public class LinkUnityEvent<Link> where Link : ILink
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
        [Button(name: "发射(默认Link)", icon: SdfIconType.SendPlusFill, Style = ButtonStyle.FoldoutButton, ButtonHeight = 40), GUIColor("orange"), PropertyOrder(1)]
        public void SendLinkSelf(BaseArchitectureWithLinkAndConfiguration architecture)
        {
            SendLinkSelf(architecture, defaultLink);
        }
        [PropertySpace(20)]
        [ShowInInspector]
        [FoldoutGroup("水平/按钮测试"), PropertyOrder(2), HideLabel]
        public string delta2 => "测试触发{自定义Link}";
        [FoldoutGroup("水平/按钮测试")]
        [Button(name: "发射(自定义Link)", icon: SdfIconType.SendPlusFill, Style = ButtonStyle.FoldoutButton, ButtonHeight = 40), GUIColor("orange"), PropertyOrder(2)]
        public void SendLinkSelf(BaseArchitectureWithLinkAndConfiguration architecture, Link link)
        {
           /* switch (triggerType)
            {
                case EnumCollect.LinkEventType.SendThenInvoke:
                    architecture ??= GetArchitecture();
                    architecture.SendLink(link);
                    unityEvent?.Invoke(defaultLink);
                    break;
                case EnumCollect.LinkEventType.InvokeThenSend:
                    unityEvent?.Invoke(defaultLink);
                    architecture ??= GetArchitecture();
                    architecture.SendLink(link);
                    break;
                case EnumCollect.LinkEventType.OnlySend:
                    architecture ??= GetArchitecture();
                    architecture.SendLink(link);
                    break;
                case EnumCollect.LinkEventType.OnlyInvoke:
                    unityEvent?.Invoke(defaultLink);
                    break;
                default: break;
            }
*/

        }

    }
    /* [Serializable]
     public class PointerForSimpleUnityEvent_WithLink<ByON, Yarn, From> : IPointerOnlyBackSingle<UnityEvent<LinkDirect<ByON, Yarn, From>>>
     {
         [LabelText("unity事件")] public UnityEvent<LinkDirect<ByON,Yarn,From>> unityevent;
         public UnityEvent<LinkDirect<ByON, Yarn, From>> Pick(IPointer launcherEntity = null, IPointer launcherEntity = null, IPointer on = null)
         {
             return unityevent;
         }
     }*/
    //标准的UnityEventLink
    /*[Serializable]
    public class PointerForSimpleUnityEvent_SelfCreateLink<ByON,Yarn,From> : IPointerOnlyBackSingle<UnityEvent<ByON,Yarn,From>>
    {
        [LabelText("unity事件")] public UnityEvent<ByON, Yarn, From> unityevent;
        public UnityEvent<ByON, Yarn, From> Pick(IPointer launcherEntity = null, IPointer launcherEntity = null, IPointer on = null)
        {
            return unityevent;
        }
    }*/
    [Serializable]
    public class PointerForSimpleUnityEvent_Direct : IPointerOnlyBackForSimpleUnityEvent
    {
        [LabelText("unity事件")] public UnityEvent unityevent;
        public UnityEvent Pick(object on = null, object from = null, object with = null)
        {
            return unityevent;
        }
        object IPointer.Pick(object a, object b, object c)
        {
            return Pick();
        }
    }
    public interface IPointerOnlyBackForSimpleUnityEvent : IPointerOnlyBackSingle<UnityEvent>
    {

    }
    #endregion
    #endregion
}

