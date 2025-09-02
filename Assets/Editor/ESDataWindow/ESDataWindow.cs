using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using ES;
using Sirenix.Serialization;
using ES.Pointer;
using System.IO;
using static ES.ESDataWindowTool;
using UnityEngine.UIElements;
using System.CodeDom.Compiler;
using System.Linq;
using YooAsset.Editor;
using UnityEditor.PackageManager.UI;
using UnityEditor.Sprites;
using DG.Tweening.Plugins.Core.PathCore;
using System.Security.Cryptography;

namespace ES
{
    //窗口总览
    public class ESDataWindow : ESWindowBase_Abstract<ESDataWindow> //OdinMenuEditorWindow
    {
        #region 简单重写
        public override GUIContent ESWindow_GetWindowGUIContent()
        {
            Texture2D texture = Resources.Load<Texture2D>("Sprites/iv2");
            var content = new GUIContent("依薇尔数据窗口", texture, "使用依薇尔数据工具完成快速各种数据配置与开发");
            return content;
        }
        public override void ESWindow_OpenHandle()
        {
            base.ESWindow_OpenHandle();
            if (usingWindow.HasDelegate)
            {
                //已经注册委托
            }
            else
            {
                usingWindow.DelegateHandle();
            }
        }
        private void DelegateHandle()
        {
            HasDelegate = true;
        }
        #endregion

        [MenuItem("Tools/ES工具/ES数据窗口")]
        public static void TryOpenWindow()
        {
            OpenWindow();
        }

        #region 数据滞留与声明

        //根页面名

        public const string PageName_DataMake = "数据脚本生成工具";

        public const string PageName_DataConfiguration = "数据全局配置创建和筛选";

        public const string PageName_DataPackCreate = "数据包创建与筛选";

        public const string PageName_DataGroupCreate = "数据组创建与筛选";

        public const string PageName_DataGroupOnChooseEditInfo = "选中组和编辑";


        [NonSerialized] public Page_Root_Data_StartUse pageForStartUsePage;
        [NonSerialized] public PageRoot_DataScpirtCodeTool pageRootForCodeGen;
        [NonSerialized] public Page_CreateNewSoPackOrSearch pageForSodataPack;
        [NonSerialized] public Page_CreateNewGroupOrSearch pageForSodataGroup;
        [NonSerialized] public Page_DataGroupOnChoose pageForGroupOnChoose;


        /*        [NonSerialized] public Page_CreateNewDataConfiguration pageForConfiguration;*/
        public string selectPackType_ = "Buff数据";
        public string selectGroupType_ = "Buff数据";
        private bool HasDelegate = false;

        public static string guidForCopyGroup = "";
        public static string CopyInfoKey = "";
        public static ScriptableObject CopyGroup;
        #endregion
        protected override void OnImGUI()
        {
            if (usingWindow == null)
            {
                usingWindow = this;
            }
            if (usingWindow != null)
            {
                if (pageForSodataGroup != null)
                {

                }
            }
            base.OnImGUI();
        }
        /// <summary>
        /// 刷新窗口
        /// </summary>
        public override void ESWindow_RefreshWindow()
        {
            base.ESWindow_RefreshWindow();
            ES_SaveData();
        }


        public string pathForDefaultSetting;
        public override void ES_LoadData()
        {
            Debug.Log("加载DataWindow");
            if (EditorPrefs.HasKey("guidForCopyGroup"))
            {
                guidForCopyGroup = EditorPrefs.GetString("guidForCopyGroup");
                CopyGroup = ESStaticDesignUtility.SafeEditor.LoadAssetByGUIDString<ScriptableObject>(guidForCopyGroup) ?? CopyGroup;
            }
            if (EditorPrefs.HasKey("CopyInfoKey"))
            {
                CopyInfoKey = EditorPrefs.GetString("CopyInfoKey");
            }
        }
        public override void ES_SaveData()
        {
            Debug.Log("保存DataWindow");
            if (CopyGroup != null)
            {
                guidForCopyGroup = ESStaticDesignUtility.SafeEditor.GetAssetGUID(CopyGroup) ?? guidForCopyGroup;
                if (guidForCopyGroup != null)
                {
                    EditorPrefs.SetString("guidForCopyGroup", guidForCopyGroup);
                    EditorPrefs.SetString("CopyInfoKey", CopyInfoKey);
                }
                
            }
        }
        protected override void ES_BuildMenuTree(OdinMenuTree tree)
        {
            base.ES_BuildMenuTree(tree);

            //开始使用界面
            Part_BuildStartPage(tree);

            {//独立功能块
                Part_BuildDataScriptCodePage(tree);
                Part_BuildSoDataConfigureSettingPage(tree);
                Part_BuildSoPackPage(tree);
                Part_BuildSoDataGroupPage(tree);
                Part_BuildSoDataGroupOnChooseAndInfos(tree);

            }
            ES_LoadData();
        }

        #region 开始使用
        private void Part_BuildStartPage(OdinMenuTree tree)
        {
            QuickBuildRootMenu(tree, "开始使用", ref pageForStartUsePage, SdfIconType.SunFill);
        }

        #endregion
        private void Part_BuildDataScriptCodePage(OdinMenuTree tree)
        {
            QuickBuildRootMenu(tree, PageName_DataMake, ref pageRootForCodeGen, SdfIconType.Braces);
        }

        private void Part_BuildSoDataConfigureSettingPage(OdinMenuTree tree)
        { }
        private void Part_BuildSoPackPage(OdinMenuTree tree)
        {

            QuickBuildRootMenu(tree, PageName_DataPackCreate, ref pageForSodataPack, SdfIconType.BoxSeam);
            var TypeSelect = ESDataWindowTool.GetPackType(pageForSodataPack.createPackType_);
            var allPacks = ESStaticDesignUtility.SafeEditor.FindSOAssets<ISoDataPack>(TypeSelect);
            foreach (var i in allPacks)
            {

                if (i is ScriptableObject so)
                {
                    tree.Add(PageName_DataPackCreate + $"/命名：{i._name} 文件:{so.name}", new Page_Index_EveryDataPack() { pack = i }.ES_Refresh(), SdfIconType.Box);
                }
            }
        }
        private void Part_BuildSoDataGroupPage(OdinMenuTree tree)
        {
            QuickBuildRootMenu(tree, PageName_DataGroupCreate, ref pageForSodataGroup, SdfIconType.BoxSeam);
            var TypeSelect = ESDataWindowTool.GetGroupType(pageForSodataGroup.createGroup_);
            var allGroups = ESStaticDesignUtility.SafeEditor.FindSOAssets<ISoDataGroup>(TypeSelect);

            foreach (var i in allGroups)
            {
                if (i is ScriptableObject so)
                {
                    tree.Add(PageName_DataGroupCreate + $"/命名：{i._name} 文件:{so.name}", new Page_Index_DataQuickSeeGroup() { group = so }, SdfIconType.Bag);
                }
            }

        }
        public void Part_BuildSoDataGroupOnChooseAndInfos(OdinMenuTree tree)
        {
            QuickBuildRootMenu(tree, PageName_DataGroupOnChooseEditInfo, ref pageForGroupOnChoose, SdfIconType.BagCheckFill);
            Debug.Log(1);
            if (Selection.activeObject is ISoDataGroup group_)
            {
                Debug.Log(2);
                pageForGroupOnChoose.group = group_;
            }
            if (pageForGroupOnChoose.group != null)
            {
                Debug.Log(3);
                foreach (var i in pageForGroupOnChoose.group.AllKeys)
                {
                    ISoDataInfo so = pageForGroupOnChoose.group.GetInfoByKey(i);
                    if (so != default)
                    {
                        Debug.Log(pageForGroupOnChoose.group + "  " + so);
                        tree.Add($"{Items[PageName_DataGroupOnChooseEditInfo].Name}/{i} 单元", new Page_Index_DataInfoSingle() { data = so }, SdfIconType.Book); ;
                    }
                }
                /* pageForGroupOnChoose.Check();*/
            }
            else
            {

            }



            return;
        }

