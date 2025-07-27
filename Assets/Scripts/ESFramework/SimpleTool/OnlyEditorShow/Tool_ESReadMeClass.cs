using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES
{
    [Serializable]
    public class Tool_ESReadMeClass
    {
        [LabelText("---开始编辑---")]
        public bool edit = true;
        [DisplayAsString(fontSize: 20, EnableRichText = true), ShowIf("@!edit"), GUIColor("showColor"), ShowInInspector, HideLabel]
        public string ReadMeOut = "编写提示文件";
        [TextArea(3, 10), ShowIf("edit"), OnValueChanged("SetString")]
        public string readMeIn = "编写提示文件";
        [ColorPalette, ShowIf("edit")]
        public Color showColor = Color.white;
        private void SetString(string edit)
        {
            ReadMeOut = readMeIn;
        }
    }
}

