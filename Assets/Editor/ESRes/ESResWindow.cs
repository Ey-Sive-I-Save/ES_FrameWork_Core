using DG.Tweening.Plugins.Core.PathCore;
using ES;
using GameKit.Dependencies.Utilities;
using NUnit.Framework.Interfaces;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using static ES.ESResMaster;
using static ES.GlobalDataForResMaster;
using static ES.SkillPointDataInfo.SkillPointLevelAllTransfomor.SkillPointSprites;
using static UnityEngine.GraphicsBuffer;
using Path = System.IO.Path;

namespace ES
{


    public class ESResWindow : ESWindowBase_Abstract<ESResWindow>
    {
        public override GUIContent ESWindow_GetWindowGUIContent()
        {
            Texture2D texture = Resources.Load<Texture2D>("Sprites/OpLow");
            var content = new GUIContent("依薇尔资源管理窗口", texture, "使用依薇尔资源管理工具优化资源管理加载流程");
            return content;
        }
        #region 数据滞留
        public PartPageClass_AssetBundle Page_Assetbundle;


        #endregion
        [MenuItem("Tools/ES工具/ES资源管理窗口")]
        public static void TryOpenWindow()
        {
            if (GlobalDataForEditorRunTime.Instance != null)
                OpenWindow();
        }


        protected override void ES_BuildMenuTree(OdinMenuTree tree)
        {
            PartPage_AssetBundle(tree);
        }
        void PartPage_AssetBundle(OdinMenuTree tree)
        {
            tree.Add("AB包管理", (Page_Assetbundle ??= new PartPageClass_AssetBundle()).ES_Refresh());
        }
        #region 持久化
        public override void ES_LoadData()
        {
            base.ES_LoadData();
        }

        public override void ES_SaveData()
        {
            base.ES_SaveData();
        }
        #endregion

        #region 可序列化的组分
        //为AssetBundle提供的页面
        [Serializable]
        public class PartPageClass_AssetBundle : ESWindowPageBase
        {
            //刷新
            public override ESWindowPageBase ES_Refresh()
            {
                RefreshBundleList();
                Handle_RefresgHotUpdateData();
                apply_FromTarget();
                return base.ES_Refresh();
            }


            #region  全局设置


            //生成--按钮
            [TabGroup("全局设置", TextColor = "@ESStaticDesignUtility.ColorSelector.Color_03"), PropertyOrder(-4), GUIColor("@ESStaticDesignUtility.ColorSelector.ColorForApply")]
            [Button("生成AB包 (需要额外确定)"), PropertySpace(15)]
            private void Handle_BuildAB()
            {

                if (AssetDatabase.IsValidFolder(genarateFolder))
                {
                    EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(genarateFolder, typeof(UnityEngine.Object)));

                    ESEditorHandle.AddSimpleHanldeTask(
                        () =>
                        {
                            if (EditorUtility.DisplayDialog("开始生成AB包", "准备生成AB，这将会覆盖原来的资源，请确定路径和平台正确", "知道了", "取消"))
                            {

                            }
                            else
                            {
                                return;
                            }
                            var options = BuildAssetBundleOptions.ForceRebuildAssetBundle | BuildAssetBundleOptions.AppendHashToAssetBundleName;
                            if (PlatformToApply == RuntimePlatform.WindowsPlayer || PlatformToApply == RuntimePlatform.WindowsEditor)
                            {

                                BuildAB(genarateFolder, RuntimePlatform.WindowsPlayer.ToString(),
                                    assetBundleOptions: options,
                                    targetPlatform: BuildTarget.StandaloneWindows);
                                BuildAB(genarateFolder, RuntimePlatform.WindowsEditor.ToString(),
                                    assetBundleOptions: options,
                                    targetPlatform: BuildTarget.StandaloneWindows);
                            }
                            else if (PlatformToApply == RuntimePlatform.Android)
                            {
                                BuildAB(genarateFolder, PlatformToApply.ToString(),
                                    assetBundleOptions: options,
                                    targetPlatform: BuildTarget.Android);
                            }
                            else if (PlatformToApply == RuntimePlatform.IPhonePlayer)
                            {
                                BuildAB(genarateFolder, PlatformToApply.ToString(),
                                    assetBundleOptions: options,
                                    targetPlatform: BuildTarget.iOS);
                            }
                        }, 20);
                }
                else
                {
                    EditorUtility.DisplayDialog("无效的文件夹路径", "这个文件夹路径是不合法的，请确定正确的文件夹再重试", "知道了");
                }

            }