        private void Part_BuildArchutectureShowerPage(OdinMenuTree tree)
        {

        }
        private void Part_EasyToolsPage(OdinMenuTree tree)
        {

        }
        private void Part_AboutPage(OdinMenuTree tree)
        {
            tree.Add("关于", new Page_About(), SdfIconType.ChatSquareQuoteFill);
        }
    }
    #region 开始
    //开始使用界面
    [Serializable]
    public class Page_Root_Data_StartUse : ESWindowPageBase
    {
        [Title("开始使用ES SO数据管理窗口！！", subtitle: "为了快速入门，我们从最简单的概念开始排列介绍和布局")]
        [DisplayAsString(alignment: TextAlignment.Center, fontSize: 20), HideLabel]
        public string titleF = "SO数据管理窗口，为SO定义，创建和修改，创建全局寻址方式，提供便捷开发工具";

        [TabGroup("操作名词", "窗口概述"), HideLabel, TextArea(5, 10), DisplayAsString(alignment: TextAlignment.Left)]
        public string aboutThisWindow = "" +
     "该窗口主要围绕So数据的游戏核心体系展开" +
      "广泛支持实体怪物，道具,技能，状态,Buff,亦可以简单自定义\n" +
     "通常提供了各种数据的创建和搜索，并且可以直接在窗口编辑\n，" +
     "其中\n******【1】So数据与多态序列化深度绑定，用尽量少的内存占用实现数据保存和功能\n" +
     " ******【2】由于多态问题，时常需要类型转换，建议使用Refer功能或者自己写首次加载\n " +
     " ******【3】更多的独立非体系So分布在Data文件夹下，这种也是可以生成和收集的\n" +
     " ******【#】GlobalData很特殊，不要滥用，建议主要使用在编辑器下即可";

        [TabGroup("操作名词", "基础操作"), HideLabel, TextArea(5, 10), DisplayAsString(alignment: TextAlignment.Left)]
        public string aboutDetail = "" +
     "" +
      "每种SO数据会分配一个中文名，在查询/创建页可以进行筛选，筛选的数据会出现在子菜单供进一步操作\n" +
     "，筛选的数据会出现在子菜单供进一步操作，并且可以替代检查器自由编辑\n，" +
     "其中\n******【1】创建新的类型建议只在窗口配置和创建\n" +
     " ******【2】窗口提供快速定位到项目功能，自己也要做好文件夹分组\n " +
     " ******【3】删除元素建议只在窗口进行，否则自己去进行手动刷新之类的\n" +
     "";
        [TabGroup("概念", "关于数据单元"), HideLabel, TextArea(5, 10), DisplayAsString(alignment: TextAlignment.Left)]
        public string aboutDataInfo = "" +
     "数据单元，存放一个类型的单个数据，他是作为子资产进入(数据组)的！\n" +
      "数据单元存放单个的独立数据，比如一个怪物，一个飞行物，一个技能\n" +
     "在没有资源更新需求下，可以考虑直接引用数据单元，他直接作为文件用代码加载步骤略多\n，" +
     "其中\n******【1】数据单元是一个数据组的子资产，因为共性被整合，每个有独立的完整数据\n" +
     " ******【2】数据单元根据作用的对象编写大量数据内容\n " +
     " ******【3】英文Info,为它的专属名\n" +
     " ******【#】数据单元还可以实现共享与变量体系，";


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

    #endregion

