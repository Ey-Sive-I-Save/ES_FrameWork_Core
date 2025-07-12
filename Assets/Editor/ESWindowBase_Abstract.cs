using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.EventSystems.EventTrigger;


namespace ES
{
    public abstract class ESWindowBase_Abstract<T> : OdinMenuEditorWindow where T : ESWindowBase_Abstract<T>
    {
        public static T usingWindow;
        private static Texture2D blackTexture;
        /*public static OdinMenuStyle style = OdinMenuStyle.TreeViewStyle;*/
        public static Dictionary<string, OdinMenuItem> Items = new Dictionary<string, OdinMenuItem>();
        public virtual GUIContent ESWindow_GetWindowGUIContent()
        {
            Texture2D texture = Resources.Load<Texture2D>("Sprites/iv2");
            var content = new GUIContent("依薇尔工具窗口", texture, "使用依薇尔工具完成快速开发");
            return content;
        }
        protected override void Initialize()
        {
            base.Initialize();
            blackTexture = new Texture2D(1, 1); 
            blackTexture.SetPixel(0, 0, Color.black + new Color(0.05f, 0.05f, 0.05f));
            blackTexture.Apply();
          /*  style.Height = 30;
            style.TriangleSize = 30;*/ 
           /* style.IconOffset = -10; 
            style.TriangleSize = 20;*/

            /*{
                Height = 30,
                Offset = 20f,
                IndentAmount = 15f,
                AlignTriangleLeft = true,
                TriangleSize = 16f,
                TrianglePadding = 0f,
                Borders = true,
                BorderPadding = 0f,
                BorderAlpha = 0.323f,
                // 正常状态下的文本颜色
                NotSelectedIconAlpha = 0.8f,
                SelectedColorDarkSkin = Color.gray,
                SelectedInactiveColorLightSkin = Color.black,
                SelectedColorLightSkin = Color.white,
                SelectedInactiveColorDarkSkin = Color.cyan,

            };*/
        }

        public virtual void ESWindow_OpenHandle()
        {
            /*if (usingWindow.HasDelegate)
            {
                //已经注册委托
            }
            else
            {
                usingWindow.DelegateHandle();
            }*/
        }
        public static void OpenWindow()
        {
            usingWindow = GetWindow<T>();
            usingWindow.ESWindow_OpenHandle();
            usingWindow.titleContent = usingWindow.ESWindow_GetWindowGUIContent();
            usingWindow.minSize = new Vector2(400, 300);
            usingWindow.maxSize = new Vector2(2500, 1800);
            usingWindow.maximized = true;
            usingWindow.MenuWidth = 250;
            usingWindow.Show();
            
            usingWindow.OnClose += () => { usingWindow.ES_SaveData(); };

        }
        protected sealed override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree();
          /*  tree.DefaultMenuStyle = style;*/ 
            ES_BuildMenuTree(tree);
            ES_LoadData();
            return tree;
        }
        protected virtual void ES_BuildMenuTree(OdinMenuTree tree)
        {

        }
        public void QuickBuildRootMenu<P>(OdinMenuTree tree,string name,ref P pageBase, SdfIconType sdfIcon) where P : ESWindowPageBase,new()
        {
            Items[name] = tree.Add(name, pageBase ??= new P(),sdfIcon).First();
        }
        protected override void OnImGUI()
        {
            if (usingWindow == null)
            {
                usingWindow = this as T;
            }
            GUI.DrawTexture(new Rect(0, 0, position.width, position.height), blackTexture);
            if (Event.current.type == EventType.Repaint)
            {
                // 绘制全屏黑色背景
                GUI.DrawTexture(new Rect(0, 0, position.width, position.height), blackTexture);
            }
            base.OnImGUI();
        }
        public static void ES_RefreshWindow()
        {
            if (usingWindow != null)
                usingWindow.ESWindow_RefreshWindow();
            else OpenWindow();
        }
        public virtual void ESWindow_RefreshWindow()
        {
            ES_SaveData();
            this.ForceMenuTreeRebuild();
            ES_LoadData();
        }
        public virtual void ES_LoadData()
        {

        }
        public virtual void ES_SaveData()
        {

        }
    }

    [Serializable]
    public abstract class ESWindowPageBase
    {
        
        public virtual void ES_Setup()
        {

        }
        public virtual bool ES_ShouldRemake()
        {
            return false;
        }
        public virtual ESWindowPageBase ES_ReFresh()
        {
            return this;
        }
        [OdinDontRegister]
        public class BlackBackgroundDrawer : OdinValueDrawer<ESWindowPageBase>
        {
            protected override void DrawPropertyLayout(GUIContent label)
            {
                var blackTexture = Texture2D.whiteTexture; // 使用白色纹理然后通过颜色控制
                                                           // 保存原始颜色
                var originalColor = GUI.color;

                // 设置黑色背景
                Rect rect = EditorGUILayout.BeginVertical();
                GUI.color = Color.black;
                GUI.DrawTexture(rect, blackTexture);
                EditorGUILayout.EndVertical();

                // 恢复颜色
                GUI.color = originalColor;
                Debug.Log(666);
                // 在背景之上绘制内容
                EditorGUILayout.BeginVertical();
                this.CallNextDrawer(label);
                
                EditorGUILayout.EndVertical();
            }

        }
       
    }

    public class BlackBackgroundDrawer : OdinValueDrawer<ESWindowPageBase>
    {
        private float lastHeight;
        protected override void DrawPropertyLayout(GUIContent label)
        {
            EditorGUILayout.Space(0);
            var space = GUILayoutUtility.GetLastRect();
            var startY1 = space.yMax;
            SirenixEditorGUI.DrawBorders(space, (int)space.width, 0, (int)lastHeight + 2, 0/* (int)lastHeight*/,(Color.white*0.05f)._WithAlpha(1));
            this.CallNextDrawer(label);
            float startY2 = GUILayoutUtility.GetLastRect().yMax;
            float f = startY2 - startY1;
            lastHeight = f > 0 ? f : lastHeight;
        }

    }
}