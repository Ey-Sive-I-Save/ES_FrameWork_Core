using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES {
    [Serializable, TypeRegistryItem("A变换-设置锚定位置(死赋值-低级)")]
    public class OutputOperationUI_Handle_SetAnchorPos : IOutputOperationUI
    {
        [LabelText("设置锚定坐标")] public Vector2 pos;
        public void TryOperation(ESUIElementCore on, ESUIElementCore from, ILink_UI_OperationOptions with)
        {
            Debug.Log("SET"+pos);
            on.Refer_Rect.Value.anchoredPosition = pos;
        }
        public void TryCancel(ESUIElementCore on, ESUIElementCore from, ILink_UI_OperationOptions with)
        {

        }
       
    }
    [Serializable, TypeRegistryItem("A变换-设置单轴锚定位置(死赋值-低级)")]
    public class OutputOperationUI_Handle_SetAnchorPos_SingleAxis : IOutputOperationUI
    {
        [LabelText("设置锚定坐标")] public float singleValue;
        [LabelText("生效的轴")] public Axis_XOrY axis = Axis_XOrY.X;
        public enum Axis_XOrY
        {
            [InspectorName("X轴")]X, [InspectorName("Y轴")] Y
        }
        public void TryOperation(ESUIElementCore on, ESUIElementCore from, ILink_UI_OperationOptions with)
        {
            var rect = on.Refer_Rect.Value;

            if (axis== Axis_XOrY.X) rect.anchoredPosition.Set(singleValue,rect.anchoredPosition.y);
            else rect.anchoredPosition.Set(singleValue, rect.anchoredPosition.y);
        }
        public void TryCancel(ESUIElementCore on, ESUIElementCore from, ILink_UI_OperationOptions with)
        {

        }
        
    }
    [Serializable, TypeRegistryItem("A变换-设置锚定位置(随机增益-中级)")]
    public class OutputOperationUI_Handle_SetAnchorPos_RandomAdding : IOutputOperationUI
    {
        [LabelText("设置基本锚定坐标")] public Vector2 pos;

        [LabelText("X增益范围")] public Vector2 rangeX;
        [LabelText("Y增益范围")] public Vector2 rangeY;

        public void TryOperation(ESUIElementCore on, ESUIElementCore from, ILink_UI_OperationOptions with)
        {
            on.Refer_Rect.Value.anchoredPosition = pos+new Vector2(UnityEngine.Random.Range(rangeX.x,rangeX.y),UnityEngine.Random.Range(rangeY.x, rangeY.y));
        }
        public void TryCancel(ESUIElementCore on, ESUIElementCore from, ILink_UI_OperationOptions with)
        {

        }

    }
}
