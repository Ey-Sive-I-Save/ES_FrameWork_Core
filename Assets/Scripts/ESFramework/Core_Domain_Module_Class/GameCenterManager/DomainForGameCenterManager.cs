using ES;
using ES.EvPointer;

using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor.TypeSearch;
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace ES
{
    public class DomainForGameCenterManager : Domain<GameCenterManager, ModuleForGamecenterManager> {
    }

    #region 基本切片模范
    [Serializable]
    public abstract class ModuleForGamecenterManager : Module<GameCenterManager, DomainForGameCenterManager>
    {
        protected override string Description_ => "游戏管理器切片域";

       
    }
    [Serializable, TypeRegistryItem("模块01")]
    public class module1 : ModuleForGamecenterManager
    {
        float f = 5;
        ESResLoader loader;
        public string StartWith = "来自模块1";
        public override string[] Editor_AllPresets => base.Editor_AllPresets;
        protected override void Start()
        {
            base.Start();
            Debug.Log("开始");
        }
        protected override void OnEnable()
        {
            loader ??= ESResMaster.Instance.GetInPool_ESLoader();
         /*   var keyFor1 = ESResMaster.Instance.GetInPool_ResSourceSearchKey("方块_prefab", loadType: ResSourceLoadType.AssetBundle);

            loader.Add2Load(keyFor1, (b, ob) => { if (b) Debug.Log(StartWith + "加载成功" + keyFor1 + ob); else Debug.LogError(StartWith + "加载失败" + keyFor1); });
            */
            var keyForOb = ESResMaster.Instance.GetInPool_ResSourceSearchKey("方块 4", "方块_prefab", loadType: ResSourceLoadType.ABAsset);
            loader.Add2Load(keyForOb ,(b, ob) => { if (b) Debug.Log(StartWith + "加载成功" + keyForOb + ob); else Debug.LogError(StartWith + "加载失败" + keyForOb); });

            Debug.Log(StartWith + "全部开始加载" + Time.time);
            loader.LoadAll_Async(() => Debug.Log(StartWith + "加载全部完成" + Time.time));
            /*
            var keyFor1 = ESResMaster.Instance.GetInPool_ResSourceSearchKey("Prefab/Rub/方块 1", loadType: ResSourceLoadType.InternalResource);
            var keyFor2 = ESResMaster.Instance.GetInPool_ResSourceSearchKey("Prefab/Rub/方块 2", loadType: ResSourceLoadType.InternalResource);
            var keyFor3 = ESResMaster.Instance.GetInPool_ResSourceSearchKey("Prefab/Rub/方块 3", loadType: ResSourceLoadType.InternalResource);
            var keyFor4 = ESResMaster.Instance.GetInPool_ResSourceSearchKey("Prefab/Rub/方块 4", loadType: ResSourceLoadType.InternalResource);
            var keyForNULL = ESResMaster.Instance.GetInPool_ResSourceSearchKey("Prefab/Rub/方块888 5", loadType: ResSourceLoadType.InternalResource);
            loader.Add2Load(keyFor1, (b,ob) => { if (b) Debug.Log(StartWith+"加载成功" +keyFor1+ob); else Debug.LogError(StartWith+"加载失败" +keyFor1); });
            loader.Add2Load(keyFor2, (b, ob) => { if (b) Debug.Log(StartWith+"加载成功" + keyFor2 + ob); else Debug.LogError(StartWith+"加载失败" + keyFor2); });
            loader.Add2Load(keyFor3, (b, ob) => { if (b) Debug.Log(StartWith+"加载成功" + keyFor3 + ob); else Debug.LogError(StartWith+"加载失败" + keyFor3); });
            loader.Add2Load(keyFor4, (b, ob) => { if (b) Debug.Log(StartWith+"加载成功" + keyFor4 + ob); else Debug.LogError(StartWith+"加载失败" + keyFor4); });
            loader.Add2Load(keyForNULL, (b, ob) => { if (b) Debug.Log(StartWith+"加载成功" + keyForNULL + ob); else Debug.LogError(StartWith+"加载失败" + keyForNULL); });
            //同步加载
            Debug.Log(StartWith+"全部开始加载" + Time.time);
            loader.LoadAll_Async(() => Debug.Log(StartWith+"加载全部完成" +Time.time));*/
            base.OnEnable();
           
        }

        protected override void Update()
        {
            base.Update();
        }
       
    }
    [Serializable, TypeRegistryItem("模块网络下载")]
    public class module0 : ModuleForGamecenterManager
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            string url = "https://eysive-teset.oss-cn-beijing.aliyuncs.com/WindowsPlayer/%E6%96%B9%E5%9D%97_prefab";
            Core.StartCoroutine(DownLoad(url));
        }
        private IEnumerator DownLoad(string url)
        {
            
            var path = Path.Combine(Application.persistentDataPath,"newAB_ab");
            using (var unityWebRequest = UnityWebRequestAssetBundle.GetAssetBundle(url)){
                unityWebRequest.downloadHandler = new DownloadHandlerFile(path);
               unityWebRequest.SendWebRequest();
                while (!unityWebRequest.isDone)
                {
                    Debug.Log("下载中" + unityWebRequest.downloadProgress*100+"%");
                    yield return null;
                }
                if(unityWebRequest.result== UnityWebRequest.Result.Success)
                {
                    AssetBundle aa = AssetBundle.LoadFromFile(path);
                    Debug.Log("下载成功" + aa);
                    foreach (var i in aa.GetAllAssetNames())
                    {
                        Debug.Log("包含资源" + i);
                    }
                    

                }else
                {
                    Debug.Log("下载傻白");
                }
            }
            yield return null;
        }
        protected override void Update()
        {

            base.Update();
        }
    }
    [Serializable, TypeRegistryItem("模块02")]
    public class module2 : ModuleForGamecenterManager
    {
        protected override void Update()
        {

            base.Update();
        }
    }
    [Serializable, TypeRegistryItem("模块03")]
    public class module3 : ModuleForGamecenterManager
    {
        protected override void Update()
        {

            base.Update();
        }
    }


    [Serializable, TypeRegistryItem("模块04")]
    public class module4 : ModuleForGamecenterManager
    {
        protected override void Update()
        {
            base.Update();
        }

        public override string[] Editor_AllPresets => presetsForModule04;
        public static string[] presetsForModule04 = {"弱小的","强大的","特殊的" };
        protected override void SetupModuleByPreset(string preset)
        {
            switch (preset)
            {
                case "弱小的":
                    Core.gameObject.AddComponent<Rigidbody>();
                    break;
                case "强大的":
                    Core.transform.position = default;
                    break;
                case "特殊的":
                    if (Core.GetComponent<Entity>() == null)
                    {
                        this.EnabledSelf = true;
                    }
                    break;

            }
        }
    }
}


#endregion