            //清空AB包 按钮
            [TabGroup("全局设置"), PropertyOrder(-4), GUIColor("@ESStaticDesignUtility.ColorSelector.ColorForCaster")]
            [Button("清空AB包 (需要额外确定)", DrawResult = false), PropertySpace(15)]
            private bool Handle_ClearAB()
            {
                if (AssetDatabase.IsValidFolder(genarateFolder))
                {
                    bool b = EditorUtility.DisplayDialog("是否清空AB包", "这会删除全部的已生成AB包(也包括目标文件夹下的全部内容)", "我确定", "放弃");
                    if (b)
                    {
                        var guids = AssetDatabase.FindAssets("", new string[] { genarateFolder });
                        foreach (var i in guids)
                        {

                            string path = AssetDatabase.GUIDToAssetPath(i);
                            AssetDatabase.DeleteAsset(path);
                            AssetDatabase.Refresh();
                        }
                        return true;
                    }
                    else
                    {

                    }
                }
                else
                {
                    EditorUtility.DisplayDialog("无效的文件夹路径", "这个文件夹路径是不合法的，请确定正确的文件夹再重试", "知道了");
                }
                return false;
            }


            //更改AB打包模式
            [LabelText("更改AB打包模式"), TabGroup("全局设置"), PropertyOrder(-3), PropertySpace(15)]
            [ShowInInspector]
            public GlobalDataForResMaster.ABPackType packType { get => GlobalDataForResMaster.Instance.abPackType; set { GlobalDataForResMaster.Instance.abPackType = value; EditorUtility.SetDirty(GlobalDataForResMaster.Instance); } }

            //打AB包时，自动辅助代码生成格式
            [LabelText("打AB包时自动辅助代码生成格式"), TabGroup("全局设置"), InfoBox("", Message = "@GetCodeInfo()", InfoMessageType = InfoMessageType.Warning)]
            [PropertyOrder(-3), ShowInInspector, InlineButton("Handle_CodeGenAssetConstName", "手动生成常量名代码(默认不改大小写)")]
            public GlobalDataForResMaster.ABForAutoCodeGen codeType
            {
                get => GlobalDataForResMaster.Instance.abFoeAutoCodeGen;
                set { GlobalDataForResMaster.Instance.abFoeAutoCodeGen = value; EditorUtility.SetDirty(GlobalDataForResMaster.Instance); }
            }
            string GetCodeInfo()
            {
                switch (codeType)
                {
                    case GlobalDataForResMaster.ABForAutoCodeGen.NoneCode: return "不生成代码";
                    case GlobalDataForResMaster.ABForAutoCodeGen.CodeAsOriginal: return "生成代码(完全按源文件名)";
                    case GlobalDataForResMaster.ABForAutoCodeGen.CodeAsLower: return "生成代码(源文件名转小写)";
                    case GlobalDataForResMaster.ABForAutoCodeGen.CodeAsUpper: return "生成代码(源文件名转大写)";
                    default: return "不生成代码";
                }
            }

