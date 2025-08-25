using ES;
using FishNet.Object;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ES
{
    [TypeRegistryItem("ES Object")]
    public abstract class ESObject : Core, IWithID
    {
        #region 原始通用域
        [LabelText("ES原始扩展域"), TabGroup("【固有】")]
        public ESObjectOriginalDomain OriginalDomain;

        #endregion

        #region 总重要信息

        protected override void OnAwakeRegisterOnly()
        {
            base.OnAwakeRegisterOnly();
            RegisterDomain(OriginalDomain);
        }

        #region 联网
        [TabGroup("【固有】"), LabelText("唯一ID"), ShowInInspector, ReadOnly]
        public int ID
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _id;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (_id != value)
                {
                    if (value == -1) { _id = -1; _OnIDNO(); }
                    if (_id == -1) { _id = value; _OnIDYes(); }
                }
            }
        }//-1代表未分配状态

        private int _id = -1;//ID=-1时，认为无身份
        #endregion

        [TabGroup("【固有】"), LabelText("刚体")] public Rigidbody Rigid;
        [TabGroup("【固有】"), LabelText("动画器")] public Animator Anim;
        [TabGroup("【固有】"), LabelText("ES超级标签")]
        public ESTagCollection ESTagsC = new ESTagCollection();


#if UNITY_EDITOR

        #region 检查器专属
        //输出网络信息
        private void DebugNO()
        {
            Debug.Log("本人的" + NetObject.IsOwner);
            Debug.Log("客户的" + NetObject.IsClientInitialized);
            Debug.Log("服务器的" + NetObject.IsServerInitialized);
        }
        #endregion
        [Required(errorMessage: "如果你制作网络游戏，一般需要配置给他一个FinshnetNetworkObject"), PropertyOrder(-10), PropertySpace(5, 15)
           /* , InlineButton("DebugNO", "输出NO信息")*/]
        [LabelText("链接为网络对象")]
#endif
        [ ToggleGroup("IsNet"), ShowIf("IsNet"),ESBackGround("yellow",0.25f,WithAlpha =0.1f)]
        public NetworkObject NetObject;
        [ToggleGroup("IsNet"), ShowIf("IsNet"), ESBackGround("yellow", 0.25f)]
        public ESNetBehaviour NetBehaviour;
        [ToggleGroup("IsNet","联网对象"), ShowInInspector, LabelText("是联网的")]
        public bool IsNet
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return _isNet; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (_isNet != value)
                {
                    _isNet = value;
                   /* if (value)
                    {
                        if (NetObject == null)
                        {
                            NetObject.GetComponentInParent<ESNetObject>();
                            if (NetObject == null)
                            {
                                *//*NetObject = gameObject.AddComponent<ESNetObject>();*//*
                            }
                        }
                    }*/
                }
            }
        }
        [SerializeField, HideInInspector] private bool _isNet = false;
        protected override void Awake()//不要有关ID 的任何操作--》
        {
            _isNet = NetObject != null;
            base.Awake();
        }
        #endregion

        protected override void OnEnable()
        {
            if (_id == -1 && IsNet)
            {
                NetBehaviour.WaitingTaskAtClient.Enqueue(() =>
                {
                    SendIDRequest();//发送ID分配请求
                    DoWhenIDYes(() => { if (this != null) base.OnEnable(); });
                });

            }
            else
            {
                base.OnEnable();//会完成子模块的OnEnable
            }
        }

        #region 检查器专属

        #endregion

        #region 委托事件

        [FoldoutGroup("选通Link"),LabelText("碰撞来源选通")] public LinkReceiveChannelPool<Channel_ColliderFrom,Link_ColEvent_3D> 
            LinkReceiveChannel_Channel_ColliderFrom = new LinkReceiveChannelPool<Channel_ColliderFrom, Link_ColEvent_3D>();


        #endregion

        #region 寻全局ID

        private Action OnIDYesTask = GameCenterManager.NULLAction;
        private Action OnIDNOTask = GameCenterManager.NULLAction;
        public void DoWhenIDYes(Action a)
        {
            OnIDYesTask += a;
        }
        public void DoWhenIDNO(Action a)
        {
            OnIDNOTask += a;
        }
        public abstract void _InTable();
        public abstract void _OutTable();
        public virtual void _OnIDYes()
        {
            _InTable();
            OnIDYesTask.Invoke();
            OnIDYesTask = GameCenterManager.NULLAction;
            base.OnEnable();//
        }
        public virtual void _OnIDNO()
        {
            _OutTable();
            OnIDNOTask.Invoke();
            OnIDNOTask = GameCenterManager.NULLAction;
            base.OnDisable();
        }

        public void SendIDRequest()
        {
            Debug.Log("ID Request " + IsNet);
            if (IsNet)
            {
                if (NetObject.IsOwner)
                    NetBehaviour.SendSelfLinkToServer(new Link_IDRequest());
            }
            else
            {
                ID = GameCenterManager.LocalIDCount;
            }
        }
        #endregion
    }
}


