using ES;
using ES.Pointer;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

        protected override void Refresh()
        {
            base.Refresh();
            if (Instance == this)
            {
                LoadESTags();
                LoadSST();
            }
        }
        private void LoadESTags()
        {
            ESTags = new List<string>();
            if (Instance.ESTagsSO_ != null)
            {
                foreach (var i in Instance.ESTagsSO_.LayerStrings)
                {
                    foreach (var ii in i.Value)
                    {
                        ESTags.Add(i.Key + "/" + ii);
                    }
                }
            }
        }
        private void LoadSST()
        {
            Instance.SearchDataTypeKey = new SelectDic_StringsToType();
            var assem = Assembly.GetExecutingAssembly();
            var types = assem.GetTypes();
            foreach (var i in types)
            {
                var at = i.GetCustomAttribute<ESDisplayNameKeyToTypeAttribute>();
                if (at != null)
                {
                    Instance.SearchDataTypeKey.TryAddOrSet(at.TeamCollect, at.DisplayKeyName, i);
                }
            }
        }
    }
}

