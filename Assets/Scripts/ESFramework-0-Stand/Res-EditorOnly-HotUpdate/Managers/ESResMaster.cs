using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using static UnityEngine.Rendering.DebugUI;
using ES.GlobalDataStand;

namespace ES
{
    [DefaultExecutionOrder(-3)]
    public class ESResMaster : SingletonAsSeriMono<ESResMaster>
    {
        #region 全局设置

        #region AB工具设置
        public GlobalDataReferStand<GlobalDataForResMaster> ReferGlobalData = new GlobalDataReferStand<GlobalDataForResMaster>();
        

        private void clearTest()
        {

            ReferGlobalData.Current.toHash = new Dictionary<string, string>();
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);//必要的
#endif
        }
        private void clearTest2()
        {
            /*#if UNITY_EDITOR
                        EditorUtility.SetDirty(this);//必要的
            #endif*/
        }

       

        #endregion

      

        #region 主包
        public AssetBundle MainBundle;
        public AssetBundleManifest MainManifest;
        #endregion

      

        #endregion

        #region 加载任务链

        private int mMaxCoroutineCount = 8; //最快协成大概在6到8之间
        private LinkedList<IWithEnumeratorTask> ResLoadTasks = new LinkedList<IWithEnumeratorTask>();
        private bool isLoading = false;
        private void TryStartLoadTask()
        {
            if (ResLoadTasks.Count == 0) return;
            if (isLoading) return;
            StartCoroutine(LoadResTask());
        }
        public void PushResLoadTask(IWithEnumeratorTask task)
        {
            if (task == null)
            {
                return;
            }
            ResLoadTasks.AddLast(task);
            TryStartLoadTask();
        }
        private void OnLoadTaskOK()
        {

        }
        private IEnumerator LoadResTask()
        {
            isLoading = true;
            while (ResLoadTasks.Count > 0)
            {
                var task = ResLoadTasks.First;
                ResLoadTasks.RemoveFirst();
                yield return StartCoroutine(task.Value.DoTaskAsync(OnLoadTaskOK));
                yield return null;
            }
            isLoading = false;
            yield return null;
        }
        #endregion

        #region 池化
        public ESSimpleObjectPool<ResSourceSearchKey> PoolForResSourceSearchKey = new ESSimpleObjectPool<ResSourceSearchKey>(() => new ResSourceSearchKey(),
            (f) =>
            {
                f.AssetPath = null;
                f.AssetType = null;
                f.OwnerAssetBundle = null;
            }
            , 30);

        public ESSimpleObjectPool<InternalResourceRes> PoolForInternalResSource = new ESSimpleObjectPool<InternalResourceRes>(() => new InternalResourceRes(),
          (f) =>
          {

          }
          , 30);

        public ESSimpleObjectPool<AssetResSource> PoolForAssetResSource = new ESSimpleObjectPool<AssetResSource>(() => new AssetResSource(),
           (f) =>
           {

           }
           , 30);
        public ESSimpleObjectPool<LocalAssetBundleResSource> PoolForAssetBundleResSource = new ESSimpleObjectPool<LocalAssetBundleResSource>(() => new LocalAssetBundleResSource(),
         (f) =>
         {

         }
         , 30);
        public ESSimpleObjectPool<LocalABAssetResSource> PoolForABAssetResSource = new ESSimpleObjectPool<LocalABAssetResSource>(() => new LocalABAssetResSource(),
         (f) =>
         {

         }
         , 30);
        public ESSimpleObjectPool<ESResLoader> PoolForESLoader = new ESSimpleObjectPool<ESResLoader>(() => new ESResLoader(),
       (f) =>
       {

       }
       , 30);
        #region 池操作在这里
        public ResSourceSearchKey GetInPool_ResSourceSearchKey(string assetName, string ownerBundleName = null, ResSourceLoadType loadType = ResSourceLoadType.ABAsset, Type assetType = null)
        {
            var resSearchRule = ESResMaster.Instance.PoolForResSourceSearchKey.GetInPool();
            resSearchRule.AssetPath = assetName.ToLower();
            resSearchRule.OwnerAssetBundle = ownerBundleName == null ? null : ownerBundleName.ToLower();
            resSearchRule.AssetType = assetType;
            resSearchRule.OriginalAssetName = assetName;
            resSearchRule.LoadType = loadType;
            return resSearchRule;
        }
        public InternalResourceRes GetInPool_InternalResSource(string name, string onwerBundleName, Type assetTypde)
        {
            var res = ESResMaster.Instance.PoolForInternalResSource.GetInPool();
            res.AssetPath = name;
            res.AssetType = typeof(AssetBundle);
            return res;
        }
        public AssetResSource GetOneInPool_AssetResSource(string name, string onwerBundleName, Type assetTypde)
        {
            var res = ESResMaster.Instance.PoolForAssetResSource.GetInPool();
            if (res != null)
            {
                res.AssetPath = name;
                res.mOwnerBundleName = onwerBundleName;
                res.AssetType = assetTypde;
                res.InitAssetBundleName();
            }

            return res;
        }
        public LocalAssetBundleResSource GetInPool_AssetBundleResSource(string name)
        {
            var res = ESResMaster.Instance.PoolForAssetBundleResSource.GetInPool();

            res.AssetPath = name;
            res.AssetType = typeof(AssetBundle);
            res.InitAssetBundleName();

            return res;
        }

