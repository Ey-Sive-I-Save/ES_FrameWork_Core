using ES;
using ES.EvPointer;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace ES
{

    public class ESPreviewWindow : ESWindowBase_Abstract<ESPreviewWindow>
    {
        #region 简单重写
        public override GUIContent ESWindow_GetWindowGUIContent()
        {
            Texture2D texture = Resources.Load<Texture2D>("Sprites/ItMoting");
            var content = new GUIContent("依薇尔预览窗口", texture, "使用依薇尔预览工具提高数据和对象查询效率");
            return content;
        }
        public override void ESWindow_OpenHandle()
        {
            base.ESWindow_OpenHandle();

        }
        #endregion
        #region 数据滞留


        [NonSerialized] public Page_Root_Preview_StartUse pageForStartUsePage;

        public Page_ShowSystemICON systemICON;
        public Page_CacheObjects cacheObjects;
        public Page_ViewFix viewFix;
        #endregion

        [MenuItem("Tools/ES工具/ES预览窗口")]
        public static void TryOpenWindow()
        {
            OpenWindow();
        }
        protected override void ES_BuildMenuTree(OdinMenuTree tree)
        {
            Part_BuildStartPage(tree);
            {//独立功能块

                base.ES_BuildMenuTree(tree);
                tree.Add("系统图标预览", systemICON ??= new Page_ShowSystemICON());
                MakePage_CacheObjects(tree, "缓存物体信息");
                tree.Add("修复", viewFix ??= new Page_ViewFix());
            }
          
        }
        public void Part_BuildStartPage(OdinMenuTree tree)
        {
            QuickBuildRootMenu(tree, "开始使用", ref pageForStartUsePage, SdfIconType.SunFill);
        }

        private void MakePage_CacheObjects(OdinMenuTree tree, string Menu)
        {
            tree.Add(Menu, cacheObjects ??= new Page_CacheObjects());
            Menu += "/";
            HashSet<GameObject> hasGS = new HashSet<GameObject>(Objects.Count);
            HashSet<UnityEngine.Object> hasOS = new HashSet<UnityEngine.Object>(Objects.Count);

            for (int i = 0; i < Objects.Count; i++)
            {
                Debug.Log(i);
                var ii = Objects[i];
                if (ii == null || hasOS.Contains(ii)) return;
                string ss = ii.name;
                if (ii is MonoBehaviour mo)
                {
                    GameObject gg = mo.gameObject;
                    if (hasGS.Contains(gg))
                    {

                    }
                    else
                    {
                        hasGS.Add(gg);
                        tree.Add(Menu + "游戏物体:" + mo.gameObject.name, new Page_Index_Object() { Object = gg });

                    }
                    ss = Menu + "游戏物体:" + mo.gameObject.name + "/" + "*脚本" + (ii.GetType());
                }
                hasOS.Add(ii);
                tree.Add(ss, new Page_Index_Object() { Object = ii });
            }
        }

        public override void ES_LoadData()
        {
            base.ES_LoadData();
            icons = new List<SystemICON>(100);
            foreach (var i in UnityEditorIcons.UnityEditorIconNames.AllChinese.Keys)
            {
                try
                {
                    string en = UnityEditorIcons.UnityEditorIconNames.AllChinese[i];
                    Texture tt = EditorGUIUtility.IconContent(en)?.image;
                    if (tt != null)
                    {

                        icons.Add(new SystemICON() { chi = i, eng = en, texture = tt });
                    }
                }
                catch (Exception e)
                {
                    var u = e.Data;
                }

            }
            if (PlayerPrefs.HasKey(keyForCache))
            {
                Debug.Log("开始加载GUID");
                pathForCache = JsonConvert.DeserializeObject<List<string>>(PlayerPrefs.GetString(keyForCache));
                if (pathForCache != null && pathForCache.Count > 0)
                {
                    Debug.Log("GUIDS" + pathForCache + pathForCache.Count);
                    Objects = new List<UnityEngine.Object>(pathForCache.Count);
                    foreach (var i in pathForCache)
                    {

                        Debug.Log("加载" + i);
                        if (int.TryParse(i, out var id))
                        {
                            Objects.Add(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(AssetDatabase.GUIDToAssetPath(i)));
                        }

                    }

                }
                else
                {
                    Debug.Log("空的");
                }
            }
        }
        public override void ES_SaveData()
        {
            base.ES_SaveData();
            pathForCache = new List<string>(Objects.Count);
            Debug.Log("开始保存GUID");
            foreach (var i in Objects)
            {

                string path = i.GetInstanceID().ToString();
                pathForCache.Add(path);
                Debug.Log("保存" + i + path);
            }
            PlayerPrefs.SetString(keyForCache, JsonConvert.SerializeObject(pathForCache, formatting: Formatting.Indented));
            PlayerPrefs.Save();
        }
        #region 存储

        public static List<SystemICON> icons = new List<SystemICON>();
        private static List<string> pathForCache = new List<string>();
        public static List<UnityEngine.Object> Objects = new List<UnityEngine.Object>();
        public static string keyForCache = "window-preview-cache-pathForCache";
        #endregion
    }
    [Serializable, TypeRegistryItem("系统ICON")]
    public class SystemICON
    {
        [LabelText("中文"), ReadOnly, VerticalGroup("a")]
        [TableColumnWidth(50)]
        public string chi;
        [LabelText("英文"), ReadOnly, VerticalGroup("a")]
        public string eng;
        [LabelText("图标"), ReadOnly, VerticalGroup("a")]
        public Texture texture;
    }

    #region 开始使用
    [Serializable]
    public class Page_Root_Preview_StartUse : ESWindowPageBase
    {
        [Title("开始使用ES Preview预览窗口！！", subtitle: "为了快速入门，我们从最简单的概念开始排列介绍和布局")]
        [DisplayAsString(alignment: TextAlignment.Center, fontSize: 20), HideLabel]
        public string titleF = " Preview预览窗口，为查询演示，介绍，功能配置提供了快捷入口功能";

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
        [TabGroup("概念", "关于ES编辑器支持"), HideLabel, TextArea(5, 10), DisplayAsString(alignment: TextAlignment.Left)]
        public string aboutDataInfo = "" +
     "数据单元，存放一个类型的单个数据，他是作为子资产进入(数据组)的！\n" +
      "数据单元存放单个的独立数据，比如一个怪物，一个飞行物，一个技能\n" +
     "在没有资源更新需求下，可以考虑直接引用数据单元，他直接作为文件用代码加载步骤略多\n，" +
     "其中\n******【1】数据单元是一个数据组的子资产，因为共性被整合，每个有独立的完整数据\n" +
     " ******【2】数据单元根据作用的对象编写大量数据内容\n " +
     " ******【3】英文Info,为它的专属名\n" +
     " ******【#】数据单元还可以实现共享与变量体系，";


        [TabGroup("概念", "关于ES功能详解"), HideLabel, TextArea(5, 10), DisplayAsString(alignment: TextAlignment.Left)]
        public string aboutDataGroup = "" +
     "数据组，数据组把数个具有同特征的数据单元包含其中，作为独立的资产的最小格式！\n" +
      "组最大的作用是分组，以一个资产包含多个子单元\n" +
     " 通常来说不推荐直接把组用来引用至游戏，而是以后面的包来完成\n，" +
     "其中\n******【1】数据组是一个数据单元的持久字典，并且原则上把子单元都作为子资产包含其中，推荐容纳5-10个为佳\n" +
     " ******【2】数据组一般只有分组和编辑功能，不推荐用于游戏引用，加载和取用，这只是一个建议和规范，可以自己定\n " +
     " ******【3】英文Group,为它的专属名词\n" +
     " ******【#】以一个资产容纳一系列数据单元,高效分类整理，可以绑定到数据包来做到输出最新的内容";

        [TabGroup("概念", "关于编辑器扩展支持"), HideLabel, TextArea(5, 10), DisplayAsString(alignment: TextAlignment.Left)]
        public string aboutEditor = "" +
      "数据包，同样以持久字典直接引用一些数据单元，跳过了数据组，一般来说一个包可以涵盖一套功能的基本结构！\n" +
       "包并不推荐为数据单元重命名,而是一般简单地从多个数据组缓冲入数据，他主要是为了能快速收集足够有效数据\n" +
      " 他的数据组主要有一个单元更新关系,，在游戏运行时，不推荐从包获得组再进行操作\n，" +
      "其中\n******【1】包只是一类数据单元引用的持久字典\n" +
      " ******【2】包可以选定和数据组建立更新链接，以便防止忘记手动载入\n " +
      " ******【3】英文Pack,为它的专属名词\n" +
      " ******【#】建议广泛使用包来简化游戏逻辑流程";

        [TabGroup("概念", "关于Dotween补充说明"), HideLabel, TextArea(5, 10), DisplayAsString(alignment: TextAlignment.Left)]
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

        [TabGroup("概念", "关于本框架"), HideLabel, TextArea(5, 10), DisplayAsString(alignment: TextAlignment.Left)]
        public string aboutCodeGen = "" +
            "SO代码生成工具用于自动化创建C# Scriptable 文件\n" +
            "用来简化类似结构代码的声明工作量,\n" +
            "其中\n******【1】So体系生成,提供从So单元，组，包 的一键构建\n" +
            "******【2】So全局配置文件生成,这种配置文件可以被轻易地引用并且提供了自动创建引导和多配置选用\n " +
            "******【3】SharedData 与 VariableData 体系，是So游戏逻辑数据的一个标准，生成的代码主要为了指引深拷贝优化\n" +
            "******【#】这里是关于SO的数据生成，还有更多代码生成属于其他功能模块！";

    }


    #endregion

    #region 预览图表集
    [Serializable]
    public class Page_ShowSystemICON
    {
        [ShowInInspector, HideLabel, InlineProperty, TableList(ShowIndexLabels = true, HideToolbar = false, AlwaysExpanded = true)]
        public List<SystemICON> show { get => ESPreviewWindow.icons; set { } }
        public Page_ShowSystemICON()
        {


        }
    }
    #endregion

    #region 缓冲物体表
    [Serializable, TypeRegistryItem("缓存物体表")]
    public class Page_CacheObjects
    {
        [ShowInInspector]
        public List<UnityEngine.Object> objects => ESPreviewWindow.Objects;
    }
    [Serializable]
    public class Page_Index_Object
    {
        [HideLabel, DisplayAsString(fontSize: 24), ShowInInspector, PropertyOrder(-1)]
        public string ss => "操作物体" + Object.name;
        [InlineEditor, HideLabel()]
        public UnityEngine.Object Object;
    }
    #endregion

    #region 修复
    public class Page_ViewFix
    {
        [ButtonGroup("视觉组")]
        [Button("修复选中物体的隐藏")]
        public void Button_FixHideGameobjectFlags()
        {
            var a = Selection.activeGameObject;
            if (a != null)
            {
                a.hideFlags = a.hideFlags & ~HideFlags.HideInInspector;
                var use = a.GetComponents<Component>();
                foreach (var i in use)
                {
                    a.hideFlags = a.hideFlags & ~HideFlags.HideInInspector;
                }
            }

        }
        [ButtonGroup("模块组")]
        [Button("移除全部空Clip")]
        public void Button_FixNullReferClips()
        {
            var all = UnityEngine.Object.FindObjectsByType<Core>(sortMode: FindObjectsSortMode.None);
            foreach (var i in all)
            {
                var allDomain = i.GetComponentsInChildren<IDomain>();
                foreach (var ii in allDomain)
                {
                    ii.TryRemoveNullModules();
                    if (ii is MonoBehaviour mono)
                    {
                        EditorUtility.SetDirty(mono);
                        Undo.RecordObject(mono, "");
                        EditorUtility.SetDirty(mono.gameObject);
                        Undo.RecordObject(mono.gameObject, "");
                        AssetDatabase.SaveAssets();

                    }

                }
            }

        }


        #region 命名组
        [Serializable]
        public class TwoString
        {
            [LabelText("老名字")] public string ago = "老名字";
            [LabelText("新名字")] public string now = "新名字";
            [LabelText("允许片段")] public bool EnablePart = true;
            [Button("全局物体命名切换"), PropertySpace(5, 15)]
            public void Button_RenameGameObject()
            {
                var all = UnityEngine.Object.FindObjectsByType<GameObject>(sortMode: FindObjectsSortMode.None);
                if (EnablePart)
                {
                    foreach (var i in all)
                    {
                        i.name = i.name.Replace(ago, now);
                    }
                }
                else
                {
                    foreach (var i in all)
                    {
                        if (i.name == ago)
                        {
                            i.name = now;
                        }
                    }
                }
            }

        }
        [LabelText("游戏物体更名")]
        public TwoString twoName = new TwoString();
        #endregion

        #region 查ID组
        [Serializable]
        public class InstanceIDSearcher
        {
            public string guid = "66660";
            public string instanceID = "001001";
            [Button("查询选中物体的信息"), PropertySpace(5, 15)]
            public void Button_FindGUID()
            {
                var use = Selection.activeObject;
                instanceID = use.GetInstanceID().ToString();
                /*UnityEngine.Object[] allObjects = UnityEngine.Object.FindObjectsOfType<UnityEngine.Object>(true); // 包含隐藏对象
                foreach (UnityEngine.Object obj in allObjects)
                {
                    if (obj.GetInstanceID() ==int.Parse(guid))
                    {
                        GameObject targetObj = obj as GameObject;
                        Selection.activeObject = targetObj;
                        break;
                    }
                }*/
                /*  string path= AssetDatabase.GUIDToAssetPath(id);
                  var uu = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                 */
            }
        }
        [LabelText("查GUID物体")]
        public InstanceIDSearcher gUIDSearcher = new InstanceIDSearcher();
        #endregion
    }

    #endregion
}

