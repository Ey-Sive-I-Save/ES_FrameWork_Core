#define ESSafe

using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES
{

    public static partial class ESStaticLogicUtility
    {
        //数据应用器
        public static class DataApply
        {
            #region CopyTo(共享与变量体系)

            #region 常规的
            /// <summary>
            ///完全同类型的具有Class变量-IWithSharedAndVariableData体系专用
            /// </summary>
            /// <typeparam name="Shared"></typeparam>
            /// <typeparam name="Variable"></typeparam>
            /// <param name="from"></param>
            /// <param name="to"></param>
            public static void CopyToClassSameType<Shared, Variable>(IWithSharedAndVariableData<Shared, Variable> from, IWithSharedAndVariableData<Shared, Variable> to)
                where Shared : ISharedData
                where Variable : class, IVariableData
            {
#if ESSafe
                if (from != null && to != null)
                {
#endif
                    to.SharedData = from.SharedData;
                    if (to.VariableData == null)
                    {
                        to.VariableData = Activator.CreateInstance<Variable>();
                    }
                    to.VariableData.DeepCloneFrom(from.VariableData);
                    {
#if ESSafe     
                    }
#endif
                }
            }

            /// <summary>
            /// 不同类型的具有Class变量 -IWithSharedAndVariableData体系专用
            /// </summary>
            /// <typeparam name="SharedFrom"></typeparam>
            /// <typeparam name="VariableFrom"></typeparam>
            /// <typeparam name="SharedTo"></typeparam>
            /// <typeparam name="VariableTo"></typeparam>
            /// <param name="from"></param>
            /// <param name="to"></param>
            public static void CopyToClassDynamic<SharedFrom, VariableFrom, SharedTo, VariableTo>(IWithSharedAndVariableData<SharedFrom, VariableFrom> from, IWithSharedAndVariableData<SharedTo, VariableTo> to)
        where SharedFrom : SharedTo, ISharedData where VariableFrom : class, VariableTo, IVariableData
        where SharedTo : ISharedData where VariableTo : class, IVariableData
            {
#if ESSafe
                if (from != null && to != null)
                {
#endif
                    to.SharedData = from.SharedData;
                    if (to.VariableData == null)
                    {
                        to.VariableData = Activator.CreateInstance<VariableTo>();
                    }
                    to.VariableData.DeepCloneFrom(from.VariableData);
#if ESSafe
                }
#endif
            }

            /// <summary>
            ///完全同类型的具有Struct变量-IWithSharedAndVariableData体系专用
            /// </summary>
            /// <typeparam name="Shared"></typeparam>
            /// <typeparam name="Variable"></typeparam>
            /// <param name="from"></param>
            /// <param name="to"></param>
            public static void CopyToStructSameType<Shared, Variable>(IWithSharedAndVariableData<Shared, Variable> from, IWithSharedAndVariableData<Shared, Variable> to)
                where Shared : ISharedData
                where Variable : struct, IVariableData
            {
#if ESSafe
                if (from != null && to != null)
                {
#endif
                    to.SharedData = from.SharedData;
                    //结构体不需要实现CopyTo
                    to.VariableData.DeepCloneFrom(from.VariableData);
#if ESSafe
                }
#endif
            }

            /// <summary>
            /// 不同类型的具有Struct变量 -IWithSharedAndVariableData体系专用
            /// </summary>
            /// <typeparam name="SharedFrom"></typeparam>
            /// <typeparam name="Variable"></typeparam>
            /// <typeparam name="SharedTo"></typeparam>
            /// <param name="from"></param>
            /// <param name="to"></param>
            public static void CopyToStructDynamic<SharedFrom, Variable, SharedTo>(IWithSharedAndVariableData<SharedFrom, Variable> from, IWithSharedAndVariableData<SharedTo, Variable> to)
                where SharedFrom : SharedTo, ISharedData where Variable : struct, IVariableData
                where SharedTo : ISharedData
            {
#if ESSafe
                if (from != null && to != null)
                {
#endif
                    to.SharedData = from.SharedData;
                    //结构体不需要实现CopyTo
                    to.VariableData.DeepCloneFrom(from.VariableData);
#if ESSafe
                }
#endif
            }
            #endregion

            #region 重载优化
            /// <summary>
            ///完全同类型的具有Class变量-IWithSharedAndVariableData体系专用
            /// </summary>
            /// <typeparam name="Shared"></typeparam>
            /// <typeparam name="Variable"></typeparam>
            /// <param name="from"></param>
            /// <param name="to"></param>
            public static void CopyTo<Shared, Variable>(IWithSharedAndVariableData<Shared, Variable> from, IWithSharedAndVariableData<Shared, Variable> to)
                where Shared : ISharedData
                where Variable :IVariableData
            {
#if ESSafe
                if (from != null && to != null)
                {
#endif
                    to.SharedData = from.SharedData;
                    if (to.VariableData == null)
                    {
                        to.VariableData = Activator.CreateInstance<Variable>();
                    }
                    to.VariableData.DeepCloneFrom(from.VariableData);
                    {
#if ESSafe     
                    }
#endif
                }
            }

            /// <summary>
            /// 不同类型的具有Class变量 -IWithSharedAndVariableData体系专用
            /// </summary>
            /// <typeparam name="SharedFrom"></typeparam>
            /// <typeparam name="VariableFrom"></typeparam>
            /// <typeparam name="SharedTo"></typeparam>
            /// <typeparam name="VariableTo"></typeparam>
            /// <param name="from"></param>
            /// <param name="to"></param>
            public static void CopyTo<SharedFrom, VariableFrom, SharedTo, VariableTo>(IWithSharedAndVariableData<SharedFrom, VariableFrom> from, IWithSharedAndVariableData<SharedTo, VariableTo> to)
        where SharedFrom : SharedTo, ISharedData where VariableFrom : class, VariableTo, IVariableData
        where SharedTo : ISharedData where VariableTo : class, IVariableData
            {
#if ESSafe
                if (from != null && to != null)
                {
#endif
                    to.SharedData = from.SharedData;
                    if (to.VariableData == null)
                    {
                        to.VariableData = Activator.CreateInstance<VariableTo>();
                    }
                    to.VariableData.DeepCloneFrom(from.VariableData);
#if ESSafe
                }
#endif
            }

            /// <summary>
            ///完全同类型的具有Struct变量-IWithSharedAndVariableData体系专用
            /// </summary>
            /// <typeparam name="Shared"></typeparam>
            /// <typeparam name="Variable"></typeparam>
            /// <param name="from"></param>
            /// <param name="to"></param>
            /// <summary>
            /// 不同类型的具有Struct变量 -IWithSharedAndVariableData体系专用
            /// </summary>
            /// <typeparam name="SharedFrom"></typeparam>
            /// <typeparam name="Variable"></typeparam>
            /// <typeparam name="SharedTo"></typeparam>
            /// <param name="from"></param>
            /// <param name="to"></param>
            public static void CopyTo<SharedFrom, Variable, SharedTo>(IWithSharedAndVariableData<SharedFrom, Variable> from, IWithSharedAndVariableData<SharedTo, Variable> to)
                where SharedFrom : SharedTo, ISharedData where Variable : struct, IVariableData
                where SharedTo : ISharedData
            {
#if ESSafe
                if (from != null && to != null)
                {
#endif
                    to.SharedData = from.SharedData;
                    //结构体不需要实现CopyTo
                    to.VariableData.DeepCloneFrom(from.VariableData);
#if ESSafe
                }
#endif
            }
            #endregion

            #endregion

            public static void ApplyStatePackToMachine(StateDataPack pack, BaseOriginalStateMachine machine)
            {
                if (pack != null && machine != null)
                {
                    foreach (var i in pack.allInfos)
                    {
                        var use = i.Value;
                        if (use == null) continue;
                        //只要第一层的直接注入哈
                        if (use.asFirstLayer)
                        {
                            IESMicroState state = Creator.CreateStateRunTimeLogicComplete(use);
                            if (state == null) continue;
                            machine.RegisterNewState_Original(i.Key, state);
                        }
                    }
                }
            }
            /*  public static BuffRunTimeLogic ApplyBuffInfoToEntity(BuffSoInfo buffSoInfo, Entity entity, BuffStatusTest? buffStatusTest = null)
              {
                  if (buffSoInfo != null && entity != null)
                  {
                      var create = Creator.CreateBuffRunTimeByInfo_Rubbish(buffSoInfo, buffStatusTest);
                      entity.BuffDomain.buffHosting.AddHandle(create);
                      return create;
                  }
                  return null;
              }

              public static void Apply_Remove_BuffInfoToEntity(BuffSoInfo buffSoInfo, Entity entity)
              {
                  if (buffSoInfo != null && entity != null)
                  {
                      string s = buffSoInfo.key.Key();
                      foreach (var i in entity.BuffDomain.buffHosting.buffRTLs.valuesNow_)
                      {
                          if (i.buffSoInfo.key.Key() == s)
                          {
                              entity.BuffDomain.buffHosting.RemoveHandle(i);
                          }
                      }
                  }
              }*/
        }
    }
}