        public ESResLoader GetInPool_ESLoader()
        {
            return PoolForESLoader.GetInPool();
        }
        #endregion

        #endregion

        #region 全局资源源加载和创建
        public class ResTable : KeyGroup<string, ESResSource>
        {
            internal ESResSource GetResBySearchKeys(ResSourceSearchKey resSearchKeys)
            {
                if (resSearchKeys == null) { Debug.LogError("空搜索键"); return null; }
                var assetName = resSearchKeys.AssetPath;
                if (!Groups.TryGetValue(assetName, out var theReses))
                {
                    return null;
                }
                IEnumerable<ESResSource> reses = theReses;

                if (resSearchKeys.AssetType != null)
                {
                    reses = reses.Where(res => res.AssetType == resSearchKeys.AssetType);
                }

                if (resSearchKeys.OwnerAssetBundle != null)
                {
                    reses = reses.Where(res => res.OwnerAssetBundleName == resSearchKeys.OwnerAssetBundle);
                }

                return reses.FirstOrDefault();
            }
        }
        public ResTable AllResSource = new ResTable();

        public ESResSource CreateNewResByKey(ResSourceSearchKey resSearchKeys)
        {
            var retRes = resSearchKeys.LoadType switch
            {
                ResSourceLoadType.AssetBundle => CreateResSource_AssetBundle(resSearchKeys),
                ResSourceLoadType.ABAsset => CreateResSource_ABResource(resSearchKeys),
                ResSourceLoadType.ABScene => null,
                ResSourceLoadType.InternalResource => CreateResSource_InternalResource(resSearchKeys),
                ResSourceLoadType.LocalImageRes => null,
                ResSourceLoadType.NetImageRes => null,
                _ => null
            }; ;
            /*.Where(creator => creator.Match(resSearchKeys))
            .Select(creator => creator.Create(resSearchKeys))
            .FirstOrDefault();*/

            if (retRes == null)
            {
                Debug.LogError("创建资源源失败了. 找不到这个查找键" + resSearchKeys);
                return null;
            }

            return retRes;
        }
        public ESResSource GetResSourceBySearchKeys(ResSourceSearchKey searchKey, bool ifNullCreateNew = true)
        {
            var res = AllResSource.GetResBySearchKeys(searchKey);

            if (res != null)
            {
                return res;
            }

            if (!ifNullCreateNew)
            {
                Debug.LogErrorFormat("没找到资源，并且这里未允许创建:{0}", searchKey);
                return null;
            }

            res = CreateNewResByKey(searchKey);

            if (res != null)
            {
                AllResSource.TryAdd(res.AssetPath, res);
            }

            return res;
        }
        public AssetBundle HasLoadedAB(string name)
        {
            foreach (var i in AssetBundle.GetAllLoadedAssetBundles())
            {
                if (i.name == name) return i;
            }
            return null;
        }
        public ESResLoader MainLoader = new ESResLoader();
        #endregion

        #region 创建各种资源源的方法
        public ESResSource CreateResSource_AssetBundle(ResSourceSearchKey resSearchKeys)
        {
            var use = PoolForAssetBundleResSource.GetInPool();
            use.AssetPath = resSearchKeys.AssetPath;
            use.AssetType = typeof(AssetBundle);
            use.InitAssetBundleName();
            return use;
        }
        public ESResSource CreateResSource_InternalResource(ResSourceSearchKey resSearchKeys)
        {
            var use = PoolForInternalResSource.GetInPool();
            use.AssetPath = resSearchKeys.AssetPath;
            use.AssetType = resSearchKeys.AssetType;
            return use;
        }

