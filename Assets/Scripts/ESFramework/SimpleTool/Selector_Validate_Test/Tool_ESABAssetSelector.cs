// #define Error

using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ES
{
    [Serializable, TypeRegistryItem("AB资源选择器", "ES选择器")]
    public class ESABAssetSelector
    {
#if Error
        [InfoBox("现在正处于错误修复阶段！！！",infoMessageType:InfoMessageType.Error)]
#endif
        [LabelText("AB包名"), ValueDropdown("@ESAssetBundlePath.AllPathsDic.Keys")]
        public string ABBundleName;
        [LabelText("资产标识名"), ValueDropdown("@ESAssetBundlePath.AllPathsDic[ABBundleName].Keys"), OnValueChanged("GetAsset")]
        public string AssetKey;
        [LabelText("查询资产实际名"), InlineButton("GetAsset", "获取")]
        public string AssetValue;
        string GetAsset()
        {
            string value =
#if Error
                "";
#else
                ESAssetBundlePath.AllPathsDic[ABBundleName][AssetKey];
#endif

#if UNITY_EDITOR
            string[] AllAssetsPaths = AssetDatabase.GetAssetPathsFromAssetBundle(ABBundleName.ToLower());
            foreach (var path in AllAssetsPaths)
            {
                if (path.Contains(value))
                {
                    EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object)));
                    break;
                }
            }

            GUIUtility.systemCopyBuffer = $"\"{value}\"";
#endif
            return AssetValue = value;
        }
    }
}

