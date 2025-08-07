using ES;
using ES.EvPointer;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES {
    //池  字符串键  值
    [Serializable]
    public abstract class ArchPoolGetter 
    {
        public abstract ArchPool GetArchPool();//获得ArchPool
    }

    [Serializable]
    public abstract class ArchOperation : IOperation<ArchPool,string,object>
    {
        public abstract void TryOperation(ArchPool arch, string key, object value);
    }
    [Serializable,TypeRegistryItem("原型池-操作")]
    public class ArchPoolHandlePointer : PointerOnlyAction
    {
        [SerializeReference,LabelText("获得原型池")]
        public ArchPoolGetter Getter;
        [LabelText("键名")]
        public string keyName = "键名";
        [SerializeReference, LabelText("操作原型池")]
        public ArchOperation Handler;
        
        public override object Pick(object on = null, object from = null, object with = null)
        {
            var use=Getter.GetArchPool();
            Handler.TryOperation(use, keyName,with);
            return null;
        }
    }

}
