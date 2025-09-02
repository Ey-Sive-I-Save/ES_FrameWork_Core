using ES;
using Sirenix.OdinInspector.Editor.StateUpdaters;
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
        public static GlobalDataForEditorOnly.ProjectShow Show;
      
            public static void ProjectItemDraw(string guid, Rect selectionRect)
        {
            Show ??= GlobalDataForEditorOnly.Instance?.Project;
            if (Show == null||!Show.EnableProjectShow) return;
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            string myName= Path.GetFileName(assetPath);
            bool net = false;
            bool draw = false;
            if (assetPath.Contains(GlobalDataForResMaster.Instance.genarateFolder))
            {
                string preName = GlobalDataForResMaster.Instance.GetPreNameFromCompleteNameWithHash(myName);
                if (GlobalDataForResMaster.Instance.toHash.ContainsKey(preName))
                {
                    //AB包
                    foreach(var i in GlobalDataForResMaster.Instance.TargetLocations)
                    {
                        if (i.ABPreName == preName)
                        {
                            if(i.ABTarget_== ESResMaster.ABTargetLocation.ABTarget.Net)
                            {
                                net = true;
                                break;
                            }
                            else
                            {
                                net = false;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (myName == "preToHashDic.json" || myName == "dependenceDic.json")
                    {
                        //网络
                        net = true;
                    }
                }
                if (Show.contentForNet != null && net)
                {
                    //是否是网络
                    Rect conRect = new Rect(selectionRect);
                    conRect.x = conRect.xMax + Show.startTextOffset_;
                    conRect.width = Show.textwidth_;
                    GUIContent con = Show.contentForNet.GetContent();
                    GUIHelper.PushColor(Show.contentForNet.color);
                    GUI.Label(conRect, con);
                    GUIHelper.PopColor();
                    draw = true;
                }
                else if (Show.contentForLocal != null && !net)
                {
                    //是否是网络
                    Rect conRect = new Rect(selectionRect);
                    conRect.x = conRect.xMax + Show.startTextOffset_;
                    conRect.width = Show.textwidth_;
                    GUIContent con = Show.contentForLocal.GetContent();
                    GUIHelper.PushColor(Show.contentForLocal.color);
                    GUI.Label(conRect, con);
                    GUIHelper.PopColor();
                    draw = true;
                }
            }
           

            if(!draw)
            foreach (var i in Show.nameToHandle)
            {
                if (i.oldName == myName&&i.content!=null)
                {
                    Rect conRect = new Rect(selectionRect);
                    conRect.x = conRect.xMax + Show.startTextOffset_;
                    conRect.width = Show.textwidth_;
                        SirenixEditorGUI.DrawSolidRect(selectionRect,UnityEngine.Color.blue._WithAlpha(0.25f));
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

