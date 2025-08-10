using ES;
using ES.EvPointer;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES
{
    /*可输出操作，可以随时装载和卸载的简单操作，
         和Process密切相关,
         OutputOperation 通常定义了 输入流 与 输出流
         并且严格针化 也就是 On(作用于) From(来源) With(凭借) -->Back 如果
         On==Back 则可定义为链，链式的Process是一种非常特殊的模式
    */
    public interface IOutputOperation<On, From, With> : IOperation<On, From, With>, IPointerNotBack<On, From, With>
    {
        void TryOperation(On on, From from, With with);
        void TryCancel(On on, From from, With with);
        void IPointerNotBack<On, From, With>.Pick(On on, From from, With with)
        {
            TryOperation(on, from, with);
        }

    }
    public abstract class OutputOperation_Abstract<On, From, With> : IOutputOperation<On, From, With>
    {
        public static void  DefaultAction(On on, From from, With with)
        {

        }
        public Action<On, From, With> OnCancel = DefaultAction;
        public  abstract void TryOperation(On on, From from, With with);
        public virtual void TryCancel(On on, From from, With with) {
            OnCancel?.Invoke(on,from,with);
        }
    }
    public interface IOutputOperationFlag_MustCancel
    {

    }

    public interface IOutputOperationEEB : IOutputOperation<Entity, Entity, EntityState_Buff> { }


    [Serializable, TypeRegistryItem("执行多个")]
    public class OutputOperationEEB_HandleList : IOutputOperationEEB
    {
        [LabelText("执行列表"), SerializeReference]
        public List<IOutputOperationEEB> handles = new List<IOutputOperationEEB>();
        public void TryCancel(Entity on, Entity from, EntityState_Buff with)
        {
            foreach(var i in handles)
            {
                i.TryOperation(on,from,with);
            }
        }

        public void TryOperation(Entity on, Entity from, EntityState_Buff with)
        {
            foreach (var i in handles)
            {
                i.TryCancel(on, from, with);
            }
        }
    }
}
