using ES.EvPointer;
using ES;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.Rendering.DebugUI;
using System.Runtime.CompilerServices;

namespace ES
{
    [Serializable]
    public class SafeUpdateList<T>
    {

        [LabelText("正在更新", SdfIconType.ArrowRepeat), SerializeReference, GUIColor("@KeyValueMatchingUtility.ColorSelector.ColorForUpdating")] public List<T> valuesNow_ = new List<T>(10);
        [FoldoutGroup("缓冲")][LabelText("缓冲添加", SdfIconType.BoxArrowInLeft), SerializeReference] public List<T> valuesToAdd = new List<T>();
        [FoldoutGroup("缓冲")][LabelText("缓冲移除", SdfIconType.BoxArrowRight), SerializeReference] public List<T> valuesToRemove = new List<T>();
        public Action<bool, T> OnChange = (Add, What) => { };

        public void Update()
        {
            foreach (var i in valuesToAdd)
            {
                if (valuesNow_.Contains(i)) continue;
                OnChange.Invoke(true, i);
                valuesNow_.Add(i);
            }
            foreach (var i in valuesToRemove)
            {
                if (!valuesNow_.Contains(i)) continue;
                OnChange.Invoke(false, i);
                valuesNow_.Remove(i);
            }
            valuesToAdd.Clear();
            valuesToRemove.Clear();
        }
    }

    [Serializable/*Link收发列表IOC*/]
    public class LinkRecieveSafeListIOC : TypeSafeListIOC<IReceiveLink>
    {
        public override string IOCName => "Link收发表";
        public void SendLink<Link>(Link link) where Link : ILink
        {

            List<IReceiveLink> links = FindList<Link>();
            foreach (var i in links)
            {

                if (i is IReceiveLink<Link> irl)
                {
                    irl.OnLink(link);
                }
            }
        }
        public IReceiveLink AddElement<Link>(IReceiveLink<Link> e) where Link : ILink
        {
            return AddElement(typeof(Link), e);
        }
        public void RemoveElement<Link>(IReceiveLink<Link> e) where Link : ILink
        {
            RemoveElement(typeof(Link), e);
        }
    }
    [Serializable/*原型全匹配安全列表IOC*/]
    public sealed class ArchutectureTypeMatchSafeListIOC : TypeMatchSafeListIOC<IArchitecture>
    {
        public override string IOCName => "类型匹配原型表(唯一)";
    }
    [Serializable/*类型全匹配安全列表IOC*/]
    public class TypeMatchSafeListIOC<Element> : TypeSafeListIOC<Element>
    {
        public override string IOCName => "类型键直接匹配IOC表";
        public Element AddElement(Element e)
        {
            return AddElement(e.GetType(), e);
        }
        public void RemoveElement(Element e)
        {
            RemoveElement(e.GetType(), e);
        }
        public override Element Find(Type k)
        {
            Element e = base.Find(k);
            if (e != null) return e;
            return AddElement(k, (Element)Activator.CreateInstance(k));
        }
        public override List<Element> FindList(Type k)
        {
            List<Element> es = base.FindList(k);
            if (es != default) return es;
            Debug.Log(k.Name);
            Debug.Log(typeof(Element).Name);
            AddElement(k, (Element)Activator.CreateInstance(k));
            return base.FindList(k);
        }
    }
    [Serializable/*类型全匹配列表IOC*/]
    public class TypeMatchListIOC<Element> : TypeListIOC<Element>
    {
        public override string IOCName => "类型键直接匹配IOC表";
        public Element AddElement(Element e)
        {
            return AddElement(e.GetType(), e);
        }
        public void RemoveElement(Element e)
        {
            RemoveElement(e.GetType(), e);
        }
        public override Element Find(Type k)
        {
            Element e = base.Find(k);
            if (e != null) return e;
            return AddElement(k, (Element)Activator.CreateInstance(k));
        }
        public override List<Element> FindList(Type k)
        {
            List<Element> es = base.FindList(k);
            if (es != default) return es;
            Debug.Log(k.Name);
            Debug.Log(typeof(Element).Name);
            AddElement(k, (Element)Activator.CreateInstance(k));
            return base.FindList(k);
        }
    }
    [Serializable/*基本安全列表IOC*/]
    public class TypeSafeListIOC<Element> : SafeListIOC<Type, Element>
    {
        public override string IOCName => "类型键IOC表";
        public virtual Element Find<T>()
        {
            return Find(typeof(T));
        }
        public virtual List<Element> FindList<T>()
        {
            Debug.Log("sendLink2");
            return FindList(typeof(T));
        }
    }
    [Serializable/*类型列表IOC*/]
    public class TypeListIOC<Element> : BaseListIOC_Arch_KeyAndList<Type, Element>
    {
        public override string IOCName => "类型键IOC表";
        public virtual Element Find<T>()
        {
            return Find(typeof(T));
        }
        public virtual List<Element> FindList<T>()
        {

            return FindList(typeof(T));
        }
    }
    [Serializable/*安全列表IOC*/]
    public abstract class SafeListIOC<Key, Element>
    {
        [SerializeReference]
        [LabelText(@"@  IOCName ", icon: SdfIconType.ListColumnsReverse), GUIColor("IOCColor")]
        public Dictionary<Key, SafeUpdateList<Element>> IOC = new Dictionary<Key, SafeUpdateList<Element>>();
        public Action OnChange = () => { };
        public virtual string IOCName => "标准安全IOC表";
        public virtual Color IOCColor => Color.yellow;
        public Element AddElementRuntime(Key k, Element e)
        {
            return AddElement(k, e, true);
        }
        public Element AddElement(Key k, Element e, bool isRuntime = false)
        {
            if (e == null) return default;

            SafeUpdateList<Element> elements = default;
            if (IOC.ContainsKey(k))
            {
                elements = IOC[k];
                if (isRuntime) elements.valuesToAdd.Add(e);
                else elements.valuesNow_.Add(e);
            }
            else
            {
                elements = new SafeUpdateList<Element>();
                if (isRuntime) elements.valuesToAdd.Add(e);
                else elements.valuesNow_.Add(e);
                IOC.Add(k, elements);
            }
            OnChange.Invoke();
            return e;
        }
        public void RemoveElementRuntime(Key k, Element e)
        {
            RemoveElement(k, e, true);
        }
        public void RemoveElement(Key k, Element e, bool isRuntime = false)
        {
            SafeUpdateList<Element> elements = default;
            if (IOC.ContainsKey(k))
            {
                elements = IOC[k];
                if (isRuntime) elements.valuesToRemove.Add(e);
                else elements.valuesNow_.Remove(e);
                OnChange.Invoke();
            }
            else
            {
#if UNITY_EDITOR
                throw new Exception("IOC没有这种键");
#endif
            }
        }
        public virtual Element Find(Key k)
        {

            if (IOC.ContainsKey(k))
            {
                foreach (var i in IOC[k].valuesNow_)
                {
                    return i;
                }
            }
            return default;
        }

