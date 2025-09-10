using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES
{
    public partial class Item : ESObjectBase, IWithSharedAndVariableData<ESItemSharedData, ESItemVariableData>
    {
        [TabGroup("扩展域", "拾取", TextColor = "@Editor_DomainTabColor(HoldableDomain)")]
        [SerializeReference, InlineProperty, HideLabel]
        //显性声明扩展域
        public HoldableDomainForItem HoldableDomain;
        [TabGroup("扩展域", "伤害", TextColor = "@Editor_DomainTabColor(HurtableDomain)")]
        [SerializeReference, InlineProperty, HideLabel]
        public HurtableDomainForItem HurtableDomain;
        //public 02DomainForXXX StateMachineDomain;
        [InfoBox("共享数据和变量数据请尽量在游戏开始前去数据工具修改")]
        [FoldoutGroup("属性")]
        [LabelText("来源SO")]
        public ItemDataInfo dataInfo;
        [FoldoutGroup("属性")]
        [SerializeReference,LabelText("共享数据")]
        public ESItemSharedData sharedData;
        [FoldoutGroup("属性")]
        [SerializeReference, LabelText("变量数据")]
        public ESItemVariableData defaultData;
        [LabelText("启用数据赋予")]
        [FoldoutGroup("属性")] public bool useDataApply = true;
        public ESItemSharedData SharedData { get => sharedData; set => sharedData = value; }
        public ESItemVariableData VariableData { get => defaultData; set => defaultData = value; }
        public Collider col;
        protected override void OnAwakeRegisterOnly()
        {
            base.OnAwakeRegisterOnly();
            RegisterDomain(HurtableDomain);
            RegisterDomain(HoldableDomain);
        }

        public override void _InTable()
        {
            GameCenterManager.ItemIDPool.Add(ID,this);
        }

        public override void _OutTable()
        {
             GameCenterManager.ItemIDPool.Remove(ID);
            Debug.Log("REMOVE" + ID);
        }

        //注册前的操作
        protected override void OnBeforeAwakeRegister()
        {
            if (dataInfo != null&&useDataApply)
                ESDesignUtility.DataApply.CopyToClassSameType(dataInfo, this);

            base.OnBeforeAwakeRegister();
        }
        public Action<Entity, Vector3> OnTriEntityHappen = (a, b) => { };
        private void OnTriggerEnter(Collider other)
        {
            if (NetObject.IsServerInitialized)
            {
                var en = other.GetComponentInParent<Entity>();
                if (en != null)
                {
                    OnTriEntityHappen?.Invoke(en, (other.ClosestPoint(transform.position)));
                }
            }
        }
    }
}
   

