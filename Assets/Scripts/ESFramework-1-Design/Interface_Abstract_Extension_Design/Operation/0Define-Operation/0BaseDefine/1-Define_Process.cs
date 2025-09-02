using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  Process 很简单，就是一个借助一定的输入值，进行一个流程进行事件数据分发的载体
 *  Process是可以高度自定义实现，他只是一个接口，规范而已
 
 */

public interface IProcess<ValueSource, ValueOutput, Opeation_,Channel_, This> 
    where Opeation_: IOperation
    where This : IProcess<ValueSource, ValueOutput, Opeation_, Channel_, This>

{
    ValueSource Source { get; set; }
    ValueOutput Output { get; set; }
    void DoProcess(ValueSource source);
    void AddOpearation(Opeation_ op, Channel_ channel);
    void RemoveOpearation(Opeation_ op, Channel_ channel);
}

