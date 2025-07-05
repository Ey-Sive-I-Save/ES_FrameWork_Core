using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
/* 安全列表 就是允许在遍历时 进行增加和删除操作
   必须在每帧或者必要时，调用他的Update

    SafeLoopUpdateList_EasyQueue_SeriNot_Dirty ->SafeLoopList
 */
namespace ES
{
    [Serializable, TypeRegistryItem("队列循环安全安全脏列表_持久")]
    public class SafeLoopList<T>
    {
        [LabelText("正在更新", SdfIconType.ArrowRepeat), SerializeReference, ShowInInspector, GUIColor("@KeyValueMatchingUtility.ColorSelector.ColorForUpdating")]
        public List<T> ValuesNow = new List<T>(10);
        [FoldoutGroup("缓冲")]
        [ShowInInspector, LabelText("缓冲添加队列", SdfIconType.BoxArrowInLeft)]
        private Queue<T> ValuesBufferToAdd = new Queue<T>();
        [FoldoutGroup("缓冲")]
        [ShowInInspector, LabelText("缓冲移除队列", SdfIconType.BoxArrowRight)]
        private Queue<T> ValuesBufferToRemove = new Queue<T>();
        private bool isDirty { get; set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TryAdd(T add)
        {
            ValuesBufferToAdd.Enqueue(add);
            isDirty = true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TryRemove(T add)
        {
            ValuesBufferToRemove.Enqueue(add);
            isDirty = true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryContains(T who)
        {
            if (ValuesBufferToRemove.Contains(who)) return false;
            if (ValuesBufferToAdd.Contains(who)) return true;
            return ValuesNow.Contains(who);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ApplyBuffers(bool forceUpdate = false)
        {
            if (isDirty || forceUpdate)
            {
                isDirty = false;
                while (ValuesBufferToAdd.Count > 0)
                {
                    ValuesNow.Add(ValuesBufferToAdd.Dequeue());
                }
                while (ValuesBufferToRemove.Count > 0)
                {
                    ValuesNow.Remove(ValuesBufferToRemove.Dequeue());
                }
            }
        }

        //相比于 Update,性能更好
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ApplyBuffersNoForce()
        {
            if (!isDirty) return;
            isDirty = false;
            while (ValuesBufferToAdd.Count > 0)
            {
                ValuesNow.Add(ValuesBufferToAdd.Dequeue());
            }
            while (ValuesBufferToRemove.Count > 0)
            {
                ValuesNow.Remove(ValuesBufferToRemove.Dequeue());
            }

        }

        #region 杂项


        public void _ES_ClearWarnning()
        {
            //只是用来清除 Warn 项 没有任何意义
            ForceUpdate();
        }

        [Button("强制更新")]
        [FoldoutGroup("缓冲")]
        private void ForceUpdate()
        {
            ApplyBuffers(true);
        }

        #endregion


    }


    #region 废案

    /// <summary>
    /// 队列安全(脏)集合_不持久 HashSet
    ///  废弃原因： 不具有可视化修改 只是具有HashSet查重 一般用不到
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable, TypeRegistryItem("队列安全集合_不持久")]
    public class OB_SafeUpdateSet_EasyQueue_SeriNot<T>
    {

        [LabelText("正在更新", SdfIconType.ArrowRepeat), GUIColor("@KeyValueMatchingUtility.ColorSelector.ColorForUpdating")]
        public HashSet<T> valuesNow_ = new HashSet<T>(10);
        [FoldoutGroup("缓冲")]
        [ShowInInspector, LabelText("缓冲添加队列", SdfIconType.BoxArrowInLeft)]
        public Queue<T> valuesToAdd = new Queue<T>();
        [FoldoutGroup("缓冲")]
        [ShowInInspector, LabelText("缓冲移除队列", SdfIconType.BoxArrowRight)]
        public Queue<T> valuesToRemove = new Queue<T>();


        public void Update()
        {
            while (valuesToAdd.Count > 0)
            {
                valuesNow_.Add(valuesToAdd.Dequeue());
            }
            while (valuesToRemove.Count > 0)
            {
                valuesNow_.Remove(valuesToRemove.Dequeue());
            }
        }
    }
    [Serializable, TypeRegistryItem("队列安全脏集合_不持久")]
    public class OB_SafeUpdateSet_EasyQueue_SeriNot_Dirty<T>
    {
        [LabelText("正在更新", SdfIconType.ArrowRepeat), ShowInInspector, GUIColor("@KeyValueMatchingUtility.ColorSelector.ColorForUpdating")]
        public HashSet<T> valuesNow_ = new HashSet<T>(10);
        [FoldoutGroup("缓冲")]
        [ShowInInspector, LabelText("缓冲添加队列", SdfIconType.BoxArrowInLeft)]
        private Queue<T> valuesToAdd = new Queue<T>();
        [FoldoutGroup("缓冲")]
        [ShowInInspector, LabelText("缓冲移除队列", SdfIconType.BoxArrowRight)]
        private Queue<T> valuesToRemove = new Queue<T>();
        private bool isDirty { get; set; }
        public void TryAdd(T add)
        {
            valuesToAdd.Enqueue(add);
            isDirty = true;
        }
        public void TryAdd(ref T add)
        {
            valuesToAdd.Enqueue(add);
            isDirty = true;
        }
        public void TryRemove(T add)
        {
            valuesToRemove.Enqueue(add);
            isDirty = true;
        }
        [Button("强制更新")]
        [FoldoutGroup("缓冲")]
        private void ForceUpdate()
        {
            Update(true);
        }
        public void Update(bool forceUpdate = false)
        {
            if (isDirty || forceUpdate)
            {
                isDirty = false;
                while (valuesToAdd.Count > 0)
                {
                    valuesNow_.Add(valuesToAdd.Dequeue());
                }
                while (valuesToRemove.Count > 0)
                {
                    valuesNow_.Remove(valuesToRemove.Dequeue());
                }
            }
        }
    }


    #endregion
}