        public ESResSource CreateResSource_ABResource(ResSourceSearchKey resSearchKeys)
        {
            var use = PoolForABAssetResSource.GetInPool();
            use.AssetPath = resSearchKeys.AssetPath;
            use.OwnerAssetBundleName = resSearchKeys.OwnerAssetBundle;
            use.AssetType = resSearchKeys.AssetType;
            return use;
        }
        #endregion

        #region AB依赖与路径
        
       
        public class TheResDatas
        {
            public string[] GetAllDependenciesByUrl(string url)
            {
                Debug.Log("依赖测试");
                return null;
            }
            public string[] GetAllDependenciesByLocalAB(string bundleName)
            {
                return ESResMaster.Instance.MainManifest.GetAllDependencies(bundleName);
            }

            public void LoadFromFile(string outRes)
            {

            }
            public void Reset()
            {

            }
            public IEnumerator LoadFromFileAsync(string outRes)
            {
                yield break;
            }
            /*  AssetData GetAssetData(ResSearchKeys resSearchKeys);
              int AddAssetBundleName(string abName, string[] depends, out AssetDataGroup @group);
            */
            public string GetABHash(string assetName)
            {
                return "";
            }
        }

        public static class ABHelper
        {
            public static TheResDatas AssetBundleConfigFile { get; } = new TheResDatas();
            //                                    streaming                                        standardDir         platformDir
            public static string StreamingABWithPlatform = Application.streamingAssetsPath + "/" + "AssetBundles/" + Application.platform;

