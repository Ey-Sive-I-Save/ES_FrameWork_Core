using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


namespace ES {
        public class LinkReceiveList<Channel,Link> where Link : ILink
        {
            public SafeNormalList<IReceiveChannelLink<Channel,Link>> IRS = new SafeNormalList<IReceiveChannelLink<Channel, Link>>();
            public IReceiveChannelLink<Channel, Link> cache;

            public void SendLink(Channel channel,Link link)
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
                    if (cache is UnityEngine.Object ob)
                    {
                        if (ob != null) cache.OnLink(channel,link);
                        else IRS.TryRemove(cache);
                    }
                    else if (cache != null) cache.OnLink(channel, link);
                    else IRS.TryRemove(cache);
                }
            }
            [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
            private void Internal_TryRemove(IReceiveChannelLink<Channel, Link> receive)
            {
                IRS.TryRemove(receive);
            }
            [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
            public void AddReceive(IReceiveChannelLink<Channel, Link> receive)
            {
                IRS.TryAdd(receive);
            }
            [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
            public void RemoveReceive(IReceiveChannelLink<Channel, Link> receive)
            {
                IRS.TryRemove(receive);
            }
        }
    //
}
