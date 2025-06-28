using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES
{
    
    public class  HoldableDomainForItem : Domain<Item, HoldableModuleForItem>
    {
        //捡起来！
    }
    [Serializable]
    public abstract class HoldableModuleForItem : Module<Item,HoldableDomainForItem>
    {

    }


    [Serializable, TypeRegistryItem("捡起来后的特效")]
    public class 捡起来后的特效 : HoldableModuleForItem
    {
        public Color color;
        public float howMuch = 10;
        protected override void Update()
        {

        }
        protected override void CreateRelationshipOnly()
        {
            base.CreateRelationshipOnly();
            //Domain.Module_XXX = this;  显性引用
        }

        public override string[] Editor_AllPresets => presetForThis;
        static string[] presetForThis = {"普通特效","珍贵特效","传奇特效" };

        protected override void SetupModuleByPreset(string preset)
        {
            
            switch (preset)
            {
                case "普通特效":
                    color = Color.white;
                    howMuch = 5;
                    break;
                case "珍贵特效":
                    string s= Core.gameObject.name;
                    if (s.StartsWith("哇"))
                    {

                    }
                    else
                    {
                        Core.gameObject.name = "哇！是" + Core.gameObject.name;
                    }
                    color = Color.yellow;
                    howMuch = 12;
                    break;
                case "传奇特效":
                    color = Color.blue;
                    howMuch = 25;
                    break;
            }
            if(preset != "珍贵特效")
            {
                string s = Core.gameObject.name;
                Core.gameObject.name = "不珍贵";
            }
 
        }

    }
    [Serializable, TypeRegistryItem("捡起来的附加条件判断")]
    public class 捡起来的附加条件判断 : HoldableModuleForItem
    {
        protected override void OnEnable()
        {
            //注册
            base.OnEnable();
        }
        protected override void OnDisable()
        {
            //注销
            base.OnDisable();
        }
        protected override void Update()
        {
            
        }
        protected override void CreateRelationshipOnly()
        {
            base.CreateRelationshipOnly();
            //Domain.Module_XXX = this;  显性引用
        }
    }
}
