using ES;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static ES.EnumCollect;
using static ES.ESCodeGenWindow.PageRoot_CoreDomainModule;
using static ES.ESCodeGenWindow.PageRoot_Enum;
using static ES.GlobalDataForEditorOnly;
using static ES.ESStaticDesignUtility;
namespace ES
{

    public class ESCodeGenWindow :  ESWindowBase_Abstract<ESCodeGenWindow> //OdinMenuEditorWindow
    {
        [MenuItem("Tools/ES工具/ES代码生成窗口")]
        public static void TryOpenWindow()
        {
            OpenWindow();
        }
        protected override void OnImGUI()
        {
            if (usingWindow == null)
            {
                usingWindow = this;
            }
            base.OnImGUI();
        }

        #region 数据滞留
        public PageRoot_StartUseCodeGen pageForStartUseCodeGen;
        public PageRoot_TagsAndLayers pageForTagsAndLayers;
        public PageRoot_CoreDomainModule pageForCoreDomain;
        public PageRoot_Enum pageForEnum;

        public const string PageNameTagAndLayer = "标签与层级";
        public const string PageNameCoreDomain = "核域代码生成";
        public const string PageNameEnum = "枚举生成";
        public const string PageNameLink = "Link代码生成";
        public const string PageNameUI = "UI代码生成";
        #endregion

        protected override void ES_BuildMenuTree(OdinMenuTree tree)
        {
            base.ES_BuildMenuTree(tree);

            //开始使用界面
            Part_BuildStartPage(tree);
            {
                Part_BuildTagsAndLayers(tree);
                Part_BuildCoreDomain(tree);
                Part_BuildEnum(tree);
            }
            /*
            {//独立功能块
                Part_BuildDataScriptCodePage(tree);
                Part_BuildSoDataConfigureSettingPage(tree);
                Part_BuildSoPackPage(tree);
                Part_BuildSoDataGroupPage(tree);
                Part_BuildSoDataGroupOnChooseAndInfos(tree);

            }*/
            ES_LoadData();
        }
        private void Part_BuildStartPage(OdinMenuTree tree)
        {
            QuickBuildRootMenu(tree, "开始使用", ref pageForStartUseCodeGen, SdfIconType.SunFill);
        }
        private void Part_BuildTagsAndLayers(OdinMenuTree tree)
        {
            QuickBuildRootMenu(tree, PageNameTagAndLayer, ref pageForTagsAndLayers, SdfIconType.BookmarkPlusFill);
        }
        private void Part_BuildCoreDomain(OdinMenuTree tree)
        {
            QuickBuildRootMenu(tree, PageNameCoreDomain, ref pageForCoreDomain, SdfIconType.Diagram2);
            var AllSoCoreSource = ESStaticDesignUtility.SafeEditor.FindSOAssets<ESCode_CoreDomainSource>();
            foreach(var i in AllSoCoreSource)
            {
                if (i != null && !i.CoreClassName.IsNullOrWhitespace())
                {
                    tree.Add(PageNameCoreDomain+"/"+i.CoreClassName,new Page_Index_Core_ASource() { source=i });
                }
            }
        }
        private void Part_BuildEnum(OdinMenuTree tree)
        {
            QuickBuildRootMenu(tree, PageNameEnum, ref pageForEnum, SdfIconType.FlagFill);
            var AllSoCoreSource = ESStaticDesignUtility.SafeEditor.FindSOAssets<ESCode_EnumSource>();
            foreach (var i in AllSoCoreSource)
            {
                if (i != null && !i.EnumSortName_.IsNullOrWhitespace())
                {
                    tree.Add(PageNameEnum + "/" + i.EnumSortName_, new Page_Index_Enum_ASource() { source = i }.ES_Refresh());
                }
            }
        }
        [Serializable]
        public class PageRoot_StartUseCodeGen : ESWindowPageBase
        {
            [Title("开始使用ES CodeGen代码生成窗口！！", subtitle: "</Scripts/ESFramework/CodeGen>存放主要的生成代码")]
            [DisplayAsString(alignment: TextAlignment.Center, fontSize: 20), HideLabel]
            public string titleF = "代码生成，通过可视化配置生成特定规格和作用的代码脚本";

