using ES;
using ES.EvPointer;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using static ES.ESResMaster;

namespace ES
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    [DefaultExecutionOrder(-10)]
    [CreateAssetMenu(fileName = "全局数据-纯编辑器管理", menuName = "全局数据/纯编辑器管理")]
    public class GlobalDataForEditorOnly : GlobalDataSupport<GlobalDataForEditorOnly>
    {
#if UNITY_EDITOR


        #region 纯编辑器部分
        #region 层级图标支持代码
        [LabelText("图标管理"), NonSerialized, OdinSerialize]
        public ESIconControl ICON = new ESIconControl();
        [Serializable]
        public class ESIconControl
        {
            [LabelText("初始偏移")] public float startOffset = -20;
            [LabelText("每次偏移")] public float offsetPerTime = -20;
            [LabelText("宽度")] public float width = 20;
            [SerializeReference, LabelText("应用图标")]
            public List<HierachySelector> HierachySelectorS = new List<HierachySelector>();

        }
        [Serializable]
        public abstract class HierachySelector
        {
            public abstract (bool, BackMessage) IsAppliable(GameObject g);
            [SerializeReference, LabelText("图标"), PropertyOrder(1), GUIColor("@KeyValueMatchingUtility.ColorSelector.Color_02")] public ContentGetter contentGetter = new ContentGetter_SystemIcon_English() { };

        }
        public struct BackMessage
        {
            public Component component;
        }
        #region 筛选器
        [Serializable, TypeRegistryItem("依赖标签筛选")]
        public class IconSelector_DependTag : HierachySelector
        {
            [LabelText("标签筛选")]
            public PointerForString_Tag tag = new PointerForString_Tag() { tagName = "Player" };
            public override (bool, BackMessage) IsAppliable(GameObject g)
            {
                if (g._IsNotNullAndUse()?.CompareTag(tag.tagName) ?? false) return (true, default);
                return (false, default);
            }
        }
        [Serializable, TypeRegistryItem("依赖层级筛选")]
        public class IconSelector_DependLayer : HierachySelector
        {
            [LabelText("层级筛选")]
            public LayerMask mask;
            public override (bool, BackMessage) IsAppliable(GameObject g)
            {
                return (g._IsInLayerMask(mask), default);
            }
        }
        [Serializable, TypeRegistryItem("脚本筛选(类型名)")]
        public class IconSelector_DependCompo_ClassName : HierachySelector
        {
            [LabelText("类型名")]
            public string typeName;
            public override (bool, BackMessage) IsAppliable(GameObject g)
            {
                var all = g.GetComponents<Component>();
                foreach (var i in all)
                {
                    if (i == null) continue;
                    if (i.GetType().Name == typeName) return (true, new BackMessage() { component = i });
                }
                return (false, default);
            }
        }
        [Serializable, TypeRegistryItem("脚本筛选(Type数据相等)")]
        public class IconSelector_DependCompo_SoInfo : HierachySelector
        {
            [LabelText("类型数据")]
            public ESTypeSelecter types;
            public override (bool, BackMessage) IsAppliable(GameObject g)
            {
                if (types == null) return (false, default);
                var all = g.GetComponents<Component>();
                foreach (var i in all)
                {
                    if (types.TypeList.Contains(i.GetType())) return (true, new BackMessage() { component = i });
                }
                return (false, default);
            }
        }
        [Serializable, TypeRegistryItem("脚本筛选(Type数据可继承)")]
        public class IconSelector_DependCompo_SoInfo_Assign : HierachySelector
        {
            [LabelText("类型数据")]
            public ESTypeSelecter types;
            public override (bool, BackMessage) IsAppliable(GameObject g)
            {
                if (types == null) return (false, default);
                var all = g.GetComponents<Component>();
                foreach (var i in all)
                {
                    foreach (var type in types.TypeList)
                    {
                        if (type.IsAssignableFrom(i.GetType())) return (true, new BackMessage() { component = i });
                    }
                }
                return (false, default);
            }
        }
        #endregion
        #region 内容源(content)
        [Serializable]
        public abstract class ContentGetter
        {
            [ColorPalette, LabelText("颜色"), GUIColor("white")]
            public Color color = Color.white;
            [LabelText("缩放")]
            public float scale = 1;
            public abstract GUIContent GetContent();
        }
        [Serializable, TypeRegistryItem("GUI内容-系统图标(英文名)")]
        public class ContentGetter_SystemIcon_English : ContentGetter
        {
            [LabelText("系统图标名"), ValueDropdown("GetNames")]
            public string contentName = "d_GameObject Icon";
            public List<string> GetNames()
            {
                return UnityEditorIcons.UnityEditorIconNames.AllEnglish;
            }
            public override GUIContent GetContent()
            {
                return EditorGUIUtility.IconContent(contentName);
            }
        }
        [Serializable, TypeRegistryItem("GUI内容-系统图标(中文名)")]
        public class ContentGetter_SystemIcon_Chinese : ContentGetter
        {
            [LabelText("系统图标名"), ValueDropdown("GetNames")]
            public string contentName = "游戏对象图标";
            public List<string> GetNames()
            {
                return UnityEditorIcons.UnityEditorIconNames.AllChinese.Keys.ToList();
            }
            public override GUIContent GetContent()
            {
                return EditorGUIUtility.IconContent(UnityEditorIcons.UnityEditorIconNames.AllChinese[contentName]);
            }
        }
        [Serializable, TypeRegistryItem("GUI内容-引用纹理")]
        public class ContentGetter_ReferenceIcon : ContentGetter
        {
            [LabelText("图标资源")] public Texture texture;
            public override GUIContent GetContent()
            {
                return new GUIContent(texture);
            }
        }
        [Serializable, TypeRegistryItem("GUI内容-文本")]
        public class ContentGetter_Label : ContentGetter
        {
            [LabelText("字符串")] public string content = "**";
            public override GUIContent GetContent()
            {
                return new GUIContent(content);
            }
        }
        /* [Serializable, TypeRegistryItem("GUI内容-Odin图标(中文名)")]
         public class ContentGetter_OdinIcon_Chinese : ContentGetter
         {
             [LabelText("系统图标名"), ValueDropdown("GetNames")]
             public string contentName = "游戏对象图标";
             public List<string> GetNames()
             {
                 return .sta UnityEditorIcons.UnityEditorIconNames.AllChinese.Keys.ToList();
             }
             public override GUIContent GetContent()
             {
                 return EditorGUIUtility.IconContent(UnityEditorIcons.UnityEditorIconNames.AllChinese[contentName]);
             }
         }*/

        #endregion
        #endregion
        #region 检查器控制支持代码
        [LabelText("检查器控制"), NonSerialized, OdinSerialize]
        public ESInspectorControl Ins = new ESInspectorControl();
        [Serializable]
        public class ESInspectorControl
        {
            [LabelText("启用控显脚本")]
            public bool EnableCompoentShowControl_ = true;
            [LabelText("缓存不显示名字")]
            public HashSet<string> cacheToggleFalseNames = new HashSet<string>();
        }
        #endregion
        #region 类型名显示控制
        [LabelText("类型名显示辅助"), NonSerialized, OdinSerialize]
        public TypeDisplay TypeDis = new TypeDisplay();


        [Serializable]
        public class TypeDisplay
        {
            [LabelText("手动重命名(类型/显示名)(最高级)")]
            public Dictionary<string, string> HandSelfRename = new Dictionary<string, string>();
            [LabelText("支持Odin的注册表")]
            public bool EnableOdinRegester = true;
            [LabelText("支持ES的SoData分组命名")]
            public bool EnableSoDataGroupUp = true;
            [LabelText("带(原名)")]
            public bool WithOriginal = true;

            public string GetNewName(string typeName)
            {
                if (HandSelfRename.ContainsKey(typeName)) return HandSelfRename[typeName] + ((WithOriginal) ? $"({typeName})" : "");
                Type type = System.Type.GetType(typeName);
                if (EnableOdinRegester)
                {
                    var ii = type.GetAttribute<TypeRegistryItemAttribute>();
                    if (ii != null)
                    {
                        return ii.Name + ((WithOriginal) ? $"({typeName})" : "");
                    }
                }
                if (EnableSoDataGroupUp)
                {
                    var ii = type.GetAttribute<ESDisplayNameKeyToTypeAttribute>();
                    if (ii != null)
                    {
                        return ii.DisplayKeyName + ((WithOriginal) ? $"({typeName})" : "");
                    }
                }
                return typeName;
            }
        }
        #endregion
        #region 项目显示控制
        [LabelText("项目显示控制")]
        public ProjectShow Project = new ProjectShow();

        [Serializable]
        public class ProjectShow
        {
            [LabelText("启用项目显示控制")] public bool EnableProjectShow = false;
            [LabelText("初始偏移")] public float startTextOffset_ = -50;
            [LabelText("默认宽度")] public float textwidth_ = 50;
            [LabelText("根据名字")] public List<ProjectShowHandle> nameToHandle = new List<ProjectShowHandle>();
            [LabelText("网络包标识"), SerializeReference] public ContentGetter contentForNet;
            [LabelText("本地包标识"), SerializeReference] public ContentGetter contentForLocal;
            [Serializable]
            public class ProjectShowHandle
            {
                [LabelText("原名字"), InlineButton("seletion", "选中的资产名")] public string oldName = "ES";
                [LabelText("图标"), SerializeReference] public ContentGetter content = new ContentGetter_Label() { content = "名字" };
                private void seletion()
                {
                    if (Selection.activeObject != null)
                    {
                        oldName = Selection.activeObject.name;
                    }
                }
            }
        }

        #endregion
        #region 标签和层级辅助
        [LabelText("标签和层级辅助"), NonSerialized, OdinSerialize]
        public TagsAndLayers TagAndLayer = new TagsAndLayers();
        [Serializable, TypeRegistryItem("标签和层级辅助")]
        public class TagsAndLayers
        {
            [LabelText("自动更新")]
            public bool autoRefresh = true;
            [LabelText("记忆标签")]
            public List<string> memoryTags = new List<string>();
            [NonSerialized, OdinSerialize]
            [LabelText("记忆层级")]
            public Dictionary<int, string> memoryLayers = new Dictionary<int, string>();

            public bool GetDirty()
            {
                bool dirty = false;
                var nowTags = KeyValueMatchingUtility.SafeEditor.GetAllTags();
                foreach (var i in nowTags)
                {
                    //不包含当前的任意值
                    if (!memoryTags.Contains(i))
                    {
                        memoryTags = nowTags.ToList();
                        dirty =true;
                        break;
                    }
                }

                var nowLayers = KeyValueMatchingUtility.SafeEditor.GetAllLayers();
                foreach(var (k,v) in nowLayers)
                {
                    if(memoryLayers.TryGetValue(k,out var value))
                    {
                        if (v != value)
                        {
                            dirty = true;
                            memoryLayers = nowLayers.ToDictionary((f)=>f.Key,(f)=>f.Value);
                            break;
                        }
                    }
                    else
                    {
                        memoryLayers = nowLayers.ToDictionary((f) => f.Key, (f) => f.Value);
                        dirty = true;
                        break;
                    }
                }
                if (dirty)
                {
                    EditorUtility.SetDirty(GlobalDataForEditorOnly.Instance);
                }
                return dirty;
            }
        }

        #endregion
        #endregion
#endif

    }

}

