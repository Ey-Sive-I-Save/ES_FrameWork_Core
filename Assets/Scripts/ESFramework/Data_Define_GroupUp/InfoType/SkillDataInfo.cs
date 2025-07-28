using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES
{
    [ESDisplayNameKeyToType("数据单元", "技能数据单元")]
    public class SkillDataInfo : SoDataInfo
    {
        [ DisplayAsString(fontSize: 25), HideLabel, GUIColor("@KeyValueMatchingUtility.ColorSelector.ColorForApply")]
        public string start = "开始创建技能序列！！";
       /* [HideLabel]
        public ReleasableSkillsSequence sequence;*/
     /*   private void OnValidate()
        {
            sequence.OnChangeSlider();
        }*/
    }
}
