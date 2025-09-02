using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES {
    #region 小型接口_内容不完善的接口
    public interface IInittable
    {
        void Init(params object[] ps);
    }
    public interface IRunLogic
    {
        object RunLogic(params object[] ps);
    }
    public interface ICancellable
    {
        object Cancel();
    }
    #endregion

}
