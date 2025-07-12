using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


#if UNITY_EDITOR

#endif


namespace ES
{
    public static partial class KeyValueMatchingUtility
    {
        //
        public class SafeEditor
        {
            public static void SetDirty(UnityEngine.Object who, bool withRefresh = true)
            {
#if UNITY_EDITOR
                if (withRefresh)
                {
                    AssetDatabase.Refresh();
                    AssetDatabase.SaveAssets();
                }
                EditorUtility.SetDirty(who);
#endif
            }

            public static bool DisplayDialog(string title, string message, string ok = "好的", string cancel = "算了")
            {
#if UNITY_EDITOR

                return EditorUtility.DisplayDialog(title, message, ok, cancel);
#else
                return false;
#endif

            }

            public static string SelectorFolder(string targetPath= "Assets/Resources/Data/GlobalData",string title="选择文件夹")
            {
#if UNITY_EDITOR
               return  EditorUtility.OpenFolderPanel(title,targetPath,"");
#else 
               return targetPath;
#endif
            }

            public static bool IsValidFolder(string path,bool want=false)
            {
#if UNITY_EDITOR
                return AssetDatabase.IsValidFolder(path);
#else
                return want;
#endif
            }

            public static T CreateSO<T>(string savePath,string name) where T : UnityEngine.ScriptableObject
            {
                Debug.Log("创建 SO :"+name+"在" +savePath );
                var ins= ScriptableObject.CreateInstance<T>();
                if(ins != null)
                {
#if UNITY_EDITOR
                    ins.name = name;
                    AssetDatabase.CreateAsset(ins, savePath + "/" + ins.name + ".asset");
                    AssetDatabase.Refresh();
                    AssetDatabase.SaveAssets();
#endif
                    return ins;
                }
                return null;
            }

            public static void InitAsset<T>() where T:UnityEngine.Object
            {
#if UNITY_EDITOR
                List<T> results = new List<T>();
                string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
                foreach (string guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    T asset = AssetDatabase.LoadAssetAtPath<T>(path);
                    if (asset != null)
                    {
                        results.Add(asset);
                        EditorUtility.SetDirty(asset);
                    }
                }
#endif
            }
        }
    }
}

