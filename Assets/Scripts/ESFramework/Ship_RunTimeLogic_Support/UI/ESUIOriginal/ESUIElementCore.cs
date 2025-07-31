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
        [LabelText("注册到Panel")] public bool register = false;
        [LabelText("唯一标识"),SerializeField] private string registerKey = "注册UI";
        public string RegisterKey { get { return registerKey; } set { registerKey = value; } }
        public ESUIPanelCore MyPanel { get { if (dirty) GetMyParentAndRegisteThis(); return _myParentPanel; } set { _myParentPanel = value; } }
        [SerializeField,LabelText("所属面板")] protected ESUIPanelCore _myParentPanel;
        protected bool dirty = false;
        public virtual ESUIPanelCore GetMyParentAndRegisteThis()
        {
            var use = this._GetCompoentInParentExcludeSelf<ESUIPanelCore>(includeInactive:true);
            if (use != null) { 
                _myParentPanel = use;
                if (register)
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
    }
}