            [TabGroup("概念", "简述代码生成"), HideLabel, TextArea(5, 10), DisplayAsString(alignment: TextAlignment.Left)]
            public string aboutThisWindow = "" +
         "生成文件夹应当在ESFramework/CodeGen下，文件夹可直接选择或者拖入string的值区域" +
          "命名严格使用规范的文件名/标识符，进行替换式的脚本生成可能导致难以修复的Bug\n" +
         "代码生成器目前使用纯代码第一代工具，比较垃圾\n，" +
         "其中\n******【1】在发布版本前可以浏览一遍全部生成器是否需要更新\n" +
         " ******【2】AB包辅助文件生成仍旧放在AB包管理器处\n " +
         " ******【3】依赖GlobalDataForEditorOnly配置文件\n" +
         " ******【#】扩展时注意写出Error会导致整个面板不可见(废话)";


            [TabGroup("概念", "关于标签与层级"), HideLabel, TextArea(5, 10), DisplayAsString(alignment: TextAlignment.Left)]
            public string aboutDataInfo = "" +
         "标签与层级辅助代码生成，最终存放在Assets/Scripts/ESFramework/CodeGen/Target/GameCenterManager_TagsAndLayers\n" +
          "完全静态的显式可用字符串(标签)和数字(Layers),帮助简化代码和防止出错\n" +
          "最终通过GameCenterManager.TagXXX/LayerXXX,LayerMaskXXX直接使用 \n，" +
         "其中\n******【1】打开本面板一般会自动检查更新\n" +
         " ******【2】注意Layer就是0-31，而LayerMask就是他的2幂次方\n " +
         " ******【3】暂时和ESTags无关\n" +
         " ******【#】标识名一般会以大写开头，并不允许有空格等，";


            [TabGroup("概念", "关于核域模块"), HideLabel, TextArea(5, 10), DisplayAsString(alignment: TextAlignment.Left)]
            public string aboutDataGroup = "核域代码生成，生成会附带核脚本(不要修改)和记录下来源,以后还可以修改调整" +
         "核是以一个核心脚本生成的大型运行时逻辑单元，比如实体，武器，建筑物！\n" +
          "核本体包含数个Domain扩展域，而每个域可放置自定义组合的扩展模块，按需求选用\n" +
         " 他们使用多态序列化，内存和运行消耗都比较低\n，" +
         "其中\n******【1】核是Mono脚本，为大型逻辑单元，可以写入必要代码\n" +
         " ******【2】域是可序列化类显式声明，可选启用的一类功能的收集，容纳大量扩展模块\n " +
         " ******【3】模块是可序列化类，可选装载在域的模块列表中，可以开始就生成或者中途加入移除\n" +
         " ******【#】这块为框架构建大型逻辑的重要部分，需要查看更多文档和使用，严格规范处可以用本工具一键生成";

            [TabGroup("概念", "关于枚举生成"), HideLabel, TextArea(5, 10), DisplayAsString(alignment: TextAlignment.Left)]
            public string aboutDataPack = "" +
           "这是一个非常简单的枚举生成工具，支持Flag的枚举，支持中文显示和ES注入信息！\n" +
            "一个源可以把多个枚举放在一个文件夹里，节约空间，也可以起到分组的目的\n" +
            "总体来说没有什么特别的，使用也很简单";

            [TabGroup("概念", "关于UI面板生成"), HideLabel, TextArea(5, 10), DisplayAsString(alignment: TextAlignment.Left)]
            public string aboutDataConfi = "" +
               "有很多面板是全局唯一，具有堆栈资格的，这里把他显式声明，\n" +
                "首先先定义确定全部的的ROOT,一般游戏内Root数量不到3个\n" +
                "把面板定义声明后，绑定到的真实面板就会自动放入自己，并且可以随时更新" +
                "即使不用本窗口，也可以创建基于实际Panel的源\n，" +
                "只需要更换配置就可以提升测试效率和实现不同章节/关卡的不同表现形式" +
               "其中\n******【1】总配置只是一类数据包引用的持久字典\n" +
               " ******【2】每类配置会自动声明到全局的数据管理器，这个数据在运行时仍可使用\n " +
               " ******【3】英文Configuration,为它的专属名词\n" +
               " ******【#】如果题材比较简单，可能根本用不到配置层！！！";

