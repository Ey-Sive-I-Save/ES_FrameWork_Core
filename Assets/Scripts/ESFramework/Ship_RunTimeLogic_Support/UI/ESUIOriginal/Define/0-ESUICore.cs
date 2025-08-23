using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ES
{
    [DefaultExecutionOrder(-2),]//顺序在前
    public abstract class ESUICore_Original : SerializedMonoBehaviour, ICore
    {
        #region 原始通用域
        [LabelText("ESUI原始扩展域"), TabGroup("【固有】",Icon =SdfIconType.BorderAll)]
        public ESUIOriginalDomain OriginalDomain;

        #endregion

        #region 可选扩展域
        [TabGroup("信息域", Icon = SdfIconType.FileSpreadsheetFill, TextColor = "@Editor_DomainTabColor(MessageProviderDomain)")]
        [SerializeReference, InlineProperty, HideLabel]
        public ESUIMessageProviderDomain MessageProviderDomain;

        #endregion

        #region 自主开关
        [ToggleGroup("AutoOpenAndCloseByEnableState", "打开自主活动")]
        public bool AutoOpenAndCloseByEnableState = true;
        [ToggleGroup("AutoOpenAndCloseByEnableState"),LabelText("第一次开启时认为已经完成初始化")]
        public bool InitOpen = false;
        #endregion

        #region 开关
        [ButtonGroup("oc"), Button("打开")]
        public void TryOpen(bool must=false)
        {
            if (!must&&enabled && InitOpen) return;//还在使用呢
            InitOpen = true;
            this.enabled = true;  //可见不一定可用把
            OnOpen();
            gameObject.SetActive(true);//打开必可见-√
        }
        [ButtonGroup("oc"),Button("关闭")]
        public void TryClose(bool must = false)
        {
            if (!must&&!enabled) return;//已经禁用了哈
            if(gameObject.activeInHierarchy)this.enabled = false;
            OnClose();
            //关闭不一定可见--或者需要等待不可见
        }
        protected virtual void OnOpen()
        {
            //
            
        }
        protected virtual void OnClose()
        {
            
        }
        #endregion

        #region 控件和脚本
        [TabGroup("常规脚本引用",Icon = SdfIconType.CodeSquare), LabelText("引用Rect Tran")]
        public ESReferLazy<RectTransform> Refer_Rect = new ESReferLazy<RectTransform>();
/*        [TabGroup("常规脚本引用"),LabelText("启用Imahge")]
        public bool EnableImage = false;
        [TabGroup("常规脚本引用")]
        public ESReferLazy<Image> Refer_Image = new ESReferLazy<Image>();
        [TabGroup("常规脚本引用"), LabelText("启用TMP_Text")]
        public bool EnableTMP = false;
        [TabGroup("常规脚本引用")]
        public ESReferLazy<TMP_Text> Refer_TextPro = new ESReferLazy<TMP_Text>();
        [TabGroup("常规脚本引用"), LabelText("启用Button")]
        public bool EnableButton = false;
        [TabGroup("常规脚本引用")]
        public ESReferLazy<Button> Refer_Button = new ESReferLazy<Button>();*/


        #endregion

        //注册和注销
        #region 检查器专属

        //域颜色赋予
        public Color Editor_DomainTabColor(IESUIDomain domain)
        {
            if (domain == null) return Color.gray;
            else return Color.yellow;
        }



        //编辑器模式下的临时关联
#if UNITY_EDITOR
        [ContextMenu("<ES>创建临时关系")]
        public void CreateCacheRelationship()
        {
            var all = GetComponentsInChildren<IDomain>();
            foreach (var i in all)
            {
                i.RegisterAllButOnlyCreateRelationship(this);
            }
        }
#endif

        #endregion

        #region 补充信息

        //获取特定域
        private List<IESUIDomain> domains = new List<IESUIDomain>(3);



        #endregion

        #region Awake流程
        //Awake回调
        protected void Awake()
        {
            Debug.Log("AWAKE UI");
            _DoAwake();
        }
        public void _DoAwake()
        {
            OnBeforeAwakeRegister();
            OnAwakeRegisterOnly();
            OnAfterAwakeRegister();
        }

        //注册扩展Domain前发生前
        protected virtual void OnBeforeAwakeRegister()
        {
            Refer_Rect.SetValueSourceGetter(()=>transform as RectTransform);
        }
        //仅用于手动注册
        protected virtual void OnAwakeRegisterOnly()
        {
            RegisterDomain(OriginalDomain);
            RegisterDomain(MessageProviderDomain);
        }
        //注册扩展Domain发生的事
        protected virtual void OnAfterAwakeRegister()
        {
            /*RegisterDomains*/
        }

        #endregion

        #region 重写逻辑

        protected virtual void OnEnable()
        {
            Debug.Log("ENable UI");
            if (AutoOpenAndCloseByEnableState) {
                TryOpen(true); 
            }
            for (int i = 0; i < domains.Count; i++)
            {
                domains[i].TryEnableSelf();
            }
        }

        protected virtual void OnDisable()
        {
            
#if UNITY_EDITOR
            if (ESEditorRuntimePartMaster.IsQuit) return;
#endif
            
            if (AutoOpenAndCloseByEnableState) {
                //防止不小心关掉
                TryClose(true); 
            }
            for (int i = 0; i < domains.Count; i++)
            {
                domains[i].TryDisableSelf();
            }
        }

        protected virtual void OnDestroy()
        {
#if UNITY_EDITOR
            if (ESEditorRuntimePartMaster.IsQuit) return;
#endif
            for (int i = 0; i < domains.Count; i++)
            {
                domains[i].TryDestroySelf();
            }
        }

        #endregion

        #region 常用功能

        //手动注册
        public void RegisterDomain(IESUIDomain domain)
        {
            if (domain != null)
            {
                domain.RegisterThisWithCore(this);
                domains.Add(domain);
            }
        }

        #endregion

    }

}
