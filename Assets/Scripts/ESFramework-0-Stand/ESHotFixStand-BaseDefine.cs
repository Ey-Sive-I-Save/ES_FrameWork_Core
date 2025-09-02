
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace ES.HotFixStand
{
    public static class ESHotFixStand
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
        public static string _KeepAfterByLast(this string source, string separator,
                                         bool includeSeparator = false,
                                         StringComparison comparison = StringComparison.Ordinal)
        {
            if (string.IsNullOrEmpty(source)) return source;

            int index = source.LastIndexOf(separator, comparison);
            if (index < 0) return source; // 未找到分隔符返回原字符串

            return includeSeparator ?
                source.Substring(index) :
                source.Substring(index + separator.Length);
        }
        public static T CreateSO<T>(string savePath, string name) where T : UnityEngine.ScriptableObject
        {
            Debug.Log("创建 SO :" + name + "在" + savePath);
            var ins = ScriptableObject.CreateInstance<T>();
            if (ins != null)
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
        public static string SelectorFolder(string targetPath = "Assets/Resources/Data/GlobalData", string title = "选择文件夹")
        {
#if UNITY_EDITOR
            return EditorUtility.OpenFolderPanel(title, targetPath, "");
#else
               return targetPath;
#endif
        }
    }
}
