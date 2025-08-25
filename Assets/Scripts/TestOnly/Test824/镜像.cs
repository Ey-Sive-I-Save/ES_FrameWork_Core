using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES {
    public class 镜像 : MonoBehaviour
    {
        public 原件 sourceMono;
        [HideLabel]
        public MirrorSync<string> mirror = new MirrorSync<string>();
        private void Awake()
        {
            var vali = new MirrorSyncListValidator<string>(mirror,this);
            sourceMono.source.BindMirrorValidators(vali);
        }
       
        private void OnEnable()
        {
            Refresh();
        }
        [Button("刷新")]
        public void Refresh()
        {
            if (mirror.HasInit)//已经初始化，以后增量
            {
                foreach (var i in mirror.SyncChanges())
                {
                    if (i.IsAdd)//添加时
                    {
                        Debug.Log("aa");
                        GameObject g = new GameObject(i.value);
                        g.transform.SetParent(transform);
                    }
                    else//移除时
                    {
                        Debug.Log("bb");
                        var t = transform.Find(i.value);
                        Debug.Log("bbAT"+t);
                        if (t != null)
                        {
                            DestroyImmediate(t.gameObject);
                        }
                    }
                }
            }
            else
            {
                foreach (var i in sourceMono.source.ValuesIEnumable)
                {
                    GameObject g = new GameObject(i);
                    g.transform.SetParent(transform);
                }
            }
        }
    }
}
