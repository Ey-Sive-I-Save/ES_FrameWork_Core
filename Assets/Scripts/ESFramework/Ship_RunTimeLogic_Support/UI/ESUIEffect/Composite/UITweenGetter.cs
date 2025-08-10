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
    public Vector2 vector2;
    public override Tween GetTween(ESUIElementCore on, ESUIElementCore from)
    {
        if (on.Refer_Rect != null)
        {
            return on.Refer_Rect.Value.DOAnchorPos(vector2, duar);
        }
        return null;
    }
}