            //↑生成代码Inline Button
            private void Handle_CodeGenAssetConstName()
            {
                StringBuilder theContent = new StringBuilder();
                //这个只是PreName的AB包
                var allAssetBundles = AssetDatabase.GetAllAssetBundleNames();
                string AllABDicContent = "";
                string AllHashDicContent = "";

                // 加载总AssetBundle（假设位于StreamingAssets）
                #region 真实AB包
                AssetBundle.UnloadAllAssetBundles(unloadAllObjects: false);
                AssetBundle mainBundle = AssetBundle.LoadFromFile(Path.Combine(genarateFolder, PlatformToApply.ToString(), PlatformToApply.ToString()));
                AssetBundleManifest manifest = mainBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                string[] AllABWithHash = manifest.GetAllAssetBundles();

                #endregion

                foreach (var ab in allAssetBundleNames)
                {

                    string SingleABField = "";
                    string showAB = ab;
                    foreach (var withHash in AllABWithHash)
                    {
                        string pre = GlobalDataForResMaster.Instance.GetPreNameFromCompleteNameWithHash(withHash);
                        //Debug.Log(pre + "*" + ab + "&" + (pre == ab) + "/" + withHash + "/" + GlobalDataForResMaster.Instance.GetHashFromCompleteNameWithHash(withHash));
                        if (pre == ab)
                        {
                            AllHashDicContent += "{\"" + ab + "\",\"" + withHash + "\"},\n";
                            SingleABField += ESStaticDesignUtility.SimpleScriptMaker.CreateFieldContent
                      ("string", "PreName", "public static", "=" + $"\"{ab}\"");

                            SingleABField += ESStaticDesignUtility.SimpleScriptMaker.CreateFieldContent
                               ("string", "WithHash", "public static", "=" + $"\"{withHash}\"");

                            SingleABField += ESStaticDesignUtility.SimpleScriptMaker.CreateFieldContent
                              ("string", "Hash", "public static", "=" + $"\"{GlobalDataForResMaster.Instance.GetHashFromCompleteNameWithHash(withHash)}\"");
                        }
                    }
                    //包名锁死为小写
                    /*
                     * if (codeType == GlobalDataForResMaster.ABForAutoCodeGen.CodeAsUpper) showAB = showAB.ToUpper();
                    else if (codeType == GlobalDataForResMaster.ABForAutoCodeGen.CodeAsLower) showAB = showAB.ToLower();*/

                    AllABDicContent += "{\"" + showAB + "\"," + showAB + ".AllPaths},\n";



                    List<string> allPathsNoRepeatOriginal = new List<string>(allAssetBundleNames.Length);
                    List<string> allPathsNoRepeatToUse = new List<string>(allAssetBundleNames.Length);
                    List<string> allValueNoRepeat = new List<string>(allAssetBundleNames.Length);
                    string[] allpath_ = AssetDatabase.GetAssetPathsFromAssetBundle(ab);


                    foreach (var field in allpath_)
                    {
                        //为空    不存在   是文件夹  都忽略
                        if (string.IsNullOrEmpty(field) || (!File.Exists(field) || AssetDatabase.IsValidFolder(field))) continue;
                        DirectoryInfo dir = new DirectoryInfo(field);
                        string origin = dir.Name.Replace('.', '_');
                        string tryUse = origin.ToString();
                        int repeat = 1;
                        while (allPathsNoRepeatOriginal.Contains(tryUse))
                        {
                            tryUse = origin + "_" + repeat;
                            repeat++;
                        }
                        string key = $"{ESStaticDesignUtility.SimpleScriptMaker.HandleString_RemoveExtension(ESStaticDesignUtility.SimpleScriptMaker.HandleString_ToValidName(tryUse))}";
                        if (codeType == GlobalDataForResMaster.ABForAutoCodeGen.CodeAsUpper) key = key.ToUpper();
                        else if (codeType == GlobalDataForResMaster.ABForAutoCodeGen.CodeAsLower) key = key.ToLower();
                        string value = $"\"{ESStaticDesignUtility.SimpleScriptMaker.HandleString_RemoveExtension(dir.Name)}\"";
                        allPathsNoRepeatOriginal.Add(tryUse);
                        allPathsNoRepeatToUse.Add(key);
                        allValueNoRepeat.Add(value);



                        string aField = ESStaticDesignUtility.SimpleScriptMaker.CreateFieldContent
                            ("string", key,
                            modifier: "public static", valueDefine: $"={value}");
                        SingleABField += aField;
                    }
                    string AllPathsKeyValueContent = "";
                    for (int i = 0; i < allPathsNoRepeatToUse.Count; i++)
                    {
                        AllPathsKeyValueContent += "{\"" + allPathsNoRepeatToUse[i] + "\" ," + allValueNoRepeat[i] + "},\n";
                        if (i % 4 == 0)
                        {
                            AllPathsKeyValueContent += '\n';
                        }
                    }
                    string AllPathsListString = ESStaticDesignUtility.SimpleScriptMaker.CreateFieldContent
                    ("Dictionary<string, string> ", "AllPaths",
                            modifier: "public static", valueDefine: $"\n=new Dictionary<string, string> {{ {AllPathsKeyValueContent} }}");


                    string ABClass = ESStaticDesignUtility.SimpleScriptMaker.CreateClassContentByString(showAB, "static", insideClass: SingleABField + AllPathsListString, parent: "");
                    theContent.Append(ABClass);
                }

                string hashTest = ESStaticDesignUtility.SimpleScriptMaker.CreateNotes(AllHashDicContent);

                string ABAssetDic = ESStaticDesignUtility.SimpleScriptMaker.CreateFieldContent("Dictionary<string, Dictionary<string,string>>",
                    "AllPathsDic", "public static", "\n=new Dictionary<string, Dictionary<string, string>>()\n{" + AllABDicContent + "}");

                string ABHashDic = ESStaticDesignUtility.SimpleScriptMaker.CreateFieldContent("Dictionary<string,string>",
                    "AllABHashDic", "public static", "\n=new Dictionary<string, string>()\n{" + AllHashDicContent + "}");


                ESStaticDesignUtility.SimpleScriptMaker.CreateScriptNormal
                    (genarateCodeFolder, "ESAssetBundlePath", "static partial",
                    insideClass: hashTest + ABAssetDic + ABHashDic + theContent.ToString(), parent: "", using_: "using System.Collections.Generic;");
            }

            //生成AB包路径
            [LabelText("生成<AB包>路径"), TabGroup("全局设置"), PropertyOrder(-2)]
            [FolderPath, ShowInInspector]
            public string genarateFolder { get => GlobalDataForResMaster.Instance.genarateFolder; set { GlobalDataForResMaster.Instance.genarateFolder = value; EditorUtility.SetDirty(GlobalDataForResMaster.Instance); } }

