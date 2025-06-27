using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace ES {
    [InitializeOnLoad]
    public class ESEditorHandle
    {
        public static ESSimpleObjectPool<ESEditorHandleTask> TaskPool = new ESSimpleObjectPool<ESEditorHandleTask>(
            ()=>new ESEditorHandleTask(),
            (f)=> { },
            5
            );
        public static Queue<ESEditorHandleTask> RunningTasks = new Queue<ESEditorHandleTask>();
        static ESEditorHandle()
        {
            EditorApplication.update += Update;
        }
        private static void Update()
        {
            if(RunningTasks.Count>0)
            {
                var use = RunningTasks.Peek();
                if (use != null)
                {
                    use.waitFrame--;
                    if (use.waitFrame < 0)
                    {
                        use.action?.Invoke();
                        use.TryAutoBePushedToPool();
                        RunningTasks.Dequeue();
                    }
                }
                else
                {
                    use.TryAutoBePushedToPool();
                    RunningTasks.Dequeue();
                }
            }
        }
        public static void AddHanldeTask(Action c,int waitframe=3)
        {
            var use = TaskPool.GetInPool();
            use.waitFrame = waitframe;
            use.action = c;
            RunningTasks.Enqueue(use);
        }
    }

    public class ESEditorHandleTask : IPoolablebAndSelfControlToWhere
    {
        public int waitFrame = 2;
        public Action action;

        public bool IsRecycled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void OnResetAsPoolable()
        {
            waitFrame = 2;
            action=null;
        }

        public void TryAutoBePushedToPool()
        {
            ESEditorHandle.TaskPool.PushToPool(this);
        }
    }
}
