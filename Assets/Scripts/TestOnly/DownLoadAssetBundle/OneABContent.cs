using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;
using UnityEngine.UI;
using static ES.ESResMaster;

namespace ES
{
    public class OneABContent : MonoBehaviour
    {
        public ESResMaster.ABTargetLocation target;


        public TMP_Text showName;
        public TMP_Text showDes;
        public Toggle IsMust;
        public Toggle IsNet;


        public Toggle HasDownLoad;
        public Button DownLoad;
        public UnityEngine.UI.Slider progress;
        private float progress_ = 0.01f;
        private bool hasStartDown = false;

        public TMP_Text alert;

        public int state;
        public void Setup(ESResMaster.ABTargetLocation t)
        {
            target = t;
            showName.text = target.ABPreName;
            showDes.text = target.description;
            IsMust.isOn = target.MustDownLoad;
            IsNet.isOn = target.ABTarget_ == ESResMaster.ABTargetLocation.ABTarget.Net;
            HasDownLoad.isOn = false;
            progress.gameObject.SetActive(false);
            DetectAgain();
        }
        public void DetectAgain()
        {
            
            if (IsNet.isOn)
            {
                if (HasDownLoad.isOn)
                {
                    alert.text = "DownLoad OK！";
                    //已经下载了
                    HasDownLoad.gameObject.SetActive(false);
                    DownLoad.gameObject.SetActive(false);
                }
                else
                {
                    if (ESResMaster.Instance.IsFileCompareComplete)
                    {
                        //测试
                        if (ESResMaster.ABHelper.States.TryGetValue(target.ABPreName,out var state))
                        {
                            this.state = state;
                            if (state == 1)
                            {
                                alert.text = "DownLoad OK";
                                DownLoad.gameObject.SetActive(false);
                                HasDownLoad.isOn = true;
                            }
                            else if (state == 0)
                            {
                                alert.text = "Need ApplyBuffers";
                                var use = DownLoad.colors;
                                use.normalColor = Color.gray;
                                DownLoad.colors = use;
                                DownLoad.gameObject.SetActive(true);
                            }
                            else if (state == -1)
                            {
                                alert.text = "Need DownLoad(NEW)";
                                var use = DownLoad.colors;
                                use.normalColor = Color.white;
                                DownLoad.colors = use;
                                DownLoad.gameObject.SetActive(true);
                            }
                        }
                    }
                    else
                    {
                        alert.text = "Confi File Comparing Or Error！";
                    }
                }

            }
            else
            {
                //非网络
                HasDownLoad.gameObject.SetActive(false);
                DownLoad.gameObject.SetActive(false);
            }
            if (IsNet.isOn&& HasDownLoad)
            {

            }
        }
        public void TryDownload()
        {
            if (hasStartDown||state==1)
            {
                return;
            }
            hasStartDown = true;
            StartCoroutine(DownLoadIt());
            //正常
        }
        private IEnumerator DownLoadIt()
        {
            string netPath = ABHelper.DownLoadUrlWithPlatform + "/" + ESResMaster.ABHelper.Hashs[target.ABPreName];
            var unityWebRequest1 = UnityWebRequest.Get(netPath);

            string PathFordownLoad = ABHelper.DownLoadLocalPath + "/" + ESResMaster.ABHelper.Hashs[target.ABPreName];


            unityWebRequest1.downloadHandler = new DownloadHandlerFile(PathFordownLoad);
            unityWebRequest1.SendWebRequest();

            while (true)
            {
                progress_ = unityWebRequest1.downloadProgress;
                progress.gameObject.SetActive(true);
                progress.value = progress_;
                if (unityWebRequest1.isDone)
                {
                    state = ESResMaster.ABHelper.States[target.ABPreName] = 1;
                    progress.gameObject.SetActive(false);
                    progress.value = 0.01f;
                    //结束
                    break;
                }
                yield return null;
            }

            yield return null;
        }
        void Start()
        {

        }


        void Update()
        {
            DetectAgain();
        }
    }
}