    #region 数据工具
    //创建 数据工具 总页面
    public class PageRoot_DataScpirtCodeTool : ESWindowPageBase
    {
        [DisplayAsString(fontSize: 30), HideLabel]
        public string readMe = "数据层级分为DataInfo(单元),DataGroup(组),DataPack(包),现在开始填表来创建新的数据类型";
        [LabelText("英文数据代码名(省略Info)")]
        public string EnglishCodeName = "DataName";
        [LabelText("中文数据显示名(省略数据单元)")]
        public string ChineseDisplayName = "数据名";
        [LabelText("数据父文件夹"), FolderPath]
        public string folder = "Assets/Scripts/ESFramework/Data/DataToolScript";
        [Button("生成")]
        public void GenerateData()
        {
            if (AssetDatabase.IsValidFolder(folder))
            {
                string toInfo = folder + "/InfoType";
                string toGroup = folder + "/GroupType";
                string toPack = folder + "/PackType";
                //查/创建文件夹↓
                {
                    if (!AssetDatabase.IsValidFolder(toInfo))
                    {
                        AssetDatabase.CreateFolder(folder, "InfoType");
                    }
                    if (!AssetDatabase.IsValidFolder(toGroup))
                    {
                        AssetDatabase.CreateFolder(folder, "GroupType");
                    }
                    if (!AssetDatabase.IsValidFolder(toPack))
                    {
                        AssetDatabase.CreateFolder(folder, "PackType");
                    }
                }
                Debug.Log("完毕");
                string infoName = EnglishCodeName + "DataInfo";
                /*
                  ESTool_ScriptMaker.Instance.CreateScript(文件夹路径,Class名,继承/实现,特性,命名空间=ES);
                 */
                ESStaticDesignUtility.SimpleScriptMaker.CreateScriptEasy(toInfo, infoName, Attribute: $"[ESDisplayNameKeyToType(\"数据单元\", \"{ChineseDisplayName}数据单元\")]", parent: ": SoDataInfo");
                ESStaticDesignUtility.SimpleScriptMaker.CreateScriptEasy(toGroup, EnglishCodeName + "DataGroup", Attribute: $"[ESDisplayNameKeyToType(\"数据组\", \"{ChineseDisplayName}数据组\")]", parent: $": SoDataGroup<{infoName}>");
                ESStaticDesignUtility.SimpleScriptMaker.CreateScriptEasy(toPack, EnglishCodeName + "DataPack", Attribute: $"[ESDisplayNameKeyToType(\"数据包\", \"{ChineseDisplayName}数据包\")]", parent: $": SoDataPack<{infoName}>");
                AssetDatabase.Refresh();
            }
            else
            {
                EditorUtility.DisplayDialog("请选择正确的文件夹", "建议使用【Assets/Scripts/ESFramework/Data/DataToolScript】作为生成总路径哦", "知道了");
            }
        }
    }
    //创建 数据配置 总页面
    /* public class Page_CreateNewDataConfiguration : ESWindowPageBase
     {
         [TabGroup("总", "设置全局默认配置")]
         [LabelText("默认全局配置"), AssetSelector, OnValueChanged("EditDefaultConfiguration"), GUIColor("@KeyValueMatchingUtility.ColorSelector.Color_03")]
         public SoDataInfoConfiguration configuration;
         [InlineEditor(Expanded = true), ShowInInspector, LabelText("展示内容")]
         [TabGroup("总", "设置全局默认配置")]
         public SoDataInfoConfiguration show => configuration;
         private SoDataInfoConfiguration cache;
         private void EditDefaultConfiguration()
         {
             //

             //不为空
             if (configuration != null)
             {

                 if (EditorUtility.DisplayDialog("更改全局配置", $"您正在更换新的配置{configuration.name}，这将会应用到游戏管理器影响整个游戏的运行，是否确定", "更换配置", "取消"))
                 {
                     cache = configuration;
                 }
                 else
                 {
                     configuration = cache;
                 }
             }
             //为空
             else
             {
                 if (EditorUtility.DisplayDialog("全局配置清空", $"您正在尝试置空配置{cache?.name ?? "无"}，建议您更换而不是直接取消否则后果可能很严重，是否确定", "置空配置", "取消"))
                 {

                 }
                 else
                 {
                     configuration = cache;
                 }
             }
             ESDataWindow.usingWindow.ES_SaveData();
             var all = UnityEngine.Object.FindObjectsByType<GameCenterManager>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
             foreach (var i in all)
             {

             }

         }
         [ShowIfGroup("总/新建配置/安全", VisibleIf = "AllIsOk"), HideLabel, DisplayAsString]
         public string readme2 = "";
         [InfoBox("全部完备！可以安全创建数据配置!!", infoMessageType: InfoMessageType.Info)]
         [ShowIfGroup("总/新建配置/安全")]
         [Button("安全创建", ButtonHeight = 50, Icon = SdfIconType.Check2Circle), GUIColor("@KeyValueMatchingUtility.ColorSelector.Color_03")]
         private void CreateConfiguration_OK()
         {
             Type targetType = typeof(SoDataInfoConfiguration);
             if (AssetDatabase.IsValidFolder(folder) && targetType != default)
             {
                 SoDataInfoConfiguration @object = ScriptableObject.CreateInstance(targetType) as SoDataInfoConfiguration;

                 @object.name = fileName + (hasChange ? "" : UnityEngine.Random.Range(0, 99999));
                 @object.PackForBuff = PackForBuff;
                 @object.PackForSkill = PackForSkill;
                 @object.PackForItem = PackForItem;
                 @object.PackForActor = PackForActor;
                 @object.PackForEventMessage = PackForEventMessage;
                 @object.PackForRune = PackForRune;
                 @object.PackForRoomGenerate = PackForRoomGenerate;
                 AssetDatabase.CreateAsset(@object, folder + "/" + @object.name + ".asset");
                 AssetDatabase.SaveAssets();
                 AssetDatabase.Refresh();
                 Selection.activeObject = @object;
                 ESDataWindow.usingWindow.ES_SaveData();
             }
             else
             {
                 Debug.LogError("非法文件夹路径或者类型错误！！");
             }

         }
         [InfoBox("请尽量装填完整的数据包再创建!!", infoMessageType: InfoMessageType.Warning)]

         [ShowIfGroup("总/新建配置/强制", VisibleIf = "HasNULL")]
         [LabelText("尝试自动装填？")]
         public bool enabledAutoSeacrch = true;
         [ShowIfGroup("总/新建配置/强制")]
         [Button("强制创建", ButtonHeight = 30, Icon = SdfIconType.PatchExclamationFill), GUIColor("@KeyValueMatchingUtility.ColorSelector.Color_04")]
         private void CreateConfiguration_Force()
         {
             if (enabledAutoSeacrch) AutoIn();
             //创建
             CreateConfiguration_OK();
         }
         [ShowIfGroup("总/新建配置/强制")]
         [Button("自动装填", ButtonHeight = 30, Icon = SdfIconType.BoxArrowInUpLeft), GUIColor("@KeyValueMatchingUtility.ColorSelector.Color_02")]
         private void AutoIn()
         {
             PackForBuff ??= ESWindowDataAndTool.SearchPack(typeof(BuffSoInfo)) as BuffDataPack;
             PackForSkill ??= ESWindowDataAndTool.SearchPack(typeof(SkillDataInfo)) as SkillDataPack;
             PackForActor ??= ESWindowDataAndTool.SearchPack(typeof(ActorDataInfo)) as ActorDataPack;
             PackForItem ??= ESWindowDataAndTool.SearchPack(typeof(ItemDataInfo)) as ItemDataPack;
             PackForEventMessage ??= ESWindowDataAndTool.SearchPack(typeof(GameEventMessageDataPack)) as GameEventMessageDataPack;
             PackForRune ??= ESWindowDataAndTool.SearchPack(typeof(RuneDataPack)) as RuneDataPack;
             PackForRoomGenerate ??= ESWindowDataAndTool.SearchPack(typeof(RoomGenerateDataPack)) as RoomGenerateDataPack;
         }

         private bool HasNULL()
         {
             return ESWindowDataAndTool.HasNull(PackForBuff, PackForActor, PackForSkill, PackForItem);
         }
         private bool AllIsOk()
         {
             return ESWindowDataAndTool.AllIsOk(PackForBuff, PackForActor, PackForSkill, PackForItem);
         }
         [TabGroup("总", "新建配置"), HideLabel, Indent(5)]
         [DisplayAsString(fontSize: 30), GUIColor("@KeyValueMatchingUtility.ColorSelector.Color_01")]
         public string readme = "↓修改配置文件名";



         private bool hasChange = false;
         private void ChangeHappen()
         {
             hasChange = true;
         }
         [InfoBox("建议修改一下文件名", infoMessageType: InfoMessageType.Warning, VisibleIf = "@!hasChange")]
         [TabGroup("总", "新建配置"), Space(5), OnValueChanged("ChangeHappen"), LabelText("设置文件名")]
         public string fileName = "数据配置文件";
         [FolderPath, LabelText("选择文件夹")]
         [TabGroup("总", "新建配置"), Space(5)]
         public string folder = "Assets/Scripts/ESFramework/Data";

         [TabGroup("总", "新建配置"), HideLabel, Indent(5)]
         [DisplayAsString(fontSize: 30), GUIColor("@KeyValueMatchingUtility.ColorSelector.Color_01")]
         public string con = "↓手动配置各种数据包";
         [LabelText("Buff效果数据包"), TabGroup("总", "新建配置"), InfoBox("装填Buff效果包", InfoMessageType.Warning, Icon = SdfIconType.Box), AssetSelector, GUIColor("@KeyValueMatchingUtility.ColorSelector.Color_03")]
         public BuffDataPack PackForBuff;
         [LabelText("Skill技能数据包"), TabGroup("总", "新建配置"), InfoBox("装填Skill技能包", InfoMessageType.Warning, Icon = SdfIconType.Box), AssetSelector, GUIColor("@KeyValueMatchingUtility.ColorSelector.Color_03")]
         public SkillDataPack PackForSkill;
         [LabelText("Actor人物数据包"), TabGroup("总", "新建配置"), InfoBox("装填人物数据包", InfoMessageType.Warning, Icon = SdfIconType.Box), AssetSelector, GUIColor("@KeyValueMatchingUtility.ColorSelector.Color_03")]
         public ActorDataPack PackForActor;
         [LabelText("Item物品数据包"), TabGroup("总", "新建配置"), InfoBox("装填物品数据包", InfoMessageType.Warning, Icon = SdfIconType.Box), AssetSelector, GUIColor("@KeyValueMatchingUtility.ColorSelector.Color_03")]
         public ItemDataPack PackForItem;
         [LabelText("EventMessage事件数据数据包"), TabGroup("总", "新建配置"), InfoBox("装填事件数据包", InfoMessageType.Warning, Icon = SdfIconType.Box), AssetSelector, GUIColor("@KeyValueMatchingUtility.ColorSelector.Color_03")]
         public GameEventMessageDataPack PackForEventMessage;
         [LabelText("Rune符文数据包"), TabGroup("总", "新建配置"), InfoBox("装填符文数据包", InfoMessageType.Warning, Icon = SdfIconType.Box), AssetSelector, GUIColor("@KeyValueMatchingUtility.ColorSelector.Color_03")]
         public RuneDataPack PackForRune;
         [LabelText("RoomGenerate房间生成数据包"), TabGroup("总", "新建配置"), InfoBox("装填房间生成数据包", InfoMessageType.Warning, Icon = SdfIconType.Box), AssetSelector, GUIColor("@KeyValueMatchingUtility.ColorSelector.Color_03")]
         public RoomGenerateDataPack PackForRoomGenerate;

         public override void ES_Setup()
         {
             base.ES_Setup();
             if (configuration != null) cache = configuration;
         }
     }*/
    //单个配置快速查看和配置
    /* public class Page_Index_QuickSeeDataConfuration : ESWindowPageBase
     {
         [HorizontalGroup("总组")]
         [VerticalGroup("总组/数据")]
         [InlineEditor(Expanded = true), LabelText("配置文件")]
         public SoDataInfoConfiguration configuration;
         [HorizontalGroup("总组", width: 100)]

         [VerticalGroup("总组/按钮")]
         [Button("设为默认数据配置"), GUIColor("@ KeyValueMatchingUtility.ColorSelector.Color_03")]
         public void SetToBeDefaultConfigure()
         {

         }
     }*/
    //创建数据包总页面
    [Serializable]
    public class Page_CreateNewSoPackOrSearch : ESWindowPageBase
    {
        [Title("开始新建数据包！", "可以先选择预设类型", bold: true, titleAlignment: TitleAlignments.Centered)]