        public virtual List<Element> FindList(Key k)
        {

            if (IOC.ContainsKey(k))
            {

                return IOC[k].valuesNow_;
            }

            return (IOC[k] = new SafeUpdateList<Element>()).valuesNow_;
        }
    }
    [Serializable/*基本列表IOC*/]
    public abstract class BaseListIOC_Arch_KeyAndList<Key, Element>
    {
        [SerializeReference]
        [LabelText(@"@  IOCName ", icon: SdfIconType.ListColumnsReverse), GUIColor("IOCColor")]
        public Dictionary<Key, List<Element>> IOC = new Dictionary<Key, List<Element>>();

        public virtual string IOCName => "标准列表IOC表";
        public virtual Color IOCColor => Color.yellow;

        public Element AddElementRuntime(Key k, Element e)
        {
            return AddElement(k, e, true);
        }
        public Element AddElement(Key k, Element e, bool isRuntime = false)
        {
            if (e == null) return default;
            List<Element> elements = default;
            if (IOC.ContainsKey(k))
            {
                elements = IOC[k];
                bool has = false;
                foreach (var i in elements)
                {
                    if (i.Equals(e))
                    {
                        has = true;
                        break;
                    }
                }
                if (!has)
                    elements.Add(e);
            }
            else
            {
                elements = new List<Element>();
                elements.Add(e);
                IOC.Add(k, elements);
            }
            return e;
        }
        public void RemoveElementRuntime(Key k, Element e)
        {
            RemoveElement(k, e, true);
        }
        public void RemoveElement(Key k, Element e, bool isRuntime = false)
        {
            List<Element> elements = default;
            if (IOC.ContainsKey(k))
            {
                elements = IOC[k];
                elements.Remove(e);
            }
            else
            {
#if UNITY_EDITOR
                throw new Exception("IOC没有这种键");
#endif
            }
        }
        public virtual Element Find(Key k)
        {

            if (IOC.ContainsKey(k))
            {
                foreach (var i in IOC[k])
                {
                    return i;
                }
            }
            return default;
        }

