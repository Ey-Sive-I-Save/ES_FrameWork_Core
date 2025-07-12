using ES;
using ES.EvPointer;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using static ES.ESResMaster;

namespace ES
{
    [CreateAssetMenu(fileName = "全局数据-运行时编辑器数据管理", menuName = "全局数据/运行时编辑器数据管理")]
    public class GlobalDataForEditorRunTime : GlobalDataSupport<GlobalDataForEditorRunTime>
    {
        public static List<string> ESTags = new List<string>();
        public static List<string> BuffKeys = new List<string>();
        [LabelText("配置的ESTags包")] public ESLayerStringSO ESTagsSO_;
        [LabelText("可寻分组键")]
        public SelectDic_StringsToType SearchDataTypeKey = new SelectDic_StringsToType();
    }
}

