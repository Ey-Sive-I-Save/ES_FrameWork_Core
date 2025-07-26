using ES;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


namespace ES
{
    [CreateAssetMenu(fileName = "独立数据-ES核域源", menuName = "ES数据/ES核域源")]
    public class ESCode_CoreDomainSource : SerializedScriptableObject
    {
        #region 关系脚本
        [Title("生成于CodeGen/Target 不推荐修改，因为会被覆盖")]
        [TabGroup("关系脚本生成(不可修改)")]
        [ShowInInspector, LabelText("首次生成(没用)"),PropertyOrder(-1)] private bool FirstCreate { get; set; } = false;
        [TabGroup("关系脚本生成(不可修改)")][LabelText("生成路径"), FolderPath, ReadOnly] public string path;
        [TabGroup("关系脚本生成(不可修改)"), TabGroup("逻辑脚本生成(建议只生成一次)")]
        [LabelText("核心命名"), ReadOnly, GUIColor("@KeyValueMatchingUtility.ColorSelector.ColorForCaster")]
        public string CoreClassName = "NewCore";
        [TabGroup("关系脚本生成(不可修改)"),TabGroup("逻辑脚本生成(建议只生成一次)"),]
        [LabelText("核心中文命名"), ReadOnly, GUIColor("@KeyValueMatchingUtility.ColorSelector.ColorForCaster")]
        public string CoreClassChinaName = "新核心";

        [LabelText("继承核心选择"), ReadOnly, GUIColor("@KeyValueMatchingUtility.ColorSelector.ColorForCaster")]
        public ParentCore parent =  ParentCore.Core;

        public enum ParentCore
        {
            [InspectorName("原始核心")]Core,
            [InspectorName("ES物体")] ESObject,
            [InspectorName("生命体")] Entity,
            [InspectorName("物品")] Item
        }
        [TabGroup("关系脚本生成(不可修改)")]
        [LabelText("全部扩展域"), GUIColor("@KeyValueMatchingUtility.ColorSelector.Color_03")] 
        public List<DomainSource> Domains = new List<DomainSource>() { new DomainSource() { DomainClassName = "Normal" } };
        [TabGroup("关系脚本生成(不可修改)")]
        [Button("生成关系脚本", ButtonHeight = 50)]
        public void GenerateRelation()
        {
            string CoreName = CoreClassName._ToValidIdentName();
            string CoreChinaName = CoreClassChinaName._ToValidIdentName();
            //创建总文件夹并且获得最新路劲
            KeyValueMatchingUtility.SafeEditor.CreateFolderDic(path, "CoreCode_" + CoreName);
            string newFolder = Path.Combine(path, "CoreCode_" + CoreName);
            string domainFieldNames = "";
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
                    string moduleName = mm.ModuleClassName._ToValidIdentName()._AddPreAndLast(absName + "_", "");
                    string moduleChinaName = "扩展模块_" + mm.ModuleChinaClassName;
                    string defineName = "Module_" + mm.ModuleClassName._ToValidIdentName();

                    if (mm.DefineAtDomain)
                    {
                        moduleInDefine += $"[NonSerialized] public {moduleName} {defineName};";
                    }
                    string inside = $"  protected override void CreateRelationshipOnly()\r\n        {{\r\n            Domain.{defineName} = this;\r\n            base.CreateRelationshipOnly();\r\n        }}";
                    string aModule = KeyValueMatchingUtility.SimpleScriptMaker.CreateClassContentByString(
                   className: moduleName,
                   beforeClassName: "partial",
                   insideClass: mm.DefineAtDomain ? inside : "",
                   parent: ":" + absName,
                   Attribute: $"[Serializable, TypeRegistryItem(\"{moduleChinaName}\")]");
                    if (mm.modules != null && mm.modules.Count > 0)
                    {
                        foreach (var mm_mm in mm.modules)
                        {
                            string module_SONChinaName = "扩展模块_" + mm.ModuleChinaClassName + "_" + mm_mm.ModuleChinaClassName;
                            string sonModule = KeyValueMatchingUtility.SimpleScriptMaker.CreateClassContentByString(
                   className: moduleName + "_" + mm_mm.ModuleClassName._ToValidIdentName(),
                   beforeClassName: "partial",
                   insideClass: mm.DefineAtDomain ? inside : "",
                   parent: ":" + moduleName,
                   Attribute: $"[Serializable, TypeRegistryItem(\"{module_SONChinaName}\")]");

                            aModule += sonModule;
                        }
                        
                    }
                    //加入
                    DomainModules_SingleContent += aModule;
                }
                domainPart += KeyValueMatchingUtility.SimpleScriptMaker.CreateClassContentByString(
                    className: DomainName,
                    beforeClassName: "partial",
                    insideClass: moduleInDefine,
                    parent: ":" + $"Domain<{CoreName}, {absName}>",
                    Attribute: $"[Serializable, TypeRegistryItem(\"{DomainChinese}\")]"
                    );

