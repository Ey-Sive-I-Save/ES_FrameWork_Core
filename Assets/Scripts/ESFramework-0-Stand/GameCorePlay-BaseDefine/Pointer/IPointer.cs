using ES;
using ES.Pointer;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace ES {
    public interface IPointer
    {
        public object Pick(object a = default, object b = default, object c = default);
        public string PickToString()
        {
            return Pick()?.ToString();
        }
    }
    #region 针与针包的初始定义
    //原始接口


    #region 分类扩展接口
    #region 常用扩展
    //纯值针
    public interface IPointerOnlyBack<out Back> : IPointer<Back, object, object, object>
    {

    }
    //接口
    public interface IPointer<out Back, in ON, in From, in With> : IPointer
    {
        Back Pick(ON on = default, From from = default, With with = default);

    }
    public interface IPointerNotBack<in ON, in From, in With> : IPointer<object, ON, From, With>
    {
        new void Pick(ON on = default, From from = default, With with = default);
        object IPointer<object, ON, From, With>.Pick(ON on, From from, With with)
        {
            Pick(on, from, with);
            return null;
        }
        object IPointer.Pick(object a, object b, object c)
        {
            Pick(a, b, c);
            return null;
        }
    }
    public interface IPointerOnlyBackList<Single> : IPointerOnlyBackIEnumerable<List<Single>>
    {

    }
    //单值针
    public interface IPointerOnlyBackSingle<Back> : IPointerOnlyBack<Back>
    {

    }
    #endregion
    #region 废案或者还没怎么用的扩展
    public interface IPointer<Back, in Link, Head> : IPointerChainAny<Back, Link, Head, object> 
    {
        Back Pick(Link link);
        // Back PickByLink(Link link) { if (link != null) return Pick(link.By_, link.Yarn_, link.On_); return default(Back); }

    }
    //多值针
    public interface IPointerOnlyBackIEnumerable<IE> : IPointerOnlyBack<IE>
    {

    }
    public class PointerOnlyBackFunc<Back> : IPointerOnlyBack<Back>
    {
        public Back Pick(object on = null, object from = null, object with = null)
        {
            return backFunc.Invoke();
        }
        public Func<Back> backFunc => default;
        object IPointer.Pick(object a, object b, object c)
        {
            return Pick();
        }
    }

    public class PointerOnlyBackDirect<Back> : IPointerOnlyBack<Back>
    {
        object IPointer.Pick(object a, object b, object c)
        {
            return Pick();
        }
        public Back Pick(object on = null, object from = null, object with = null)
        {
            return back;
        }
        public Back back;
    }
    #endregion
    #endregion

    #endregion

    #region 重要扩展接口-链
    public interface IPointerChain : IPointer { }

    public interface IPointerChain<T> : IPointer<T, T, object, object>, IPointerChain
    {

    }
    public interface IPointerChainAny<Next, in Last, Head, On> : IPointer<Next, Last, Head, On>, IPointerChain
    {

    }
    public interface IPointerChainLink<Next, in Last> : IPointer<Next, Last, object, object>, IPointerChain 
    {

    }
    #endregion


    #region 杂碎功能
    #region 返回针的针
    public interface IPointerForIPointer<On, From, With> : IPointer<IPointer, On, From, With>
    {

    }
    public interface IPointerForIPointer_Only : IPointerForIPointer<object, object, object>, IPointerOnlyBack<IPointer>
    {
        object IPointer.Pick(object a, object b, object c)
        {
            return Pick();
        }
    }
    #endregion



    #endregion
}
