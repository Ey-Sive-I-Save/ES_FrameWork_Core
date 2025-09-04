#if UNITY_EDITOR
using UnityEditorInternal;
#endif


using ES;
using ES.Pointer;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


using static ES.EnumCollect;

//两者共性
using IPointerForSystemObject = ES.Pointer;



namespace ES.Pointer
  
{

    #region 基础包
    //基础针包
    public abstract class PointerPackerBase<Back, On, From, With, USE> : IPointer<Back, On, From, With>
    {
        public virtual string intName => "上次指针";
        [BoxGroup("针包", showLabel: false), LabelText(@"@intName", SdfIconType.Pin)] public int intHepler;
        [BoxGroup("针包", showLabel: false), Indent(2), LabelText("针包", SdfIconType.BagCheckFill), SerializeReference, PropertyOrder(1)] public List<USE> pointers = new List<USE>();

        public virtual Back Pick(On on = default, From from = default, With with = default)
        {
            return default;
        }
        object IPointer.Pick(object a, object b, object c)
        {
            return Pick();
        }
    }
    //基础值集合
    public abstract class PointerForValueListBase<Back, On, From, With, Use> : IPointer<Back, On, From, With>
    {
        public virtual string intName => "上次指针";
        [BoxGroup("全部值", showLabel: false), LabelText(@"@intName", SdfIconType.Pin)] public int intHepler;
        [BoxGroup("全部值", showLabel: false), Indent(2), LabelText("全部值", SdfIconType.BagCheckFill), PropertyOrder(1)] public List<Use> values = new List<Use>();

        public virtual Back Pick(On on = default, From from = default, With with = default)
        {
            return default;
        }
        object IPointer.Pick(object a, object b, object c)
        {
            return Pick();
        }
    }
    [Serializable, TypeRegistryItem("字符串针包_选中几个")]
    public class PointerForBaseIPointer_PackerSelectSome : PointerPackerForSelectSomeBack<String, IPointerForString_Only>, IPointerForStringList
    {
        object IPointer.Pick(object a, object b, object c)
        {
            return (this as PointerPackerForSelectSomeBack<String, IPointerForString_Only>)?.Pick();
        }
    }
    #endregion
    #region 常规重写包
    //多back针包
    public abstract class PointerPackerForSelectSomeBack<Back, Use> : PointerPackerBase<List<Back>, object, object, object, Use>, IPointerOnlyBackList<Back> where Use : IPointer<Back, object, object, object>
    {
        public override string intName => "预期长度";
        [LabelText("筛选方式", SdfIconType.Shuffle), Indent(-1), PropertyOrder(-1)]
        public EnumCollect.PointerSelectSomeType selectSomeType;
        object IPointer.Pick(object a, object b, object c)
        {
            return Pick();
        }
        public override List<Back> Pick(object on = null, object from = null, object with = null)
        {
            if (pointers != null)
            {
                if (pointers.Count > 0)
                {

                    if (pointers.Count > 1)
                    {
                        List<Use> ps = pointers.Where(n => n != null).ToList();
                        List<Back> bs = new List<Back>();
                        switch (selectSomeType)
                        {
                            case EnumCollect.PointerSelectSomeType.AllNotNull:
                                foreach (var i in ps)
                                {
                                    Back b = i.Pick();
                                    if (b != null) bs.Add(b);
                                }
                                break;
                            case EnumCollect.PointerSelectSomeType.StartSome:
                                for (int i = 0; i < ps.Count && i < intHepler; i++)
                                {
                                    Back b = ps[i].Pick();
                                    if (b != null) bs.Add(b);
                                }
                                break;
                            case EnumCollect.PointerSelectSomeType.EndSome:
                                for (int i = 0; i < ps.Count && i < intHepler; i++)
                                {
                                    Back b = ps[ps.Count - (1 + i)].Pick();
                                    if (b != null) bs.Add(b);
                                }
                                break;
                            case EnumCollect.PointerSelectSomeType.RandomSome:
                                int num = Mathf.Min(ps.Count, intHepler);
                                List<Use> ps2 = ps.OrderBy(n => UnityEngine.Random.value).Take(num).ToList();
                                foreach (var i in ps2)
                                {
                                    Back b = i.Pick();
                                    if (b != null) bs.Add(b);
                                }

                                break;
                            case EnumCollect.PointerSelectSomeType.Selector: break;
                            case EnumCollect.PointerSelectSomeType.TrySort: break;
                        }
                        return bs;
                    }
                }
            }
            return base.Pick(on, from, with);
        }
        public virtual void Addition(List<Back> backs)
        {

        }
    }
    //选一个back针
    public abstract class PointerPackerForOnlySelectBackOne<Back, USE> : PointerPackerBase<Back, object, object, object, USE>, IPointerOnlyBackSingle<Back> where USE : class, IPointer<Back, object, object, object>
    {
        [InfoBox("还没更改过该枚举值，确定收起警告？", VisibleIf = "@!ISee&&PointerSelectOneType.GetHashCode()==0"), GUIColor("@Color.red")]
        [ShowIfGroup("警告", VisibleIf = "@!ISee&&PointerSelectOneType.GetHashCode()==0"), PropertyOrder(-1)] public bool ISee = false;
        [LabelText("筛选方式", SdfIconType.Shuffle), Indent(-1), PropertyOrder(-1), GUIColor("@Color.gray"), EnumToggleButtons, OnValueChanged("Isee_")] public EnumCollect.PointerSelectOneType PointerSelectOneType;
        private void Isee_()
        {
            ISee = true;
        }

