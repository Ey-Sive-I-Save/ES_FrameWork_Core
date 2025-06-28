using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

namespace ES
{
    [InitializeOnLoad]
    public class ESSceneShow 
    {
        static ESSceneShow()
        {
            // 监听Project绘制前的回调
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            Event e = Event.current;
        }

        }
}

