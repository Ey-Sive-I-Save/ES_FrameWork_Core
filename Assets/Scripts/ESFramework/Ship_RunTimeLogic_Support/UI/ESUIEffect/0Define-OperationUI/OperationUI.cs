
using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


namespace ES {
    // UI 通用 可输出操作  ---  on 作用于  from 影响源  with 选通，执行类型 等
    public interface IOutputOperationUI : IOutputOperation<ESUIElementCore, ESUIElementCore, ILink_UI_OperationOptions>
    {

    }
    #region 组分
    [Serializable/* 变对象执行 把本身的 on 和 from 替换为这个新的 元素 和 它的 from */]
    public abstract class ESUIElement_ON_Getter
    {
        public abstract ESUIElementCore Get(ESUIElementCore on, ESUIElementCore from);
    }
    #endregion

    [Serializable,TypeRegistryItem("0A扩展-转换作用源")]
    public class OutputOperationUI_Expand_SwitchOnFrom : IOutputOperationUI
    {
        [SerializeReference, LabelText("作用在")]
        public ESUIElement_ON_Getter getter = new ESUIElementGetter_DirectHandRefer();
        
        [SerializeReference, LabelText("执行内容")]
        public IOutputOperationUI op;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TryOperation(ESUIElementCore on, ESUIElementCore from, ILink_UI_OperationOptions with)
        {
            var get = getter.Get(on,from);
            if (get != null)
            {
                op.TryOperation(get,from,with);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TryCancel(ESUIElementCore on, ESUIElementCore from, ILink_UI_OperationOptions with)
        {
            var get = getter.Get(on, from);
            if (get != null)
            {
                op.TryCancel(get, from, with);
            }
        }
       
    }

}
