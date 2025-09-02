using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES
{

    public abstract class SingletonAsCore<This> : Core where This : MonoBehaviour
    {
        [LabelText("不销毁"), PropertyOrder(-5)] public bool DontDestroy = true;
        public static This Instance
        {
            get
            {
                if (_instance != null) return _instance;
                This t = Object.FindAnyObjectByType<This>();
                if (t != null)
                {
                    _instance = t;
                    return t;
                }
                Debug.LogError($"单例类{typeof(This).Name}场景中不存在");
                /*  GameObject g = GameObject.FindGameObjectWithTag("Manager");
                  if (g == null) {
                      g = new GameObject();
                      g.name = $"临时的---单例类{typeof(This).Name}";
                  }
                  return _instance=g.AddComponent<This>();*/
                return null;
            }
            set { if (value != null) { _instance = value; }; }
        }

        private static This _instance;
        protected override void OnBeforeAwakeRegister()
        {
            base.OnBeforeAwakeRegister();
            if (_instance == null || _instance == this)
            {
                _instance = this as This;
                if (_instance != null)
                {
                    if (DontDestroy) DontDestroyOnLoad(transform.root.gameObject);
                }
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }
        // Start is called before the first frame update
        // Update is called once per frame

    }
   
}