                string field = "Domain_" + i.DomainClassName._ToValidIdentName();
                domainInDefine += $" [TabGroup(\"{i.DomainChinaClassName._ToValidIdentName()}\", TextColor = \"@Editor_DomainTabColor({field})\")]\r\n        [SerializeReference,InlineProperty, HideLabel] \r\n        public {DomainName} {field};";
                
                domainFieldNames += (domainFieldNames.IsNullOrWhitespace() ? "" : ",") + field;

                KeyValueMatchingUtility.SimpleScriptMaker.CreateScriptBounds(newFolder, "ModulesDefineFor_" + DomainName + ".cs",
                    using_: "using ES;\r\nusing Sirenix.OdinInspector;\r\nusing System;\r\nusing System.Collections;\r\nusing System.Collections.Generic;\r\nusing UnityEngine;",
                    content: DomainModules_SingleContent,
                    TruelyCreate: true
                    );
            }

            corePart = KeyValueMatchingUtility.SimpleScriptMaker.CreateClassContentByString(
                    className: CoreName,
                    beforeClassName: "partial",
                    insideClass: $" protected override void OnAwakeRegisterOnly()\r\n        {{\r\n            RegisterDomains({domainFieldNames});\r\n        }}\n" + domainInDefine,
                    parent: ":" + $"{parent.ToString()}",
                    Attribute: $""
                    );


