#if UNITY_EDITOR
using UnityEditor;
#endif
using ES;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;


using System.Linq;
using System.Runtime.CompilerServices;

using UnityEngine;
using System.Collections;
using System;

using System.Collections.Generic;  

namespace ES
{

    [TypeRegistryItem("纯编辑器控制器")]
    //必须纯编辑器环境才能用
    public class ESEditorOnlyPartMaster : SingletonAsSeriMono<ESEditorOnlyPartMaster>
    {
        [LabelText("全局数据引用")]
        public GlobalDataRefer<GlobalDataForEditorOnly> ReferGlobal = new GlobalDataRefer<GlobalDataForEditorOnly>();
#if UNITY_EDITOR
       
#endif
    }



#if UNITY_EDITOR
    #region 静态图标获取
    public static class UnityEditorIcons
    {
        #region 属性
        // 基本图标
        public static GUIContent GameObjectIcon => EditorGUIUtility.IconContent("GameObject Icon");
        public static GUIContent TransformIcon => EditorGUIUtility.IconContent("Transform Icon");
        public static GUIContent CameraIcon => EditorGUIUtility.IconContent("Camera Icon");
        public static GUIContent LightIcon => EditorGUIUtility.IconContent("Light Icon");
        public static GUIContent MeshRendererIcon => EditorGUIUtility.IconContent("MeshRenderer Icon");

        // 物理系统
        public static GUIContent RigidbodyIcon => EditorGUIUtility.IconContent("Rigidbody Icon");
        public static GUIContent ColliderIcon => EditorGUIUtility.IconContent("Collider Icon");
        public static GUIContent BoxColliderIcon => EditorGUIUtility.IconContent("BoxCollider Icon");
        public static GUIContent SphereColliderIcon => EditorGUIUtility.IconContent("SphereCollider Icon");

        // UI系统
        public static GUIContent CanvasIcon => EditorGUIUtility.IconContent("Canvas Icon");
        public static GUIContent ImageIcon => EditorGUIUtility.IconContent("Image Icon");
        public static GUIContent TextIcon => EditorGUIUtility.IconContent("Text Icon");
        public static GUIContent ButtonIcon => EditorGUIUtility.IconContent("Button Icon");

        // 2D系统
        public static GUIContent SpriteRendererIcon => EditorGUIUtility.IconContent("SpriteRenderer Icon");
        public static GUIContent TilemapIcon => EditorGUIUtility.IconContent("Tilemap Icon");
        public static GUIContent TileIcon => EditorGUIUtility.IconContent("Tile Icon");

        // 动画系统
        public static GUIContent AnimatorIcon => EditorGUIUtility.IconContent("Animator Icon");
        public static GUIContent AnimationIcon => EditorGUIUtility.IconContent("Animation Icon");
        public static GUIContent AvatarIcon => EditorGUIUtility.IconContent("Avatar Icon");

        // 音频系统
        public static GUIContent AudioSourceIcon => EditorGUIUtility.IconContent("AudioSource Icon");
        public static GUIContent AudioListenerIcon => EditorGUIUtility.IconContent("AudioListener Icon");

        // 特效系统
        public static GUIContent ParticleSystemIcon => EditorGUIUtility.IconContent("ParticleSystem Icon");
        public static GUIContent TrailRendererIcon => EditorGUIUtility.IconContent("TrailRenderer Icon");
        public static GUIContent LineRendererIcon => EditorGUIUtility.IconContent("LineRenderer Icon");

        // 资源类型
        public static GUIContent PrefabIcon => EditorGUIUtility.IconContent("Prefab Icon");
        public static GUIContent MaterialIcon => EditorGUIUtility.IconContent("Material Icon");
        public static GUIContent TextureIcon => EditorGUIUtility.IconContent("Texture Icon");
        public static GUIContent MeshIcon => EditorGUIUtility.IconContent("Mesh Icon");
        public static GUIContent ScriptableObjectIcon => EditorGUIUtility.IconContent("ScriptableObject Icon");

        // 编辑器UI图标
        public static GUIContent RefreshIcon => EditorGUIUtility.IconContent("Refresh");
        public static GUIContent SettingsIcon => EditorGUIUtility.IconContent("Settings");
        public static GUIContent SearchIcon => EditorGUIUtility.IconContent("Search");
        public static GUIContent ViewToolOrbit => EditorGUIUtility.IconContent("ViewToolOrbit");

        // 控制台图标
        public static GUIContent ConsoleErrorIcon => EditorGUIUtility.IconContent("console.erroricon");
        public static GUIContent ConsoleWarningIcon => EditorGUIUtility.IconContent("console.warnicon");
        public static GUIContent ConsoleInfoIcon => EditorGUIUtility.IconContent("console.infoicon");
        public static GUIContent ConsoleLogIcon => EditorGUIUtility.IconContent("console.logIcon");

