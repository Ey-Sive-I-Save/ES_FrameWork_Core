using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES
{

    public static partial class ESStaticLogicUtility
    {
        //取数据器
        public static class DataInfoPointer { 
      

            public static T PickASoInfoByKey<T>(string key, ISoDataPack pack = null) where T : class
            {
                var dic = pack?.AllInfos;
                if (dic != null)
                {

                    return dic[key] as T;
                }

                return default;
            }

            public static List<T> PickSoInfoListByKey<T>(string[] keys, ISoDataPack pack = null) where T : class
            {
                if (keys == null || keys.Length == 0) return new List<T>();
                var dic = pack?.AllInfos;
                if (dic != null)
                {
                    List<T> ts = new List<T>();
                    foreach (var i in keys)
                    {
                        ts.Add(dic[i] as T);
                    }
                    return ts;
                }

                return new List<T>();
            }
        }
    }
}