        [HorizontalGroup("总组")]
        [DisplayAsString(fontSize: 30), HideLabel, GUIColor("@ESStaticDesignUtility.ColorSelector.Color_01")]
        [VerticalGroup("总组/数据")]
        public string createText = "创建新数据包";
        [LabelText("搜集的数据组"), SerializeReference, GUIColor("@ESStaticDesignUtility.ColorSelector.Color_03")]
        [VerticalGroup("总组/数据")]
        public List<ISoDataGroup> CachingAddGroups = new List<ISoDataGroup>();


        [DetailedInfoBox("创建一个数据包包含大量数据！", "创建一个数据包！！将会支持Buff,技能,人物,物品等", infoMessageType: InfoMessageType.Warning)]
        [InfoBox("请修改一下文件名否则会分配随机数字后缀", VisibleIf = "@!hasChange", InfoMessageType = InfoMessageType.Warning)]
        [VerticalGroup("总组/数据"), LabelText("文件命名"), Space(5), GUIColor("@ESStaticDesignUtility.ColorSelector.Color_04"), OnValueChanged("ChangeHappen")]
        public string createName_ = "Buff新建数据包";
        private bool hasChange = false;
        private void ChangeHappen()
        {
            hasChange = true;
        }
        [FolderPath, OnValueChanged("Refresh")]
        [VerticalGroup("总组/数据"), LabelText("保存文件夹"), Space(5), GUIColor("@ESStaticDesignUtility.ColorSelector.Color_04")]
        public string FolderPath_ = "Assets/Resources/Data/PackData";
        private void Refresh()
        {
            AssetDatabase.Refresh();
        }

        [DisplayAsString(fontSize: 30), HideLabel, GUIColor("@ESStaticDesignUtility.ColorSelector.Color_01")]
        [VerticalGroup("总组/数据")]
        public string createTypeText = "筛选数据包类型";


        [OnValueChanged("ResetConfigure")]
        [VerticalGroup("总组/数据"), Space(5), LabelText("预定义类型")]
        [ValueDropdown("@ESDataWindowTool.GetPackNames()", AppendNextDrawer = false)]
        public string createPackType_ = "Buff数据包";



        #region 按钮
        [HorizontalGroup("总组", width: 100)]
        [VerticalGroup("总组/按钮")]
        [PropertySpace(15)]
        [Button(ButtonHeight = 20, Name = "创建数据包", Icon = SdfIconType.Check2Square), GUIColor("@ ESStaticDesignUtility.ColorSelector.Color_03")]
        public void CreateSoPackAsset()
        {
            Type targetType = ESDataWindowTool.GetPackType(createPackType_);
            var create = ESStaticDesignUtility.SafeEditor.CreateSOAsset(targetType, FolderPath_, createName_, true, hasChange, beforeSave);
            void beforeSave(ScriptableObject so)
            {
                if (so is ISoDataPack pack)
                {
                    if (CachingAddGroups != null)
                    {
                        foreach (var i in CachingAddGroups)
                        {
                            if (i != null)
                                pack.AddInfosByGroup(i);
                        }
                    }
                    Selection.activeObject = so;
                }
                else
                {
                    Debug.LogError("非法文件夹路径或者类型错误！！");
                }
            }

        }


        [VerticalGroup("总组/按钮")]
        [PropertySpace(15)]
        [Button(ButtonHeight = 20, Name = "搜集可用数据组"), GUIColor("@ ESStaticDesignUtility.ColorSelector.Color_04")]
        public void FindAInfoGroupAsset()
        {
            //找到全部数据组
            CachingAddGroups = ESStaticDesignUtility.SafeEditor.FindSOAssets<ISoDataGroup>(ESDataWindowTool.GetGroupType(createPackType_));
        }
        private void ResetConfigure()
        {
            hasChange = false;
            ESDataWindow.usingWindow.selectPackType_ = createPackType_;
            ESDataWindow.usingWindow.ESWindow_RefreshWindow();
            createName_ = "新建" + createPackType_;
        }

