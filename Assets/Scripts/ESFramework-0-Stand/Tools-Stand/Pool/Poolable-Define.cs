using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


namespace ES {
    #region 万能对象池
    public interface IPoolable
    {
        void OnResetAsPoolable();
        bool IsRecycled { get; set; }
    }
    public interface IPoolablebAndSelfControlToWhere : IPoolable
    {
        void TryAutoBePushedToPool();
    }
    public interface IPool<T> where T : IPoolable
    {
        T GetInPool();
        bool PushToPool(T obj);
    }

    public abstract class Pool<T> : IPool<T> where T : IPoolable
    {
        public int CurCount
        {
            get { return mObjectStack.Count; }
        }


        protected IFactory<T> mFactory;

        public void SetFactoryDirectly(IFactory<T> factory)
        {
            mFactory = factory;
        }

        public void SetHowToCreate(Func<T> factoryMethod)
        {
            mFactory = new ESFactory_CustomFunction<T>(factoryMethod);
        }

        /// <summary>
        /// 存储相关数据的栈
        /// </summary>
        protected readonly Stack<T> mObjectStack = new Stack<T>();

        public void Clear(Action<T> onClearItem = null)
        {
            if (onClearItem != null)
            {
                foreach (var poolObject in mObjectStack)
                {
                    onClearItem(poolObject);
                }
            }

            mObjectStack.Clear();
        }

        /// <summary>
        /// default is 5
        /// </summary>
        protected int mMaxCount = 12;

        public virtual T GetInPool()
        {
            var use = mObjectStack.Count == 0
                ? mFactory.Create()
                : mObjectStack.Pop();
            use.IsRecycled = false;
            return use;
        }

        public abstract bool PushToPool(T obj);
    }

    public class ESSimpleObjectPool<T> : Pool<T> where T : IPoolable
    {
        protected readonly Action<T> mResetMethod;

        public ESSimpleObjectPool(Func<T> factoryMethod, Action<T> resetMethod = null, int initCount = 0)
        {
            mFactory = new ESFactory_CustomFunction<T>(factoryMethod);
            mResetMethod = resetMethod;

            for (var i = 0; i < initCount; i++)
            {
                mObjectStack.Push(mFactory.Create());
            }
        }

        public override bool PushToPool(T obj)
        {
            mResetMethod?.Invoke(obj);
            obj.IsRecycled = true;
            mObjectStack.Push(obj);

            return true;
        }
    }
    public class ESSimpleObjectPoolSingleton<T> : ESSimpleObjectPool<T> where T : IPoolable, new()
    {
        private static ESSimpleObjectPoolSingleton<T> pool;
        public static ESSimpleObjectPoolSingleton<T> Pool { [MethodImpl(MethodImplOptions.AggressiveInlining)] get { if (pool != null) return pool; return CreatePool(); } set { } }
        public ESSimpleObjectPoolSingleton(Func<T> factoryMethod, Action<T> resetMethod, int initCount)
        : base(factoryMethod, resetMethod, initCount)
        {
            pool = this;
        }
        public static ESSimpleObjectPoolSingleton<T> CreatePool()
        {
            return pool = new ESSimpleObjectPoolSingleton<T>(() => new T(), null, 10);
        }
        public override bool PushToPool(T obj)
        {
            obj.OnResetAsPoolable();
            return base.PushToPool(obj);
        }
    }
    #endregion
}
