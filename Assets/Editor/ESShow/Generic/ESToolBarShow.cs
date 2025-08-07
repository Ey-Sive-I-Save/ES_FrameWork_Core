using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Toolbars;
using UnityEditor;
using UnityEngine;

using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEditor.SearchService;
using UnityEditor.SceneManagement;
using System.IO;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using FishNet.Demo.AdditiveScenes;
using static FishNet.Component.Transforming.NetworkTransform;

namespace ES
{

    public class CustomToolBar
    {

        [InitializeOnLoad]
        public static class CustomToolbarMenu
        {
            public static bool IncludeNoBuild = false;
            public static bool AdditiveModel = false;
            public static bool WithTheAlwaysNoDieCore = false;
            static CustomToolbarMenu()
            {
                // 注册到主工具栏
                ToolbarExtender.RightToolbarGUI.Add(OnSceneSelectorToolbarGUI);
                ToolbarExtender.RightToolbarGUI.Add(OnSceneSelectorSettingsToolbarGUI);
                //左边
                ToolbarExtender.LeftToolbarGUI.Add(OnQuickSelectionToolbarGUI);
            }
            public static bool init = true;
            static void OnSceneSelectorToolbarGUI()
            {
                if (init)
                {
                    IncludeNoBuild = EditorPrefs.GetBool("IncludeNoBuild", false);
                    AdditiveModel = EditorPrefs.GetBool("AdditiveModel", false);
                    WithTheAlwaysNoDieCore = EditorPrefs.GetBool("WithTheAlwaysNoDieCore", false);
                    init = false;
                }
                // 创建下拉菜单按钮
                if (EditorGUILayout.DropdownButton(
                    new GUIContent("场景跳转", EditorGUIUtility.IconContent("d__Popup").image),
                    FocusType.Passive,
                    EditorStyles.toolbarDropDown))
                {
                    var menu = new GenericMenu();

                    if (IncludeNoBuild)
                    {
                        string assetsPath = Application.dataPath;
                        string[] allFiles = Directory.GetFiles(assetsPath, "*.unity", SearchOption.AllDirectories);

                        foreach (string file in allFiles)
                        {
                            // 转换为Unity相对路径（如 "Assets/Scenes/Menu.unity"）
                            string relativePath = "Assets" + file.Replace(Application.dataPath, "").Replace('\\', '/');

                            string use = relativePath;
                            int indexXIE = relativePath.LastIndexOf('/') + 1;
                            int indexLast = relativePath.LastIndexOf(".unity");
                            if (indexXIE >= 0 && indexLast >= 0)
                            {
                                string display = relativePath.Substring(indexXIE, indexLast - indexXIE);
                                menu.AddItem(new GUIContent("<场景>" + display), false, () =>
                                {
                                    UnityEngine.SceneManagement.Scene activeScene = EditorSceneManager.GetActiveScene();
                                    bool b = EditorSceneManager.SaveScene(activeScene);
                                    Debug.Log("自动保存场景" + activeScene + (b ? "成功" : "失败"));
                                    if (AdditiveModel) EditorSceneManager.OpenScene(use, mode: OpenSceneMode.Additive);
                                    else EditorSceneManager.OpenScene(use);
                                });
                            }

                        }

                    }
                    else
                    {
                        var ss = EditorBuildSettings.scenes;
                        foreach (var i in ss)
                        {
                            string use = i.path;
                            int indexXIE = i.path.LastIndexOf('/') + 1;
                            int indexLast = i.path.LastIndexOf(".unity");
                            if (indexXIE >= 0 && indexLast >= 0)
                            {
                                string display = i.path.Substring(indexXIE, indexLast - indexXIE);
                                menu.AddItem(new GUIContent("<场景>" + display), false, () =>
                            {
                                UnityEngine.SceneManagement.Scene activeScene = EditorSceneManager.GetActiveScene();
                                bool b = EditorSceneManager.SaveScene(activeScene);
                                Debug.Log("自动保存场景" + activeScene + (b ? "成功" : "失败"));
                                if (AdditiveModel) EditorSceneManager.OpenScene(use, mode: OpenSceneMode.Additive);
                                else EditorSceneManager.OpenScene(use);
                            });
                                /* int indexXIE = i.path.LastIndexOf('/') + 1;
                                 int indexLast = i.path.LastIndexOf(".unity");
                                 if (indexXIE >= 0 && indexLast >= 0)
                                 {
                                     string use = i.path.Substring(indexXIE, indexLast - indexXIE);
                                     menu.AddItem(new GUIContent("<场景>" + use), false, () =>
                                     {
                                         EditorSceneManager.OpenScene(use);
                                     });
                                 }*/
                            }
                        }
                    }
                    // 添加菜单项

                    menu.AddSeparator("");

                    // 带图标的菜单项
                    /*  menu.AddItem(
                          new GUIContent("带图标的项", EditorGUIUtility.IconContent("d_SceneViewFx").image),
                          false,
                          () => Debug.Log("图标项点击")
                      );*/

                    // 显示菜单
                    menu.ShowAsContext();
                }
            }
            static void OnSceneSelectorSettingsToolbarGUI()
            {
                // 创建下拉菜单按钮
                if (EditorGUILayout.DropdownButton(
                    new GUIContent("场景跳转设置", EditorGUIUtility.IconContent("d__Popup").image),
                    FocusType.Passive,
                    EditorStyles.toolbarDropDown))
                {
                    var menu = new GenericMenu();
                    var ss = EditorBuildSettings.scenes;
                    bool thisDirty = false;
                    menu.AddItem(new GUIContent("<包含未构建场景>"), IncludeNoBuild, () =>
                    {
                        IncludeNoBuild = !IncludeNoBuild;
                        thisDirty = true;
                    });
                    menu.AddItem(new GUIContent("<使用叠加场景模式>"), AdditiveModel, () =>
                    {
                        AdditiveModel = !AdditiveModel;
                        thisDirty = true;
                    });
                    menu.AddItem(new GUIContent("<保持核心存在>"), WithTheAlwaysNoDieCore, () =>
                    {
                        WithTheAlwaysNoDieCore = !WithTheAlwaysNoDieCore;
                        thisDirty = true;
                    });
                    if (thisDirty)
                    {
                        EditorPrefs.SetBool("IncludeNoBuild", IncludeNoBuild);
                        EditorPrefs.SetBool("AdditiveModel", AdditiveModel);
                        EditorPrefs.SetBool("WithTheAlwaysNoDieCore", WithTheAlwaysNoDieCore);
                    }
                    // 添加菜单项

                    menu.AddSeparator("");
                    menu.ShowAsContext();
                }
            }

