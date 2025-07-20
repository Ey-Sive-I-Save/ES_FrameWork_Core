using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


namespace ES
{
    [CreateAssetMenu(fileName = "独立数据-ES核域源", menuName = "ES数据/ES核域源")]
    public class ESCode_CoreDomainSource : SerializedScriptableObject
    {
        [ShowInInspector, LabelText("首次生成")] private bool FirstCreate { get; set; } = false;
        [LabelText("生成路径"), FolderPath, ReadOnly] public string path;
        [LabelText("核心命名"), ReadOnly, GUIColor("@KeyValueMatchingUtility.ColorSelector.ColorForCaster")]
        public string CoreClassName = "NewCore";
        [LabelText("核心中文命名"), ReadOnly, GUIColor("@KeyValueMatchingUtility.ColorSelector.ColorForCaster")]
        public string CoreClassChinaName = "新核心";
        [LabelText("全部扩展域")] public List<DomainSource> Domains = new List<DomainSource>() { new DomainSource() { DomainClassName = "Normal" } };
        [Button("生成", ButtonHeight = 50)]
        public void Generate()
        {
            string CoreName = CoreClassName._ToValidIdentName();
            string CoreChinaName = CoreClassChinaName._ToValidIdentName();
            //创建总文件夹并且获得最新路劲
            KeyValueMatchingUtility.SafeEditor.CreateFolderDic(path, "CoreCode_" + CoreName);
            string newFolder = Path.Combine(path, "CoreCode_" + CoreName);

            //第一个文件写基础定义
            string corePart = "";
            string domainPart = "";
            string modulePart = "";

            string domainInDefine = "";
            //第二类N个文件写扩展模块


            foreach (var i in Domains)
            {
                //开始一个Domain
                if (i == null) continue;
                string DomainName = i.DomainClassName._ToValidIdentName()._AddPreAndLast(CoreName, "Domain");
                string DomainChinese = i.DomainChinaClassName._ToValidIdentName()._AddPreAndLast(CoreChinaName, "扩展域");

                string moduleInDefine = "";

                string DomainModules_SingleContent = $"/*{DomainName} （{DomainChinese}）的*/";

                //抽象的
                string absName = CoreName + i.DomainClassName._ToValidIdentName() + "Module";
                string abs = KeyValueMatchingUtility.SimpleScriptMaker.CreateClassContentByString(
                   className: absName,
                   beforeClassName: "abstract",
                   insideClass: $"/* 抽象模块,核心:{CoreName},域:{DomainName}  */",
                   parent: ":" + $"Module<{CoreName}, {DomainName}>",
                   Attribute: $"[Serializable, TypeRegistryItem(\"{DomainChinese._RemoveString("扩展")}扩展模块\")]"
                   );
                modulePart += abs;
                foreach (var mm in i.Modules)
                {
                    if (mm == null) continue;
                    string moduleName = mm.ModuleClassName._ToValidIdentName()._AddPreAndLast(absName+"_", "");
                    string moduleChinaName = "扩展模块_" + mm.ModuleChinaClassName;
                    string defineName = "Module_" + mm.ModuleClassName._ToValidIdentName();

                    if (mm.DefineAtDomain)
                    {
                        moduleInDefine += $"[NonSerialized] public {moduleName} {defineName};";
                    }
                    string inside = $"  protected override void CreateRelationshipOnly()\r\n        {{\r\n            Domain.{defineName} = this;\r\n            base.CreateRelationshipOnly();\r\n        }}";
                    string aModule = KeyValueMatchingUtility.SimpleScriptMaker.CreateClassContentByString(
                   className: absName+"_"+mm.ModuleClassName,
                   beforeClassName: "",
                   insideClass: mm.DefineAtDomain ? inside : "",
                   parent: ":"+ absName,
                   Attribute: $"[Serializable, TypeRegistryItem(\"{moduleChinaName}\")]");
                    //加入
                   DomainModules_SingleContent += aModule;
                }
                domainPart += KeyValueMatchingUtility.SimpleScriptMaker.CreateClassContentByString(
                    className:DomainName,
                    beforeClassName: "partial",
                    insideClass: moduleInDefine,
                    parent: ":" + $"Domain<{CoreName}, {absName}>",
                    Attribute: $"[Serializable, TypeRegistryItem(\"{DomainChinese}\")]"
                    );

                string field = "Domain_" + i.DomainClassName._ToValidIdentName();
                domainInDefine += $" [TabGroup(\"{i.DomainChinaClassName._ToValidIdentName()}\", TextColor = \"@Editor_DomainTabColor({field})\")]\r\n        [SerializeReference,InlineProperty, HideLabel] \r\n        public {DomainName} {field};";

                KeyValueMatchingUtility.SimpleScriptMaker.CreateScriptBounds(newFolder,"ModulesDefineFor_"+DomainName+".cs",
                    using_: "using ES;\r\nusing Sirenix.OdinInspector;\r\nusing System;\r\nusing System.Collections;\r\nusing System.Collections.Generic;\r\nusing UnityEngine;",
                    content: DomainModules_SingleContent,
                    TruelyCreate:true
                    );
            }

            corePart = KeyValueMatchingUtility.SimpleScriptMaker.CreateClassContentByString(
                    className: CoreName,
                    beforeClassName: "partial",
                    insideClass: domainInDefine,
                    parent: ":" + $"Core",
                    Attribute: $""
                    );


            KeyValueMatchingUtility.SimpleScriptMaker.CreateScriptBounds(newFolder, "BaseDefineForCore_" + CoreName+".cs",
                using_: "using ES;\r\nusing Sirenix.OdinInspector;\r\nusing System;\r\nusing System.Collections;\r\nusing System.Collections.Generic;\r\nusing UnityEngine;",
                content:corePart+domainPart+modulePart
                   , TruelyCreate: true);
        }
    }
    [Serializable]
    public class DomainSource
    {
        [LabelText("扩展域命名")] public string DomainClassName = "NewDomainName";
        [LabelText("扩展域中文命名")] public string DomainChinaClassName = "新的域";
        [LabelText("全部剪影")]
        public List<ModuleSource> Modules = new List<ModuleSource>();

    }
    [Serializable]
    public class ModuleSource
    {
        [LabelText("模块名字")]
        public string ModuleClassName = "TestModule";
        [LabelText("模块中文命名")] public string ModuleChinaClassName = "新的剪影";
        [LabelText("显式声明的")]
        public bool DefineAtDomain = false;
    }
}
