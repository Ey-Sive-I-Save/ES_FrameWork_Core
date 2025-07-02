using DG.Tweening.Plugins.Core.PathCore;
using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ES
{
    public class ESResEditor
    {
        #region Asset菜单部分
        [InitializeOnLoad]
        public class ESResEditorForAssetMenu
        {
            #region 常量表
            public const string _ConstString_MenuForMarkAssetBundle = "Assets/【ES资源】标记到AssetBundle";
            public const string _ConstString_MenuForReMarkAssetBundle = "Assets/【ES资源】强制刷新标记到AssetBundle";

            #endregion

            #region 改变选择对象——调整Check
            static ESResEditorForAssetMenu()
            {
                Selection.selectionChanged = OnSelectionChanged;
            }
            //选择目标改变，修改标记状态
            public static void OnSelectionChanged()
            {
                var paths = GetSelectedObjectsAllPaths();
                if (paths.Count == 0) return;
                //感觉一个就够了？按第一个(不为NULL)来吧
                string apply = paths.First((i) => i != null && !string.IsNullOrEmpty(i));
                if (!string.IsNullOrEmpty(apply))
                {
                    var ai = AssetImporter.GetAtPath(apply);
                    var dir = new DirectoryInfo(apply);

                    Menu.SetChecked(_ConstString_MenuForMarkAssetBundle, IsObjectPathMarked(apply));
                    Debug.Log("Check" + IsObjectPathMarked(apply));
                }
            }
            #endregion 
            //编辑或者取消标记
            [MenuItem(_ConstString_MenuForMarkAssetBundle)]
            public static void Handle_TryMarkToAssetBundleOrCancel()
            {
                var ss = GetSelectedObjectsAllPaths();
                //应该按照选择的第一个作为基准，而不是全部反向
                bool now = Menu.GetChecked(_ConstString_MenuForMarkAssetBundle);
                foreach (var i in ss)
                {
                    MarkToABOrCancelTruely(i, !now);
                }
            }
            [MenuItem(_ConstString_MenuForReMarkAssetBundle)]
            public static void Handle_TryReMarkToAssetBundle()
            {
                var ss = GetSelectedObjectsAllPaths();
                //必定标记
                foreach (var i in ss)
                {
                    MarkToABOrCancelTruely(i, true);
                }
            }
            #region 辅助功能
            public static List<string> GetSelectedObjectsAllPaths()
            {
                var paths = new List<string>(3);

                //筛选选中的资源(包括文件夹)
                foreach (var obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
                {
                    var path = AssetDatabase.GetAssetPath(obj);
                    if (!string.IsNullOrEmpty(path) && (File.Exists(path) || AssetDatabase.IsValidFolder(path)))
                    {
                        paths.Add(path);
                    }
                }
                return paths;
            }
            public static void MarkToABOrCancelTruely(string path, bool? hardSetCheck = null)
            {
                if (!string.IsNullOrEmpty(path))
                {
            
                   //Asset导入设置
                   AssetImporter ai = AssetImporter.GetAtPath(path);
                    
                    //获得文件夹
                    DirectoryInfo dir = new DirectoryInfo(path);
 
                    //设置为
                    bool ToCheck = hardSetCheck ?? !IsObjectPathMarked(path);

                    if (ToCheck)
                    {
                        Menu.SetChecked(_ConstString_MenuForMarkAssetBundle, true);
                        Debug.Log("Check True");
                        
                        if (ESResMaster.Instance.abMaskType == ESResMaster.ABMaskType.AsOrinal)
                        {
                            ai.assetBundleName = dir.Name.Replace(".", "_");
                            AssetDatabase.SaveAssets();
                            AssetDatabase.Refresh();
                        }
                        else if (ESResMaster.Instance.abMaskType == ESResMaster.ABMaskType.AsFolder)
                        {
                            //文件夹维持自身
                            if(AssetDatabase.IsValidFolder(path)) ai.assetBundleName = dir.Name.Replace(".", "_");
                            //非文件夹用父级
                            else ai.assetBundleName = dir.Parent.Name.Replace(".", "_");
                            AssetDatabase.SaveAssets();
                            AssetDatabase.Refresh();
                        }
                        else
                        {
                            //自定义
                            ai.assetBundleName = ESResMaster.Instance.ABName.Replace(".", "_");
                            AssetDatabase.SaveAssets();
                            AssetDatabase.Refresh();
                        }
                    }
                    else
                    {
                        Menu.SetChecked(_ConstString_MenuForMarkAssetBundle, false);
                        Debug.Log("Check false");
                        ai.assetBundleName = "XXXX";
                        ai.assetBundleName = string.Empty;
                        ai.assetBundleVariant ??= string.Empty;
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }

                    AssetDatabase.RemoveUnusedAssetBundleNames();
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }

            public static bool IsObjectPathMarked(string path)
            {
                try
                {
                    var ai = AssetImporter.GetAtPath(path);
                    var dir = new DirectoryInfo(path);
                    return string.Equals(ai.assetBundleName, dir.Name.Replace(".", "_").ToLower());
                }
                catch (Exception _)
                {
                    Debug.LogError(_);
                    return false;
                }
            }
            #endregion
        }


        #endregion
    }
}


