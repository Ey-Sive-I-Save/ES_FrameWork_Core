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
    [TypeRegistryItem("Entity,Item的父类")]
    public abstract class ESObject : Core, IWithID
    {

        #region 总重要信息
        #region 联网
        [FoldoutGroup("【固有】"), LabelText("唯一ID"), ReadOnly]
        public int ID
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _id;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (_id != value)
                {
                    _id = value;
                    if (_id == -1) _OnIDNO();
                    else _OnIDYes();
                }
            }
        }//-1代表未分配状态
        private int _id = -1;//ID=-1时，认为无身份
        #endregion
#if UNITY_EDITOR
        [FoldoutGroup("【固有】"), LabelText("是联网的"), SerializeField] private bool Editor_IsNetObject = false;
#endif

        [FoldoutGroup("【固有】"), LabelText("刚体")] public Rigidbody Rigid;
        [FoldoutGroup("【固有】"), LabelText("原始动画器")] public Animator Anim;
        [FoldoutGroup("【固有】"), LabelText("ES超级标签")]
        public ESTagCollection ESTagsC = new ESTagCollection();


#if UNITY_EDITOR
        [BoxGroup("网络支持", VisibleIf = "Editor_IsNetObject")]
        [Required(errorMessage: "如果你制作网络游戏，一般需要配置给他一个FinshnetNetworkObject"), PropertyOrder(-10), PropertySpace(5, 15), InlineButton("DebugNO", "输出NO信息")]
        [LabelText("链接为网络对象")]
#endif

        public ESNetObject NetObject;
        [ShowInInspector, LabelText("是联网的")]
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
                            NetObject.GetComponentInParent<ESNetObject>();
                            if (NetObject == null)
                            {
                                NetObject = gameObject.AddComponent<ESNetObject>();
                            }
                        }
                    }
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
                SendIDRequest();//发送ID分配请求
                DoWhenIDYes(() => { if (this != null) base.OnEnable(); });
            }
            else base.OnEnable();//会完成子模块的OnEnable
        }

        #region 检查器专属
        //输出网络信息
        private void DebugNO()
        {
            Debug.Log("本人的" + NetObject.IsOwner);
            Debug.Log("客户的" + NetObject.IsClientInitialized);
            Debug.Log("服务器的" + NetObject.IsServerInitialized);
        }

        #endregion

        #region 委托事件

        [FoldoutGroup("委托事件(Unity)")] public Action<Collision, Vector3, bool> OnColHappen = (col, where, isEntity) => { };
        [FoldoutGroup("委托事件(Unity)")] public Action<Collider, Vector3, bool> OnTriHappen = (tri, where, isEntity) => { };

        [FoldoutGroup("委托事件(Unity)")] public Action<Entity, Vector3> OnColEntityHappen = (col, where) => { };
        [FoldoutGroup("委托事件(Unity)")] public Action<Entity, Vector3> OnTriEntityHappen = (tri, where) => { };

        [FoldoutGroup("委托事件(Unity)")] public Action<Link_DestroyWhy> OnDestroyHappen = (why) => { };


        #endregion

        #region 隐藏属性表
        [NonSerialized] public Link_DestroyWhy whyDes;
        [NonSerialized] public Queue<Entity> ignoreEntities = new Queue<Entity>();
        #endregion

        #region 回调

        public virtual void TryDestroyThisESObject()
        {
            OnDestroyHappen?.Invoke(whyDes);
            Destroy(gameObject);
        }
        public void AddIgnoreEntity(Entity e)
        {
            ignoreEntities.Enqueue(e);

            if (ignoreEntities.Peek() == null)
            {
                ignoreEntities.Dequeue();
            }
        }


        #endregion

        #region 寻ID

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
            OnIDYesTask.Invoke();
            OnIDYesTask = GameCenterManager.NULLAction;
            base.OnEnable();//
        }
        public virtual void _OnIDNO()
        {
            OnIDNOTask.Invoke();
            OnIDNOTask = GameCenterManager.NULLAction;
            base.OnDisable();
        }

        public void SendIDRequest()
        {
            if (IsNet)
            {

            }
            else
            {
                ID = GameCenterManager.LocalIDCount;
            }
        }
        #endregion
    }
}


