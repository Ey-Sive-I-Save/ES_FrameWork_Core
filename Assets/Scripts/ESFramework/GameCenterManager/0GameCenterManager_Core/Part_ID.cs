using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ES
{
    /* ID 管理部分，对性能要求极高，
     * 这里会使用大量接口和显式 数据结构 避免 as/强转 和 a.b.c 的出现
     
     */
    public partial class GameCenterManager
    {
        #region ID容器(因为对性能要求高，这里写为一步静态/)
        public static IDToWhoTable<Entity> EntityIDPool = new IDToWhoTable<Entity>();
        public static IDToWhoTable<Item> ItemIDPool = new IDToWhoTable<Item>();

        public static int LocalIDCount {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { _localIDCount--;return _localIDCount; } }
        private static int _localIDCount = -100;

        public static int NetIDCount {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { _netIDCount++; return _netIDCount; } }
        private static int _netIDCount = 1;
        //每个ID表 实质上是一个 字典(键为long,值为特定类型)
        [Serializable]
        public class IDToWhoTable<For>:Dictionary<int,For>
        {
            
        }

        #endregion

    }
    #region 可实现接口
    public interface IWithID
    {
        public void SendIDRequest();
        public void _InTable();
        public void _OutTable();
        public void _OnIDYes();
        public void _OnIDNO();
    }
    #endregion
}

