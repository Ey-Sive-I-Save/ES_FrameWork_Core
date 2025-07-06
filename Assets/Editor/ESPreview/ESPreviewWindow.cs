using ES;
using ES.EvPointer;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace ES
{
    
    public class ESPreviewWindow : ESWindowBase_Abstract<ESPreviewWindow>
    {
        public override GUIContent ESWindow_GetWindowGUIContent()
        {
            Texture2D texture = Resources.Load<Texture2D>("Sprites/ItMoting");
            var content = new GUIContent("依薇尔预览窗口", texture, "使用依薇尔预览工具提高数据查询效率");
            return content;
        }
        #region 数据滞留
        public Page_ShowSystemICON systemICON;
        public Page_CacheObjects cacheObjects;
        public Page_ViewFix viewFix;
        #endregion
        [MenuItem("Tools/ES工具/ES预览窗口")]
        public static void TryOpenWindow()
        {
            if (ESEditorRuntimePartMaster.Instance != null)
                OpenWindow();
            else Debug.LogError("确保场景中有ESEditorRuntimePartMaster");
        }

        protected override void ES_BuildMenuTree(OdinMenuTree tree)
        {
            base.ES_BuildMenuTree(tree);
            tree.Add("系统图标预览", systemICON ??= new Page_ShowSystemICON());
            MakePage_CacheObjects(tree, "缓存物体信息");
            tree.Add("修复", viewFix ??= new Page_ViewFix());
        }

        private void MakePage_CacheObjects(OdinMenuTree tree, string Menu)
        {
            tree.Add(Menu, cacheObjects ??= new Page_CacheObjects());
            Menu += "/";
            HashSet<GameObject> hasGS = new HashSet<GameObject>(Objects.Count);
            HashSet<UnityEngine.Object> hasOS = new HashSet<UnityEngine.Object>(Objects.Count);

            for (int i = 0; i < Objects.Count; i++)
            {
                Debug.Log(i);
                var ii = Objects[i];
                if (ii == null || hasOS.Contains(ii)) return;
                string ss = ii.name;
                if (ii is MonoBehaviour mo)
                {
                    GameObject gg = mo.gameObject;
                    if (hasGS.Contains(gg))
                    {

                    }
                    else
                    {
                        hasGS.Add(gg);
                        tree.Add(Menu + "游戏物体:" + mo.gameObject.name, new Page_Index_Object() { Object = gg });

                    }
                    ss = Menu + "游戏物体:" + mo.gameObject.name + "/" + "*脚本" + (ii.GetType());
                }
                hasOS.Add(ii);
                tree.Add(ss, new Page_Index_Object() { Object = ii });
            }
        }

        public override void ES_LoadData()
        {
            base.ES_LoadData();
            icons = new List<SystemICON>(100);
            foreach (var i in UnityEditorIcons.UnityEditorIconNames.AllChinese.Keys)
            {
                try
                {
                    string en = UnityEditorIcons.UnityEditorIconNames.AllChinese[i];
                    Texture tt = EditorGUIUtility.IconContent(en)?.image;
                    if (tt != null)
                    {

                        icons.Add(new SystemICON() { chi = i, eng = en, texture = tt });
                    }
                }
                catch (Exception e)
                {
                    var u = e.Data;
                }

            }
            if (PlayerPrefs.HasKey(keyForCache))
            {
                Debug.Log("开始加载GUID");
                pathForCache = JsonConvert.DeserializeObject<List<string>>(PlayerPrefs.GetString(keyForCache));
                if (pathForCache != null && pathForCache.Count > 0)
                {
                    Debug.Log("GUIDS" + pathForCache + pathForCache.Count);
                    Objects = new List<UnityEngine.Object>(pathForCache.Count);
                    foreach (var i in pathForCache)
                    {

                        Debug.Log("加载" + i);
                        if (int.TryParse(i, out var id))
                        {
                            Objects.Add(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(AssetDatabase.GUIDToAssetPath(i)));
                        }

                    }

                }
                else
                {
                    Debug.Log("空的");
                }
            }
        }
        public override void ES_SaveData()
        {
            base.ES_SaveData();
            pathForCache = new List<string>(Objects.Count);
            Debug.Log("开始保存GUID");
            foreach (var i in Objects)
            {

                string path = i.GetInstanceID().ToString();
                pathForCache.Add(path);
                Debug.Log("保存" + i + path);
            }
            PlayerPrefs.SetString(keyForCache, JsonConvert.SerializeObject(pathForCache, formatting: Formatting.Indented));
            PlayerPrefs.Save();
        }
        #region 存储

        public static List<SystemICON> icons = new List<SystemICON>();
        private static List<string> pathForCache = new List<string>();
        public static List<UnityEngine.Object> Objects = new List<UnityEngine.Object>();
        public static string keyForCache = "window-preview-cache-pathForCache";
        #endregion
    }
    [Serializable, TypeRegistryItem("系统ICON")]
    public class SystemICON
    {
        [LabelText("中文"), ReadOnly, VerticalGroup("a")]
        [TableColumnWidth(50)]
        public string chi;
        [LabelText("英文"), ReadOnly, VerticalGroup("a")]
        public string eng;
        [LabelText("图标"), ReadOnly, VerticalGroup("a")]
        public Texture texture;
    }
    #region 预览图表集
    [Serializable]
    public class Page_ShowSystemICON
    {
        [ShowInInspector, HideLabel, InlineProperty, TableList(ShowIndexLabels = true, HideToolbar = false, AlwaysExpanded = true)]
        public List<SystemICON> show { get => ESPreviewWindow.icons; set { } }
        public Page_ShowSystemICON()
        {


        }
    }
    #endregion

    #region 缓冲物体表
    [Serializable, TypeRegistryItem("缓存物体表")]
    public class Page_CacheObjects
    {
        [ShowInInspector]
        public List<UnityEngine.Object> objects => ESPreviewWindow.Objects;
    }
    [Serializable]
    public class Page_Index_Object
    {
        [HideLabel, DisplayAsString(fontSize: 24), ShowInInspector, PropertyOrder(-1)]
        public string ss => "操作物体" + Object.name;
        [InlineEditor, HideLabel()]
        public UnityEngine.Object Object;
    }
    #endregion

    #region 修复
    public class Page_ViewFix
    {
        [ButtonGroup("视觉组")]
        [Button("修复选中物体的隐藏")]
        public void Button_FixHideGameobjectFlags()
        {
            var a = Selection.activeGameObject;
            if (a != null)
            {
                a.hideFlags = a.hideFlags & ~HideFlags.HideInInspector;
                var use = a.GetComponents<Component>();
                foreach (var i in use)
                {
                    a.hideFlags = a.hideFlags & ~HideFlags.HideInInspector;
                }
            }

        }
        [ButtonGroup("模块组")]
        [Button("移除全部空Clip")]
        public void Button_FixNullReferClips()
        {
            var all = UnityEngine.Object.FindObjectsByType<Core>(sortMode: FindObjectsSortMode.None);
            foreach (var i in all)
            {
                var allDomain = i.GetComponentsInChildren<IDomain>();
                foreach (var ii in allDomain)
                {
                    ii.TryRemoveNullModules();
                    if (ii is MonoBehaviour mono)
                    {
                        EditorUtility.SetDirty(mono);
                        Undo.RecordObject(mono, "");
                        EditorUtility.SetDirty(mono.gameObject);
                        Undo.RecordObject(mono.gameObject, "");
                        AssetDatabase.SaveAssets();

                    }

                }
            }

        }


        #region 命名组
        [Serializable]
        public class TwoString
        {
            [LabelText("老名字")] public string ago = "老名字";
            [LabelText("新名字")] public string now = "新名字";
            [LabelText("允许片段")] public bool EnablePart = true;
            [Button("全局物体命名切换"), PropertySpace(5, 15)]
            public void Button_RenameGameObject()
            {
                var all = UnityEngine.Object.FindObjectsByType<GameObject>(sortMode: FindObjectsSortMode.None);
                if (EnablePart)
                {
                    foreach (var i in all)
                    {
                        i.name = i.name.Replace(ago, now);
                    }
                }
                else
                {
                    foreach (var i in all)
                    {
                        if (i.name == ago)
                        {
                            i.name = now;
                        }
                    }
                }
            }

        }
        [LabelText("游戏物体更名")]
        public TwoString twoName = new TwoString();
        #endregion

        #region 查ID组
        [Serializable]
        public class InstanceIDSearcher
        {
            public string guid = "66660";
            public string instanceID = "001001";
            [Button("查询选中物体的信息"), PropertySpace(5, 15)]
            public void Button_FindGUID()
            {
                var use= Selection.activeObject;
                instanceID = use.GetInstanceID().ToString();
                /*UnityEngine.Object[] allObjects = UnityEngine.Object.FindObjectsOfType<UnityEngine.Object>(true); // 包含隐藏对象
                foreach (UnityEngine.Object obj in allObjects)
                {
                    if (obj.GetInstanceID() ==int.Parse(guid))
                    {
                        GameObject targetObj = obj as GameObject;
                        Selection.activeObject = targetObj;
                        break;
                    }
                }*/
              /*  string path= AssetDatabase.GUIDToAssetPath(id);
                var uu = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
               */
            }
        }
        [LabelText("查GUID物体")]
        public InstanceIDSearcher gUIDSearcher = new InstanceIDSearcher();
        #endregion
    }

    #endregion
}

