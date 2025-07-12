using ES;
using ES.EvPointer;
using Sirenix.Serialization.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ES
{

    public class LinkReceiveList<Link>  where Link : ILink {  
        public SafeNormalList<IReceiveLink<Link>> IRS = new SafeNormalList<IReceiveLink<Link>>();
        public IReceiveLink<Link> cache;

        public void SendLink(Link link)
        {
            /* foreach(var i in IRS)//隐含ApplyBuffers
             {
                 i.OnLink(link);
             }*/

            IRS.ApplyBuffers();

            int count = IRS.ValuesNow.Count;
            for (int i = 0; i < count; i++)
            {
                cache = IRS.ValuesNow[i];
                if(cache is UnityEngine.Object ob)
                {
                    if (ob != null) cache.OnLink(link);
                    else IRS.TryRemove(cache);
                }
                else if (cache != null) cache.OnLink(link);
                else IRS.TryRemove(cache);
            }
        }
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        private void Internal_TryRemove(IReceiveLink<Link> ir)
        {
            Debug.Log("RemoveInfo");
            IRS.TryRemove(ir);
        }
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public void AddReceive(IReceiveLink<Link> e) 
        {
            IRS.TryAdd(e);
        }
        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public void RemoveReceive(IReceiveLink<Link> e)
        {
            IRS.TryRemove(e);
        }
    }
}

