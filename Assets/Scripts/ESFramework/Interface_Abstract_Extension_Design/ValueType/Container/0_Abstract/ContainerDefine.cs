using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ES
{
    //安全列表 接口
    public interface ISafeList<T>
    {
        public void TryAdd(T add);
        public void TryRemove(T remove);
        public bool TryContains(T who);
        public void TryApplyBuffers();//不强制更新
        public void ApplyBuffers(bool force=false);//可选强制更新
    }
}