        public override bool ES_ShouldRemake()
        {
            return base.ES_ShouldRemake();
        }
        private bool TypeSelectorSettingForDataPack(Type type)
        {
            return !type.IsAbstract && !type.IsInterface && typeof(ISoDataPack).IsAssignableFrom(type);
        }
        #endregion
    }
    //子层 搜集的数据包页面
    [Serializable]
    public class Page_Index_EveryDataPack : ESWindowPageBase
    {
        [HorizontalGroup("总组")]

        [Title("开始配置数据包！!", "数据包可以把一系列数据组整合起来使用", titleAlignment: TitleAlignments.Centered, Title = @"@  ""开始配置数据包:※ 【"" + pack._name  + ""】""   ")]
        [VerticalGroup("总组/数据包")]

        /*  [DisplayAsString(fontSize: 30), ShowInInspector, HideLabel, GUIColor("@KeyValueMatchingUtility.ColorSelector.Color_03")]
          [VerticalGroup("总组/数据包"), PropertyOrder(-1)]
          public string addText => "新建数据包";*/

        [VerticalGroup("总组/数据包"), AssetSelector, SerializeReference, LabelText("拖入或者用按钮用于缓冲的数据组")]
        public List<ISoDataGroup> soGroups = new List<ISoDataGroup>();
        [DisplayAsString(fontSize: 30), HideLabel, GUIColor("@ESStaticDesignUtility.ColorSelector.Color_01")]
        [VerticalGroup("总组/数据包")]
        public string showText = "查看数据包详情";
        [InlineEditor(Expanded = true), SerializeReference, LabelText("数据包"), GUIColor("@ESStaticDesignUtility.ColorSelector.Color_03")]
        [VerticalGroup("总组/数据包")]
        public ISoDataPack pack;

        public override ESWindowPageBase ES_Refresh()
        {
            pack.Check();
            if (pack.EnableAutoRefresh && soGroups != null)
            {
                Undo.RecordObject(this.pack as ScriptableObject, "this");
                foreach (var i in pack.ApplingGroups)
                {
                    if (i == null) continue;
                    if (i is not ScriptableObject) continue;

                    if (pack.GetSoInfoType() == i.getSoInfoType())
                    {
                        pack.AddInfosByGroup(i);
                    }
                    else
                    {
                        Debug.LogError("数据组" + i._name + "的类型不合适或者已经销毁");
                    }
                }
                Undo.RecordObject(this.pack as ScriptableObject, "this");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            return base.ES_Refresh();

        }

        #region 按钮
        [HorizontalGroup("总组", width: 100)]
        [VerticalGroup("总组/按钮组")]
        [PropertySpace(15)]
        [Button("自动搜集全部数据组", ButtonHeight = 20), GUIColor("@ESStaticDesignUtility.ColorSelector.Color_03")]
        public void SearchAllGroup()
        {
            //找到全部数据组
            var all = AssetDatabase.FindAssets("t:ScriptableObject");
            soGroups = new List<ISoDataGroup>();
            foreach (var i in all)
            {
                GUID id = default; GUID.TryParse(i, out id);
                Type type = AssetDatabase.GetMainAssetTypeFromGUID(id);

                if (typeof(ISoDataGroup).IsAssignableFrom(type))
                {
                    string path = AssetDatabase.GUIDToAssetPath(id);
                    UnityEngine.Object ob = AssetDatabase.LoadAssetAtPath(path, type);
                    if (ob == null) continue;
                    ISoDataGroup dataGroup = ob as ISoDataGroup;
                    if (dataGroup.getSoInfoType() == pack.GetSoInfoType())
                    {
                        soGroups.Add(dataGroup);
                    }
                }
            }

        }
        [VerticalGroup("总组/按钮组")]
        [PropertySpace(15)]
        [Button("缓冲入数据组", ButtonHeight = 20), GUIColor("@ESStaticDesignUtility.ColorSelector.Color_04")]
        public void PushInDataGroup()
        {

            if (soGroups != null)
            {

                Undo.RecordObject(this.pack as ScriptableObject, "this");
                foreach (var i in soGroups)
                {
                    if (i == null) continue;
                    if (i is not ScriptableObject) continue;

                    if (pack.GetSoInfoType() == i.getSoInfoType())
                    {
                        pack.AddInfosByGroup(i);
                    }
                    else
                    {
                        Debug.LogError("数据组" + i._name + "的类型不合适或者已经销毁");
                    }

                }
                Undo.RecordObject(this.pack as ScriptableObject, "this");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

            }

            /* string s = @"@  ""开始配置数据组！！ 【"" + group.name_  + ""】""   ";*/

        }
        #endregion
    }
    //数据组创建页面
    [Serializable]
    public class Page_CreateNewGroupOrSearch : ESWindowPageBase
    {
        [Title("开始新建配置数据组！", "可以先选择预设类型", bold: true, titleAlignment: TitleAlignments.Centered)]
        [HorizontalGroup("总组")]
        [DisplayAsString(fontSize: 30), HideLabel, GUIColor("@ESStaticDesignUtility.ColorSelector.Color_01")]
        [VerticalGroup("总组/数据")]
        public string createText = "创建新数据组文件配置";
        [DetailedInfoBox("未选中任何数据组！", "创建一个数据组来快捷编辑数据内容！！将会支持Buff,技能,人物,物品等", infoMessageType: InfoMessageType.Warning)]
        [InfoBox("请修改一下文件名否则会分配随机数字后缀", VisibleIf = "@!hasChange", InfoMessageType = InfoMessageType.Warning)]
        [VerticalGroup("总组/数据"), LabelText("文件命名"), Space(5), GUIColor("@ESStaticDesignUtility.ColorSelector.Color_04"), OnValueChanged("ChangeHappen")]
        public string createName_ = "Buff新建配置组";

        private bool hasChange = false;
        private void ChangeHappen()
        {

            hasChange = true;

        }
        [FolderPath, OnValueChanged("Refresh")]
        [VerticalGroup("总组/数据"), LabelText("保存文件夹"), Space(5), GUIColor("@ESStaticDesignUtility.ColorSelector.Color_04")]
        public string FolderPath_ = "Assets/Resources/Data/GroupData";
        private void Refresh()
        {
            AssetDatabase.Refresh();
            ESDataWindow.usingWindow.selectPackType_ = createGroup_;

        }

        [DisplayAsString(fontSize: 30), HideLabel, GUIColor("@ESStaticDesignUtility.ColorSelector.Color_01")]
        [VerticalGroup("总组/数据")]
        public string createTypeText = "筛选数据组类型";


        [OnValueChanged("ResetConfigure"), InfoBox("建议选择一个预设类型的配置组,或者自己创建支持,默认类型无法直接使用", infoMessageType: InfoMessageType.Warning/*, VisibleIf = "@createGroup_==EvWindowDataAndTool.DataType.None"*/)]
        [VerticalGroup("总组/数据"), Space(5), LabelText("预定义类型")]
        [ValueDropdown("@ESDataWindowTool.GetGroupNames()", AppendNextDrawer = false)]
        public string createGroup_ = "Buff数据组";

