using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


namespace ES
{
    [Serializable]
    public class ESRefer<T>
    {
        [ShowInInspector]
        public T Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (_current != null) return _current;
                return _current = HandleIfNull();
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            { _current = value; }
        }
        
        private T _current;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetCurrent(T t, bool internalSet = false)
        {
            if (internalSet) _current = t;
            else Current = t;
        }
        public virtual T HandleIfNull()
        {
            return _current;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator T(ESRefer<T> from) {
            return from.Current;
        }

    }
}