        // 版本控制
        public static GUIContent VersionControl => EditorGUIUtility.IconContent("VersionControl");
        public static GUIContent VersionControlUpdated => EditorGUIUtility.IconContent("VersionControl/Updated");
        public static GUIContent VersionControlAdded => EditorGUIUtility.IconContent("VersionControl/Added");

        // 其他常用图标
        public static GUIContent FolderIcon => EditorGUIUtility.IconContent("Folder Icon");
        public static GUIContent SceneAssetIcon => EditorGUIUtility.IconContent("SceneAsset Icon");
        public static GUIContent PlayButton => EditorGUIUtility.IconContent("PlayButton");
        public static GUIContent PauseButton => EditorGUIUtility.IconContent("PauseButton");
        public static GUIContent StepButton => EditorGUIUtility.IconContent("StepButton");

        // 深色/浅色主题适配
        public static GUIContent GetGameObjectIcon(bool darkTheme = true) =>
            EditorGUIUtility.IconContent(darkTheme ? "d_GameObject Icon" : "sv_GameObject Icon");

        public static GUIContent GetFolderIcon(bool darkTheme = true) =>
            EditorGUIUtility.IconContent(darkTheme ? "d_Folder Icon" : "sv_Folder Icon");
        #endregion
        #region 静态字典
        public static class UnityEditorIconNames
        {
            public static readonly List<string> AllEnglish = new List<string>
    {
        // 基本对象图标
        "GameObject Icon",
        "Transform Icon",
        "Camera Icon",
        "Light Icon",
        "MeshRenderer Icon",
        
        // 物理系统
        "Rigidbody Icon",
        "Collider Icon",
        "BoxCollider Icon",
        "SphereCollider Icon",
        "CapsuleCollider Icon",
        "MeshCollider Icon",
        "WheelCollider Icon",
        "TerrainCollider Icon",
        "Cloth Icon",
        "HingeJoint Icon",
        
        // UI系统
        "Canvas Icon",
        "RectTransform Icon",
        "CanvasRenderer Icon",
        "Image Icon",
        "Text Icon",
        "Button Icon",
        "InputField Icon",
        "Slider Icon",
        "Scrollbar Icon",
        "Dropdown Icon",
        "Toggle Icon",
        
        // 2D系统
        "SpriteRenderer Icon",
        "Tilemap Icon",
        "Tile Icon",
        "SpriteMask Icon",
        "SortingGroup Icon",
        
        // 动画系统
        "Animator Icon",
        "Animation Icon",
        "Avatar Icon",
        "AvatarMask Icon",
        "RuntimeAnimatorController Icon",
        
        // 音频系统
        "AudioSource Icon",
        "AudioListener Icon",
        "AudioClip Icon",
        "AudioReverbZone Icon",
        
        // 特效系统
        "ParticleSystem Icon",
        "TrailRenderer Icon",
        "LineRenderer Icon",
        "LensFlare Icon",
        
        // 资源类型
        "Prefab Icon",
        "Material Icon",
        "Texture Icon",
        "Mesh Icon",
        "ScriptableObject Icon",
        "Shader Icon",
        "ComputeShader Icon",
        "PhysicMaterial Icon",
        
        // 编辑器UI图标
        "Refresh",
        "Settings",
        "Search",
        "ViewToolOrbit",
        "ViewToolMove",
        "ViewToolZoom",
        "ViewToolFPS",
        
        // 控制台图标
        "console.erroricon",
        "console.warnicon",
        "console.infoicon",
        "console.logIcon",
        
        // 版本控制
        "VersionControl",
        "VersionControl/Updated",
        "VersionControl/Added",
        "VersionControl/Deleted",
        "VersionControl/Conflicted",
        "VersionControl/Local",
        
        // 其他常用图标
        "Folder Icon",
        "SceneAsset Icon",
        "PlayButton",
        "PauseButton",
        "StepButton",
        "StopButton",
        "RecordButton",
        "PlayButtonProfile",
        "PreMatCube",
        "PreMatSphere",
        "PreMatCylinder",
        "PreMatQuad",
        
        // 深色主题前缀(d_)和浅色主题前缀(sv_)
        "d_GameObject Icon",
        "sv_GameObject Icon",
        "d_Folder Icon",
        "sv_Folder Icon",
        "d_SceneAsset Icon",
        "sv_SceneAsset Icon",
        
        // 编辑器窗口图标
        "UnityEditor.AnimationWindow",
        "UnityEditor.ConsoleWindow",
        "UnityEditor.GameView",
        "UnityEditor.HierarchyWindow",
        "UnityEditor.InspectorWindow",
        "UnityEditor.ProjectWindow",
        "UnityEditor.SceneView",
        
        // 工具栏图标
        "Toolbar Plus",
        "Toolbar Minus",
        "Toolbar Plus More",
        "Toolbar Minus More",
        "FilterByLabel",
        "FilterByType",
        
        // 脚本图标
        "cs Script Icon",
        "dll Script Icon",
        "js Script Icon",
        "boo Script Icon",
        
        // 平台相关
        "BuildSettings.Standalone",
        "BuildSettings.Android",
        "BuildSettings.iPhone",
        "BuildSettings.WebGL",
        "BuildSettings.WindowsStoreApps",
        
        // 其他
        "SaveActive",
        "TreeEditor.Trash",
        "TreeEditor.Refresh",
        "Moduleboard",
        "Profiler.CPU",
        "Profiler.Memory",
        "Profiler.Rendering",
        "Profiler.Open",
        "Profiler.Record"
    };

