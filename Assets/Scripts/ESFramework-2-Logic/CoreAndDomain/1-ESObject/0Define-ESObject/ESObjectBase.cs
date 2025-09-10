using ES;
using FishNet.Object;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ES
{
    [TypeRegistryItem("ES Object")]
    public abstract class ESObjectBase : Core, IWithID
    {
        
        #region 原始通用域
        [HideLabel, TabGroup("扩展域","【原始】",TabLayouting = TabLayouting.MultiRow)]
        public ESObjectOriginalDomain OriginalDomain;

        #endregion

        #region 总重要信息
        [TabGroup("常规", "【固有】"), LabelText("状态"),ReadOnly]
        public ESObjectState State = ESObjectState.None;
        [TabGroup("常规", "【固有】"), LabelText("刚体")] public Rigidbody Rigid;
        [TabGroup("常规", "【固有】"), LabelText("动画器")] public Animator Anim;
        [TabGroup("常规", "【固有】"), LabelText("ES超级标签")]
        public ESTagCollection ESTagsC = new ESTagCollection();

        

        #endregion
        protected override void OnAwakeRegisterOnly()
        {
            base.OnAwakeRegisterOnly();
            RegisterDomain(OriginalDomain);
        }

        #region 联网
        [TabGroup("常规","【固有】"), LabelText("唯一ID"), ShowInInspector, ReadOnly]
        public int ID
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _id;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (_id != value)
                {
                    if (value == -1) { _OnIDNO(); _id = -1;}
                    else if (_id == -1) { _id = value; _OnIDYes(); }
                }
            }
        }//-1代表未分配状态

        private int _id = -1;//ID=-1时，认为无身份
        #endregion



        [ToggleGroup("IsNet", "联网对象"), ShowInInspector, LabelText("是联网的")]
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
                    if (value)
                    {
                        if (NetObject == null)
                        {
                            NetObject = gameObject.GetComponentInParent<NetworkObject>();
                            if (NetObject == null)
                            {
                                NetObject = gameObject.AddComponent<NetworkObject>();
                            }
                        }
                    }
                    else
                    {
                        if (NetObject != null && Application.isEditor)
                        {
                            DestroyImmediate(NetObject);
                            NetObject = null;
                        }

                    }
                }
            }
        }
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
        
        [ToggleGroup("IsNet"), ShowIf("IsNet"),ESBackGround("yellow",0.25f,WithAlpha =0.1f)]
        public NetworkObject NetObject;
        [ToggleGroup("IsNet"), ShowIf("IsNet"), ESBackGround("yellow", 0.25f)]
       
        [SerializeField, HideInInspector] private bool _isNet = false;
        protected override void Awake()//不要有关ID 的任何操作--》
        {
            _isNet = NetObject != null;
            base.Awake();
        }
        #region 检查器专属

        #endregion

        #region 委托事件

        [FoldoutGroup("选通Link"),LabelText("碰撞来源选通")] public LinkReceiveChannelPool<Channel_ColliderFrom,Link_ColEvent_3D> 
            LinkReceiveChannel_Channel_ColliderFrom = new LinkReceiveChannelPool<Channel_ColliderFrom, Link_ColEvent_3D>();


        #endregion

        protected override void OnEnable()
        {
            if (IsNet)
            {

            }
            else
            {
                ID = GameCenterManager.LocalIDCount;
            }
            base.OnEnable();
            
        }

        protected override void OnDisable()
        {
            if(State== ESObjectState.Reset)
            {
                ID = -1;
            }
            base.OnDisable();
        }

        #region 寻全局ID
        public abstract void _InTable();
        public abstract void _OutTable();
        public virtual void _OnIDYes()
        {
            _InTable();
            State = ESObjectState.Live;
        }
        public virtual void _OnIDNO()
        {
            _OutTable();
            State = ESObjectState.Reset;
        }

        public void SendIDRequest()
        {
            if (IsNet)
            {
                
            }
            else
            {
               //本地的
            }
        }
        #endregion

        protected override void OnDestroy()
        {
            base.OnDestroy();
            ID = -1;
        }
    }

    public enum ESObjectState
    {
        [InspectorName("未定义")]None,//未初始化
        [InspectorName("有生命的")] Live,//存活的 -- 即使被禁用
        [InspectorName("重置的")] Reset, // 重置的--》意味着需要重置,也许在池中
    }
}


