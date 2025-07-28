using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//只是一种定义 用来规范架构 师出有名 没有实际逻辑
/* Channel非常简单 ，一般就是一个枚举或者静态类
 * 他旨在为同参数，同类型的事件进行一个特征鲜明的初步划分
 * 用来减少频繁事件的遍历if 损耗 和 用于决定接下来的任务走向
 * 主要的 和 Link收发事件  集成
 * (比如 FishNet 的channel "可靠"和"不可靠")
 * 一般只对于设计层进行 大部分的零碎枚举 分布在各自的文件或者EnumCollect里
 */

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