        public override Back Pick(object on = null, object from = null, object with = null)
        {

            var p = ESStaticDesignUtility.Function.GetOne(pointers, PointerSelectOneType, ref intHepler);
            if (intHepler >= 0 && p != null) return p.Pick();
            return base.Pick(on, from, with);
        }
        object IPointer.Pick(object a, object b, object c)
        {
            return Pick();
        }
    }
    //back值包
    public abstract class PointerForValueListSelectSomeBack<Back> : PointerForValueListBase<List<Back>, object, object, object, Back>, IPointerOnlyBackList<Back>
    {
        public override string intName => "预期长度";
        [LabelText("筛选方式", SdfIconType.Shuffle), Indent(-1), PropertyOrder(-1)]
        public EnumCollect.PointerSelectSomeType selectSomeType;

        public override List<Back> Pick(object on = null, object from = null, object with = null)
        {
            var p = ESStaticDesignUtility.Function.GetSome(values, selectSomeType, ref intHepler);
            if (intHepler >= 0 && p != null) return p;

            return base.Pick(on, from, with);
        }
        object IPointer.Pick(object a, object b, object c)
        {
            return Pick();
        }
    }
    //单back有值包
    public abstract class PointerForValueListSelectBackOne<Back> : PointerForValueListBase<Back, object, object, object, Back>, IPointerOnlyBackSingle<Back>
    {
        [InfoBox("还没更改过该枚举值，确定收起警告？", VisibleIf = "@!ISee&&PointerSelectOneType.GetHashCode()==0"), GUIColor("@Color.red")]
        [ShowIfGroup("警告", VisibleIf = "@!ISee&&PointerSelectOneType.GetHashCode()==0"), PropertyOrder(-1)] public bool ISee = false;
        [LabelText("筛选方式", SdfIconType.Shuffle), Indent(-1), PropertyOrder(-1), GUIColor("@Color.gray"), EnumToggleButtons, OnValueChanged("Isee_")] public EnumCollect.PointerSelectOneType PointerSelectOneType;
        private void Isee_()
        {
            ISee = true;
        }

        public override Back Pick(object on = null, object from = null, object with = null)
        {

            var p = ESStaticDesignUtility.Function.GetOne(values, PointerSelectOneType, ref intHepler);
            if (intHepler >= 0 && p != null) return p;
            return base.Pick(on, from, with);
        }
        object IPointer.Pick(object a, object b, object c)
        {
            return Pick();
        }
    }
    #endregion
    #region 特殊重写包
    //同by 包
    public abstract class PointerPackForSameByOnlyOne<Back, By, USE> : PointerPackerBase<Back, By, object, object, USE>
    {
        public abstract By byFrom { get; }

        public EnumCollect.PointerSelectOneType selectSomeType;
        public override Back Pick(By by = default, object yarn = null, object on = null)
        {
            var p = ESStaticDesignUtility.Function.GetOne(pointers, selectSomeType, ref intHepler);
            if (intHepler >= 0 && p != null) return PickOne(p);
            return default;
        }

