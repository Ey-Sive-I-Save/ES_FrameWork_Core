using ES;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using Sirenix.Utilities.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
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
            SelectFoldState = new Dictionary<InpectorCompoTypeSelect, bool>();
            SelectFoldState.Add(InpectorCompoTypeSelect.UnityCore, false);
            SelectFoldState.Add(InpectorCompoTypeSelect.Support, false);
            SelectFoldState.Add(InpectorCompoTypeSelect.NotDefine, false);
            SelectFoldState.Add(InpectorCompoTypeSelect.ES, false);
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
        private static bool showAllFold = false;
        public static bool IsCacheingDirty = true;
        public static List<Component> CachingCompos = new List<Component>();
        public static Dictionary<InpectorCompoTypeSelect, bool> SelectFoldState = new Dictionary<InpectorCompoTypeSelect, bool>();
        public static SelectDic<InpectorCompoTypeSelect, Component, EditorAndIsShowing> LastEditors = new SelectDic<InpectorCompoTypeSelect, Component, EditorAndIsShowing>();
        public static SelectDic<InpectorCompoTypeSelect, Component, EditorAndIsShowing> CacheingEditors = new SelectDic<InpectorCompoTypeSelect, Component, EditorAndIsShowing>();
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
                        if(go==null/*关闭*/) {//尝试绘制收集
                            SirenixEditorGUI.BeginBox();
                            SirenixEditorGUI.BeginBoxHeader();
                            showAllFold = SirenixEditorGUI.Foldout(showAllFold, "脚本收集");
                            SirenixEditorGUI.EndBoxHeader();
                            if (IsCacheingDirty)
                            {
                                IsCacheingDirty = false;
                                LastEditors = CacheingEditors;
                                CacheingEditors = new SelectDic<InpectorCompoTypeSelect, Component, EditorAndIsShowing>();
                                foreach (var i in cs)
                                {
                                    var select = GetSelectType(i);
                                    CacheingEditors.TryAddOrSet(select, i, GetOrCreateEditor(select, i));
                                }
                            }
                            if (showAllFold)
                            {
                                CheckAndDrawSelectTypeCompo(InpectorCompoTypeSelect.UnityCore);
                                CheckAndDrawSelectTypeCompo(InpectorCompoTypeSelect.ES);
                                CheckAndDrawSelectTypeCompo(InpectorCompoTypeSelect.Support);
                                CheckAndDrawSelectTypeCompo(InpectorCompoTypeSelect.NotDefine);
                                
                                var csList = cs.ToList();
                                if (!csList.SequenceEqual(CachingCompos))
                                {
                                    CachingCompos = csList;
                                    IsCacheingDirty = true;
                                }
                                
                            }
                            SirenixEditorGUI.EndBox();
                            #endregion
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            #endregion

        }
        //绘制一个分类
        public static void CheckAndDrawSelectTypeCompo(InpectorCompoTypeSelect select)
        {
            SirenixEditorGUI.BeginBox();
            SirenixEditorGUI.BeginBoxHeader();
            SelectFoldState[select] = SirenixEditorGUI.Foldout(SelectFoldState[select], select._Get_ATT_ESStringMessage("未知分组"));
            SirenixEditorGUI.EndBoxHeader();
            if (SelectFoldState[select])
            {
                EditorGUI.indentLevel++;
                var selectDic = CacheingEditors.GetDic(select);
                var UsingAllCompoKeys = CacheingEditors.GetDic(select).Keys.ToList();
                foreach (var (i, k) in selectDic)
                {
                    var ed = CacheingEditors.GetElement(select, i);
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
                        IsCacheingDirty = true;
                    }
                }
                EditorGUI.indentLevel--;
            }
            SirenixEditorGUI.EndBox();
        }

        //新建或者查询Editor项目
        public static EditorAndIsShowing GetOrCreateEditor(InpectorCompoTypeSelect select, Component c)
        {
            
            var use = LastEditors.GetElement(select, c);
            if (use == null) {
                var OE = OdinEditor.CreateEditor(c,typeof(OdinEditor));
                Debug.Log(OE);
                return new EditorAndIsShowing() { _Editor =OE, show = false };
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

#endregion