            //生成代码路径
            [LabelText("生成<代码>路径"), TabGroup("全局设置"), PropertyOrder(-2)]
            [FolderPath, ShowInInspector]
            public string genarateCodeFolder { get => GlobalDataForResMaster.Instance.genarateCodeFolder_; set { GlobalDataForResMaster.Instance.genarateCodeFolder_ = value; EditorUtility.SetDirty(GlobalDataForResMaster.Instance); } }

            //应用平台
            [LabelText("应用平台"), TabGroup("全局设置"), ShowInInspector]
            public RuntimePlatform PlatformToApply
            {
                get => GlobalDataForResMaster.Instance.applyPlatform; set
                {
                    GlobalDataForResMaster.Instance.applyPlatform = value;
                    EditorUtility.SetDirty(GlobalDataForResMaster.Instance);
                }
            }


            //生成实际逻辑
            private void BuildAB(string parent, string theFolder, BuildAssetBundleOptions assetBundleOptions, BuildTarget targetPlatform)
            {
                string path = parent + "/" + theFolder;
                if (!AssetDatabase.IsValidFolder(path))
                {
                    AssetDatabase.CreateFolder(parent, theFolder);
                }
                BuildPipeline.BuildAssetBundles(path, assetBundleOptions, targetPlatform);
            }

            #endregion


            #region 标记收集和查询


            //按钮-刷新显示AB标记
            [Button("刷新显示AB标记"), PropertySpace(15)]
            [TabGroup("标记收集与查询", TextColor = "@ESStaticDesignUtility.ColorSelector.ColorForCatcher"), PropertyOrder(-4), GUIColor("@ESStaticDesignUtility.ColorSelector.ColorForApply")]
            public void RefreshBundleList()
            {
                AllPath = new List<string>();
                allAssetBundleNames = AssetDatabase.GetAllAssetBundleNames();
                foreach (var i in allAssetBundleNames)
                {
                    string[] path_ = AssetDatabase.GetAssetPathsFromAssetBundle(i);
                    AllPath.AddRange(path_);
                }
            }

            //按钮-清除AB标记
            [Button("清除AB标记(需要额外确认)"), PropertySpace(15)]
            [TabGroup("标记收集与查询"), PropertyOrder(-1), GUIColor("@ESStaticDesignUtility.ColorSelector.ColorForCaster")]
            public void ClearBundleList()
            {
                bool b = EditorUtility.DisplayDialog("是否清空AB包标记", "这会移除全部的AB包标记，意味着要重新标记", "我确定", "放弃");
                if (b)
                {

                    string[] assetPaths = AssetDatabase.GetAllAssetPaths();
                    Debug.Log("" + assetPaths);
                    foreach (string path in assetPaths)
                    {
                        // 跳过文件夹和未导入资产
                        if (!AssetDatabase.IsMainAssetAtPathLoaded(path) || path.EndsWith(".cs"))
                            continue;
                        // 获取资产导入器
                        AssetImporter importer = AssetImporter.GetAtPath(path);
                        if (importer == null)
                            continue;
                        // 清除标记
                        importer.assetBundleName = "XXXX";
                        importer.assetBundleName = string.Empty; // 清除名称
                        importer.assetBundleVariant ??= string.Empty;
                        AssetDatabase.Refresh();
                        AssetDatabase.SaveAssets();

                        /*importer.assetBundleVariant=(string.Empty); // 清除变体后缀*/
                        allAssetBundleNames = AssetDatabase.GetAllAssetBundleNames();
                        foreach (var i in allAssetBundleNames)
                        {
                            AssetDatabase.RemoveAssetBundleName(i, forceRemove: true);
                        }
                        AssetDatabase.RemoveUnusedAssetBundleNames();
                    }
                }
                else
                {

                }
                RefreshBundleList();

            }


            //缓存AB包名
            private string[] allAssetBundleNames;

            //警告信息
            [TabGroup("标记收集与查询")]
            [ShowIf("warnIfChineseOrSymbol"), ShowInInspector, HideLabel, DisplayAsString(fontSize: 22, Alignment = TextAlignment.Center, EnableRichText = true), GUIColor("@ESStaticDesignUtility.ColorSelector.Color_03")]
            private string warn_ = "！不要使用包含<b><i>中文或者违规符号</i></b> ,  \"_\"可用 包名！！！";

