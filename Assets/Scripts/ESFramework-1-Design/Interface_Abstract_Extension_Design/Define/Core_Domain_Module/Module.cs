using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;


namespace ES
{
    [TypeRegistryItem("空模块")]
    public interface IModule : IESModule
    {
        public Core Core_Object { get; }
        public void _SetDomainAndCreateRelationshipOnly(IDomain Domain);
        public void _TrySetupPresetModule();
        public void FixedUpdate_MustSelfDelegate();

    }
    [TypeRegistryItem("抽象模块定义")]
    public abstract class Module<Core_, Domain_> : BaseESModule<Domain_>, IModule where Core_ : Core where Domain_ : class, IDomain<Core_>
    {

        #region 总重要信息

        [/*绑定扩展域*/ NonSerialized]
        public Domain_ Domain;//所在域-

        /*[LabelText("更新网络权限")]
        public NetWorkUpdateOption updateOption = NetWorkUpdateOption.Always;*/

        #endregion

        #region 只读便捷属性

        public Core_ Core//核心
        {
            get => Domain.Core;
        }
        public Core Core_Object => Core;

        public sealed override Domain_ GetHost //重写-还是获取核心
        {
            get => Domain;
        }
      /*  public DomainForGameCenterManager GameNormalDomain//游戏核心常规域
        {

            get =>null GameCenterManager.Instance.NormalDomain;
        }*/

        #endregion

        #region 补充逻辑
        public sealed override bool CanUpdating
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return true;
                /* if (Core.NetObject == null ||
                 updateOption == NetWorkUpdateOption.Always) return true;
                 if (updateOption == NetWorkUpdateOption.OnlyOwner)
                 {
                     if (Core.NetObject.IsOwner) return true;
                     return false;
                 }
                 if (updateOption == NetWorkUpdateOption.OnlyOther)
                 {
                     if (!Core.NetObject.IsOwner) return true;
                     return false;
                 }
                 if (updateOption == NetWorkUpdateOption.OnlyClient)
                 {
                     if (Core.NetObject.IsClientInitialized) return true;
                     return false;
                 }
                 if (updateOption == NetWorkUpdateOption.OnlyServer)
                 {
                     if (Core.NetObject.IsServerInitialized) return true;
                     return false;
                 }
                 if (updateOption == NetWorkUpdateOption.OnlyHost)
                 {
                     if (Core.NetObject.IsHostInitialized) return true;
                     return false;
                 }
                 return true;*/
            }
        }

        public sealed override void _SetHost(Domain_ host)
        {
            Domain = host;
            CreateRelationshipOnly();
            Signal_HasSubmit = true;
        }
        public sealed override void TryDestroySelf()
        {
            Signal_HasSubmit = false;
            Domain.TryRemoveFromIEnumableOnly(this);
        }
        #endregion

        #region 托管相关
        /*        protected override void Start(Domain_ hosting)
                {
                    Domain = hosting;
                    CreateRelationship();
                    base.Start(hosting);

                }

                protected override void OnWithDrawHosting(Domain_ hosting)
                {
                    Domain = hosting;
                    base.OnWithDrawHosting(hosting);

                }*/
        #endregion

        #region 可直接重写扩展逻辑(关键)
        /*  
         *  开始时(对于一次域和模块的绑定-只会进行一次)
          protected override void Start()
          {
              base.Start();
              &初始化数据，创建对象
              &整个模块周期只执行一次
        // 初始化/添加时  Submit->
          }
        *  启用时(从禁用到启用进行--和脚本几乎一致)
          protected override void OnEnable()
          {
              base.OnEnable();
              &可配合OnDisable重复触发
              &注销委托
              &仅启用时相关逻辑的开启
          }
          protected override void OnDisable()
          {
              base.OnDisable();
              &可配合OnEnable重复触发
              &注销委托
              &仅启用时相关逻辑的关闭
          }
          protected override void Update()
          {   
              &启用时每帧执行
              base.Update();
          }

          protected override void OnDestroy()
          {
              &被销毁，解除绑定,整个生命周期只有一次(可重复)
              base.OnDestroy();
               // 物体销毁/移除时  Submit->
               
          }*/

        //仅用于创建双向引用--不要写逻辑
        protected virtual void CreateRelationshipOnly() //创建   域 对自己 的 显性引用关系
        {
            //演示：  Domain.Module_AABB=this;
        }

        #endregion

        #region 自主扩展手动委托功能(为了性能考虑)
        public virtual void FixedUpdate_MustSelfDelegate()
        {

        }
        #endregion

        #region 显示提示等
        [ShowInInspector, GUIColor("Editor_ColorGetter"), LabelText(SdfIconType.CashCoin, Text = @"@ ""功能：""+ Editor_Description_  "),
            PropertyOrder(-100), Indent(-2), Tooltip("模块细则")]
        protected virtual string Editor_Description_ => "描述";


        protected virtual Color Editor_ColorGetter()
        {
            return Color.yellow;
        }
        #endregion

        #region 辅助功能
        //一般编辑器模式会用--用来单纯链接而不进行逻辑运行
        public void _SetDomainAndCreateRelationshipOnly(IDomain Domain)
        {
            if (Domain is Domain_ domain_)
            {
                this.Domain = domain_;
                CreateRelationshipOnly();
            }
        }
        //进行预设的设置
        public void _TrySetupPresetModule()
        {
#if UNITY_EDITOR
            if (Domain == null)
            {
                Debug.LogError("请先通过核心创建临时关系");

                if (Application.isEditor)
                    if (EditorUtility.DisplayDialog("尝试使用预设", $"您正在尝试应用预设<{Editor_Preset}>，在游戏未开始前,他需要核心先发起一次临时关系绑定！请去核心处》三小点》<ES>创建临时关系,创建临时的关系绑定后，再继续", "我这就去", "不去也得去！"))
                    {

                    }
                    else
                    {

                    }

                return;
            }
            Editor_SetupModuleByPreset(Editor_Preset);
#endif
        }
        #endregion

        #region 预设相关
#if UNITY_EDITOR
        [ValueDropdown("Editor_AllPresets"), LabelText("选择预设", SdfIconType.CaretDownSquareFill), SerializeField, InlineButton("_TrySetupPresetModule", "使用该预设"), GUIColor("@KeyValueMatchingUtility.ColorSelector.Color_01")]
        private string Editor_Preset = "预设1";
#endif
        public virtual string[] Editor_AllPresets => Editor_DefaultPresets;

      
        public static string[] Editor_DefaultPresets = { "预设1", "预设2", "预设3" };

        //重写该方法创建预设效果
        protected virtual void Editor_SetupModuleByPreset(string preset)
        {
            //演示
            /* switch (preset)
             {
                 case "预设1":
                     Debug.Log("使用预设1，把对象移动到000处");
                     Core.transform.position = default;
                     break;
                 case "预设2":
                     Debug.Log("使用预设2，给对象添加刚体");
                     Core.gameObject.AddComponent<Rigidbody>();
                         break;
                 case "预设3":
                     Debug.Log("使用预设1，把对象移动到000处");
                     Core.transform.position = default;
                     break;
             }*/
        }
        #endregion

    }

}