            public static string DownLoadUrlWithPlatform
            {
                [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
                get
                {
                    if (_DownLoadUrlWithPlatform == null) _DownLoadUrlWithPlatform = GlobalDataForResMaster.Instance.DownLoadURL + "/" + Application.platform.ToString();
                    return _DownLoadUrlWithPlatform;
                }
            }

            private static string _DownLoadUrlWithPlatform = null;
            public static string DownLoadLocalPath
            {
                [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
                get
                {
                    if (_DownLoadLocalPath == null) _DownLoadLocalPath = Path.Combine(Application.persistentDataPath, GlobalDataForResMaster.Instance.LocalDownLoadAdditionPath);
                    return _DownLoadLocalPath;
                }
            }
            private static string _DownLoadLocalPath = null;
            public static Dictionary<string, string> Hashs = new Dictionary<string, string>();
            public static Dictionary<string, WrapListString> Depends = new Dictionary<string, WrapListString>();
            public static Dictionary<string, int> States = new Dictionary<string, int>();//-1 无 0更新 1完美
        }

        #endregion

        #region 杂碎
        public T GetRes<T>(ResSourceSearchKey ResSourceSearchKey) where T : class, IResSource
        {
            return GetResSourceBySearchKeys(ResSourceSearchKey) as T;
        }

        public static void UnloadRes(UnityEngine.Object asset)
        {
            if (asset is GameObject)
            {
            }
            else
            {
                Resources.UnloadAsset(asset);
            }
        }

        public static void DestroyAObject(UnityEngine.Object asset)
        {
            UnityEngine.Object.Destroy(asset);
        }
        #endregion

        #region 类型支持
        [Serializable]
        public class ABTargetLocation
        {
            [LabelText("AB预先名")] public string ABPreName = "";
            [LabelText("首要加载目标"), EnumToggleButtons] public ABTarget ABTarget_ = ABTargetLocation.ABTarget.Net;
            [LabelText("必须下载")] public bool MustDownLoad = true;
            [LabelText("描述")] public string description = "i am only A AB";
            public enum ABTarget
            {
                [InspectorName("网络(可更新)")] Net,
                [InspectorName("本地(拒绝更新)")] Local,
            }
        }


        #endregion
        public bool IsFileCompareComplete = false;
        protected override void Awake()
        {
            base.Awake();
            AssetBundle.UnloadAllAssetBundles(unloadAllObjects:false);
            MainBundle = AssetBundle.LoadFromFile(ABHelper.StreamingABWithPlatform + "/" + Application.platform);
            Debug.Log("加载主包" + MainBundle);
            MainManifest = MainBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            Debug.Log("加载主依赖" + MainManifest);

            StartCoroutine(DownLoadBaseFile());



            /*  string[] allBundle = MainManifest.GetAllAssetBundles();

              foreach(var i in allBundle)
              {
                  Debug.Log("AB包" + i);
                  string[] allDepends = MainManifest.GetAllDependencies(i);
                  foreach(var ii in allDepends)
                  {
                      Debug.Log("---" + i + "依赖" + ii);
                  }
              }*/
        }
        IEnumerator DownLoadBaseFile()
        {
            IsFileCompareComplete = false;
            string netpathDependence = ABHelper.DownLoadUrlWithPlatform + "/" +ReferGlobalData.Current.HotUpdateDependenceFileName;
            string netpathToHash = ABHelper.DownLoadUrlWithPlatform + "/" + ReferGlobalData.Current.HotUpdatePreToHashFileName;
            Debug.Log(netpathDependence);
            var unityWebRequest1 = UnityWebRequest.Get(netpathDependence);



            string PathFordownLoadDependenceFile = ABHelper.DownLoadLocalPath + "/" + ReferGlobalData.Current.HotUpdateDependenceFileName;
            string PathFordownLoadPreToHashFile = ABHelper.DownLoadLocalPath + "/" + ReferGlobalData.Current.HotUpdatePreToHashFileName;


            unityWebRequest1.downloadHandler = new DownloadHandlerFile(PathFordownLoadDependenceFile);
            unityWebRequest1.SendWebRequest();

            var unityWebRequest2 = UnityWebRequest.Get(netpathToHash);
            unityWebRequest2.downloadHandler = new DownloadHandlerFile(PathFordownLoadPreToHashFile);
            unityWebRequest2.SendWebRequest();

            while (true)
            {
                Debug.Log(unityWebRequest1.downloadProgress + "&" + unityWebRequest2.downloadProgress);
                if (unityWebRequest1.isDone && unityWebRequest2.isDone)
                {
                    string jsonContentDepend = File.ReadAllText(PathFordownLoadDependenceFile);
                    string jsonContentHash = File.ReadAllText(PathFordownLoadPreToHashFile);

                    SerializedDictionary<string, string> handleAgo = JsonUtility.FromJson<SerializedDictionary<string, string>>(jsonContentDepend);
                    if (handleAgo != null)
                    {
                        ABHelper.Depends = new Dictionary<string, WrapListString>() { };
                        foreach (var (i, k) in handleAgo)
                        {
                            ABHelper.Depends.Add(i, new WrapListString() { strings = k.Split('&').ToList() });
                        }
                    }

                    SerializedDictionary<string, string> handleAgo2 = JsonUtility.FromJson<SerializedDictionary<string, string>>(jsonContentHash);
                    if (handleAgo2 != null)
                    {
                        ABHelper.Hashs = new Dictionary<string, string>();
                        foreach (var (i, k) in handleAgo2)
                        {
                            ABHelper.Hashs.Add(i, k);
                        }
                    }
                    else
                    {
                        Debug.LogError(jsonContentHash);
                    }
                    IsFileCompareComplete = true;
                    TestAllABStartState();
                    //结束
                    break;
                }
                yield return null;
            }

            yield return null;
        }

        private void TestAllABStartState()
        {
            List<string> UseFiles = new List<string>();
            if (Directory.Exists(ABHelper.DownLoadLocalPath))
            {
                Debug.Log("存在下载目标路径");
                // 设置搜索模式

                string fileExtension = "";

                string searchPattern = string.IsNullOrEmpty(fileExtension) ?
                    "*.*" : $"*.{fileExtension.TrimStart('.')}";

                // 获取所有文件
                string[] files = Directory.GetFiles(ABHelper.DownLoadLocalPath, searchPattern, SearchOption.AllDirectories);
                UseFiles.AddRange(files.Select((n)=>GlobalDataForResMaster.Instance.GetPostNameFromCompleteNameBySymbol(GlobalDataForResMaster.Instance.GetPostNameFromCompleteNameBySymbol(n,'/'),'\\')));

                // 过滤掉.meta文件
                UseFiles.RemoveAll(path => path.EndsWith(".meta"));
            }
            //测试
            foreach (var i in UseFiles)
            {
                Debug.Log("I Have File"+i);
                string pre = GlobalDataForResMaster.Instance.GetPreNameFromCompleteNameWithHash(i);
                if (ABHelper.Hashs.TryGetValue(pre, out var itWithHash))
                {
                    if (itWithHash == i) { ABHelper.States[pre] = 1; }
                    else
                    {
                        ABHelper.States[pre] = 0;
                    }
                }

            }
            foreach (var i in ABHelper.Hashs.Keys)
            {
                if (ABHelper.States.TryGetValue(i,out var State))
                {

                }
                else
                {
                    ABHelper.States[i] = -1;
                }
            }
        }
    }
}