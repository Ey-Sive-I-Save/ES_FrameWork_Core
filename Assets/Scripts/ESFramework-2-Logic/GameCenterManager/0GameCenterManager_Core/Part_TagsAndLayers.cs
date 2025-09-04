using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ES
{
    /* Tags And Layers 管理部分，这部分会被EditorOnly 功能生成作为
     * GameCenterManager的static parical
     * 具体代码见  :  ,手动生成 见 纯代码生成窗口的页面
     Assets/Scripts/ESFramework/CodeGen/Target/GameCenterManager_TagsAndLayers/GameCenterManager_TagsAndLayers.cs
     */

    //使用演示
    public partial class GameCenterManager
    {
        void Sample_UseTag()
        {
            gameObject.CompareTag(GameCenterManager.Tag_Enemy);
        }
    }

}

