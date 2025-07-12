using ES;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
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
        static ESInspectorShow()
        {
            // 监听Inspector绘制前的回调
            Editor.finishedDefaultHeaderGUI += DrawCustomHeader;
            Selection.selectionChanged += MakeSelectionDirty;

        }
        static bool dirty = true;//首次必定加载
        private static void MakeSelectionDirty()
        {
            dirty = true;
        }
        // 在Inspector顶部绘制自定义内容
        private static void DrawCustomHeader(Editor editor)
        {
            if (!GlobalDataForEditorOnly.Instance?.Ins.EnableCompoentShowControl_??false) return;
            var cache = GlobalDataForEditorOnly.Instance.Ins?.cacheToggleFalseNames;
            if (cache == null) return;
            try {
                if (editor.target is GameObject go)
                {
                    if (editor.targets.Length == 1)
                    {

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
                            var cs = go.GetComponents<Component>();


                            foreach (var i in cs)
                            {
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

                    }
                }
            } catch (Exception e)
            {
                Debug.LogError(e);
            }
            

        }
    }
}


