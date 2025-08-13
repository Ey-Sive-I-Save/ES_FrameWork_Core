using DG.Tweening;
using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;


[Serializable/**/]
public abstract class UITweenGetter
{
    [LabelText("持续时间")]
    public float duar=1;
    public abstract Tween GetTween(ESUIElementCore on, ESUIElementCore from);
}

[Serializable,TypeRegistryItem("UITween_简易位移")]
public class UITweenGetter_EasyAnchorPos : UITweenGetter
{
    [LabelText("位置")]
    public Vector2 vector2;
    [LabelText("是From的")]
    public bool isFrom;
    public override Tween GetTween(ESUIElementCore on, ESUIElementCore from)
    {
        if (on.Refer_Rect != null)
        {
            var d = on.Refer_Rect.Value.DOAnchorPos(vector2, duar);
            if (isFrom) d.From();
            return d;
        }
        return null;
    }
}