            [TabGroup("概念", "关于代码生成工具"), HideLabel, TextArea(5, 10), DisplayAsString(alignment: TextAlignment.Left)]
            public string aboutCodeGen = "" +
                "SO代码生成工具用于自动化创建C# Scriptable 文件\n" +
                "用来简化类似结构代码的声明工作量,\n" +
                "其中\n******【1】So体系生成,提供从So单元，组，包 的一键构建\n" +
                "******【2】So全局配置文件生成,这种配置文件可以被轻易地引用并且提供了自动创建引导和多配置选用\n " +
                "******【3】SharedData 与 VariableData 体系，是So游戏逻辑数据的一个标准，生成的代码主要为了指引深拷贝优化\n" +
                "******【#】这里是关于SO的数据生成，还有更多代码生成属于其他功能模块！";

        }

        [Serializable]
        public class PageRoot_TagsAndLayers : ESWindowPageBase
        {
            [ShowInInspector,InlineProperty,LabelText("记忆")]
            public TagsAndLayers TagAndLayer =>GlobalDataForEditorOnly.Instance.TagAndLayer;
            [LabelText("自动更新")]
            public bool AutoRefresh { get=> TagAndLayer.autoRefresh; set{ if (TagAndLayer.autoRefresh != value) { TagAndLayer.autoRefresh = value; EditorUtility.SetDirty(GlobalDataForEditorOnly.Instance); }  } }
            [LabelText("保存路径"), FolderPath]
            [Title("标签与层级辅助代码生成", "完全静态的显式可用字符串(标签)和数字(Layers),帮助简化代码和防止出错", TitleAlignments.Centered), PropertyOrder(-1)]

            public string pathToSave = DefaultSavePath;
            [LabelText("处理标识符方式"),InlineButton("reGenCode","手动更新")]
            public HandleIndentStringName handleIndentNames = HandleIndentStringName.StartToUpper;
            public const string DefaultSavePath = "Assets/Scripts/ESFramework/CodeGen/Target/GameCenterManager_TagsAndLayers";
            private void reGenCode()
            {
                if (AssetDatabase.IsValidFolder(pathToSave))
                {
                    string content = SimpleScriptMaker.CreateNotes("为标签和层级自动生成的辅助代码");
                    foreach(var i in TagAndLayer.memoryTags)
                    {
                        string fieldName = "Tag_"+ i._ToValidIdentName();
                        fieldName = ESStaticDesignUtility.Function.FunctionForStringAsIndentNameCase(fieldName,handleIndentNames);
                        content += SimpleScriptMaker.CreateFieldContent("string", fieldName, modifier: "public static", valueDefine:"="+ i._AsStringValue());
                    }
                    foreach(var (layer,name) in TagAndLayer.memoryLayers)
                    {
                        string fieldName = "Layer_" + name._ToValidIdentName();
                        fieldName = ESStaticDesignUtility.Function.FunctionForStringAsIndentNameCase(fieldName, handleIndentNames);
                        content += SimpleScriptMaker.CreateFieldContent("int", fieldName, modifier: "public static", valueDefine:"="+layer.ToString());
                        string fieldName2 = "LayerMask_" + name._ToValidIdentName();
                        fieldName2 = ESStaticDesignUtility.Function.FunctionForStringAsIndentNameCase(fieldName2, handleIndentNames);

                        content += SimpleScriptMaker.CreateFieldContent("int", fieldName2, modifier: "public static", valueDefine:"="+((int)Mathf.Round(Mathf.Pow(2,layer))).ToString());
                    }

                    SimpleScriptMaker.CreateScriptNormal(pathToSave, nameof(GameCenterManager), "partial", content,parent:"",AdditonFileName:"_TagsAndLayers");
                }
                else
                {
                   bool reTry= ESStaticDesignUtility.SafeEditor.DisplayDialog("不可用的保存路径", "想要保存代码到" + pathToSave + "\n" +
                        $"，这是不可用的路径，是否替换为推荐的\n{DefaultSavePath}" +
                        "并再次生成");
                    if (reTry)
                    {
                        pathToSave = DefaultSavePath;
                        reGenCode();
                    }
                }
            }
            public override ESWindowPageBase ES_Refresh()
            {
                if (AutoRefresh)
                {
                    if (TagAndLayer.GetDirty())
                    {
                       bool b= ESStaticDesignUtility.SafeEditor.DisplayDialog("代码重生成", "检测到标签层级声称代码需要更新！！", "开始生成", "算了");
                        if (b)
                        {
                            reGenCode();
                        }
                    }
                }
                return base.ES_Refresh();
            }
        }

