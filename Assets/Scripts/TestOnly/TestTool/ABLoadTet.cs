using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES {
    public class ABLoadTet : SerializedMonoBehaviour
    {
        [LabelText("测试模式"),EnumToggleButtons]
        public TestMode testMode = TestMode.AssetFinder;
        public enum TestMode
        {
            [InspectorName("加载资产")]LoadAAssetLocal,
            [InspectorName("查询资产")] AssetFinder,
            [InspectorName("实机模拟")] RunTimeTry,
        }
        #region 加载组

        [LabelText("结果"),ShowIfGroup("LoadAAssetLocal",VisibleIf ="@testMode==TestMode.LoadAAssetLocal")]
        public UnityEngine.Object Result;
        [LabelText("AB路径"),FilePath, ShowIfGroup("LoadAAssetLocal")]
        public string abPath = "";
        [LabelText("资源路径"), ShowIfGroup("LoadAAssetLocal")]
        public string assetPath = "";
        [LabelText("规定类型"),TypeSelectorSettings(FilterTypesFunction = "TypeFilterBoolForUnityObject"), ShowIfGroup("LoadAAssetLocal")]
        public Type type=null;
        [Button("加载"), ShowIfGroup("LoadAAssetLocal")]
        public void LoadTest()
        {
            AssetBundle.UnloadAllAssetBundles(false);

            AssetBundle ab;
            
            ab= AssetBundle.LoadFromFile(abPath);
            if(type==null)
            Result= ab.LoadAsset(assetPath);
            else Result = ab.LoadAsset(assetPath,type);
        }
        [Button("实例化一个"), ShowIfGroup("LoadAAssetLocal")]
        public void InsOne()
        {
            if(Result is GameObject g)
            {
                Instantiate(g);
            }
        }
        public bool TypeFilterBoolForUnityObject(Type type)
        {
            if (type.IsSubclassOf(typeof(UnityEngine.Object))&&!type.IsSubclassOf(typeof(UnityEngine.Component))) return true;
            return false;
        }
        #endregion

        #region 查询组
        [LabelText("查询资产"), ShowIfGroup("AssetFinder", VisibleIf = "@testMode==TestMode.AssetFinder")]
        public ESABAssetSelector selector=new ESABAssetSelector();

        #endregion

        void Test()
        {
            
        }
    }
}
