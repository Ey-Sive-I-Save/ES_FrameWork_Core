using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ES.EnumCollect;

namespace ES
{
    /*
     * Link任务键组 就是处理一个带参数的Link事物，强调所在环境，最终可能被发送到某个池
     */
    [Serializable, TypeRegistryItem("Link任务键组")]
    public class LinkWaitingToDoTaskPool : SafeKeyGroup<LinkTaskEnvironment, ILink> /**/
    {
        public override string Editor_ShowDes => "Link任务处理键组";
        bool Dirty = false;
        public void RunAllTasks()
        {
            if (Dirty)
            {

                foreach (var (e, k) in Groups)
                {
                    if (k.MayHasElement)
                        foreach (var link in k)
                        {
                            HandleOneLink(e, link);
                        }
                }
                Groups.Clear();
            }
            //无新任务
        }
        public void RunEnviromentTasks(LinkTaskEnvironment e)
        {
            var group = GetGroupDirectly(e);
            if (group.MayHasElement)
                foreach (var link in group)
                {
                    HandleOneLink(e, link);
                }
            group.Clear();
        }
        public virtual void HandleOneLink(LinkTaskEnvironment t, ILink link)
        {
            if (link is IAutoTaskLink taskLink)
            {
                taskLink.RunTaskLinkAt(t);
            }
        }
        public void AddLink<Link>(LinkTaskEnvironment e, Link link) where Link : ILink
        {
            TryAdd(e, link);
            Dirty = true;
        }
        public void RemoveLink<Link>(LinkTaskEnvironment e, Link link) where Link : ILink
        {
            TryRemove(e, link);
            Dirty = true;
        }
    }

}