        public abstract Back PickOne(USE use);
    }
    //链模式
    public abstract class PointerPackForSimpleChain<Back, USE> : PointerPackerBase<Back, object, object, object, USE>, IPointerOnlyBack<Back> where USE : IPointerChain<Back>
    {
        public abstract Back head { get; }
        public override Back Pick(object by = default, object yarn = null, object on = null)
        {
            Back back = head;
            if (pointers == null || pointers.Count == 0) return default;
            for (int i = 0; i < pointers.Count; i++)
            {
                if (pointers[i] == null) continue;
                back = pointers[i].Pick(back);
            }
            return back;
        }
        object IPointer.Pick(object a, object b, object c)
        {
            return Pick();
        }
    }

    public abstract class PointerPackForDynamicChain<Back, AtFirst, USE, AtLast> : PointerPackerBase<Back, object, object, object, USE>, IPointerOnlyBack<Back> where USE : IPointerChain, IPointer
        where AtFirst : IPointer, IPointerChain
        where AtLast : IPointer, IPointerChain
    {
        public abstract AtFirst head { get; }
        public abstract AtLast end { get; }
        public override Back Pick(object launcherEntity = default, object yarn = null, object on = null)
        {
            if (head == null || end == null) { Debug.LogError("链条首位不完整"); return default(Back); }
            object headUse = head?.Pick(launcherEntity, launcherEntity, on);
            //Debug.Log("筛选的头部"+headUse);

            return PickAfterHead(headUse, launcherEntity, on);
        }
        public Back PickAfterHead(object startAsHead, object launcherEntity, object on = null)
        {
            object headUse;
            object current;
            headUse = current = startAsHead;
            if (pointers == null || pointers.Count == 0) { }
            else
            {
                for (int i = 0; i < pointers.Count; i++)
                {
                    if (pointers[i] == null) { /*Debug.LogError("链条断裂");*/ continue; }
                    current = pointers[i].Pick(current, headUse);
                }
            }
            //Debug.Log("尾部"+current+headUse+launcherEntity);
            Back back = ESStaticDesignUtility.Matcher.SystemObjectToT<Back>(end.Pick(current, launcherEntity));
            return back;

        }
        object IPointer.Pick(object a, object b, object c)
        {
            return Pick();
        }
    }

    public abstract class PointerPackerForSelectSome_BackSelfDefine<Back, BackForUse, Use> : PointerPackerBase<Back, object, object, object, Use>, IPointerOnlyBack<Back> where Use : IPointer<BackForUse, object, object, object>
    {
        public override string intName => "预期长度";
        [LabelText("筛选方式", SdfIconType.Shuffle), Indent(-1), PropertyOrder(-1)]
        public EnumCollect.PointerSelectSomeType selectSomeType;

        public override Back Pick(object on = null, object from = null, object with = null)
        {
            if (pointers != null)
            {
                if (pointers.Count > 0)
                {

                    if (pointers.Count > 1)
                    {
                        List<Use> ps = pointers.Where(n => n != null).ToList();
                        List<BackForUse> bs = new List<BackForUse>();
                        switch (selectSomeType)
                        {
                            case EnumCollect.PointerSelectSomeType.AllNotNull:
                                foreach (var i in ps)
                                {
                                    BackForUse b = i.Pick();
                                    if (b != null) bs.Add(b);
                                }
                                break;
                            case EnumCollect.PointerSelectSomeType.StartSome:
                                for (int i = 0; i < ps.Count && i < intHepler; i++)
                                {
                                    BackForUse b = ps[i].Pick();
                                    if (b != null) bs.Add(b);
                                }
                                break;
                            case EnumCollect.PointerSelectSomeType.EndSome:
                                for (int i = 0; i < ps.Count && i < intHepler; i++)
                                {
                                    BackForUse b = ps[ps.Count - (1 + i)].Pick();
                                    if (b != null) bs.Add(b);
                                }
                                break;
                            case EnumCollect.PointerSelectSomeType.RandomSome:
                                int num = Mathf.Min(ps.Count, intHepler);
                                List<Use> ps2 = ps.OrderBy(n => UnityEngine.Random.value).Take(num).ToList();
                                foreach (var i in ps2)
                                {
                                    BackForUse b = i.Pick();
                                    if (b != null) bs.Add(b);
                                }

                                break;
                            case EnumCollect.PointerSelectSomeType.Selector: break;
                            case EnumCollect.PointerSelectSomeType.TrySort: break;
                        }
                        return Addition(bs);
                    }
                }
            }
            return base.Pick(on, from, with);
        }
        public virtual Back Addition(List<BackForUse> backs)
        {
            return default;
        }
        object IPointer.Pick(object a, object b, object c)
        {
            return Pick();
        }
    }
    #endregion







}