            public static readonly Dictionary<string, string> AllChinese = new Dictionary<string, string>
            {
        // 基础对象
        {"游戏对象图标", "GameObject Icon"},
        {"变换组件图标", "Transform Icon"},
        {"相机图标", "Camera Icon"},
        {"灯光图标", "Light Icon"},
        
        // 物理系统
        {"刚体图标", "Rigidbody Icon"},
        {"盒型碰撞体图标", "BoxCollider Icon"},
        {"球形碰撞体图标", "SphereCollider Icon"},
        {"胶囊碰撞体图标", "CapsuleCollider Icon"},
        
        // UI系统
        {"画布图标", "Canvas Icon"},
        {"图片组件图标", "Image Icon"},
        {"文本组件图标", "Text Icon"},
        {"按钮组件图标", "Button Icon"},
        
        // 2D系统
        {"精灵渲染器图标", "SpriteRenderer Icon"},
        {"瓦片地图图标", "Tilemap Icon"},
        {"瓦片图标", "Tile Icon"},
        
        // 动画系统
        {"动画控制器图标", "Animator Icon"},
        {"动画组件图标", "Animation Icon"},
        {"角色化身图标", "Avatar Icon"},
        
        // 音频系统
        {"音频源图标", "AudioSource Icon"},
        {"音频监听器图标", "AudioListener Icon"},
        
        // 特效系统
        {"粒子系统图标", "ParticleSystem Icon"},
        {"轨迹渲染器图标", "TrailRenderer Icon"},
        {"线条渲染器图标", "LineRenderer Icon"},
        
        // 资源类型
        {"预制体图标", "Prefab Icon"},
        {"材质图标", "Material Icon"},
        {"纹理图标", "Texture Icon"},
        {"网格图标", "Mesh Icon"},
        {"可编程对象图标", "ScriptableObject Icon"},
        
        // 编辑器UI
        {"刷新图标", "Refresh"},
        {"设置图标", "Settings"},
        {"搜索图标", "Search"},
        {"视图旋转工具图标", "ViewToolOrbit"},
        
        // 控制台
        {"错误图标", "console.erroricon"},
        {"警告图标", "console.warnicon"},
        {"信息图标", "console.infoicon"},
        
        // 版本控制
/*        {"版本控制图标", "VersionControl"},
        {"已更新图标", "VersionControl/Updated"},
        {"已添加图标", "VersionControl/Added"},*/
        
        // 其他常用
        {"文件夹图标", "Folder Icon"},
        {"场景资源图标", "SceneAsset Icon"},
        {"播放按钮", "PlayButton"},
        {"暂停按钮", "PauseButton"},
/*        {"停止按钮", "StopButton"},*/
        
        // 主题相关
        {"深色游戏对象图标", "d_GameObject Icon"},
/*        {"浅色游戏对象图标", "sv_GameObject Icon"},*/
        {"深色文件夹图标", "d_Folder Icon"},
/*        {"浅色文件夹图标", "sv_Folder Icon"},*/
        
        // 编辑器窗口
        {"动画窗口图标", "UnityEditor.AnimationWindow"},
        {"控制台窗口图标", "UnityEditor.ConsoleWindow"},
        {"游戏视图图标", "UnityEditor.GameView"},
        {"层级窗口图标", "UnityEditor.HierarchyWindow"},
        
        // 脚本类型
        {"C#脚本图标", "cs Script Icon"},
        {"DLL图标", "dll Script Icon"},
        {"JavaScript图标", "js Script Icon"},
        
        // 平台
        {"PC平台图标", "BuildSettings.Standalone"},
        {"安卓平台图标", "BuildSettings.Android"},
        {"iOS平台图标", "BuildSettings.iPhone"},
        {"WebGL平台图标", "BuildSettings.WebGL"},
        
        // 分析器
        {"CPU分析器图标", "Profiler.CPU"},
        {"内存分析器图标", "Profiler.Memory"},
        {"渲染分析器图标", "Profiler.Rendering"}
    };

        }
        #endregion
    }

    #endregion

#endif
}

