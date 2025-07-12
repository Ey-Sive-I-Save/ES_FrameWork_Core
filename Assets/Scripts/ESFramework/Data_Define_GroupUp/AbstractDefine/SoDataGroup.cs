using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

namespace ES
{
    //  [CreateAssetMenu(fileName = "SoDataInfoGroup",menuName = "EvData/SoGroup")]
    public interface ISoDataGroup
    {
        string _name { get; }
        Type getSoInfoType();
        bool ContainsInfo(string s);
        void AddInfo(string key, ScriptableObject o);
        List<string> AllKeys { get; }
        ISoDataInfo GetInfoByKey(string k);
        void RemoveInfo(string s);
    }
    public abstract class SoDataGroup<SoType> : SerializedScriptableObject, ISoDataGroup where SoType : ScriptableObject, ISoDataInfo
    {

        [LabelText("数据组名字"), GUIColor("@KeyValueMatchingUtility.ColorSelector.Color_03")] public string name__ = "数据组";
        [LabelText("数据组字典")]
        public Dictionary<string, SoType> keyValues = new Dictionary<string, SoType>();
        public string _name => name__;
        public List<string> AllKeys => keyValues.Keys.ToList();
        public void AddInfo(string s, ScriptableObject o)
        {
            if (keyValues.ContainsKey(s))
            {
                //键重复
            }
            else if (o is SoType typeMatch)
            {
                keyValues.Add(s, typeMatch);
            }
        }
        public bool ContainsInfo(string s)
        {
            if (keyValues.ContainsKey(s)) return false;
            else return true;
        }

        public ISoDataInfo GetInfoByKey(string k)
        {
            if (keyValues.ContainsKey(k))
            {
                return keyValues[k];
            }
            else
            {
                return default;
            }
        }

        public Type getSoInfoType()
        {
            return typeof(SoType);
        }
        public void RemoveInfo(string k)
        {
            if (keyValues.TryGetValue(k, out var info))
            {
                keyValues.Remove(k);

            }
        }
    }
}
