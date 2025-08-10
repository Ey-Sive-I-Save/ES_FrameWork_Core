using DG.Tweening;
using ES;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ES
{

    public class ESSimpleWindow : OdinEditorWindow
    {
        
    }

    #region 测试
    public class ESWindow_RectTransformSetter : ESSimpleWindow
    {
        void t()
        {
            Transform t=null;
           /* t.DOMove(default,default).SetEase( Ease.Unset)*/
        }
    }

    #endregion
}

