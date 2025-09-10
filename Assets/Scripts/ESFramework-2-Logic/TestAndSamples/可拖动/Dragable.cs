using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ES
{
    public class Dragable : MonoBehaviour
    {
        [LabelText("资源引用")]
        public ESAssetRefer assetrefer;
        [LabelText("资源引用")]
        public ESAssetRefer assetrefer2;

        [Space(40)]
        public string ss;
        [LabelText("资源引用")]
        public ESAssetRefer assetrefer3;










        [LabelText("资源引用")]
        public ESAssetRefer assetrefer4;
        public ESResLoader loader;
        private void OnEnable()
        {
            loader ??= new ESResLoader();
        }
        void Start()
        {
            assetrefer.BeLoadAssetAsync(loader,
                (b, ob) =>
                {
                    if (b)
                    {
                        
                        Debug.Log("成功加载" + ob);
                    }
                    else
                    {
                        Debug.Log("失败");
                    }
                });

            loader.LoadAll_Async();
        }
        private void OnDisable()
        {
            loader.TryAutoBePushedToPool();
        }


        void Update()
        {

        }
    }

    [Serializable]
    public class ESAssetRefer
    {
        [LabelText("搜索键"),InlineProperty]
        public ResSourceSearchKey key;
#if UNITY_EDITOR
        [LabelText("虚拟资源<仅编辑器>"), SerializeField, ReadOnly]
        private UnityEngine.Object vAsset;
#endif
        public void BeLoadAssetAsync(ESResLoader loader, Action<bool, IResSource> action)
        {
            loader.Add2Load(key, action);
        }
        public void EditorOnly_SetVAsset(UnityEngine.Object @object)
        {
#if UNITY_EDITOR
            vAsset = @object;

#endif
        }
        public UnityEngine.Object EditorOnly_GetVAsset()
        {
#if UNITY_EDITOR
            return vAsset;
#endif
        }
    }
}