            //警告信息
            [TabGroup("标记收集与查询")]
            [ShowIf("warnIfPoint"), ShowInInspector, HideLabel, DisplayAsString(fontSize: 18, Alignment = TextAlignment.Center, EnableRichText = true), GUIColor("@ESStaticDesignUtility.ColorSelector.Color_02")]
            private string warn2 = "！包名所有的<b><i>\".\"</i></b> 会被替换为 <b><i>\"_\"</i></b> ！";

            //选中查询AB包名
            [LabelText("", Text = "@showAssetBundleName()"), OnValueChanged("PingABAsset")]
            [TabGroup("标记收集与查询"), PropertySpace(15)]
            [ValueDropdown("allAssetBundleNames", AppendNextDrawer = true), ShowInInspector, GUIColor("@ESStaticDesignUtility.ColorSelector.ColorForCaster")]
            public string selectBundleName
            {
                get => GlobalDataForResMaster.Instance.ABName; set
                {
                    GlobalDataForResMaster.Instance.ABName = value;
                    EditorUtility.SetDirty(GlobalDataForResMaster.Instance);
                }
            }

            //标记模式
            [LabelText("标记模式"), ShowInInspector]
            [TabGroup("标记收集与查询")]
            [InfoBox("", Message = "@desMaskType()"), GUIColor("@ESStaticDesignUtility.ColorSelector.ColorForCatcher")]
            public GlobalDataForResMaster.ABMaskType maskType { get => GlobalDataForResMaster.Instance.abMaskType; set { GlobalDataForResMaster.Instance.abMaskType = value; EditorUtility.SetDirty(GlobalDataForResMaster.Instance); } }

            [LabelText("该AB包名对应的资源")]
            [TabGroup("标记收集与查询")]
            [ListDrawerSettings(DefaultExpandedState = true/*, OnEndListElementGUI = "OnEndDrawItemTypes"*/), ReadOnly, ShowInInspector]
            public string[] ShowAssets => AssetDatabase.GetAssetPathsFromAssetBundle(selectBundleName);

            /*#region 查询类型列表扩展


            private void OnEndDrawItemTypes(int index)
            {
                if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
                {
                    // 检测鼠标是否在当前项范围内
                    Rect itemRect = GUILayoutUtility.GetLastRect();
                    if (itemRect.Contains(Event.current.mousePosition))
                    {
                        ShowContextMenuTypes(index); // 显示右键菜单
                        Event.current.Use();    // 阻止事件冒泡
                    }
                }
            }

            private void ShowContextMenuTypes(int index)
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("查询位置"), false, () => { 
                    string path = ShowAssets[index];
                    EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(path,typeof(UnityEngine.Object)));
                });
                menu.AddItem(new GUIContent("从AB包移除"), false, () => {
                    string path = ShowAssets[index];
                    AssetImporter importer = AssetImporter.GetAtPath(path);
                    if (importer == null) return;
                    // 清除标记
                    importer.assetBundleName = "XXXX";
                    importer.assetBundleName = string.Empty; // 清除名称
                    importer.assetBundleVariant ??= string.Empty;
                    AssetDatabase.Refresh();
                    AssetDatabase.SaveAssets();
                    RefreshBundleList();
                });
                menu.ShowAsContext();
            }
            #endregion*/

            //补充描述信息--标记模式
            private string desMaskType()
            {
                if (maskType == GlobalDataForResMaster.ABMaskType.AsOrinal) return "!作为资源时，会标记自身为单独的AB包，作为文件夹时，标记该文件夹下所有资源为同一个AB包";
                else if (maskType == GlobalDataForResMaster.ABMaskType.AsFolder) return "作为资源时，会收纳到自身所在文件夹，作为文件夹时，标记该文件夹下所有资源为同一个AB包";
                else /*if (maskType == GlobalDataForResMaster.ABMaskType.SelfDefine)*/
                    return "自定义模式，使用<上面输入或者选择的>作为标记收集包名";
            }

            //补充描述信息--显示AssetBundle名
            private string showAssetBundleName()
            {
                if (maskType == GlobalDataForResMaster.ABMaskType.SelfDefine) return "查询/标记收集到自定义包名";

                return "查询的包名";
            }
            //补充描述信息--中文警告
            private bool warnIfChineseOrSymbol()
            {
                return selectBundleName._ContainChineseCharacterOrNormalSymbol();
            }
            //补充描述信息--点警告
            private bool warnIfPoint()
            {
                return selectBundleName.Contains('.');
            }
            //标出AB资源
            private void PingABAsset()
            {
                if (ShowAssets.Length > 0)
                {
                    EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(ShowAssets[0], typeof(UnityEngine.Object)));
                }
            }
            //末尾放置全部信息
            [TabGroup("标记收集与查询"), LabelText("全部的资源路径"), GUIColor("@ESStaticDesignUtility.ColorSelector.ColorForDes")]
            [PropertyOrder(1), ReadOnly, ListDrawerSettings(DefaultExpandedState = false)]
            public List<string> AllPath = new List<string>();