            static void OnQuickSelectionToolbarGUI()
            {
                // 创建下拉菜单按钮
                if (EditorGUILayout.DropdownButton(
                    new GUIContent("快速定位操作", EditorGUIUtility.IconContent("d__Popup").image),
                    FocusType.Passive,
                    EditorStyles.toolbarDropDown, GUILayout.Width(300)))
                {
                    var menu = new GenericMenu();
                    var ss = EditorBuildSettings.scenes;
                    menu.AddItem(new GUIContent("<框架总文件夹>"), false, () =>
                    {
                        string[] guids = AssetDatabase.FindAssets("ESFramework");
                        foreach (var i in guids)
                        {
                            string path = AssetDatabase.GUIDToAssetPath(i);
                            var use = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                            Selection.activeObject = use;
                            EditorGUIUtility.PingObject(use);
                            break;
                        }
                    });
                    menu.AddItem(new GUIContent("<So数据总文件夹>"), false, () =>
                    {
                        string[] guids = AssetDatabase.FindAssets("SingleData");
                        foreach (var i in guids)
                        {
                            string path = AssetDatabase.GUIDToAssetPath(i);
                            var use = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                            Selection.activeObject = use;
                            EditorGUIUtility.PingObject(use);
                            break;
                        }
                    });
                    menu.AddItem(new GUIContent("<编辑器总文件夹>"), false, () =>
                    {
                        string[] guids = AssetDatabase.FindAssets("Editor");
                        foreach (var i in guids)
                        {
                            string path = AssetDatabase.GUIDToAssetPath(i);
                            var use = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                            Selection.activeObject = use;
                            EditorGUIUtility.PingObject(use);
                            break;
                        }
                    });
                    menu.AddItem(new GUIContent("<静态策略工具总文件夹>"), false, () =>
                    {
                        string[] guids = AssetDatabase.FindAssets("Static_KeyValueMaching_Partial");
                        foreach (var i in guids)
                        {
                            string path = AssetDatabase.GUIDToAssetPath(i);
                            var use = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                            Selection.activeObject = use;
                            EditorGUIUtility.PingObject(use);
                            break;
                        }
                    });
                    menu.AddItem(new GUIContent("<全局数据总文件夹>"), false, () =>
                    {
                        string[] guids = AssetDatabase.FindAssets("GlobalData");
                        foreach (var i in guids)
                        {
                            string path = AssetDatabase.GUIDToAssetPath(i);
                            var use = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                            Selection.activeObject = use;
                            EditorGUIUtility.PingObject(use);
                            break;
                        }
                    });
                    
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("<玩家对象>"), false, () =>
                    {
                        var player = GameObject.FindGameObjectWithTag("Player");
                        if (player != null) { Selection.activeGameObject = player; EditorGUIUtility.PingObject(player); }
                    });
                    menu.AddItem(new GUIContent("<纯编辑器管理器>"), false, () =>
                    {
                        var it = UnityEngine.Object.FindAnyObjectByType<ESEditorOnlyPartMaster>();
                        if (it != null) { Selection.activeGameObject = it.gameObject; EditorGUIUtility.PingObject(it.gameObject); }
                    });
                    menu.AddItem(new GUIContent("<游戏核心管理器>"), false, () =>
                    {
                        var it = UnityEngine.Object.FindAnyObjectByType<GameCenterManager>();
                        if (it != null) { Selection.activeGameObject = it.gameObject; EditorGUIUtility.PingObject(it.gameObject); }
                    });
                    menu.AddItem(new GUIContent("<对象池管理器>"), false, () =>
                    {
                        var it = UnityEngine.Object.FindAnyObjectByType<ESPoolMaster>();
                        if (it != null) { Selection.activeGameObject = it.gameObject; EditorGUIUtility.PingObject(it.gameObject); }
                    });
                    menu.AddItem(new GUIContent("<不死联网核心管理器>"), false, () =>
                    {
                        var it = UnityEngine.Object.FindAnyObjectByType<ESNetManager>();
                        if (it != null) { Selection.activeGameObject = it.gameObject; EditorGUIUtility.PingObject(it.gameObject); }
                    });
                  
                    // 添加菜单项

                    menu.AddSeparator("");
                    menu.ShowAsContext();
                }
            }

        }
    }
}




