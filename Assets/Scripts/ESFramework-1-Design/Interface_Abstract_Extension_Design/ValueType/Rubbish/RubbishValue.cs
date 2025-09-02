using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES
{
    #region BuffS状态(过时)
    [Serializable]
    public struct BuffStatusTest
    {
        [LabelText("持续时间")] public float duration;
        [LabelText("等级")] public float level;
    }
    #endregion
}
