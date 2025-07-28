using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES {
    public static partial class EnumCollect
    {
        //比较两个Float 获得 Bool ,详见KeyValueMatching.Function
        public enum CompareTwoFunction
        {
            [InspectorName("相等")] Equal,
            [InspectorName("不等")] NotEqual,
            [InspectorName("小于")] Less,
            [InspectorName("小于等于")] LessEqual,
            [InspectorName("大于")] Greater,
            [InspectorName("大于等于")] GreaterEqual,
            [InspectorName("恒假")] Never,
            [InspectorName("恒真")] Always,
            [InspectorName("同向")] SameDirect,//两者相乘大于0
            [InspectorName("异向")] NotSameDirect,//两者相乘小于0
            [InspectorName("含0")] HasZero,//两者相乘等于0
            [InspectorName("都不为0")] NoZero,//两者相乘不为0
            [InspectorName("整除")] ModelMatch,//两者相乘不为0
            [InspectorName("不整除")] NotModelMatch,//两者相乘不为0
            [InspectorName("互为倒数")] Recipprocal,//两者相乘为1
            [InspectorName("互相不为倒数")] NotRecipprocal,//两者相乘不为1
            [InspectorName("二进制按位与不为0")] Mask_And_NotZero,//按位与不为0
            [InspectorName("二进制按位与为0")] Mask_ANd_Zero,//按位与为0
        }

        //操作两个Float 获得 Float ,详见KeyValueMatching.Function
        public enum HandleTwoFloatFunction
        {
            [InspectorName("设置")] Set,
            [InspectorName("加")] Add,
            [InspectorName("减")] Sub,
            [InspectorName("乘")] Muti,
            [InspectorName("除")] Divide,
            [InspectorName("取余")] Model,

            [InspectorName("Mask且")] Mask_And,
            [InspectorName("Mask或(开启)")] Mask_Or,
            [InspectorName("Mask异或(切换)")] Mask_Xor,
            [InspectorName("Mask且反(关闭)")] Mask_And_Not
        }

        //从列表选择出一个,详见KeyValueMatching.Function
        public enum PointerSelectOneType
        {
            [InspectorName("第一个不为空的")] NotNullFirst,
            [InspectorName("随机选择")] RandomOnly,
            [InspectorName("下一个")] Next,
            [InspectorName("上一个")] Last,
            [InspectorName("尝试排序")] TrySort,
            [InspectorName("筛选器")] Selector,
        }

        //从列表选择出多个,详见KeyValueMatching.Function
        public enum PointerSelectSomeType
        {
            [InspectorName("全部不为空的")] AllNotNull,
            [InspectorName("前几个")] StartSome,
            [InspectorName("后几个")] EndSome,
            [InspectorName("随机几个")] RandomSome,
            [InspectorName("筛选器")] Selector,
            [InspectorName("尝试排序")] TrySort,
        }

        //路径排序机制-,详见KeyValueMatching.Sorter
        public enum PathSortType
        {
            [InspectorName("不排序")] NoneSort,
            [InspectorName("按初始从近到远")] StartFromNearToFar,
            [InspectorName("按初始从远到近")] StartFromFarToNear,
            [InspectorName("总是最近")] AlwaysFirstNear,
            [InspectorName("总是最远")] AlwaysFirstFar,
            [InspectorName("按Y向上")] Yup,
            [InspectorName("按Y向下")] Ydown,
            [InspectorName("按X增大")] Xup,
            [InspectorName("按X变小")] Xdown,
            [InspectorName("按Z变大")] Zup,
            [InspectorName("按Z变小")] Zdown,
            [InspectorName("按初始面向的变大")] StartForwardZup,
            [InspectorName("按初始面向的变小")] StartForwardZdown,
            [InspectorName("总是按面向的变大")] AlwaysForwardZup,
            [InspectorName("总是按面向的变小")] AlwaysForwardZdown,
            [InspectorName("随机")] Random
        }


    }
}
