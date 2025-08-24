using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


namespace ES {
    #region 定义VersionedList
    //版本化的列表
    public class VersionedList<T> : ISafeList<T>
    {
        [LabelText("正在更新", SdfIconType.ArrowRepeat), SerializeReference, GUIColor("@KeyValueMatchingUtility.ColorSelector.ColorForUpdating")]
        public List<T> ValuesNow = new List<T>(10);
        [FoldoutGroup("缓冲")]
        [ShowInInspector, NonSerialized, LabelText("缓冲添加队列", SdfIconType.BoxArrowInLeft)]
        private Queue<T> ValuesBufferToAdd = new Queue<T>();
        [FoldoutGroup("缓冲")]
        [ShowInInspector, NonSerialized, LabelText("缓冲移除队列",  SdfIconType.BoxArrowRight)]
        private Queue<T> ValuesBufferToRemove = new Queue<T>();
        private bool isDirty;
        public bool MayHasElement = true;
        public int VersionMin = 0;
        public int VersionMax = 0;
        public Queue<VersionedRecordChange<T>> VersionedRecordChanges = new Queue<VersionedRecordChange<T>>();
        public List<MirrorSyncListValidator<T>> Validators = new List<MirrorSyncListValidator<T>>();
        public bool AutoApplyBuffers { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; [MethodImpl(MethodImplOptions.AggressiveInlining)] set; } = true;
        public void SetAutoApplyBuffers(bool b) => AutoApplyBuffers = b;
        public IEnumerable<T> ValuesIEnumable
        {
            get
            {
                // 内联迭代器：直接返回 ValuesNow 的枚举器
                foreach (var item in ValuesNow)
                {
                    yield return item;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TryAdd(T add)
        {
            ValuesBufferToAdd.Enqueue(add);
            isDirty = true;
            MayHasElement = true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TryRemove(T add)
        {
            ValuesBufferToRemove.Enqueue(add);
            isDirty = true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TryAddRange(IEnumerable<T> add)
        {
            foreach (var i in add)
            {
                ValuesBufferToAdd.Enqueue(i);
            }
            isDirty = true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TryRemoveRange(IEnumerable<T> remove)
        {
            foreach (var i in remove)
            {
                ValuesBufferToRemove.Enqueue(i);
            }
            isDirty = true;
        }
        public bool TryContains(T who)
        {
            if (ValuesBufferToRemove.Contains(who)) return false;
            if (ValuesBufferToAdd.Contains(who)) return true;
            return ValuesNow.Contains(who);
        }
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

        //Dirty模式>相比Update,性能更好
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TryApplyBuffers()
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            ValuesNow.Clear();
            ValuesBufferToAdd.Clear();
            ValuesBufferToRemove.Clear();
            MayHasElement = false;
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
    #endregion

    #region 定义MirrorSyncList
    //版本化的列表
    public class MirrorSyncList<T>
    {
        [NonSerialized]
        public VersionedList<T> Source=null;
    }
    #endregion

    #region 辅助类-VersionRecordChange
    public class VersionedRecordChange<T>
    {
        public bool add = true;
        public T value;
    }
    #endregion

    #region 辅助类-MirrorListValidator
    public class MirrorSyncListValidator<T>
    {
        public static Func<bool> DefaultAvailable => () => true;
        public Func<bool> Available = DefaultAvailable;
        public MirrorSyncList<T> mirror;
        public MirrorSyncListValidator(MirrorSyncList<T> mirror, Func<bool> Available)
        {
            this.mirror = mirror;
            this.Available = Available;
        }
        public MirrorSyncListValidator(MirrorSyncList<T> mirror, UnityEngine.Object baseOn)
        {
            this.mirror = mirror;
            this.Available = ()=>baseOn!=null;
        }
    }

    #endregion
}
