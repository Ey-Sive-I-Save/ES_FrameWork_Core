using ES;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using static ES.ESResMaster;
using ES.GlobalDataStand;
namespace ES
{ 
    [CreateAssetMenu(fileName = "全局数据-资源管理", menuName = "全局数据/资源管理")]
    public class GlobalDataForResMaster : GlobalDataSupportStand<GlobalDataForResMaster>
    {
        [FoldoutGroup("AB包工具")][LabelText("AB打包模式")] public ABPackType abPackType;
        [FoldoutGroup("AB包工具")][LabelText("AB代码生成模式")] public ABForAutoCodeGen abFoeAutoCodeGen = ABForAutoCodeGen.CodeAsOriginal;
        [FoldoutGroup("AB包工具"), FolderPath, LabelText("代码生成路径"), ReadOnly]
        public string genarateCodeFolder_ = "Assets/Scripts/ESFramework/CodeGen/Res";
        [FoldoutGroup("AB包工具"), FolderPath, LabelText("AB包生成路径"), ReadOnly]
        public string genarateFolder = "Assets/StreamingAssets/AssetBundles";
        [FoldoutGroup("AB包工具"), FolderPath, LabelText("热更新数据"), ReadOnly]
        public string genarateHotUpdateFolder_ = "Assets/StreamingAssets/AssetBundles";
        [FoldoutGroup("AB包工具"), LabelText("AB包标记模式")]
        public ABMaskType abMaskType = ABMaskType.AsFolder;
        [FoldoutGroup("AB包工具"), LabelText("AB包标记自定义名字")]
        public string ABName = "defaultABName";


        [FoldoutGroup("AB包工具"), LabelText("应用平台")]
        public RuntimePlatform applyPlatform = Application.platform;

        [FoldoutGroup("AB包工具"), LabelText("下载数据网址")]
        public string DownLoadURL = "https://eysive-teset.oss-cn-beijing.aliyuncs.com";
        [FoldoutGroup("AB包工具"), LabelText("下载测试地址")]
        public string LocalTestDownLoadPath = "Assets/StreamingAssets/AssetBundles/DownLoadTest";

        [FoldoutGroup("AB包工具"), LabelText("真实下载附加路径(相对于持久化地址)")]
        public string LocalDownLoadAdditionPath = "DownLoad";

        [FoldoutGroup("AB包工具"), LabelText("Hash信息字典"), InlineButton("clearTest2", "cesgu")]
        public Dictionary<string, string> toHash = new Dictionary<string, string>();

        public string GetLocalAssetBundlePath()
        {
            return Path.Combine(genarateFolder, applyPlatform.ToString());
        }
        public string GetLocalAssetBundlePath(string platform)
        {
            return Path.Combine(genarateFolder, platform);
        }
        [FoldoutGroup("AB包工具"), LabelText("依赖信息字典"), NonSerialized, OdinSerialize]
        public Dictionary<string, WrapListString> Depend = new Dictionary<string, WrapListString>();
        [FoldoutGroup("AB包工具"), LabelText("AB优先加载目标")]
        public List<ABTargetLocation> TargetLocations = new List<ABTargetLocation>();
        public string HotUpdateDependenceFileName => "dependenceDic.json";
        public string HotUpdatePreToHashFileName => "preToHashDic.json";
        #region 枚举和类
        public enum ABPackType
        {
            [InspectorName("模拟模式")] Simulate,
            [InspectorName("发布模式")] Release
        }
        public enum ABForAutoCodeGen
        {
            [InspectorName("不生成代码")] NoneCode,
            [InspectorName("生成同名代码")] CodeAsOriginal,
            [InspectorName("生成代码转大写")] CodeAsUpper,
            [InspectorName("生成代码转小写")] CodeAsLower
        }

        public enum ABMaskType
        {
            [InspectorName("原名")] AsOrinal,
            [InspectorName("收集到文件夹名")] AsFolder,
            [InspectorName("自定义名")] SelfDefine,
        }
        #region Hash获取和处理
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public string GetHashFromCompleteNameWithHash(string input)
        {
            int index = input.LastIndexOf("_");
            if (index >= 0) return input.Substring(index + 1, input.Length - index - 1);
            return input;
        }
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public string GetPreNameFromCompleteNameWithHash(string input)
        {
            int index = input.LastIndexOf("_");
            if (index >= 0) return input.Substring(0, index);
            return input;
        }
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public string GetCompleteNameFromPreName(string input)
        {
            /* if( ESAssetBundlePath.AssetBundleHashes.TryGetValue(input,out var withHash))
             {
                 return withHash;
             }*/
            return input;
        }
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public string GetPreNameFromCompleteNameBySymbol(string input, char sym)
        {
            int index = input.LastIndexOf(sym);
            if (index >= 0) return input.Substring(0, index);
            return input;
        }
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public string GetPostNameFromCompleteNameBySymbol(string input, char sym)
        {
            int index = input.LastIndexOf(sym);
            if (index >= 0) return input.Substring(index + 1, input.Length - index - 1);
            return input;
        }
        #endregion
        #endregion
    }
}

