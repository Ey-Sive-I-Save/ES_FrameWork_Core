using ES.MonoTool;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


namespace ES
{

    public partial class ESEditorRuntimePartMaster : SingletonAsMono<ESEditorRuntimePartMaster>
    {
#if UNITY_EDITOR
        [LabelText("提示"), PropertyOrder(-5)]
        public Tool_ESReadMeClass readme = new Tool_ESReadMeClass() { readMeIn = "请在尽量在窗口处进行调整,这里不推荐\n因为" };
#endif
        [Button("test")]
        public void test()
        {
            var i= GlobalDataForEditorRunTime.Instance;
            Debug.Log(i);
        }
        [ShowInInspector,LabelText("<静态>游戏已退出")]
        public static bool IsQuit = false;
        private void OnApplicationQuit()
        {
            IsQuit = true;
        }
        #region 加载到
     
        #endregion 

        #region 配置源

        #endregion

        #region LayerMask
        #region 静态支持LayerMask Int
        public static int LayerDefault = 0;
        public static int LayerTransparentFX = 1;
        public static int LayerIgnoreRaycast = 2;
        // public static int LayerDefault = 0;
        public static int LayerWater = 4;
        public static int LayerUI = 5;
        public static int LayerWall = 6;
        public static int LayerGround = 7;
        /*  public static int LayerDefault = 0;
          public static int LayerDefault = 0;
          public static int LayerDefault = 0;
          public static int LayerDefault = 0;
          public static int LayerDefault = 0;*/
        public static int LayerEntity = 11;
        public static int LayerDroppedItem = 12;
        public static int LayerDamagable = 13;
        public static int LayerInteractive = 14;
        public static int LayerOnlyShow = 15;
        /*public static int LayerDefault = 0;
        public static int LayerDefault = 0;
        public static int LayerDefault = 0;
        public static int LayerDefault = 0;
        public static int LayerDefault = 0;*/



        #endregion
        #region 静态支持LayerMask LayerMask
        public static int LayerMaskMaskDefault = 1;
        public static int LayerMaskTransparentFX = 1 << 1;
        public static int LayerMaskIgnoreRaycast = 1 << 2;
        // public static int LayerMaskDefault = 0;
        public static int LayerMaskWater = 1 << 4;
        public static int LayerMaskUI = 1 << 5;
        public static int LayerMaskWall = 1 << 6;
        public static int LayerMaskGround = 1 << 7;
        /*  public static int LayerMaskDefault = 0;
          public static int LayerMaskDefault = 0;
          public static int LayerMaskDefault = 0;
          public static int LayerMaskDefault = 0;
          public static int LayerMaskDefault = 0;*/
        public static int LayerMaskEntity = 1 << 11;
        public static int LayerMaskDroppedItem = 1 << 12;
        public static int LayerMaskDamagable = 1 << 13;
        public static int LayerMaskInteractive = 1 << 14;
        public static int LayerMaskOnlyShow = 1 << 15;
        /*public static int LayerMaskDefault = 0;
        public static int LayerMaskDefault = 0;
        public static int LayerMaskDefault = 0;
        public static int LayerMaskDefault = 0;
        public static int LayerMaskDefault = 0;*/



        #endregion
        #region 静态支持LayerMask 特殊
        public static int LayerMaskSpecialMaskDefault = 1;
        public static int LayerMaskSpecialTransparentFX = 1 << 1;
        public static int LayerMaskSpecialIgnoreRaycast = 1 << 2;
        // public static int LayerMaskSpecialDefault = 0;
        public static int LayerMaskSpecialWater = 1 << 4;
        public static int LayerMaskSpecialUI = 1 << 5;
        public static int LayerMaskSpecialWall = 1 << 6;
        /*  public static int LayerMaskSpecialDefault = 0;
          public static int LayerMaskSpecialDefault = 0;
          public static int LayerMaskSpecialDefault = 0;
          public static int LayerMaskSpecialDefault = 0;
          public static int LayerMaskSpecialDefault = 0;*/
        public static int LayerMaskSpecialEntity = 1 << 11;
        public static int LayerMaskSpecialDroppedItem = 1 << 12;
        public static int LayerMaskSpecialDamagable = 1 << 13;
        public static int LayerMaskSpecialInteractive = 1 << 14;
        public static int LayerMaskSpecialOnlyShow = 1 << 15;
        /*public static int LayerMaskDefault = 0;
        public static int LayerMaskDefault = 0;
        public static int LayerMaskDefault = 0;
        public static int LayerMaskDefault = 0;
        public static int LayerMaskDefault = 0;*/



        #endregion
        #endregion

        #region 总览
        protected override void Awake()
        {
            IsQuit = false;
            base.Awake();
        }
        private void OnValidate()
        {
            Debug.Log("开始加载编辑器缓存数据");
        }
        private void Load()
        {
           
        }
     
        private void LoadTypesInfoKeys()
        {

        }
        #endregion

        #region 可寻类型分组键
  
       
        #endregion


    }
}