            /* #region 查询类型列表扩展


             private void OnEndDrawItemAll(int index)
             {
                 if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                 {
                     // 检测鼠标是否在当前项范围内
                     Rect itemRect = GUILayoutUtility.GetLastRect();
                     if (itemRect.Contains(Event.current.mousePosition))
                     {
                         ShowContextMenuAll(index); // 显示右键菜单
                         Event.current.Use();    // 阻止事件冒泡
                     }
                 }
             }

             private void ShowContextMenuAll(int index)
             {
                 GenericMenu menu = new GenericMenu();
                 menu.AddItem(new GUIContent("查询位置"), false, () => {
                     string path = AllPath[index];
                     EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object)));
                 });
                 menu.AddItem(new GUIContent("从AB包移除"), false, () => {
                     string path = AllPath[index];
                     AssetImporter importer = AssetImporter.GetAtPath(path);
                     if (importer == null) return;
                     // 清除标记
                     importer.assetBundleName = "XXXX";
                     importer.assetBundleName = string.Empty; // 清除名称
                     importer.assetBundleVariant ??= string.Empty;
                     AssetDatabase.Refresh();
                     AssetDatabase.SaveAssets();
                     RefreshBundleList();
                 });
                 menu.ShowAsContext();
             }
             #endregion*/

            #endregion

