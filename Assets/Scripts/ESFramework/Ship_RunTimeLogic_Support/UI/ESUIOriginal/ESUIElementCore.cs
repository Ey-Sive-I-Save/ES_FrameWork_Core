using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ES
{

    public class ESUIElementCore : ESUICore_Original
    {
        #region 辅助杂项

        public static ILink_UI_OperationOptions defaultLink = new ILink_UI_OperationOptions();
        #endregion

        #region Tween容器
        [FoldoutGroup("缓冲")]
        public SelectDic<UITweenType, UITweenOutputOperation, Tween> CachingUITweens = new ();


        #endregion

        [ToggleGroup("RegisterToPanel","注册到面板")]
        [LabelText("注册到父级Panel")] public bool RegisterToPanel = false;
        [ToggleGroup("RegisterToPanel")]
        [LabelText("唯一标识"),SerializeField] private string _registerKey = "注册UI";
        public string RegisterKey { get { return _registerKey; } set { _registerKey = value; } }
        [ShowInInspector,ReadOnly,LabelText("依赖面板"),FoldoutGroup("详细信息")]
        public virtual ESUIPanelCore MyPanel { get { if (dirty) GetMyParentAndRegisteThis(); return _myParentPanel; } set { _myParentPanel = value; } }
        [ToggleGroup("RegisterToPanel")]
        [SerializeField,LabelText("所属面板"),HideInInspector] protected ESUIPanelCore _myParentPanel;
        protected bool dirty = false;
        public virtual ESUIPanelCore GetMyParentAndRegisteThis()
        {
            var use = this._GetCompoentInParentExcludeSelf<ESUIPanelCore>(includeInactive:true);
            if (use != null) { 
                _myParentPanel = use;

                Debug.Log(66666);
                if (RegisterToPanel)
                {
                    _myParentPanel._RegisterElement(this);
                }
            }
            return _myParentPanel;
        }
        protected override void OnAwakeRegisterOnly()
        {
            base.OnAwakeRegisterOnly();
            
            GetMyParentAndRegisteThis();
        }
        protected override void OnEnable()
        {
            base.OnEnable();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
        }


        [TabGroup("开关执行"), LabelText("开启时执行"), SerializeReference]
        public IOutputOperationUI whenOpen;
        [TabGroup("开关执行"), LabelText("关闭时执行"), SerializeReference]
        public IOutputOperationUI whenClose;

        protected override void OnOpen()
        {
            base.OnOpen();
            if (whenClose != null)
            {
                whenClose.TryCancel(this, _myParentPanel, defaultLink);
            }
            if (whenOpen != null)
            {
                whenOpen.TryOperation(this,_myParentPanel, defaultLink);
            }
        }
        protected override void OnClose()
        {
            base.OnClose();
            if (whenOpen != null)
            {
                whenOpen.TryCancel(this, _myParentPanel, defaultLink);
            }
            if (whenClose != null)
            {
                whenClose.TryOperation(this, _myParentPanel, defaultLink);
            }
        }

    }
}