        public virtual List<Element> FindList(Key k)
        {
            if (IOC.ContainsKey(k))
            {
                return IOC[k];
            }
            return (IOC[k] = new List<Element>());
        }



    }



    [Serializable/*基本字典IOC*/]
    public abstract class BaseDicIOC_TypeSelectAndKeyValue<TypeSelect_, Key, Element>
    {
        [SerializeReference]
        [LabelText(@"@  IOCName ", icon: SdfIconType.ListColumnsReverse), GUIColor("IOCColor")]
        public Dictionary<TypeSelect_, Dictionary<Key, Element>> IOC = new Dictionary<TypeSelect_, Dictionary<Key, Element>>();

        public virtual string IOCName => "标准字典IOC表";
        public virtual Color IOCColor => Color.yellow;

        public Element AddElementRuntime(TypeSelect_ t, Key k, Element e)
        {
            return AddElement(t, k, e, true);
        }
        public Element AddElement(TypeSelect_ t, Key k, Element e, bool isRuntime = false)
        {
            if (e == null) return default;
            Dictionary<Key, Element> elements = default;
            if (IOC.ContainsKey(t))
            {
                elements = IOC[t];

                elements.Add(k, e);
            }
            else
            {
                elements = new Dictionary<Key, Element>();
                elements.Add(k, e);
                IOC.Add(t, elements);
            }
            return e;
        }
        public void RemoveElementRuntime(TypeSelect_ t, Key k)
        {
            RemoveElement(t, k, true);
        }
        public void RemoveElement(TypeSelect_ t, Key k, bool isRuntime = false)
        {
            Dictionary<Key, Element> elements = default;
            if (IOC.ContainsKey(t))
            {
                elements = IOC[t];
                elements.Remove(k);
            }
            else
            {
#if UNITY_EDITOR
                throw new Exception("IOC没有这种键");
#endif
            }
        }
        public virtual Element Find(TypeSelect_ t, Key k)
        {

            if (IOC.ContainsKey(t))
            {
                foreach (var (i, d) in IOC[t])
                {
                    if (i.Equals(k))
                    {
                        return d;
                    }
                }
            }
            return default;
        }

        public virtual Dictionary<Key, Element> FindDic(TypeSelect_ k)
        {
            if (IOC.ContainsKey(k))
            {
                return IOC[k];
            }
            return IOC[k] = new Dictionary<Key, Element>();
        }

        public bool Start(IArchitecture hosting, bool asVirtual = false)
        {
            throw new NotImplementedException();
        }

        public bool OnWithDrawHosting(IArchitecture hosting, bool asVirtual = false)
        {
            throw new NotImplementedException();
        }


    }
    [Serializable/*基本String键字典IOC*/]
    public abstract class BaseDicIOCWithStringKey<TypeSelect_, Element> : BaseDicIOC_TypeSelectAndKeyValue<TypeSelect_, string, Element>
    {

    }
    [Serializable/*基本String键字典IOC*/]
    public abstract class BaseDicIOCWithStringKeyAndStringSelect<Element> : BaseDicIOC_TypeSelectAndKeyValue<string, string, Element>
    {

    }
    [Serializable, TypeRegistryItem("字符串分组+键 Type映射")]
    public class DicIOCWithStringSelectStringKey_TypeValue : BaseDicIOCWithStringKeyAndStringSelect<Type>
    {

    }
    #region 特殊IOC —— 原型参数键池专属参数集合
    [Serializable, TypeRegistryItem("原型参数IOC")/*技能树参数分类-*/]
    public class ArchitectureKeyValuePoolTypeListIOC : BaseListIOC_Arch_KeyAndList<EnumCollect.ArchitectureKeyValuePoolType, IArchitectureKeyValuePoolTypeValue>
    {
        public Action<EnumCollect.ArchitectureKeyValuePoolType, IArchitectureKeyValuePoolTypeValue> OnHandle = (at, who) => { };
    }

    #endregion



}