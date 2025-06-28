using ES;
using GluonGui.WorkspaceWindow.Views.WorkspaceExplorer;
using Sirenix.Utilities.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ES
{
    [InitializeOnLoad]
    public class ESProjectShow 
    {
        static ESProjectShow()
        {
            // 监听Project绘制前的回调
            EditorApplication.projectWindowItemOnGUI += ProjectItemDraw;
        }
        public static ESEditorOnlyPartMaster.ProjectShow Show;
        public static void ProjectItemDraw(string guid, Rect selectionRect)
        {
            Show ??= ESEditorOnlyPartMaster.Instance.Project;
            if (Show == null||!Show.EnableProjectShow) return;
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            string myName= Path.GetFileName(assetPath);
            foreach(var i in Show.nameToHandle)
            {
                if (i.oldName == myName&&i.content!=null)
                {
                    Rect conRect = new Rect(selectionRect);
                    conRect.x = conRect.xMax + Show.startTextOffset_;
                    conRect.width = Show.textwidth_;

                    GUIContent con = i.content.GetContent();


                    GUIHelper.PushColor(i.content.color);
                    GUI.Label(conRect, con);
                    GUIHelper.PopColor();
                    break;
                }
            }
            //名字判断
        }

    }
}

