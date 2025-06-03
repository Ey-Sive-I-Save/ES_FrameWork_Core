using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ES
{

    public class ESMonoShow 
    {
        public static class GlobalMonoBehaviourMenuItems
        {
            [MenuItem("CONTEXT/MonoBehaviour/全局功能/禁用组件")]
            private static void DisableComponent(MenuCommand command)
            {
                MonoBehaviour comp = command.context as MonoBehaviour;
                if (comp != null) comp.enabled = false;
            }

            [MenuItem("CONTEXT/MonoBehaviour/全局功能/启用组件")]
            private static void EnableComponent(MenuCommand command)
            {
                MonoBehaviour comp = command.context as MonoBehaviour;
                if (comp != null) comp.enabled = true;
            }
            [MenuItem("CONTEXT/MonoBehaviour/ES编辑器/缓存脚本")]
            private static void AddToCache(MenuCommand command)
            {
                MonoBehaviour comp = command.context as MonoBehaviour;
                if (comp != null) comp.enabled = true;
            }
        }
    }
}

