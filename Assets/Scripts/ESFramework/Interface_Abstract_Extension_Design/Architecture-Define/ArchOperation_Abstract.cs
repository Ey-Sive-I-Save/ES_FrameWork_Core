using ES;
using ES.EvPointer;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES {

    [Serializable]
    /* 
     *简单值控 的实现 -》  Assets/Scripts/ESFramework/Ship_RunTimeLogic_Support/Arch/ArchOperation_EasyValueSet.cs
     
     */
    public abstract class ArchOperation_Abstract : IOperation<ArchPool, string, object>
    {
        public abstract void TryOperation(ArchPool arch, string key, object value = null);
    }
    //池  字符串键  值
    [Serializable]
    public abstract class ArchPoolGetterEasy 
    {
        public abstract ArchPool GetArchPool();//获得ArchPool
    }

    [Serializable,TypeRegistryItem("原型池-简易通用操作")]
    public class ArchPoolHandleEasyPointer : PointerOnlyAction
    {
        [SerializeReference,LabelText("获得原型池")]
        public ArchPoolGetterEasy Getter;
        [LabelText("键名")]
        public string keyName = "键名";
        [SerializeReference, LabelText("操作原型池")]
        public ArchOperation_Abstract Handler;
        
        public override object Pick(object on = null, object from = null, object with = null)
        {
            var use=Getter.GetArchPool();
            Handler.TryOperation(use, keyName,with);
            return null;
        }
    }

}
