using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES
{

    public class ESUIPanelCore : ESUIElementCore
    {
        [LabelText("全部元素(持久)")]
        public Dictionary<string, ESUIElementCore> AllElements = new Dictionary<string, ESUIElementCore>();
#if UNITY_EDITOR
        [ValueDropdown("ElementKeys",AppendNextDrawer =true),LabelText("元素测试")]
        public string testElement = "测试";
#endif

        public ESUIRoot MyRoot { get { if (dirty) GetMyParentAndRegisteThis(); return _myRoot=_myRoot._IsNotNullAndUse()??_myParentPanel._IsNotNullAndUse().MyRoot; } set { _myRoot = value; } }
        [SerializeField, LabelText("所属根节点")] private ESUIRoot _myRoot;
        public List<string> ElementKeys()
        {
            List<string> strings = new List<string>(5);
            var es = transform.GetComponentsInChildren<ESUIElementCore>();
            foreach (var i in es)
            {
                if (i.register&&i!=this)
                {
                    AddKeyReleThis(i,strings);
                }
            }
            return strings;
        }
        public void AddKeyReleThis(ESUIElementCore element, List<string> toAdd,string post="")
        {
            var parent = element._GetCompoentInParentExcludeSelf<ESUIPanelCore>();
            if (parent == this||parent==null) toAdd.Add(element.RegisterKey+post);
            else AddKeyReleThis(parent,toAdd,"/"+ element.RegisterKey + post); 
        }
        public string GetKeyReleThis(ESUIElementCore element,string post = "")
        {
            var parent = element._GetCompoentInParentExcludeSelf<ESUIPanelCore>();
            if (parent == this || parent == null) return element.RegisterKey+post;
            else return GetKeyReleThis(parent, "/" + element.RegisterKey + post);
        }
        [Button("注册全部元素")]
        public void RegisterAllElements()
        {
            var es = transform._GetCompoentsInChildExcludeSelf<ESUIElementCore>();
            foreach (var i in es)
            {
                i.GetMyParentAndRegisteThis();
             
 
            }
        }
        public void _UnRegisterElement(ESUIElementCore i)
        {
            if (AllElements.TryGetValue(i.RegisterKey, out var e))
            {
                if (e == i)
                {
                    AllElements.Remove(i.RegisterKey);
                }
            }
            else
            {

            }
        }
        public void _RegisterElement(ESUIElementCore i)
        {
            if (AllElements.TryGetValue(i.RegisterKey, out var e))
            {

            }
            else
            {
                AllElements.Add(i.RegisterKey, i);
            }

        }


        public override ESUIPanelCore GetMyParentAndRegisteThis()
        {
            var root = this._GetCompoentInParentExcludeSelf<ESUIRoot>();
            if (root != null)
            {
                _myRoot = root;
                if (register)
                {
                    _myRoot._RegisterPanel(this);
                }
            }
            return base.GetMyParentAndRegisteThis();
        }
    }
}