        #region 按钮
        [HorizontalGroup("总组", width: 100)]
        [VerticalGroup("总组/按钮")]
        [PropertySpace(15)]
        [Button(ButtonHeight = 20, Name = "创建数据组", Icon = SdfIconType.Check2Square), GUIColor("@ ESStaticDesignUtility.ColorSelector.Color_03")]
        public void CreateInfoGroupAsset()
        {
            Type targetType = ESDataWindowTool.GetGroupType(createGroup_);
            ESStaticDesignUtility.SafeEditor.CreateSOAsset(targetType, FolderPath_, createName_, true, hasChange, beforeSave);
            void beforeSave(ScriptableObject so)
            {
                Selection.activeObject = so;
            }
        }
        [VerticalGroup("总组/按钮")]
        [PropertySpace(15)]
        [Button(ButtonHeight = 20, Name = "选中数据组", Icon = SdfIconType.Check2Square), GUIColor("@ ESStaticDesignUtility.ColorSelector.Color_04")]
        public void FindAInfoGroupAsset()
        {
            //找到全部数据组
            var all = ESStaticDesignUtility.SafeEditor.FindSOAssets<ISoDataGroup>(ESDataWindowTool.GetGroupType(createGroup_));
            foreach (var i in all)
            {
                if (i is ScriptableObject so)
                {
                    ESDataWindowTool.SelectSoGroup(so);
                    break;
                }
            }
        }

        private void ResetConfigure()
        {
            hasChange = false;
            ESDataWindow.usingWindow.selectGroupType_ = createGroup_;
            ESDataWindow.usingWindow.ESWindow_RefreshWindow();
            createName_ = "新建" + createGroup_;
        }
        private bool TypeSelectorSettingForDataGroup(Type type)
        {
            return !type.IsAbstract && !type.IsInterface && typeof(ISoDataGroup).IsAssignableFrom(type);
        }
        #endregion
    }
    //子页面 数据组快速查看
    [Serializable]
    public class Page_Index_DataQuickSeeGroup : ESWindowPageBase
    {
        [HorizontalGroup("总组", VisibleIf = "@group!=null")]
        [VerticalGroup("总组/数据组")]
        [DisplayAsString(fontSize: 30), GUIColor("@ESStaticDesignUtility.ColorSelector.Color_01"), HideLabel]
        public string readme = "双击↓SO编辑该数据组";
        [VerticalGroup("总组/数据组")]
        [InlineEditor(Expanded = true), ReadOnly, SerializeReference, LabelText("数据组")]
        public ScriptableObject group;

        [HorizontalGroup("总组", width: 100)]
        [VerticalGroup("总组/按钮组")]
        [Button("编辑", ButtonHeight = 20), GUIColor("@ESStaticDesignUtility.ColorSelector.Color_03")]
        private void EditThisGroup()
        {
            ESDataWindowTool.SelectSoGroup(group);
        }
    }
    //数据组配置页面
    [Serializable]
    public class Page_DataGroupOnChoose : ESWindowPageBase
    {

        [Title("", "数据组可以把一类数据整合集中配置和保存", titleAlignment: TitleAlignments.Centered, Title = "@ShowTitle()"), DisplayAsString, HideLabel]
        public string s = "";
        [HorizontalGroup("总组", VisibleIf = "@group!=null")]
        [VerticalGroup("总组/数据组")]
        [DisplayAsString(fontSize: 30), HideLabel, GUIColor("@ESStaticDesignUtility.ColorSelector.Color_01")]
        [VerticalGroup("总组/数据组")]
        public string createText = "创建新单元配置";
        [InfoBox("建议修改一下键名或者单元名防止重复！", VisibleIf = "@!hasChange", InfoMessageType = InfoMessageType.Warning)]
        [InfoBox("该元素的键已经出现了！！请修改", VisibleIf = "@!(group?.NotContainsInfo(DataKey)??false)", InfoMessageType = InfoMessageType.Error)]
        [OnValueChanged("Change"), LabelText("数据单元的键")]
        [VerticalGroup("总组/数据组")]
        public string DataKey = "数据键";
        [OnValueChanged("Change"), LabelText("数据单元的文件名")]
        [VerticalGroup("总组/数据组")]
        public string DataFileName = "数据单元";


        [VerticalGroup("总组/数据组"), LabelText("从本组复制"), Space(5), GUIColor("@ESStaticDesignUtility.ColorSelector.Color_04"), ValueDropdown("@group.AllKeys")
            , InlineButton("pastePersis", "持久粘贴"), InlineButton("copyPersis", "持久复制")]
        public string copyFrom = "fromInfo";

        private bool hasChange = false;
        private void Change()
        {
            hasChange = true;
        }
        [DisplayAsString(fontSize: 30), HideLabel, GUIColor("@ESStaticDesignUtility.ColorSelector.Color_01")]
        [VerticalGroup("总组/数据组")]
        public string addText = "缓冲入单元列表";
        [VerticalGroup("总组/数据组"), LabelText("缓冲子数据"), SerializeReference]
        public List<ISoDataInfo> soInfos = new List<ISoDataInfo>();
        [DisplayAsString(fontSize: 30), HideLabel, GUIColor("@ESStaticDesignUtility.ColorSelector.Color_01")]
        [VerticalGroup("总组/数据组")]
        public string showText = "查看数据组详情";
        [InlineEditor(Expanded = true), SerializeReference, LabelText("数据组")]
        [VerticalGroup("总组/数据组"), OnValueChanged("RefreshInfos")]
        public ISoDataGroup group;

        private string ShowTitle()
        {
            return group != null ? "开始配置数据组:※ 【" + group._name + "】" : "!!先选中要配置的数据组！!";
        }

        #region 按钮
        [HorizontalGroup("总组", width: 100)]
        [VerticalGroup("总组/按钮组")]
        [PropertySpace(15)]
        [Button("去新建或者筛选", ButtonHeight = 20), GUIColor("@ESStaticDesignUtility.ColorSelector.Color_04")]
        public void CreateNewPage()
        {
            if (ESDataWindow.Items.TryGetValue(ESDataWindow.PageName_DataGroupCreate, out var pageItem))
            {
                ESDataWindow.usingWindow.MenuTree.Selection.Add(pageItem);
            }
        }
        [VerticalGroup("总组/按钮组")]
        [PropertySpace(15)]
        [Button("新建单元数据", ButtonHeight = 20), GUIColor("@ESStaticDesignUtility.ColorSelector.Color_03")]
        public void CreateNewSoDataInfo()
        {
            Type type = group.getSoInfoType();
            ScriptableObject @object = ScriptableObject.CreateInstance(type);
            @object.name = DataFileName + DataKey + (hasChange ? "" : UnityEngine.Random.Range(0, 99999));
            if (@object is IWithKey with && group.NotContainsInfo(DataKey))
            {
                with.SetKey(DataKey);
                Debug.Log(DataKey + "Info Register");
                group.AddInfo(DataKey, @object);
                AssetDatabase.AddObjectToAsset(@object, AssetDatabase.GetAssetPath(group as ScriptableObject));
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogError("不合理的值或者重复键");
            }

        }

