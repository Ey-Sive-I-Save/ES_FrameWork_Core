using ES;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static ES.ESResWindow;

namespace ES
{

    public class ESNodeWindow : ESWindowBase_Abstract<ESNodeWindow> {
        public override GUIContent ESWindow_GetWindowGUIContent()
        {
            Texture2D texture = Resources.Load<Texture2D>("Sprites/Boki");
            var content = new GUIContent("依薇尔节点图管理窗口", texture, "使用依薇尔节点图工具可视化建立逻辑");
            return content;
        }
        #region 数据滞留
        public Page_NodeSettings page_Node;


        #endregion
        [MenuItem("Tools/ES工具/ES节点图管理窗口")]
        public static void TryOpenWindow()
        {
            if (ESEditorRuntimePartMaster.Instance != null)
                OpenWindow();
            else Debug.LogError("确保场景中有EditorMaster");
        }

        protected override void ES_BuildMenuTree(OdinMenuTree tree)
        {
            PartPage_NodeBaseSettings(tree);
        }
        void PartPage_NodeBaseSettings(OdinMenuTree tree)
        {
            tree.Add("总设置", page_Node ??= new Page_NodeSettings());
        }

        #region 序列化组分
        [Serializable]
        public class Page_NodeSettings : ESWindowPageBase
        {
            public class MyVector2Drawer : OdinValueDrawer<Page_NodeSettings>
            {
                private Texture2D _bgTexture;
                private Vector2 _bgOffset;
                public Rect position = new Rect(0,100,2000,2000);
                private Color _bgColor = new Color(0.15f, 0.15f, 0.2f); // 深蓝灰色背景
                List<NodeData> _nodes = new List<NodeData>();
                void GenerateBgTexture()
                {
                    _bgTexture = new Texture2D(1, 1);
                    _bgTexture.SetPixel(0, 0, _bgColor);
                    _bgTexture.Apply();
                }

                protected override void DrawPropertyLayout(GUIContent label)
                {
                    // 获取当前值
                    Rect bgRect = new Rect(Vector2.zero, position.size);
                    if (_bgTexture == null) GenerateBgTexture();
                    GUI.DrawTextureWithTexCoords(bgRect, _bgTexture, new Rect(_bgOffset, position.size / 64f));
                    DrawGrid(10,0.2f);

                    foreach (var node in _nodes)
                    {
                        new NodeView().Draw(node,_bgOffset);
                        ProcessNodeEvents(node);
                    }

                    if (Event.current.type == EventType.ContextClick)
                    {
                        Vector2 vv = Event.current.mousePosition;
                        GenericMenu menu = new GenericMenu();
                        menu.AddItem(new GUIContent("Create Node"), false, () => {
                            _nodes.Add(new NodeData { rect = new Rect(vv-_bgOffset, new Vector2(200, 150)) });
                        });

                        menu.AddItem(new GUIContent("Create 100 Node"), false, () => {

                            for(int x = 0; x < 100; x += 10)
                            {
                                for (int y = 0; y < 100; y += 10)
                                {
                                    _nodes.Add(new NodeData { rect = new Rect(vv - _bgOffset+new Vector2(x,y), new Vector2(200, 150)) });

                                }
                            }
                            _nodes.Add(new NodeData { rect = new Rect(vv - _bgOffset, new Vector2(200, 150)) });
                        });
                        menu.ShowAsContext();
                    }
                    HandleCanvasDrag();
                       // 布局控制
                       Rect rect = EditorGUILayout.GetControlRect();
                    if (label != null) rect = EditorGUI.PrefixLabel(rect, label);

                    // 双滑块并排布局
                    GUIHelper.PushLabelWidth(20);
                   
                    GUIHelper.PopLabelWidth();

                }

                void DrawGrid(float gridSpacing, float gridOpacity)
                {
                    int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
                    int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);
                    Handles.BeginGUI();
                    Handles.color = new Color(0.5f, 0.5f, 0.5f, gridOpacity);

                    // 绘制垂直线
                    for (int i = 0; i < widthDivs; i++)
                    {
                        float x = _bgOffset.x % gridSpacing + i * gridSpacing;
                        Handles.DrawLine(new Vector2(x, 0), new Vector2(x, position.height));
                    }
                    for (int i = 0; i < heightDivs; i++)
                    {
                        float y = _bgOffset.y % gridSpacing + i * gridSpacing;
                        Handles.DrawLine(new Vector2(0, y), new Vector2(position.width, y));
                    }
                    // 绘制水平线（类似逻辑）
                    Handles.EndGUI();
                }
                void HandleCanvasDrag()
                {
                    if (Event.current.type == EventType.MouseDrag && Event.current.button == 2)
                    { // 中键拖拽
                        _bgOffset += Event.current.delta;
                        /*Repaint();*/
                    }
                }
                [System.Serializable]
                public class NodeData
                {
                    public string guid = GUID.Generate().ToString();
                    public Rect rect = new Rect(0, 0, 200, 150);
                    public bool isDragging;
                    public Vector2 dragOffset;
                }

                public class NodeView
                {
                    public void Draw(NodeData data,Vector2 offset)
                    {
                        // 自定义节点样式
                        GUIStyle style = new GUIStyle("flow node 0")
                        {
                            normal = { background = MakeTex(600, 400, new Color(0.3f, 0.3f, 0.4f)) }
                        };
                        var rect = data.rect;
                        rect.position += offset;
                        GUI.Box(rect, "Node " + data.guid.Substring(0, 5), style);
                    }

                    private Texture2D MakeTex(int v1, int v2, Color color)
                    {
                        var _bgTexture = new Texture2D(1, 1);
                        _bgTexture.SetPixel(0, 0, color);
                        _bgTexture.Apply();
                        return _bgTexture;
                    }
                }
                void ProcessNodeEvents(NodeData node)
                {
                    Event e = Event.current;
                    switch (e.type)
                    {
                        case EventType.MouseDown:
                            if (node.rect.Contains(e.mousePosition))
                            {
                                node.isDragging = true;
                                node.dragOffset = node.rect.position - e.mousePosition;
                                GUI.changed = true; // 强制重绘
                            }
                            break;
                        case EventType.MouseDrag:
                            if (node.isDragging)
                            {
                                node.rect.position = e.mousePosition + node.dragOffset;
                                e.Use(); // 阻止事件冒泡
                            }
                            break;
                        case EventType.MouseUp:
                            node.isDragging = false;
                            break;
                    }
                }
            }
        }
          
        #endregion
    }
}