        [Serializable]
        public class PageRoot_CoreDomainModule : ESWindowPageBase
        {
            [Title("核域代码生成", "，生成会附带核脚本(不要修改)和记录下来源,以后还可以修改调整", TitleAlignments.Centered), PropertyOrder(-1)]
            [LabelText("生成脚本路径"), FolderPath] public string genePathFolder = "Assets/Scripts/ESFramework/CodeGen/Target/Core_Domain_Module";
            [LabelText("生成源路径"), FolderPath,ReadOnly] public string geneSourcePathFolder = "Assets/Resources/Data/SingleData/ESCoreSource";

            [LabelText("核心命名"),InlineButton("Gene","生成源"), GUIColor("@ESStaticDesignUtility.ColorSelector.ColorForCaster")] 
            public string CoreClassName = "NewCore";
            [LabelText("核心中文命名"),GUIColor("@ESStaticDesignUtility.ColorSelector.ColorForCaster")]
            public string CoreClassChinaName = "新核心";
            [LabelText("全部扩展域"), GUIColor("@ESStaticDesignUtility.ColorSelector.Color_03")] public List<DomainSource> Domains = new List<DomainSource>() { new DomainSource() { DomainClassName = "Normal" } };

 
            private void Gene()
            {
                ESCode_CoreDomainSource source = ScriptableObject.CreateInstance<ESCode_CoreDomainSource>();
                string ss = CoreClassName._ToValidIdentName();
                source.CoreClassName = ss;
                source.CoreClassChinaName = CoreClassChinaName._ToValidIdentName();
                source.Domains = new List<DomainSource>();
                source.name = ss + "_CoreSource";

                source.path = genePathFolder;
                foreach (var i in Domains)
                {
                    source.Domains.Add(i);
                }
                EditorUtility.SetDirty(source);
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
                AssetDatabase.CreateAsset(source,Path.Combine(geneSourcePathFolder,source.name+".asset"));
                EditorGUIUtility.PingObject(source);
               /* source.Generate();*/
            }
            [Serializable]
            public class Page_Index_Core_ASource : ESWindowPageBase
            {
                [InlineEditor(inlineEditorMode:InlineEditorModes.GUIOnly,Expanded =true)]
                public ESCode_CoreDomainSource source;

            }
        }
        [Serializable]
        public class PageRoot_Enum : ESWindowPageBase
        {
            [Title("枚举代码生成", "一个枚举源可以容纳多份，最终放到一个文件里，这样有利于压缩文件数", TitleAlignments.Centered), PropertyOrder(-1)]
            [LabelText("生成脚本路径"), FolderPath] public string genePathFolder = "Assets/Scripts/ESFramework/CodeGen/Enum";
            [LabelText("生成源路径"), FolderPath, ReadOnly] public string geneSourcePathFolder = "Assets/Resources/Data/SingleData/ESEnumSource";

            [LabelText("枚举分类命名"), InlineButton("Gene", "生成源"), GUIColor("@ESStaticDesignUtility.ColorSelector.ColorForCaster")]
            public string EnumSortName = "EnumSort";
            [LabelText("全部枚举"), GUIColor("@ESStaticDesignUtility.ColorSelector.Color_03"),] public List<ESCode_EnumSource.EnumType> EnumTypes = new List<ESCode_EnumSource.EnumType>() {  };


            private void Gene()
            {
                ESCode_EnumSource source = ScriptableObject.CreateInstance<ESCode_EnumSource>();
                string ss = EnumSortName._ToValidIdentName();
                source.EnumSortName_ = ss;
                source.EnumTypes= new List<ESCode_EnumSource.EnumType>();
                source.name ="Sort_"+ ss + "_EnumSource";

                source.path_   = genePathFolder;
                foreach (var i in EnumTypes)
                {
                    i.EDIT = false;
                    source.EnumTypes.Add(i);
                }
                EnumTypes.Clear();
                EnumSortName = "New";
                EditorUtility.SetDirty(source);
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
                AssetDatabase.CreateAsset(source, Path.Combine(geneSourcePathFolder, source.name + ".asset"));
                EditorGUIUtility.PingObject(source);
                /* source.Generate();*/
            }
            [Serializable]
            public class Page_Index_Enum_ASource : ESWindowPageBase
            {
                [InlineEditor(inlineEditorMode: InlineEditorModes.GUIOnly, Expanded = true)]
                public ESCode_EnumSource source;
                public override ESWindowPageBase ES_Refresh()
                {
                    source?.load();
                    return base.ES_Refresh();
                }
            }
        }
    }
}

