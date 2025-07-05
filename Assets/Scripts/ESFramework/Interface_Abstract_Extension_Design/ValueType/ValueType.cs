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
    [Serializable/*安全列表IOC*/]
    public abstract class SafeListIOC<Key, Element>
    {
        [SerializeReference]
        [LabelText(@"@  Editor_ShowDes ", icon: SdfIconType.ListColumnsReverse), GUIColor("Editor_ShowColor")]
        public Dictionary<Key, SafeBasicList<Element>> IOC = new Dictionary<Key, SafeBasicList<Element>>();
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

            SafeBasicList<Element> elements = default;
            if (IOC.ContainsKey(k))
            {
                elements = IOC[k];
                if (isRuntime) elements.ValuesBufferToAdd.Add(e);
                else elements.ValuesNow.Add(e);
            }
            else
            {
                elements = new SafeBasicList<Element>();
                if (isRuntime) elements.ValuesBufferToAdd.Add(e);
                else elements.ValuesNow.Add(e);
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
            SafeBasicList<Element> elements = default;
            if (IOC.ContainsKey(k))
            {
                elements = IOC[k];
                if (isRuntime) elements.ValuesBufferToRemove.Add(e);
                else elements.ValuesNow.Remove(e);
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
                foreach (var i in IOC[k].ValuesNow)
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

                return IOC[k].ValuesNow;
            }

            return (IOC[k] = new SafeBasicList<Element>()).ValuesNow;
        }
    }
    



  
    #region 特殊IOC —— 原型参数键池专属参数集合
    [Serializable, TypeRegistryItem("原型参数IOC")/*技能树参数分类-*/]
    public class ArchitectureKeyValuePoolTypeListIOC : KeyGroup<EnumCollect.ArchitectureKeyValuePoolType, IArchitectureKeyValuePoolTypeValue>
    {
        public Action<EnumCollect.ArchitectureKeyValuePoolType, IArchitectureKeyValuePoolTypeValue> OnHandle = (at, who) => { };
    }

    #endregion



}