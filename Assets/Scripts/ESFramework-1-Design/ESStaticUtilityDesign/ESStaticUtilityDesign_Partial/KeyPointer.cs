using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ES
{

    public static partial class ESStaticDesignUtility
    {
        //数据针集合器
        public static class KeyPointer
        {
            //为了性能，分成5种把
            public static string[] PickPackAllKeys(ISoDataPack pack)
            {
                var skeys = pack?.AllInfos?.Keys;
                int ii = skeys?.Count ?? 0;
                string[] keys = new string[ii];
                if (ii > 0)
                {
                    for (int i = 0; i < ii; i++)
                    {
                        skeys.CopyTo(keys, 0);
                    }
                }

                return keys;

            }

        }
    }
}