            #region 热更新
            [TabGroup("热更新配置", TextColor = "@ESStaticDesignUtility.ColorSelector.Color_03"), PropertyOrder(-4), GUIColor("@ESStaticDesignUtility.ColorSelector.ColorForApply")]
            [Button("创建热更新数据"), PropertySpace(15)]
            private void Handle_CreateHotUpdateData()
            {
                if (AssetDatabase.IsValidFolder(genarateFolder))
                {
                    EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(genarateFolder, typeof(UnityEngine.Object)));

                    ESEditorHandle.AddSimpleHanldeTask(
                        () =>
                        {
                            if (EditorUtility.DisplayDialog("开始生成热更新文件", "准备生成热更新文件，这将会覆盖原来的资源,可能产生新的依赖和包名，请确定路径和平台正确", "知道了", "取消"))
                            {

                            }
                            else
                            {
                                return;
                            }
                            if (PlatformToApply == RuntimePlatform.WindowsPlayer || PlatformToApply == RuntimePlatform.WindowsEditor)
                            {
                                CreateHotUpdateFiles(RuntimePlatform.WindowsPlayer);
                                CreateHotUpdateFiles(RuntimePlatform.WindowsEditor);
                            }
                            else if (PlatformToApply == RuntimePlatform.Android)
                            {
                                CreateHotUpdateFiles(RuntimePlatform.Android);
                            }
                            else if (PlatformToApply == RuntimePlatform.IPhonePlayer)
                            {
                                CreateHotUpdateFiles(RuntimePlatform.IPhonePlayer);
                            }
                        }, 20);
                }
                else
                {
                    EditorUtility.DisplayDialog("无效的文件夹路径", "这个文件夹路径是不合法的，请确定正确的文件夹再重试", "知道了");
                }

            }
            private void CreateHotUpdateFiles(RuntimePlatform platform)
            {
                AssetBundle.UnloadAllAssetBundles(unloadAllObjects: false);
                AssetBundle mainBundle = AssetBundle.LoadFromFile(Path.Combine(genarateFolder, platform.ToString(), platform.ToString()));
                AssetBundleManifest manifest = mainBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                string[] AllABWithHash = manifest.GetAllAssetBundles();
                // 使用 Odin 序列化后的字典可直接转为 JSON
                SerializedDictionary<string, string> dependenceDic = new SerializedDictionary<string, string>();
                SerializedDictionary<string, string> preToHashDic = new SerializedDictionary<string, string>();
                foreach (var ab in allAssetBundleNames)
                {

                    string showAB = ab;
                    foreach (var withHash in AllABWithHash)
                    {
                        string pre = GlobalDataForResMaster.Instance.GetPreNameFromCompleteNameWithHash(withHash);
                        if (pre == ab)
                        {
                            preToHashDic.Add(ab, withHash);
                            string[] abDepend = manifest.GetAllDependencies(withHash);
                            string abDependPreLink = "";
                            bool first = true;
                            foreach (var i in abDepend)
                            {
                                if (!first) abDependPreLink += '&';
                                abDependPreLink += GlobalDataForResMaster.Instance.GetPreNameFromCompleteNameWithHash(i);
                                first = false;
                            }
                            if (abDepend.Length > 0) dependenceDic.Add(ab, abDependPreLink);
                        }
                    }

                }
                string json = JsonUtility.ToJson(dependenceDic);
                string dependPath = GlobalDataForResMaster.Instance.GetLocalAssetBundlePath(platform.ToString()) + "/" + GlobalDataForResMaster.Instance.HotUpdateDependenceFileName;
                File.WriteAllText(dependPath, json);
                string json2 = JsonUtility.ToJson(preToHashDic);

                string hashPath = GlobalDataForResMaster.Instance.GetLocalAssetBundlePath(platform.ToString()) + "/" + GlobalDataForResMaster.Instance.HotUpdatePreToHashFileName;
                File.WriteAllText(hashPath, json2);
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
                EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(dependPath, typeof(UnityEngine.Object)));
            }


            //清空AB包 按钮
            [TabGroup("热更新配置"), PropertyOrder(-4), GUIColor("@ESStaticDesignUtility.ColorSelector.ColorForCaster")]
            [Button("刷新热更新信息", DrawResult = false), PropertySpace(15)]
            private void Handle_RefresgHotUpdateData()
            {
                if (AssetDatabase.IsValidFolder(genarateFolder))
                {
                    try
                    {
                        string forDepend = GlobalDataForResMaster.Instance.GetLocalAssetBundlePath() + "/" + GlobalDataForResMaster.Instance.HotUpdateDependenceFileName;
                        string forHash = GlobalDataForResMaster.Instance.GetLocalAssetBundlePath() + "/" + GlobalDataForResMaster.Instance.HotUpdatePreToHashFileName;
                        string jsonContentDepend = File.ReadAllText(forDepend);
                        string jsonContentHash = File.ReadAllText(forHash);


                        SerializedDictionary<string, string> handleAgo = JsonUtility.FromJson<SerializedDictionary<string, string>>(jsonContentDepend);
                        Undo.RecordObject(GlobalDataForResMaster.Instance, "set1");
                        GlobalDataForResMaster.Instance.Depend = new Dictionary<string, WrapListString>() { };
                        foreach (var (i, k) in handleAgo)
                        {
                            Depend.Add(i, new WrapListString() { strings = k.Split('&').ToList() });
                        }
                        EditorUtility.SetDirty(GlobalDataForResMaster.Instance);

                        Undo.RecordObject(GlobalDataForResMaster.Instance, "set");
                        SerializedDictionary<string, string> handleAgo2 = JsonUtility.FromJson<SerializedDictionary<string, string>>(jsonContentHash);
                        GlobalDataForResMaster.Instance.toHash = new Dictionary<string, string>();
                        foreach (var (i, k) in handleAgo2)
                        {
                            GlobalDataForResMaster.Instance.toHash.Add(i, k);
                        }

                        EditorUtility.SetDirty(GlobalDataForResMaster.Instance);


                        Debug.Log("refresh");
                        foreach (var (i, k) in toHash)
                        {
                            bool hasIt = false;
                            foreach (var target in TargetLocations)
                            {
                                if (target.ABPreName == i)
                                {
                                    hasIt = true;
                                    //
                                }
                            }
                            if (!hasIt)
                            {
                                TargetLocations.Add(new ABTargetLocation() { ABPreName = i, ABTarget_ = ABTargetLocation.ABTarget.Net });
                            }
                        }
                        //刷新设置列表
                        // TargetLocations


                    }
                    catch (Exception e)
                    {
                        Debug.Log("无文件"+e);
                    }
                }
                #endregion
            }

            [TabGroup("热更新配置"), PropertyOrder(-4), GUIColor("@ESStaticDesignUtility.ColorSelector.ColorForCaster")]
            [Button("打开下载目标文件夹(persistent)", DrawResult = false), PropertySpace(15)]
            private void Handle_OpenDownloadHotUpdateDir()
            {
                string path = Application.persistentDataPath + "/" + GlobalDataForResMaster.Instance.LocalDownLoadAdditionPath;

                if (Directory.Exists(path) || Directory.CreateDirectory(path) != null)
                {
                    // 在默认文本编辑器中打开文件夹
                    EditorUtility.OpenWithDefaultApp(path);
                }
            }

            /*  [Button("Test2")]
              private void ThisRefresh()
              {
                  GlobalDataForResMaster.Instance.strings = new SerializedDictionary<string, string>();
              }*/
            [LabelText("下载到本地相对附加路径"), ShowInInspector, PropertyOrder(-3)]
            public string DownloadAddition { get => GlobalDataForResMaster.Instance.LocalDownLoadAdditionPath; set { GlobalDataForResMaster.Instance.LocalDownLoadAdditionPath = value; EditorUtility.SetDirty(GlobalDataForResMaster.Instance); } }


            [LabelText("下载测试(仅测试)"), InlineButton("tryDownLoad", "下载测试"), ShowInInspector, PropertyOrder(-3)]
            public string DownloadURL { get => GlobalDataForResMaster.Instance.DownLoadURL; set { GlobalDataForResMaster.Instance.DownLoadURL = value; EditorUtility.SetDirty(GlobalDataForResMaster.Instance); } }
            private void tryDownLoad()
            {
                TryLoad = !TryLoad;
            }
            private static bool TryLoad = false;
            [InfoBox("不需要写平台路径！,可以直接写文件名(包括后缀)"), ShowIf("@TryLoad"), InlineButton("DownLoadIt", "下载"), PropertyOrder(-2)]
            [LabelText("-》文件名(含后缀)")]
            public string FileName = "";
            [LabelText("下载资源测试本地地址"), ShowIf("@TryLoad"), ShowInInspector, PropertyOrder(-2), FolderPath]
            public string DownloadLocalPath { get => GlobalDataForResMaster.Instance.LocalTestDownLoadPath; set { GlobalDataForResMaster.Instance.LocalTestDownLoadPath = value; EditorUtility.SetDirty(GlobalDataForResMaster.Instance); } }
            private float progress = 0;
            private void DownLoadIt()
            {
                TryLoad = true;
                string netpath = DownloadURL + "/" + PlatformToApply + "/" + FileName;

                var unityWebRequest = UnityWebRequest.Get(netpath);

                unityWebRequest.downloadHandler = new DownloadHandlerFile(DownloadLocalPath + "/" + FileName);
                unityWebRequest.SendWebRequest();
                Debug.Log("发送请求");
                ESEditorHandle.AddRunningHanldeTask(
                    () =>
                    {
                        Debug.Log("action");
                        progress = Mathf.Max(unityWebRequest.downloadProgress, 1);
                    },
                    () =>
                    {
                        if (!unityWebRequest.isDone)
                        {
                            Debug.Log("progress");
                            EditorUtility.DisplayCancelableProgressBar
                            ("测试下载资源", "测试下载：" + FileName + ",进度：" + progress * 100 + "%", progress);

                            return false;
                        }
                        else if (unityWebRequest.result == UnityWebRequest.Result.Success)
                        {
                            AssetDatabase.Refresh();
                            AssetDatabase.SaveAssets();
                            EditorUtility.ClearProgressBar();
                            EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(DownloadLocalPath + "/" + FileName, typeof(UnityEngine.Object)));
                            ESEditorHandle.AddSimpleHanldeTask(() =>
                            {
                                EditorUtility.DisplayDialog("下载完成!", "测试下载：" + netpath + "\n下载到：" + DownloadLocalPath + "成功！", "好的");
                            });
                            Debug.Log("成功");
                            /*                            unityWebRequest.downloadHandler.Dispose();
                                                        unityWebRequest.Dispose();*/
                            return true;
                        }
                        else
                        {
                            AssetDatabase.Refresh();
                            AssetDatabase.SaveAssets();
                            EditorUtility.ClearProgressBar();
                            ESEditorHandle.AddSimpleHanldeTask(() =>
                            {
                                EditorUtility.DisplayDialog("下载失败!", "测试下载：" + netpath + "\n下载到：" + DownloadLocalPath + "失败！", "好的");
                            });
                            Debug.Log("失败");
                            /*                            unityWebRequest.downloadHandler.Dispose();
                                                        unityWebRequest.Dispose();*/
                            return true;
                        }
                    });



            }

            [LabelText("Hash信息字典"), ShowInInspector]
            public Dictionary<string, string> toHash { get => GlobalDataForResMaster.Instance.toHash; }
            [LabelText("依赖信息字典"), ShowInInspector]
            public Dictionary<string, WrapListString> Depend { get => GlobalDataForResMaster.Instance.Depend; }


            [LabelText("AB优先加载目标"), ShowInInspector, HideReferenceObjectPicker, InlineButton("apply_ABTarget", "应用")]
            public List<ABTargetLocation> TargetLocations = new List<ABTargetLocation>();

            private void apply_ABTarget()
            {
                GlobalDataForResMaster.Instance.TargetLocations = new List<ABTargetLocation>();
                foreach (var i in TargetLocations)
                {
                    GlobalDataForResMaster.Instance.TargetLocations.Add(i);
                }
                EditorUtility.SetDirty(GlobalDataForResMaster.Instance);
            }
            private void apply_FromTarget()
            {
                TargetLocations = new List<ABTargetLocation>();
                foreach (var i in GlobalDataForResMaster.Instance.TargetLocations)
                {
                    TargetLocations.Add(i);
                }
                EditorUtility.SetDirty(GlobalDataForResMaster.Instance);
            }
            #endregion

            #region 值



            #endregion
        }
    }

}