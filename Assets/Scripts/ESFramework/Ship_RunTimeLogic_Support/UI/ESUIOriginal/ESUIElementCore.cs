using DG.Tweening;
using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES
{

    public class ESUIElementCore : ESUICore_Original
    {
        #region 辅助杂项

        public static ILink_UI_OperationOptions defaultLink = new ILink_UI_OperationOptions();
        #endregion

        #region Tween容器
        public SelectDic<UITweenType, UITweenOutputOperation, Tween> UITweens = new ();


        #endregion

        [ToggleGroup("RegisterToPanel","注册到面板")]
        [LabelText("注册到父级Panel")] public bool RegisterToPanel = false;
        [ToggleGroup("RegisterToPanel")]
        [LabelText("唯一标识"),SerializeField] private string _registerKey = "注册UI";
        public string RegisterKey { get { return _registerKey; } set { _registerKey = value; } }
        public ESUIPanelCore MyPanel { get { if (dirty) GetMyParentAndRegisteThis(); return _myParentPanel; } set { _myParentPanel = value; } }
        [ToggleGroup("RegisterToPanel")]
        [SerializeField,LabelText("所属面板"),HideInInspector] protected ESUIPanelCore _myParentPanel;
        protected bool dirty = false;
        public virtual ESUIPanelCore GetMyParentAndRegisteThis()
        {
            var use = this._GetCompoentInParentExcludeSelf<ESUIPanelCore>(includeInactive:true);
            if (use != null) { 
                _myParentPanel = use;
                if (RegisterToPanel)
                {
                    _myParentPanel._RegisterElement(this);
                }
            }
            return _myParentPanel;
        }
        protected override void Awake()
        {
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

        protected override void OnOpen(ILink link)
        {
            base.OnOpen(link);
            if (whenOpen != null)
            {
                whenOpen.TryOperation(this,_myParentPanel, defaultLink);
            }
        }
        protected override void OnClose(ILink link)
        {
            base.OnClose(link);
            if (whenOpen != null)
            {
                whenOpen.TryOperation(this, _myParentPanel, defaultLink);
            }
        }

    }
}

