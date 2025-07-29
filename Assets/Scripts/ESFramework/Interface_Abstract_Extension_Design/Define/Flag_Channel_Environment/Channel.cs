using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//只是一种定义 用来规范架构 师出有名 没有实际逻辑
/* Channel非常简单 ，一般就是一个带类型标识的类
 * 他旨在为同参数，同类型的事件进行一个特征鲜明的初步划分
 * 用来减少频繁事件的遍历if 损耗 和 用于决定接下来的任务走向
 * 主要的 和 Link收发事件  集成
 *  由于 逆变不支持 数据类型 所以推荐使用的是class static channel 的形式
 * 一般只对于设计层进行 大部分的零碎枚举 分布在各自的文件或者EnumCollect里
 */
public interface IChannel
{

}
public class Channel<T> : IChannel where T : Channel<T>,new()
{
    public static T channel = new T();
}

public interface IChannel_Example_ItemGetter_ : IChannel
{

}
public class Channel_Example_ItemGetter_<T> : Channel<T>, IChannel_Example_ItemGetter_ where T : Channel_Example_ItemGetter_<T>, new()
{

}

public class Channel_Example_ItemGetter_GetByPick : Channel_Example_ItemGetter_<Channel_Example_ItemGetter_GetByPick>
{

}
public class Channel_Example_ItemGetter_GetBySplit : Channel_Example_ItemGetter_<Channel_Example_ItemGetter_GetBySplit>
{

}
public class Channel_Example_ItemGetter_GetByBuy : Channel_Example_ItemGetter_<Channel_Example_ItemGetter_GetByBuy>
{

}
public class Channel_Example_ItemGetter_GetByReward : Channel_Example_ItemGetter_<Channel_Example_ItemGetter_GetByReward>
{

}
public class Channel_Example_ItemGetter_GetByOtherGive : Channel_Example_ItemGetter_<Channel_Example_ItemGetter_GetByOtherGive>
{

}
//例子 收集物品时 物品获取来源
public enum Channel_Example_ItemGetter
{
    [InspectorName("拾取的")]GetByPick,
    [InspectorName("分解的")] GetBySplit,
    [InspectorName("购买的")] GetByBuy,
    [InspectorName("奖励的")] GetByReward,
    [InspectorName("被赠与的")] GetByOtherGive
}

//例子 恢复生命时 为何恢复
public static class Channel_Example_HealType
{
    public static int ByAuto = 0;//自动恢复
    public static int ByBuff = 1;//由于Buff
    public static int ByReset = 2;//因为重置(比如开始游戏或者备战)
    public static int ByItem= 3;//因为道具
    public static int ByFriend = 4;//友方

}

//默认选通
public enum Channel_DefaultLink
{
    [InspectorName("无类型")]None,
    [InspectorName("从外到内")] OutToIn,
    [InspectorName("从内到外")] InToOut
}

