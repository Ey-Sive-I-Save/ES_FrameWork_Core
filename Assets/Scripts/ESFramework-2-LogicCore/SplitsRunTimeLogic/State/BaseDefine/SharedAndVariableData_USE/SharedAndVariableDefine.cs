using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace ES {

    #region 纳米级别 : ESNano
    //纳米状态不具有共享/变化数据 hhh
    //他们甚至不继承RunTimeLogic 
    //并且保证了同时
    #endregion

    #region 微型级别: ESMicro -适合自定义

    [Serializable, TypeRegistryItem("微型状态共享数据")]
    public struct ESMicroStateSharedData : IStateSharedData
    {
        [LabelText("优先级")] public int order;
        [LabelText("默认退出事件时间")] public float defaultStayToExit;
        [LabelText("是否能被打断")] public bool canBeHit;
        [LabelText("是否能打断别人")]public bool canHit;
        [LabelText("无条件被打断")] public string[] _BeHitWithoutCondition;
        [LabelText("微型通道")] public StateData_ESMicroChannel channel;
        public int Order => order;

        public bool CanBeHit => canBeHit;
        public bool CanHit => canHit;
        public string[] BeHitWithoutCondition => _BeHitWithoutCondition;

        public Enum Channel => channel;

        
        #region 额外定义
       
        [Flags]//基本状态合并机制通道枚举定义
        public enum StateData_ESMicroChannel
        {
            [InspectorName("下半身")] LowerBody = 1 << 0,
            [InspectorName("上半身")] UpperBody = 1 << 1,
            [InspectorName("身体")] AllBaseBody = LowerBody | UpperBody,
            [InspectorName("头部")] Head = 1 << 2,
            [InspectorName("全身占据活动")] AllBodyActive = AllBaseBody | Head,
            [InspectorName("心灵:思考")] Heart = 1 << 3,
            [InspectorName("眼睛")] Eye = 1 << 4,
            [InspectorName("耳朵")] Ear = 1 << 5,
            [InspectorName("全身心")] AllBodyAndHeartAndMore = AllBodyActive | Heart | Eye | Ear,
            [InspectorName("目标")] Target = 1 << 6,
        }
        #endregion
        //执行合并
        public static HandleMergeBack HandleMerge
            (IStateSharedData left, IStateSharedData right, string leftName = null, string rightName = null)
        {
            //进行两边的合并冲突测试
            //冲突
            do  //第一波测试 --- 无条件打断 和 不参与打断
            {
                //第一层
                if (left.BeHitWithoutCondition?.Contains(rightName) ?? false)
                { return HandleMergeBack.HitAndReplace; }//左边无条件被打断
                //合并冲突哈
                var channel = left.Channel.GetHashCode() & right.Channel.GetHashCode();
                //第二层 //不在意打断哈
                if (!(left.CanBeHit&&right.CanHit))
                {
                    if (channel == 0) return HandleMergeBack.MergeComplete; //无冲突就合并了
                    else return HandleMergeBack.MergeFail;
                }
                if (channel > 0)//有冲突要解决
                {
                        //优先级的问题
                        if (left.Order <= right.Order)
                        {
                            return HandleMergeBack.HitAndReplace;
                        }
                        else
                        {
                            return HandleMergeBack.MergeFail;
                        }
                }
                else
                {
                    return HandleMergeBack.MergeComplete;
                }

            } while (false);
            
            //左边在高层级哈
            //能否合并
            //层级碾压
            //相交  双方层级碾压-->只允许层级碾压m
            //如果有一方是可同级别的,左边要求层级碾压时，
        }
    }

    [Serializable, TypeRegistryItem("微型状态运行情况")]
    public struct ESMicroStateStatus : IStateVariableData
    {
        [LabelText("状态开始时间")] public float hasEnterTime;

        public void DeepCloneFrom(object from)
        {
           
        }

        public void Init(params object[] ps)
        {

        }
    }
    #endregion

    #region 标准级别 : ESStandard
    [Serializable, TypeRegistryItem("标准状态共享数据")]
    public class ESStandardStateSharedData : IStateSharedData
    {
        [LabelText("冲突与合并")]
        public StateDataModule_StringKeyMergeAndConflict MergePart_ = new StateDataModule_StringKeyMergeAndConflict() { channel = StateDataModule_Ind_ESStandardChannel.AllBodyActive };

        #region 实现共享状态属性
        public int Order => MergePart_.BeHitOrder+MergePart_.HitOrder;
        public bool CanBeHit => MergePart_.CanBeHit!=HitOption.Never;
        public bool CanHit => MergePart_.CanHit != HitOption.Never;
        public string[] BeHitWithoutCondition => MergePart_.BeHitWithoutCondition;
        public Enum Channel => MergePart_.channel;
        #endregion

        #region 定义
        [Serializable, TypeRegistryItem("状态共享数据组分_冲突与合并")]
        public struct StateDataModule_StringKeyMergeAndConflict
        {
            //最高级别
            [Header("最高级别")]
            [LabelText("无条件被融入"), Tooltip("最高级别的优先级，必定可被打断")] public string[] BeCombinedWithoutCondition;
            [LabelText("无条件融入"), Tooltip("最高级别的优先级,必定可以打断")] public string[] CombinedWithoutCondition;
            [LabelText("无条件被打断"), Tooltip("最高级别的优先级，必定可被打断")] public string[] BeHitWithoutCondition;
            [LabelText("无条件打断"), Tooltip("最高级别的优先级,必定可以打断")] public string[] HitWithoutCondition;
            //第二级别
            [Header("第三级别")]
            [LabelText("是否可被打断")] public HitOption CanBeHit;//不发生,碾压,仅测试同级别
            [LabelText("是否可打断")] public HitOption CanHit;//不发生,碾压,仅测试同级别
            [LabelText("逻辑层"), Tooltip("有重合时可同级别判断，无重合时直接碾压判断,Rub层就没啥打断能力了")] public StateDataModule_Ind_LogicAtLayer logicLayer;
            [LabelText("占据通道"), Tooltip("占据相同通道后开始判断是否可以打断等")] public StateDataModule_Ind_ESStandardChannel channel;

            //第三级别
            //可以相容吗
            [Header("第二级别")]
            [LabelText("可被打断的优先级(byte)")] public byte BeHitOrder;
            [LabelText("打断的优先级(byte)")] public byte HitOrder;
        }
        [TypeRegistryItem("状态共享数据_动画器的集成")]
        public struct StateDataModule_Animator
        {

        }
        //逻辑层级--优先级的断档体现
        [Flags]
        public enum StateDataModule_Ind_LogicAtLayer
        {
            [InspectorName("垃圾层")] Rubbish = 0,//---永远不依赖优先级，层级总是不交和，必定打断
            [InspectorName("低等级")] Low = 1,
            [InspectorName("中等级")] Middle = 2,
            [InspectorName("高等级")] High = 4,
            [InspectorName("超等级")] Super = 8,
        }
        //打断机制
        public enum HitOption
        {
            [InspectorName("同级别测试")] SameLayTest,
            [InspectorName("只允许层级碾压,忽略同级别")] LayerCrush,
            [InspectorName("永远不发生")] Never,
        }
        [Flags]//基本状态合并机制通道枚举定义
        public enum StateDataModule_Ind_ESStandardChannel
        {
            [InspectorName("右手")] RightHand = 1 << 0,
            [InspectorName("左手")] LeftHand = 1 << 1,
            [InspectorName("双手")] DoubleHand = RightHand | LeftHand,
            [InspectorName("右腿")] RightLeg = 1 << 2,
            [InspectorName("左腿")] LeftLeg = 1 << 3,
            [InspectorName("双腿")] DoubleLeg = RightLeg | LeftLeg,
            [InspectorName("四肢")] FourLimbs = DoubleHand | DoubleLeg,
            [InspectorName("头")] Head = 1 << 4,
            [InspectorName("身体骨架")] BodySpine = 1 << 5,
            [InspectorName("全身占据活动")] AllBodyActive = FourLimbs | Head | BodySpine,
            [InspectorName("心灵:思考")] Heart = 1 << 6,
            [InspectorName("眼睛")] Eye = 1 << 7,
            [InspectorName("耳朵")] Ear = 1 << 8,
            [InspectorName("全身心")] AllBodyAndHeartAndMore = AllBodyActive | Heart | Eye | Ear,
            [InspectorName("目标")] Target = 1 << 9,
        }
       
        //执行合并
        public static HandleMergeBack HandleMerge
            (StateDataModule_StringKeyMergeAndConflict left, StateDataModule_StringKeyMergeAndConflict right, string leftName = null, string rightName = null)
        {
            //进行两边的合并冲突测试
            //冲突
            do  //第一波测试 --- 无条件打断 和 不参与打断
            {
                //第一层
                if (left.BeHitWithoutCondition?.Contains(rightName) ?? false)
                { return HandleMergeBack.HitAndReplace; }//左边无条件被打断
                if ((right.HitWithoutCondition?.Contains(leftName) ?? false))
                { return HandleMergeBack.HitAndReplace; }//右边无条件打断

                if (left.BeCombinedWithoutCondition?.Contains(rightName) ?? false)
                {
                    return HandleMergeBack.MergeComplete;
                }
                if ((right.CombinedWithoutCondition?.Contains(leftName) ?? false))
                { return HandleMergeBack.MergeComplete; }//右边无条件打断


                var channel = left.channel & right.channel;
                //第二层 //两者都不在意打断 
                if (left.CanBeHit == HitOption.Never || right.CanHit == HitOption.Never)
                {
                    if (channel == 0) return HandleMergeBack.MergeComplete; //无冲突就合并了
                    else return HandleMergeBack.MergeFail;
                }
                var layerAND = left.logicLayer & right.logicLayer;
                if (channel > 0)//有冲突要解决
                {
                    //必须解决冲突 -- 形成一方碾压
                    if (layerAND == 0)
                    {
                        if (left.logicLayer > right.logicLayer)
                        {
                            return HandleMergeBack.MergeFail;//左边碾压 解决不了
                        }
                        else
                        {
                     
                            return HandleMergeBack.HitAndReplace;//右边碾压 直接拿下
                        }
                    }
                    else //层级有重叠
                    {
                        if (left.CanBeHit == HitOption.LayerCrush || right.CanHit == HitOption.LayerCrush)
                        {
                            return HandleMergeBack.MergeFail;//没有达成条件 合并失败
                        }

                        //优先级的问题
                        if (left.BeHitOrder <= right.HitOrder)
                        {

                            return HandleMergeBack.HitAndReplace;
                        }
                        else
                        {

                            return HandleMergeBack.MergeFail;
                        }
                    }
                }
                else
                {
                    return HandleMergeBack.MergeComplete;
                }

            } while (false);
            //左边在高层级哈
            //能否合并
            //层级碾压
            //相交  双方层级碾压-->只允许层级碾压m
            //如果有一方是可同级别的,左边要求层级碾压时，
        }
        #endregion
    }

    [Serializable, TypeRegistryItem("标准状态运行状态")]
    //目前还没有特殊的玩意
    public class ESStandardStateVariableData : IStateVariableData
    {
        [LabelText("状态开始时间")] public float hasEnterTime;

        public void DeepCloneFrom(object from)
        {
          
        }

        public void Init(params object[] ps)
        {

        }
    }

    #endregion

    #region 相关枚举
    public enum HandleMergeBack
    {
        [InspectorName("打断并替换")] HitAndReplace,
        [InspectorName("合并成功")] MergeComplete,
        [InspectorName("合并失败")] MergeFail
    }//合并产生的结果

    #endregion
}
