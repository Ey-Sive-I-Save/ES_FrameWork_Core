using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;


namespace ES
{
    [CreateAssetMenu(fileName = "独立数据-ES枚举包源", menuName = "ES数据/ES枚举源")]

    public class ESCode_EnumSource : SerializedScriptableObject
    {
        [LabelText("仅分类")]
        public string EnumSortName_ = "SortEnum";

        [LabelText("生成路径"), FolderPath, ReadOnly] public string path_= "Assets/Scripts/ESFramework/CodeGen/Enum";


        [LabelText("全部枚举"), GUIColor("@ESStaticDesignUtility.ColorSelector.Color_03"),]
        public List<EnumType> EnumTypes = new List<EnumType>();


        private void OnEnable()
        {
            load();
        }
        private void OnValidate()
        {
            load();
        }
        public void load()
        {
            foreach (var i in EnumTypes)
            {
                if (i != null)
                {
                    foreach (var ii in i.All)
                    {
                        if (ii != null) ii.enumType = i;
                    }
                }
            }
        }

        [Button("生成枚举脚本", ButtonHeight = 50)]
        public void Generate()
        {
            string allContent = $"/*生成枚举集合{EnumSortName_}*/\n";
            foreach (var type in EnumTypes)
            {
                if (type != null)
                {
                    int Now = 0;
                    allContent += ESStaticDesignUtility.SimpleScriptMaker.CreateNotes($"枚举类型名:{type.EnumTypeName},枚举中文名{type.EnumChineseName}");
                    if (type.IsFlag)
                    {
                        allContent += $"[Flags]\n";
                    }
                    allContent += $"public enum {type.EnumTypeName}{{\n";
                    foreach (var ele in type.All)
                    {
                        if (ele != null)
                        {
                            string OneElement = "";
                            if (ele.WIthMessage)
                            {
                                OneElement += $"[ESMessage({ele.withSTR._AsStringValue()},{ele.withFloat},{ele.withInt})]";
                            }
                            OneElement += $"[InspectorName(\"{ele.enumChinaName}\")]";
                            OneElement += ele.enumElementName;
                            {
                                if (ele.defineType == OneEnumElement.DefineValueType.InputHard)
                                {
                                    OneElement += "=" + (Now = ele.Input);
                                }
                                else if (ele.defineType == OneEnumElement.DefineValueType.OneByOne)
                                {
                                    Now++;
                                    if (type.IsFlag)
                                    {
                                        OneElement += "=" + "1<<" + Now;
                                    }
                                    else
                                    {
                                        //啥也不用写
                                    }
                                }
                                else if (ele.defineType == OneEnumElement.DefineValueType.Input)
                                {
                                    Now = ele.Input;
                                    if (type.IsFlag)
                                    {
                                        OneElement += "=" + "1<<" + Now;
                                    }
                                    else
                                    {
                                        OneElement += "=" + Now;
                                    }
                                }
                                else if (ele.defineType == OneEnumElement.DefineValueType.Compo)
                                {
                                    OneElement += "=";
                                    if (type.IsFlag)
                                    {
                                        if (ele.CompoNum == 2)
                                        {
                                            OneElement += string.Join(" | ", ele.Compo1, ele.Compo2);
                                        }
                                        else if (ele.CompoNum == 3)
                                        {
                                            OneElement += string.Join(" | ", ele.Compo1, ele.Compo2, ele.Compo3);
                                        }
                                        else if (ele.CompoNum == 4)
                                        {
                                            OneElement += string.Join(" | ", ele.Compo1, ele.Compo2, ele.Compo3, ele.Compo4);
                                        }
                                    }
                                    else
                                    {
                                        if (ele.CompoNum == 2)
                                        {
                                            OneElement += string.Join(" + ", ele.Compo1, ele.Compo2);
                                        }
                                        else if (ele.CompoNum == 3)
                                        {
                                            OneElement += string.Join(" + ", ele.Compo1, ele.Compo2, ele.Compo3);
                                        }
                                        else if (ele.CompoNum == 4)
                                        {
                                            OneElement += string.Join(" + ", ele.Compo1, ele.Compo2, ele.Compo3, ele.Compo4);
                                        }
                                    }
                                }
                            }
                            OneElement += ",\n";
                            allContent += OneElement;
                        }
                    }
                    allContent += $"}}\n";
                }
            }
            ESStaticDesignUtility.SimpleScriptMaker.CreateScriptBounds(path_, "EnumDefineSort_" + EnumSortName_ + ".cs",
              using_: "using ES;\r\nusing Sirenix.OdinInspector;\r\nusing System;\r\nusing System.Collections;\r\nusing System.Collections.Generic;\r\nusing UnityEngine;",
              content: allContent
                 , TruelyCreate: true);
        }
        public enum _SHOW
        {
             [ESMessage("",4,1)][InspectorName("")]a=1<<4,
            b,
            c=1<<5
        }
        [Serializable]
        public class EnumType
        {