            KeyValueMatchingUtility.SimpleScriptMaker.CreateScriptBounds(newFolder, "BaseDefineForCore_" + CoreName + ".cs",
                using_: "using ES;\r\nusing Sirenix.OdinInspector;\r\nusing System;\r\nusing System.Collections;\r\nusing System.Collections.Generic;\r\nusing UnityEngine;",
                content: corePart + domainPart + modulePart
                   , TruelyCreate: true);
        }
        #endregion

        #region 逻辑脚本
        [TabGroup("逻辑脚本生成(建议只生成一次)"),LabelText("生成路径"),FolderPath]
        public string pathForLogic_ = "Assets/Scripts/ESFramework/Core_Domain_Module_Class";

        [TabGroup("逻辑脚本生成(建议只生成一次)"), LabelText("生成逻辑类"), GUIColor("@KeyValueMatchingUtility.ColorSelector.Color_03")]
        public List<_DomainAndList> domains_copycontent = new List<_DomainAndList>();

        [TabGroup("逻辑脚本生成(建议只生成一次)"), Button("生成逻辑脚本", ButtonHeight = 50)]
        public void GenerateTruely()
        {
            GenerateLogic(true);
        }
        [TabGroup("逻辑脚本生成(建议只生成一次)"), Button("刷新已有模块", ButtonHeight = 50)]
        public void RefreshOnly()
        {
            GenerateLogic(false);
        }
        public void GenerateLogic(bool create)
        {
            
            if (!create||KeyValueMatchingUtility.SafeEditor.DisplayDialog("创建逻辑脚本","只推荐创建一次，因为该脚本时允许你自己修改的，强制创建会覆盖以前的内容","我是第一次/覆盖","算了"))
            {
                string CoreName = CoreClassName._ToValidIdentName();
                string CoreChinaName = CoreClassChinaName._ToValidIdentName();
                //创建总文件夹并且获得最新路劲
                KeyValueMatchingUtility.SafeEditor.CreateFolderDic(pathForLogic_, CoreName);
                string coreFolder = Path.Combine(pathForLogic_, CoreName);

                //第一个文件写基础定义
                string corePart = "";
                string domainPart = "";
                string modulePart = "";

                string domainInDefine = "";

                //第二类N个文件写扩展模块
                domains_copycontent = new List<_DomainAndList>();

                foreach (var i in Domains)
                {
                    //开始一个Domain
                    if (i == null) continue;

                    _DomainAndList aDomainContent = new _DomainAndList();
                    domains_copycontent.Add(aDomainContent);
                    string DomainName = i.DomainClassName._ToValidIdentName()._AddPreAndLast(CoreName, "Domain");
                    string DomainChinese = i.DomainChinaClassName._ToValidIdentName()._AddPreAndLast(CoreChinaName, "扩展域");

                   


                    aDomainContent.domainName = DomainName;
                    aDomainContent.domainChineseName = DomainChinese;

                    string DomainModules_SingleContent = $"/*{DomainName} （{DomainChinese}）的*/";

                    //抽象的
                    string absName = CoreName + i.DomainClassName._ToValidIdentName() + "Module";
                    string abs = $"/* 抽象模块{absName}【{DomainChinese._RemoveString("扩展")}扩展模块】," +
                        $"\n核心:{CoreName}【{CoreClassChinaName}】," +
                        $"]n域:{DomainName} 【{DomainChinese}】 */ \n";
                    DomainModules_SingleContent += abs;
                    foreach (var mm in i.Modules)
                    {
                        if (mm == null) continue;
                        string moduleName = mm.ModuleClassName._ToValidIdentName()._AddPreAndLast(absName + "_", "");
                        string moduleChinaName = "扩展模块_" + mm.ModuleChinaClassName;
                        string defineName = "Module_" + mm.ModuleClassName._ToValidIdentName();
                        string inside = $"  protected override void CreateRelationshipOnly()\r\n        {{\r\n            Domain.{defineName} = this;\r\n            base.CreateRelationshipOnly();\r\n        }}";
                        string aModule = KeyValueMatchingUtility.SimpleScriptMaker.CreateClassContentByString(
                       className: moduleName,
                       beforeClassName: "partial",
                       insideClass: " protected override void OnEnable()\r\n        {\r\n            base.OnEnable();\r\n        }\r\n        protected override void Update()\r\n        {\r\n            base.Update();\r\n        }\r\n        protected override void OnDisable()\r\n        {\r\n            base.OnDisable();\r\n        }",
                      parent: ":" + absName,
                       Attribute: $"//{moduleChinaName}");

                        aDomainContent.modules.Add(new _DomainAndList._ModuleClass() { ModuleName=moduleName,ModuleChineseName=moduleChinaName,content=aModule });

                        if (mm.modules != null && mm.modules.Count > 0)
                        {
                            foreach (var mm_mm in mm.modules)
                            {
                                string module_SONChinaName = "扩展模块_" + mm.ModuleChinaClassName + "_" + mm_mm.ModuleChinaClassName;
                                string sonModule = KeyValueMatchingUtility.SimpleScriptMaker.CreateClassContentByString(
                       className: moduleName + "_" + mm_mm.ModuleClassName._ToValidIdentName(),
                       beforeClassName: "partial",
                       insideClass: " protected override void OnEnable()\r\n        {\r\n            base.OnEnable();\r\n        }\r\n        protected override void Update()\r\n        {\r\n            base.Update();\r\n        }\r\n        protected override void OnDisable()\r\n        {\r\n            base.OnDisable();\r\n        }",

                       parent: ":" + moduleName,
                       Attribute: $"//{module_SONChinaName}");

                                aModule += sonModule;
                                aDomainContent.modules.Add(new _DomainAndList._ModuleClass() { ModuleName = moduleName + "_" + mm_mm.ModuleClassName._ToValidIdentName(), 
                                    ModuleChineseName = module_SONChinaName, content = sonModule
                                });

                            }

                        }
                        //加入
                        DomainModules_SingleContent += aModule;
                    }
                    aDomainContent.content= domainPart += KeyValueMatchingUtility.SimpleScriptMaker.CreateClassContentByString(
                        className: DomainName,
                        beforeClassName: "partial",
                        insideClass: " protected override void OnEnable()\r\n        {\r\n            base.OnEnable();\r\n        }\r\n        protected override void Update()\r\n        {\r\n            base.Update();\r\n        }\r\n        protected override void OnDisable()\r\n        {\r\n            base.OnDisable();\r\n        }",
                        parent: "",
                        Attribute: $"//{DomainChinese}"
                        );

                    string field = "Domain_" + i.DomainClassName._ToValidIdentName();
                    domainInDefine += $"// {DomainChinese}：{DomainName},作为核心的{field}";

                    KeyValueMatchingUtility.SimpleScriptMaker.CreateScriptBounds(coreFolder, "ModulesFor_" + DomainName + ".cs",
                        using_: "using ES;\r\nusing Sirenix.OdinInspector;\r\nusing System;\r\nusing System.Collections;\r\nusing System.Collections.Generic;\r\nusing UnityEngine;",
                        content: DomainModules_SingleContent,
                        TruelyCreate: create
                        );
                }

                corePart = KeyValueMatchingUtility.SimpleScriptMaker.CreateClassContentByString(
                        className: CoreName,
                        beforeClassName: "partial",
                        insideClass: domainInDefine,
                        parent: ":" + $"Core",
                        Attribute: $""
                        );


                KeyValueMatchingUtility.SimpleScriptMaker.CreateScriptBounds(coreFolder, CoreName + ".cs",
                    using_: "using ES;\r\nusing Sirenix.OdinInspector;\r\nusing System;\r\nusing System.Collections;\r\nusing System.Collections.Generic;\r\nusing UnityEngine;",
                    content: corePart + domainPart + modulePart
                       , TruelyCreate: create);
            }
        }
        [Serializable]
        public class _DomainAndList
        {
            [LabelText("中文名"),InlineButton("GetModel","获得模版")]
            public string domainChineseName = "";
            [LabelText("类名")]
            public string domainName = "";
            [HideInInspector]
            public string content = "";
            [LabelText("模块"), GUIColor("@KeyValueMatchingUtility.ColorSelector.ColorForESValue")]
            public List<_ModuleClass> modules = new List<_ModuleClass>();

            private void GetModel()
            {
                GUIUtility.systemCopyBuffer = content;
            }
            [Serializable]
            public class _ModuleClass
            {
                [LabelText("中文名"), InlineButton("GetModel", "获得模版")]
                public string ModuleChineseName = "";
                [LabelText("类名")] public string ModuleName = "";
                [HideInInspector]
                public string content = "";
                private void GetModel()
                {
                    GUIUtility.systemCopyBuffer = content;
                }
            }
        }
       
        #endregion

    }
    [Serializable]
    public class DomainSource
    {
        [LabelText("扩展域命名")] public string DomainClassName = "NewDomainName";
        [LabelText("扩展域中文命名")] public string DomainChinaClassName = "新的域";
        [LabelText("全部剪影"), GUIColor("@KeyValueMatchingUtility.ColorSelector.ColorForESValue")]
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
        [LabelText("有继承"), GUIColor("@KeyValueMatchingUtility.ColorSelector.ColorForDes")]
        public List<ModuleSource_Son> modules = new List<ModuleSource_Son>();
    }
    [Serializable]
    public class ModuleSource_Son
    {
        [LabelText("模块名字")]
        public string ModuleClassName = "TestModule";
        [LabelText("模块中文命名"),ListDrawerSettings()] public string ModuleChinaClassName = "新的剪影";
    }
}
