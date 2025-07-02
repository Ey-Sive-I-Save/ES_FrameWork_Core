using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ES
{

    public class TestABDownload : MonoBehaviour
    {
        public GameObject prefabForOneABMessage;
        public GameObject scrollView;

        private void Awake()
        {
            var AllAssetBundleMessage = ESResMaster.Instance.TargetLocations;
            var sort = AllAssetBundleMessage.OrderBy((n)=>(n.MustDownLoad?-1:1)+(n.ABTarget_== ESResMaster.ABTargetLocation.ABTarget.Net?0f:0.5f));

            foreach(var i in sort)
            {
                GameObject gg = Instantiate(prefabForOneABMessage,scrollView.transform);
                gg.GetComponent<OneABContent>().Setup(i);
                Debug.Log("排序后的AB包"+i+"Pre："+i.ABPreName+"\nDes："+i.description+"\nMust"+i.MustDownLoad+"\nTarget"+i.ABTarget_.ToString());
            }
           
        }
    }
}

