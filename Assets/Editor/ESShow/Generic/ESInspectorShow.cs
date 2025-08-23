using ES;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using Sirenix.Utilities.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace ES
{

    [InitializeOnLoad]
    public class ESInspectorShow
    {

        // 创建全局Inspector处理器 

        #region 初始化监听
        static ESInspectorShow()
        {
            Init_SelectFoldState();
            // 监听Inspector绘制前的回调
            Editor.finishedDefaultHeaderGUI += DrawCustomHeader;
            Selection.selectionChanged += MakeSelectionDirty;

        }
        #endregion

        #region 初始化Fold状态*(懒得写判空)
        private static void Init_SelectFoldState()
        {
            SelectFoldState_Compos = new Dictionary<InpectorCompoTypeSelect, bool>();
            SelectFoldState_Compos.Add(InpectorCompoTypeSelect.UnityCore, false);
            SelectFoldState_Compos.Add(InpectorCompoTypeSelect.Support, false);
            SelectFoldState_Compos.Add(InpectorCompoTypeSelect.NotDefine, false);
            SelectFoldState_Compos.Add(InpectorCompoTypeSelect.ES, false);

            SelectFoldState_Assets = new Dictionary<InpectorAssetTypeSelect, bool>();
            SelectFoldState_Assets.Add(InpectorAssetTypeSelect.Folder, true);
            SelectFoldState_Assets.Add(InpectorAssetTypeSelect.Script, true);
            SelectFoldState_Assets.Add(InpectorAssetTypeSelect.Prefab, true);
            SelectFoldState_Assets.Add(InpectorAssetTypeSelect.Material, true);
            SelectFoldState_Assets.Add(InpectorAssetTypeSelect.Image, true);
            SelectFoldState_Assets.Add(InpectorAssetTypeSelect.Anim, true);
            SelectFoldState_Assets.Add(InpectorAssetTypeSelect.None, true);

            AssetGUIContent = new Dictionary<InpectorAssetTypeSelect, GUIContent>();
            AssetGUIContent.Add(InpectorAssetTypeSelect.Folder, new GUIContent(InpectorAssetTypeSelect.Folder._Get_ATT_ESStringMessage(), EditorIcons.Folder.Highlighted));
            AssetGUIContent.Add(InpectorAssetTypeSelect.Script, new GUIContent(InpectorAssetTypeSelect.Script._Get_ATT_ESStringMessage(), EditorGUIUtility.IconContent("cs Script Icon").image));
            AssetGUIContent.Add(InpectorAssetTypeSelect.Prefab, new GUIContent(InpectorAssetTypeSelect.Prefab._Get_ATT_ESStringMessage(), EditorGUIUtility.IconContent("Prefab Icon").image));
            AssetGUIContent.Add(InpectorAssetTypeSelect.Material, new GUIContent(InpectorAssetTypeSelect.Material._Get_ATT_ESStringMessage(), EditorGUIUtility.IconContent("Material Icon").image)); 
            AssetGUIContent.Add(InpectorAssetTypeSelect.Image, new GUIContent(InpectorAssetTypeSelect.Image._Get_ATT_ESStringMessage(), EditorIcons.Image.Highlighted));
            AssetGUIContent.Add(InpectorAssetTypeSelect.Anim, new GUIContent(InpectorAssetTypeSelect.Anim._Get_ATT_ESStringMessage(), EditorGUIUtility.IconContent("Animation Icon").image));
            AssetGUIContent.Add(InpectorAssetTypeSelect.None, new GUIContent(InpectorAssetTypeSelect.None._Get_ATT_ESStringMessage(), EditorIcons.UnityWarningIcon));
        }
        #endregion

        #region 选开部分声明变量
        static bool dirty = true;//首次必定加载
        private static void MakeSelectionDirty()
        {
            dirty = true;
        }
        #endregion

        #region 分组部分声明变量
        private static bool showAllFold_Compos = false;
        public static bool IsCacheingDirty_Compos = true;
        public static List<Component> CachingCompos = new List<Component>();
        public static Dictionary<InpectorCompoTypeSelect, bool> SelectFoldState_Compos = new Dictionary<InpectorCompoTypeSelect, bool>();
        public static SelectDic<InpectorCompoTypeSelect, Component, EditorAndIsShowing> LastEditors_Compos = new SelectDic<InpectorCompoTypeSelect, Component, EditorAndIsShowing>();
        public static SelectDic<InpectorCompoTypeSelect, Component, EditorAndIsShowing> CacheingEditors_Compos = new SelectDic<InpectorCompoTypeSelect, Component, EditorAndIsShowing>();
        #endregion

        #region 文件夹内容绘制声明
        public static UnityEngine.Object lastSelectAsset;
        public static string pathAssetSelecting;
        public static bool IsCacheingDirty_FolderAssets = true;
        public static Dictionary<InpectorAssetTypeSelect, bool> SelectFoldState_Assets = new Dictionary<InpectorAssetTypeSelect, bool>();
        public static KeyGroup<InpectorAssetTypeSelect, string/*path*/> CachingAssets = new KeyGroup<InpectorAssetTypeSelect, string>();

        public static Dictionary<InpectorAssetTypeSelect, GUIContent> AssetGUIContent = new Dictionary<InpectorAssetTypeSelect, GUIContent>();
        public static Dictionary<InpectorAssetTypeSelect, Color> AssetColor = new Dictionary<InpectorAssetTypeSelect, Color>();

        #endregion

        // 在Inspector顶部绘制自定义内容
        private static void DrawCustomHeader(Editor editor)
        {
            #region 绘制检查器总控--目标为单个游戏对象时
            if (!GlobalDataForEditorOnly.Instance?.Ins.EnableCompoentShowControl_ ?? false) return;
            var cache = GlobalDataForEditorOnly.Instance.Ins?.cacheToggleFalseNames;
            if (cache == null) return;
            try
            {
                if (editor.target is GameObject go)
                {
                    var cs = go.GetComponents<Component>();
                    if (editor.targets.Length == 1)
                    {
                        #region 绘制主控和选显示
                        {
                            // 1. 绘制标题
                            EditorGUILayout.LabelField("ES控制面板", EditorStyles.boldLabel);

                            // 2. 绘制按钮
                            if (GUILayout.Button("重置位置(测试)"))
                            {
                                Undo.RecordObject(go.transform, "Reset Position");
                                go.transform.position = Vector3.zero;
                            }
                            if (GUILayout.Button("刷新显示"))
                            {
                                EditorUtility.SetDirty(go);
                                Selection.activeGameObject = null;
                                ESEditorHandle.AddSimpleHanldeTask(() => { Selection.activeGameObject = go; Debug.Log(886); });
                            }
                            //绘制脚本列



                            foreach (var i in cs)
                            {
                                if (i == null) continue;
                                string forType = i.GetType().Name;
                                string dis = GlobalDataForEditorOnly.Instance.TypeDis.GetNewName(forType);
                                bool NowShow = !cache.Contains(forType);
                                bool newNowShow = EditorGUILayout.Toggle(dis, NowShow);

                                if (NowShow && !newNowShow)
                                {
                                    cache.Add(forType);
                                    dirty = true;
                                }
                                else if (!NowShow && newNowShow)
                                {
                                    cache.Remove(forType);
                                    dirty = true;
                                }
                                if (dirty)
                                {
                                    if (newNowShow) i.hideFlags &= ~HideFlags.HideInInspector;
                                    else i.hideFlags |= HideFlags.HideInInspector;
                                    EditorUtility.SetDirty(go);

                                }

                            }
                            dirty = false;
                            // 3. 绘制分隔线
                            EditorGUILayout.Space();
                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                            EditorGUILayout.Space();
                        }
                        #endregion

                        #region 绘制脚本收集
                        if (go == null/*关闭*/)
                        {//尝试绘制收集
                            SirenixEditorGUI.BeginBox();
                            SirenixEditorGUI.BeginBoxHeader();
                            showAllFold_Compos = SirenixEditorGUI.Foldout(showAllFold_Compos, "脚本收集");
                            SirenixEditorGUI.EndBoxHeader();
                            if (IsCacheingDirty_Compos)
                            {
                                IsCacheingDirty_Compos = false;
                                LastEditors_Compos = CacheingEditors_Compos;
                                CacheingEditors_Compos = new SelectDic<InpectorCompoTypeSelect, Component, EditorAndIsShowing>();
                                foreach (var i in cs)
                                {
                                    var select = GetSelectType(i);
                                    CacheingEditors_Compos.TryAddOrSet(select, i, GetOrCreateEditor(select, i));
                                }
                            }
                            if (showAllFold_Compos)
                            {
                                CheckAndDrawSelectTypeCompo(InpectorCompoTypeSelect.UnityCore);
                                CheckAndDrawSelectTypeCompo(InpectorCompoTypeSelect.ES);
                                CheckAndDrawSelectTypeCompo(InpectorCompoTypeSelect.Support);
                                CheckAndDrawSelectTypeCompo(InpectorCompoTypeSelect.NotDefine);

                                var csList = cs.ToList();
                                if (!csList.SequenceEqual(CachingCompos))
                                {
                                    CachingCompos = csList;
                                    IsCacheingDirty_Compos = true;
                                }

                            }
                            SirenixEditorGUI.EndBox();
                            #endregion
                        }
                    }
                }

                else //文件夹
                {

                    var path = KeyValueMatchingUtility.SafeEditor.GetAssetPath(editor.target);
                    if (AssetDatabase.IsValidFolder(path))
                    {
                        #region 刷新
                        if (lastSelectAsset != editor.target)
                        {
                            lastSelectAsset = editor.target;
                            pathAssetSelecting = path;
                            //刷新了
                            CachingAssets.Clear();
                            var allAssetPath= Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Where(file => !file.EndsWith(".meta")).ToList();
                            allAssetPath.AddRange(Directory.GetDirectories(path, "*.*", SearchOption.AllDirectories).ToList());
                           
                            allAssetPath.Sort();
                            ;
                            for (int i = 0; i < allAssetPath.Count; i++)
                            {
                                CheckAndAddAssetInKeyGroup(allAssetPath[i]);
                            }
                        }
                        #endregion

                        EditorGUILayout.LabelField("ES文件夹详情", EditorStyles.boldLabel);
                        CheckAndDrawSelectTypeAsset(InpectorAssetTypeSelect.Folder);
                        CheckAndDrawSelectTypeAsset(InpectorAssetTypeSelect.Script);
                        CheckAndDrawSelectTypeAsset(InpectorAssetTypeSelect.Prefab);
                        CheckAndDrawSelectTypeAsset(InpectorAssetTypeSelect.Material);
                        CheckAndDrawSelectTypeAsset(InpectorAssetTypeSelect.Image);
                        CheckAndDrawSelectTypeAsset(InpectorAssetTypeSelect.Anim);
                        CheckAndDrawSelectTypeAsset(InpectorAssetTypeSelect.None);
                    }


                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            #endregion

        }
        #region 脚本绘制方法支持
        //绘制一个分类
        public static void CheckAndDrawSelectTypeCompo(InpectorCompoTypeSelect select)
        {
            SirenixEditorGUI.BeginBox();
            SirenixEditorGUI.BeginBoxHeader();
            SelectFoldState_Compos[select] = SirenixEditorGUI.Foldout(SelectFoldState_Compos[select], select._Get_ATT_ESStringMessage("未知分组"));
            SirenixEditorGUI.EndBoxHeader();
            if (SelectFoldState_Compos[select])
            {
                EditorGUI.indentLevel++;
                var selectDic = CacheingEditors_Compos.GetDic(select);
                var UsingAllCompoKeys = CacheingEditors_Compos.GetDic(select).Keys.ToList();
                foreach (var (i, k) in selectDic)
                {
                    var ed = CacheingEditors_Compos.GetElement(select, i);
                    if (ed != null && ed._Editor != null)
                    {
                        UsingAllCompoKeys.Remove(i);
                        SirenixEditorGUI.BeginBox();
                        SirenixEditorGUI.BeginBoxHeader();
                        SirenixEditorGUI.EndBoxHeader();
                        ed.show = SirenixEditorGUI.Foldout(ed.show, i.GetType().Name);


                        if (ed.show)
                        {
                            if (ed._Editor is OdinEditor oe)
                            {

                                oe.DrawDefaultInspector();
                            }
                            else
                            {
                                ed._Editor.DrawDefaultInspector();
                            }
                        }

                        SirenixEditorGUI.EndBox();
                    }
                    else
                    {
                        IsCacheingDirty_Compos = true;
                    }
                }
                EditorGUI.indentLevel--;
            }
            SirenixEditorGUI.EndBox();
        }

        //新建或者查询Editor项目
        public static EditorAndIsShowing GetOrCreateEditor(InpectorCompoTypeSelect select, Component c)
        {

            var use = LastEditors_Compos.GetElement(select, c);
            if (use == null)
            {
                var OE = OdinEditor.CreateEditor(c, typeof(OdinEditor));
                Debug.Log(OE);
                return new EditorAndIsShowing() { _Editor = OE, show = false };
            }
            return use;
        }
        //自定义分组逻辑
        public static InpectorCompoTypeSelect GetSelectType(Component cc)
        {
            var type = cc.GetType();
            if (cc is Core) return InpectorCompoTypeSelect.ES;
            if (cc is EMS_Abstract) return InpectorCompoTypeSelect.Support;
            bool isOfficial = type.Namespace != null &&
                 (type.Namespace.StartsWith("UnityEngine") ||
                  type.Namespace.StartsWith("UnityEditor"));

            return isOfficial ? InpectorCompoTypeSelect.UnityCore : InpectorCompoTypeSelect.NotDefine;
        }
        #endregion

        #region 文件夹绘制支持
        public static void CheckAndAddAssetInKeyGroup(string path)
        {
            if (KeyValueMatchingUtility.SafeEditor.IsValidFolder(path))
            {
                CachingAssets.TryAdd(InpectorAssetTypeSelect.Folder, path);
                return;
            }
            if (path.EndsWith(".cs"))
            {
                CachingAssets.TryAdd(InpectorAssetTypeSelect.Script, path);
                return;
            }
            if (path.EndsWith(".prefab"))
            {
                CachingAssets.TryAdd(InpectorAssetTypeSelect.Prefab, path);
                return;
            }
            if (path.EndsWith(".mat"))
            {
                CachingAssets.TryAdd(InpectorAssetTypeSelect.Material, path);
                return;
            }
            if (path.EndsWith(".png") || path.EndsWith(".jpg"))
            {
                CachingAssets.TryAdd(InpectorAssetTypeSelect.Image, path);
                return;
            }
            if (path.EndsWith(".anim"))
            {
                CachingAssets.TryAdd(InpectorAssetTypeSelect.Anim, path);
                return;
            }
        }
        
        public static void CheckAndDrawSelectTypeAsset(InpectorAssetTypeSelect select)
        {
            string pathParent = pathAssetSelecting.Replace("\\", "/");
            int slashCountParent = pathParent.Count(f => f == '/');
            SirenixEditorGUI.BeginBox();
            SirenixEditorGUI.BeginBoxHeader();
            SelectFoldState_Assets[select] = SirenixEditorGUI.Foldout(SelectFoldState_Assets[select], AssetGUIContent[select]);
            SirenixEditorGUI.EndBoxHeader();
            if (SelectFoldState_Assets[select])
            {
                
               
                var selectList = CachingAssets.GetGroupAsList(select);

                foreach (var k in selectList)
                {
                    string normalizedPath = k.Replace("\\", "/");
                    // 统计正斜杠数量
                    int slashCountForIndent = normalizedPath.Count(f => f == '/');

                    int sub = slashCountForIndent - slashCountParent-1;
                    bool indent = sub > 0;
                    if (indent)
                    {
                        GUIHelper.PushIndentLevel(sub*3);
                    }
                    SirenixEditorGUI.BeginIndentedHorizontal();
                    if (normalizedPath.EndsWith("/"))
                    {
                        normalizedPath = normalizedPath._KeepBeforeByLast("/");
                    }
                    if (SirenixEditorGUI.Button(normalizedPath._KeepAfterByLast("/")._RemoveExtension(), ButtonSizes.Medium))
                    {
                        KeyValueMatchingUtility.SafeEditor.PingAssetByPath(k);
                    }
                    if (GUILayout.Button("直接选中",GUILayout.Width(100)))
                    {
                        KeyValueMatchingUtility.SafeEditor.SelectAssetByPath(k);
                    }
                    SirenixEditorGUI.EndIndentedHorizontal();
                    if (indent) GUIHelper.PopIndentLevel();


                }
                
            }
            SirenixEditorGUI.EndBox();
        }

        
        #endregion
    }
}

#region 补充
//分类
public enum InpectorCompoTypeSelect
{
    [ESMessage("Unity核心")] UnityCore,
    [ESMessage("未定义")] NotDefine,
    [ESMessage("【ES】")] ES,
    [ESMessage("支持类")] Support
}
//编辑器与折叠态
public class EditorAndIsShowing
{
    public Editor _Editor;
    public bool show;
}

public enum InpectorAssetTypeSelect
{
    [ESMessage("文件夹")] Folder,
    [ESMessage("脚本")] Script,
    [ESMessage("预制件")] Prefab,
    [ESMessage("材质")] Material,
    [ESMessage("图片")] Image,
    [ESMessage("动画")] Anim,
    [ESMessage("无")] None,
}

#endregion