        [VerticalGroup("总组/按钮组")]
        [PropertySpace(15)]
        [Button("拷贝本组单元", ButtonHeight = 20), GUIColor("@ESStaticDesignUtility.ColorSelector.Color_03")]
        public void CopyFromSoDataInfo()
        {
            var from = group.GetInfoByKey(copyFrom) as ScriptableObject;
            if (from == null||!group.getSoInfoType().IsAssignableFrom(from.GetType()))
            {
                if(from!=null) ESStaticDesignUtility.SafeEditor.DisplayDialog("类型不匹配！", $"想要拷贝单元数据为{from}({from?.GetType()})" +
                    $"，但是这个类型和当前数据组{group.getSoInfoType()}不符");
                else ESStaticDesignUtility.SafeEditor.DisplayDialog("为空！", $"没有可用的拷贝源");
                return;
            }
            ScriptableObject copy = ScriptableObject.Instantiate(from);
            copy.name = DataFileName + DataKey + (hasChange ? "" : UnityEngine.Random.Range(0, 99999));
            if (copy is IWithKey with && group.NotContainsInfo(DataKey))
            {
                with.SetKey(DataKey);
                Debug.Log(DataKey + "Info Register");
                group.AddInfo(DataKey, copy);
                AssetDatabase.AddObjectToAsset(copy, AssetDatabase.GetAssetPath(group as ScriptableObject));
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogError("不合理的值或者重复键");
            }

        }

        private void copyPersis()
        {
            var Ifrom = group.GetInfoByKey(copyFrom);
            var from = Ifrom as ScriptableObject;
            if (from != null)
            {
                ESDataWindow.CopyGroup = group as ScriptableObject;
                ESDataWindow.CopyInfoKey = Ifrom.key.str_direc;
            }
        }
        private void pastePersis()
        {
            if (ESDataWindow.CopyGroup != null)
            {
                var groupcopy = ESDataWindow.CopyGroup as ISoDataGroup;
                if (groupcopy == null)
                {
                    ESStaticDesignUtility.SafeEditor.DisplayDialog("为空！", $"没有可用的拷贝源<数据组不存在>");
                    return;
                }
                var from = groupcopy.GetInfoByKey(ESDataWindow.CopyInfoKey) as ScriptableObject;
                Debug.Log(group.getSoInfoType().IsAssignableFrom(from?.GetType())+ ESDataWindow.CopyInfoKey);
                if (from == null || !group.getSoInfoType().IsAssignableFrom(from.GetType()))
                {
                    if (from != null) ESStaticDesignUtility.SafeEditor.DisplayDialog("类型不匹配！", $"想要拷贝单元数据为{from}({from?.GetType()})" +
                        $"，但是这个类型和当前数据组需要的{group.getSoInfoType()}不符");
                    else ESStaticDesignUtility.SafeEditor.DisplayDialog("为空！", $"没有可用的拷贝信息<数据组>"+ groupcopy+"<键>"+ESDataWindow.CopyInfoKey);
                    return;
                }
                ScriptableObject copy = ScriptableObject.Instantiate(from);
                copy.name = DataFileName + DataKey + (hasChange ? "" : UnityEngine.Random.Range(0, 99999));
                if (copy is IWithKey with && group.NotContainsInfo(DataKey))
                {
                    with.SetKey(DataKey);
                    Debug.Log(DataKey + "Info Register");
                    group.AddInfo(DataKey, copy);
                    AssetDatabase.AddObjectToAsset(copy, AssetDatabase.GetAssetPath(group as ScriptableObject));
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
                else
                {
                    Debug.LogError("不合理的值或者重复键");
                }
            }
        }
        public void RefreshInfos()
        {
            if (group != null)
            {
                if (ESDataWindow.Items.TryGetValue(ESDataWindow.PageName_DataGroupOnChooseEditInfo, out var item))
                {
                    ESDataWindow.ES_RefreshWindow();
                }
            }
        }
        public override ESWindowPageBase ES_Refresh()
        {
            if (ESDataWindow.Items.TryGetValue(ESDataWindow.PageName_DataGroupOnChooseEditInfo, out var item))
            {
                if (group == null) group = Selection.activeObject as ISoDataGroup;
                if (group != null) item.Name = ESDataWindow.PageName_DataGroupOnChooseEditInfo + "<" + group._name.Replace("新建", "") + ">";
            }
            if (group != null)
            {
                var info= group.GetInfoByKey(copyFrom);
                if (info == null&&group.AllKeys.Count>0)
                {
                    copyFrom = group?.AllKeys.First();
                }
            }
            Check();
            return base.ES_Refresh();
        }



        [VerticalGroup("总组/按钮组")]
        [PropertySpace(15)]
        [Button("检查数据", ButtonHeight = 20), GUIColor("@ESStaticDesignUtility.ColorSelector.Color_03")]
        public void Check()
        {
            List<string> ToRemove = new List<string>();
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            bool hasChange = false;
            if(group!=null)
            foreach (var i in group.AllKeys)
            {

                ISoDataInfo so = group.GetInfoByKey(i);
                ScriptableObject so_ = so as ScriptableObject;
                if (so_ != null)
                {
                    Debug.Log("apply");
                    if (so.key.str_direc != i)
                    {
                        so.SetKey(i);
                        hasChange = true;
                    }
                }
                else
                {
                    Debug.Log("RemoveInfo");
                    ToRemove.Add(i);
                    hasChange = true;
                }
            }
            foreach (var i in ToRemove)
            {
                group.RemoveInfo(i);
            }
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            if (hasChange)
            {
                ESDataWindow.usingWindow?.ESWindow_RefreshWindow();
            }

        }
        [VerticalGroup("总组/按钮组")]
        [PropertySpace(15)]
        [Button("缓冲入子数据", ButtonHeight = 20), GUIColor("@ESStaticDesignUtility.ColorSelector.Color_03")]
        public void Collect()
        {
            UnityEngine.Object ob = group as ScriptableObject;
            string groupPath = AssetDatabase.GetAssetPath(ob);

            foreach (var i in soInfos)
            {
                ScriptableObject obd = i as ScriptableObject;
                string soPath = AssetDatabase.GetAssetPath(obd);
                Debug.Log(soPath);
                if (soPath.StartsWith(groupPath))
                {
                    group.AddInfo(i.key.str_direc, obd);
                }
            }
        }
        #endregion
    }
    //子层 数据单元配置页面
    [Serializable]
    public class Page_Index_DataInfoSingle : ESWindowPageBase
    {

        [HorizontalGroup("总组")]
        [Title("开始配置单元数据！!", "配置单个数据应用到游戏逻辑", titleAlignment: TitleAlignments.Centered, Title = "@Title()")]

        [DisplayAsString(fontSize: 30), GUIColor("@ESStaticDesignUtility.ColorSelector.Color_04"), HideLabel]
        [VerticalGroup("总组/数据组")]
        public string handleName = "文件配置相关";

