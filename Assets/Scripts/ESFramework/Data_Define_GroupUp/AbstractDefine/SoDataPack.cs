using ES.EvPointer;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;

namespace ES
{
    public interface ISoDataPack : IWithStringKey
    {
        string _name { get; }
        Type GetSoInfoType();
        void AddInfosByGroup(ISoDataGroup group);
        void AddInfo(string s, ScriptableObject so);
        ISoDataInfo GetInfoByKey(string key);
        IDictionary AllInfos { get; }
        IEnumerable<string> InfoKeys { get; }
        List<ISoDataGroup> ApplingGroups { get; }
        bool EnableAutoRefresh { get; }
        void Check();
    }
    public abstract class SoDataPack<Info> : SerializedScriptableObject, ISoDataPack where Info : ScriptableObject, ISoDataInfo
    {
        [LabelText("该包的键")] public KeyString_Direct keyString = new KeyString_Direct();
        public KeyString key => keyString;
        [LabelText("启用自动更新")] public bool enableAutoRefresh = true;
        [LabelText("已经应用过的数据组 列表"), NonSerialized, OdinSerialize]
        public List<ISoDataGroup> applingGroups = new List<ISoDataGroup>();
        [LabelText("预览全部数据")]
        public Dictionary<string, Info> allInfos = new Dictionary<string, Info>();
        public IDictionary AllInfos => allInfos;
        public string _name => keyString.str_direc;

        public List<ISoDataGroup> ApplingGroups => applingGroups;

        public bool EnableAutoRefresh => enableAutoRefresh;

        public IEnumerable<string> InfoKeys => allInfos.Keys;

        public Type GetSoInfoType()
        {
            return typeof(Info);
        }

        public void SetKey(object o)
        {
            keyString.str_direc = o.ToString();
        }

        public void AddInfo(string k, ScriptableObject so)
        {
            if (allInfos.ContainsKey(k) && allInfos[k] != null)
            {
                Debug.LogWarning($"发现重复的键{k}，默认跳过处理");
            }
            else if (so is Info info)
            {
                allInfos[k]=info;
            }
            else
            {
                Debug.LogWarning($"发现无效或者已经销毁的内容，键{k}，值{so}");
            }
        }

        public void AddInfosByGroup(ISoDataGroup group)
        {
            if (group.getSoInfoType() != typeof(Info))
            {
                Debug.LogError("试图加入不合法数据组:" + group._name);
                return;
            }
            var keys = group.AllKeys;
            //加入已经缓存
            if (applingGroups.Contains(group))
            {

            }
            else
            {
                applingGroups.Add(group);
            }
            foreach (var k in keys)
            {
                ISoDataInfo use = group.GetInfoByKey(k);
                var so = use as SerializedScriptableObject;
                if(so!=null)
                this.AddInfo(use.key.str_direc, so);
            }
        }

        public ISoDataInfo GetInfoByKey(string key)
        {
            if(allInfos.TryGetValue(key,out var value))
            {
                return value;
            }
            return null;
        }

        public void Check()
        {
            var keys = allInfos.Keys.ToArray();
            for(int i = 0; i < keys.Length; i++)
            {
                var info = allInfos[keys[i]];
                if ((info as UnityEngine.Object) == null) { allInfos.Remove(keys[i]);continue; }
                if (info.key.str_direc != keys[i])
                {
                    allInfos.Remove(keys[i]);
                    allInfos.Add(info.key.str_direc, info);
                }
            }
            foreach(var (i,k) in allInfos)
            {
               
            }
        }
    }


}