            [FoldoutGroup("枚举类", GroupName = "@EnumChineseName", Expanded = true)]
            [InfoBox("是Flag时，实际上的值是1<<N的N次方", VisibleIf = "IsFlag")]
            [LabelText("是Flag的"), GUIColor("@ESStaticDesignUtility.ColorSelector.Color_04")] public bool IsFlag = false;
            [FoldoutGroup("枚举类"), GUIColor("@ESStaticDesignUtility.ColorSelector.Color_04")]
            [LabelText("强制修改(有风险)"), OnValueChanged("Warn")]
            public bool EDIT = true;
            [FoldoutGroup("枚举类")][LabelText("枚举类型名"), EnableIf("EDIT")] public string EnumTypeName = "NewEnumType";
            [FoldoutGroup("枚举类")][LabelText("枚举中文名"), EnableIf("EDIT")] public string EnumChineseName = "新的枚举";
            [FoldoutGroup("枚举类")][LabelText("全部元素"), GUIColor("@ESStaticDesignUtility.ColorSelector.ColorForESValue")] public List<OneEnumElement> All = new List<OneEnumElement>();



            public List<string> KeysToMe(OneEnumElement enumTo)
            {
                List<string> keys = new List<string>();
                foreach (var i in All)
                {
                    if (i != enumTo)
                    {
                        keys.Add(i.enumElementName);
                    }
                }
                return keys;
            }



            private void Warn()
            {
                if (EDIT)
                    EDIT = ESStaticDesignUtility.SafeEditor.DisplayDialog("开启强制修改", "这只能修改枚举源代码的内容，不会影响已经有的引用，可能造成大范围的Error", "继续", "算了");
            }
        }

        [Serializable]
        public class OneEnumElement
        {
            [NonSerialized]
            public EnumType enumType;
            [LabelText("枚举名(大写开头英文)")]
            public string enumElementName = "MyName";
            [LabelText("枚举中文名")]
            public string enumChinaName = "你的名字";

            #region 携带
            [LabelText("枚举携带信息(反射获取)")]
            public bool WIthMessage = false;
            [LabelText("携带字符串信息"), ShowIfGroup("withMessage", VisibleIf = "@WIthMessage")]
            public string withSTR = "message";
            [LabelText("携带浮点数信息"), ShowIfGroup("withMessage")]
            public float withFloat = 0;
            [LabelText("携带整数信息"), ShowIfGroup("withMessage")]
            public int withInt = 0;
            #endregion

            #region 定义值
            [LabelText("枚举值定义方式")]
            public DefineValueType defineType;
            [LabelText("直接输入值"), ShowIf("@defineType==DefineValueType.Input||defineType==DefineValueType.InputHard")]
            public int Input = 0;
            [LabelText("组分数量"), Range(2, 4), ShowIfGroup("加组分", VisibleIf = "@defineType==DefineValueType.Compo")]
            public int CompoNum = 2;
            [LabelText("组分1"), ShowIfGroup("加组分"), ValueDropdown("GetKeysTo")]
            public string Compo1 = "";
            [LabelText("组分2"), ShowIfGroup("加组分"), ValueDropdown("GetKeysTo")]
            public string Compo2 = "";
            [LabelText("组分3"), ValueDropdown("GetKeysTo"), ShowIf("@defineType==DefineValueType.Compo&&CompoNum>2")]
            public string Compo3 = "";
            [LabelText("组分4"), ValueDropdown("GetKeysTo"), ShowIf("@defineType==DefineValueType.Compo&&CompoNum>3")]
            public string Compo4 = "";
            public enum DefineValueType
            {
                [InspectorName("紧接着")] OneByOne,
                [InspectorName("直接输入")] Input,
                [InspectorName("组合")] Compo,
                [InspectorName("强制直接输入(无1<<)")] InputHard,
            }
            public List<string> GetKeysTo()
            {

                if (enumType != null)
                {
                    return enumType.KeysToMe(this);
                }
                Debug.Log("空");
                return new List<string>();
            }

            #endregion

        }
    }
}
