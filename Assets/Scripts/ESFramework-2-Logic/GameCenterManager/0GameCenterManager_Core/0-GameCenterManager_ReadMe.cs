using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES
{
    /*
          GameCenterManager 是 全局最核心的 游戏逻辑 单例管理器
          这里把他使用 <partial> 拆分为多个文件
          他不仅是单例  同时也是一个 Core 类型
          
     */


    /*  XX 更多细分功能(一般性能不敏感)会拆分到 Domain和 Module 里
     *  ##PartForID                     为全局大型逻辑单元分配ID(网络寻值的主要判据，也可以本地使用) 
     *  ##EnumForFrameWorkDesign        用于框架的底层设计
     *  ##EnumForUnitySelfOperation     和Unity原生内容紧密相关
     *  ##EnumForGameCoreLogic          用于游戏的核心逻辑
     *  ##EnumForLessUseNotImportant    不常用的内容，但是可能修改频繁而收录
     *  ##EnumForUnityPlunginsOrPackage 用于对插件和库的扩展
     *  ##EnumForComputeFunction        用于数学计算和容器排序
     *  
     *  此外 被代码生成的枚举是可能的---在ESFrameWork / CodeGen / Enum 寻找
     */
    public partial class GameCenterManager
    {
        public static Action NULLAction = () => { };
    }
}

