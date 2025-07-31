using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ES
{
    public enum Channel_ColliderFrom
    {
        [InspectorName("身体")]Body,
        [InspectorName("脚")] Foot,
        [InspectorName("手")] Hand,
        [InspectorName("外轮廓")] Outline,
        [InspectorName("武器")] Weapon
    }
}

