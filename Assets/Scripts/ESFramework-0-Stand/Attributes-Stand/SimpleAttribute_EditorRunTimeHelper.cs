using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES
{

    /// <summary>
    /// ##这个特性专用于收集通过外观显示名-》到RunTime类型
    /// 用于中文化＋自动化 管理 SoDataInfo
    /// ESEditorRuntimePartMaster 读取和存储 关于它的信息
    /// </summary>
    public class ESDisplayNameKeyToTypeAttribute : Attribute
    {
        public string TeamCollect = "收集到";
        public string DisplayKeyName = "显示名与键";
        public ESDisplayNameKeyToTypeAttribute(string team,string display)
        {
            TeamCollect = team;
            DisplayKeyName = display;
        }
    }
}