        [VerticalGroup("总组/数据组"), LabelText("重命名文件"), GUIColor("@ESStaticDesignUtility.ColorSelector.Color_02")]
        public string renameFile = "新文件名";
        [VerticalGroup("总组/数据组")]
        [DisplayAsString(fontSize: 30), GUIColor("@ESStaticDesignUtility.ColorSelector.Color_04"), HideLabel]
        public string handleSOData = "配置数据";
        [InlineEditor, LabelText("数据")]

        [VerticalGroup("总组/数据组"), SerializeReference]
        public ISoDataInfo data;
        [HorizontalGroup("总组", width: 100)]

        [VerticalGroup("总组/按钮组")]
        [PropertySpace(15)]
        [Button("重命名文件", ButtonHeight = 20), GUIColor("@ESStaticDesignUtility.ColorSelector.Color_04")]
        public void RenameFileThis()
        {
            data.key.KeySelf();
            var file = data as ScriptableObject;
            if (file != null)
            {
                file.name = renameFile;
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
        private string Title()
        {
            return "开始配置数据文件:※ 【" + data.key.KeySelf() + "】";
        }
        [VerticalGroup("总组/按钮组")]
        [PropertySpace(15)]
        [Button("删除", ButtonHeight = 20), GUIColor("@ESStaticDesignUtility.ColorSelector.Color_03")]
        public void DeleteThis()
        {
            Undo.DestroyObjectImmediate(data as ScriptableObject);
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            ESDataWindow.usingWindow.pageForGroupOnChoose.Check();
        }
    }
    #endregion

    #region 关于
    //关于页面
    [Serializable]
    public class Page_About : ESWindowPageBase
    {
        [Title("欢迎使用依稀开发框架工具包")]
        [LabelText("版本号"), ShowInInspector, GUIColor("@ESStaticDesignUtility.ColorSelector.Color_01")]
        public string ToolVersion => "ES 0.0.3 测试版本";
        [LabelText("插件依赖"), ShowInInspector, GUIColor("@ESStaticDesignUtility.ColorSelector.Color_01")]
        public string PluginsDependence => "Odin插件,Dotween插件,FishNet插件，和一些库";
        [PropertySpace(15)]
        [Title("开发者", "工具开发者相关信息", titleAlignment: TitleAlignments.Split)]
        [LabelText("开发者团队"), ShowInInspector, GUIColor("@ESStaticDesignUtility.ColorSelector.Color_02")]
        public string DeveloperTeam => "Ey Sive企划";
        [LabelText("开发者名称"), ShowInInspector, GUIColor("@ESStaticDesignUtility.ColorSelector.Color_02")]
        public string DeveloperName => "超级依薇尔";
        [LabelText("开发者QQ"), ShowInInspector, GUIColor("@ESStaticDesignUtility.ColorSelector.Color_01")]
        public string DeveloperQQ => "2650026906";
        [LabelText("开发者邮箱"), ShowInInspector, GUIColor("@ESStaticDesignUtility.ColorSelector.Color_01")]
        public string DeveloperEmail => "2650026906@qq.com";
        [TitleGroup("最后的话", "来自超级依薇尔", alignment: TitleAlignments.Split), HideLabel, PropertyOrder(3), ReadOnly, TextArea(5, 10)]
        public string WordsOnEnd = "我是哔哩哔哩的超级依薇尔，欢迎关注我，另外我创建了QQ交流群982703564\n" +
            "欢迎前来进行Unity技术交流讨论和分享自己的作品和开发见解。\n" +
            "本框架特别感谢 凉鞋大佬(Q FrameWork作者)\n" + ""
            ;
    }
    #endregion

    #region 辅助
    //数据源和辅助工具
    [InitializeOnLoad]
    public static class ESDataWindowTool
    {
        static ESDataWindowTool()
        {
            Selection.selectionChanged += DataWindowSelection;
        }
        private static void DataWindowSelection()
        {
            if (Selection.activeObject is ISoDataGroup group)
            {
                var page = ESDataWindow.usingWindow?.pageForGroupOnChoose;
                if(page==null|| ESDataWindow.usingWindow.MenuTree == null)
                {
                    ESDataWindow.OpenWindow();
                    ESDataWindow.usingWindow.ESWindow_RefreshWindow();
                    Debug.Log("自动打开数据窗口");
                    page = ESDataWindow.usingWindow.pageForGroupOnChoose;
                }
                if (page != null&& ESDataWindow.usingWindow.MenuTree!=null)
                {
                    page.group = group;
                    SelectSoGroup(group as ScriptableObject);
                    page.RefreshInfos();
                    if (ESDataWindow.Items.TryGetValue(ESDataWindow.PageName_DataGroupOnChooseEditInfo, out var item))
                    {
                        item.Name = ESDataWindow.PageName_DataGroupOnChooseEditInfo + "<" + group._name.Replace("新建", "") + ">";
                        Debug.Log("Selection" + ESDataWindow.usingWindow);
                        Debug.Log("Selection"+ ESDataWindow.usingWindow.MenuTree.Selection);
                        ESDataWindow.usingWindow.MenuTree.Selection.Add(item);
                    }
                }
            }
        }
        public static void SelectSoGroup(ScriptableObject so)
        {
            Selection.activeObject = so;
            if (ESDataWindow.Items.TryGetValue(ESDataWindow.PageName_DataGroupOnChooseEditInfo, out var pageItem))
            {
                ESDataWindow.usingWindow.MenuTree.Selection.Add(pageItem);
            }
        }
        #region 简单方法
        public static string[] GetInfoNames()
        {
            return GlobalDataForEditorRunTime.Instance.SearchDataTypeKey.GetDic("数据单元").Keys.ToArray();
        }
        public static string[] GetGroupNames()
        {
            return GlobalDataForEditorRunTime.Instance.SearchDataTypeKey.GetDic("数据组").Keys.ToArray();
        }
        public static string[] GetPackNames()
        {
            return GlobalDataForEditorRunTime.Instance.SearchDataTypeKey.GetDic("数据包").Keys.ToArray();
        }
        public static Type GetInfoType(string name)
        {
            return GlobalDataForEditorRunTime.Instance.SearchDataTypeKey.GetElement("数据单元", name);
        }
        public static Type GetInfoTypeByGroupName(string name)
        {
            return GetInfoType(name.Replace("组", "单元"));
        }
        public static Type GetInfoTypeByPackName(string name)
        {
            return GetInfoType(name.Replace("包", "单元"));
        }
        public static Type GetGroupType(string name)
        {
            return GlobalDataForEditorRunTime.Instance.SearchDataTypeKey.GetElement("数据组", name);
        }
        public static Type GetPackType(string name)
        {
            return GlobalDataForEditorRunTime.Instance.SearchDataTypeKey.GetElement("数据包", name);
        }
        public static bool HasNull(params UnityEngine.Object[] objects)
        {
            if (objects == null) return true;
            foreach (var i in objects)
            {
                if (i == null) return true;
            }
            return false;
        }
        public static bool AllIsOk(params UnityEngine.Object[] objects)
        {
            if (objects == null) return false;
            foreach (var i in objects)
            {
                if (i == null) return false;
            }
            return true;
        }
        #endregion
    }

    #endregion
}
