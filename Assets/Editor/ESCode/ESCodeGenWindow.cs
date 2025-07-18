using ES;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static ES.EnumCollect;
using static ES.GlobalDataForEditorOnly;
using static ES.KeyValueMatchingUtility;
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
        public Page_TagsAndLayers pageForTagsAndLayers;
        #endregion

        protected override void ES_BuildMenuTree(OdinMenuTree tree)
        {
            base.ES_BuildMenuTree(tree);

            //开始使用界面
            Part_BuildStartPage(tree);
            {
                Part_BuildTagsAndLayers(tree);
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
            QuickBuildRootMenu(tree, "标签与层级", ref pageForTagsAndLayers, SdfIconType.SunFill);
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


            [TabGroup("概念", "关于数据组"), HideLabel, TextArea(5, 10), DisplayAsString(alignment: TextAlignment.Left)]
            public string aboutDataGroup = "" +
         "数据组，数据组把数个具有同特征的数据单元包含其中，作为独立的资产的最小格式！\n" +
          "组最大的作用是分组，以一个资产包含多个子单元\n" +
         " 通常来说不推荐直接把组用来引用至游戏，而是以后面的包来完成\n，" +
         "其中\n******【1】数据组是一个数据单元的持久字典，并且原则上把子单元都作为子资产包含其中，推荐容纳5-10个为佳\n" +
         " ******【2】数据组一般只有分组和编辑功能，不推荐用于游戏引用，加载和取用，这只是一个建议和规范，可以自己定\n " +
         " ******【3】英文Group,为它的专属名词\n" +
         " ******【#】以一个资产容纳一系列数据单元,高效分类整理，可以绑定到数据包来做到输出最新的内容";

            [TabGroup("概念", "关于数据包"), HideLabel, TextArea(5, 10), DisplayAsString(alignment: TextAlignment.Left)]
            public string aboutDataPack = "" +
           "数据包，同样以持久字典直接引用一些数据单元，跳过了数据组，一般来说一个包可以涵盖一套功能的基本结构！\n" +
            "包并不推荐为数据单元重命名,而是一般简单地从多个数据组缓冲入数据，他主要是为了能快速收集足够有效数据\n" +
           " 他的数据组主要有一个单元更新关系,，在游戏运行时，不推荐从包获得组再进行操作\n，" +
           "其中\n******【1】包只是一类数据单元引用的持久字典\n" +
           " ******【2】包可以选定和数据组建立更新链接，以便防止忘记手动载入\n " +
           " ******【3】英文Pack,为它的专属名词\n" +
           " ******【#】建议广泛使用包来简化游戏逻辑流程";

            /*  [TabGroup("概念", "关于数据总配置"), HideLabel, TextArea(5, 10), DisplayAsString(alignment: TextAlignment.Left)]
              public string aboutDataConfi = "" +
                 "数据总配置，自主收纳SO包， 让从配置到包再到单元的搜索简单而高效！\n" +
                  "在这里为包重定义标识，通过简单的API搜索到包乃至数据单元\n" +
                 "最重要的是，在不同的配置中，同样的标识名对应的包有可能不同\n，" +
                  "只需要更换配置就可以提升测试效率和实现不同章节/关卡的不同表现形式" +
                 "其中\n******【1】总配置只是一类数据包引用的持久字典\n" +
                 " ******【2】每类配置会自动声明到全局的数据管理器，这个数据在运行时仍可使用\n " +
                 " ******【3】英文Configuration,为它的专属名词\n" +
                 " ******【#】如果题材比较简单，可能根本用不到配置层！！！";*/

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
        public class Page_TagsAndLayers : ESWindowPageBase
        {
            [ShowInInspector,InlineProperty,LabelText("记忆")]
            public TagsAndLayers TagAndLayer =>GlobalDataForEditorOnly.Instance.TagAndLayer;
            [LabelText("自动更新")]
            public bool AutoRefresh { get=> TagAndLayer.autoRefresh; set{ if (TagAndLayer.autoRefresh != value) { TagAndLayer.autoRefresh = value; EditorUtility.SetDirty(GlobalDataForEditorOnly.Instance); }  } }
            [LabelText("保存路径"), FolderPath]
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
                        fieldName = KeyValueMatchingUtility.Function.FunctionForStringAsIndentNameCase(fieldName,handleIndentNames);
                        content += SimpleScriptMaker.CreateFieldContent("string", fieldName, modifier: "public static", valueDefine:"="+ i._AsStringValue());
                    }
                    foreach(var (layer,name) in TagAndLayer.memoryLayers)
                    {
                        string fieldName = "Layer_" + name._ToValidIdentName();
                        fieldName = KeyValueMatchingUtility.Function.FunctionForStringAsIndentNameCase(fieldName, handleIndentNames);
                        content += SimpleScriptMaker.CreateFieldContent("int", fieldName, modifier: "public static", valueDefine:"="+layer.ToString());
                        string fieldName2 = "LayerMask_" + name._ToValidIdentName();
                        fieldName2 = KeyValueMatchingUtility.Function.FunctionForStringAsIndentNameCase(fieldName2, handleIndentNames);

                        content += SimpleScriptMaker.CreateFieldContent("int", fieldName2, modifier: "public static", valueDefine:"="+((int)Mathf.Round(Mathf.Pow(2,layer))).ToString());
                    }

                    SimpleScriptMaker.CreateScriptNormal(pathToSave, nameof(GameCenterManager), "partial", content,parent:"",AdditonFileName:"_TagsAndLayers");
                }
                else
                {
                   bool reTry= KeyValueMatchingUtility.SafeEditor.DisplayDialog("不可用的保存路径", "想要保存代码到" + pathToSave + "\n" +
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
                       bool b= KeyValueMatchingUtility.SafeEditor.DisplayDialog("代码重生成", "检测到标签层级声称代码需要更新！！", "开始生成", "算了");
                        if (b)
                        {
                            reGenCode();
                        }
                    }
                }
                return base.ES_Refresh();
            }
        }
    }
}

