using ES;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES {
    public abstract class SingletonAsMono<This> : MonoBehaviour where This : MonoBehaviour
    {
        [LabelText("不销毁")] public bool DontDestroy = true;
        public static This Instance
        {
            get
            {
                if (_instance != null) return _instance;
                This t = UnityEngine.Object.FindAnyObjectByType<This>();
                if (t != null)
                {
                    _instance = t;
                    return t;
                }
#if UNITY_EDITOR

                if (maxDebug > 0) { Debug.LogError($"单例类{typeof(This).Name}场景中不存在"); maxDebug--; }
#endif
                /*GameObject g = GameObject.FindGameObjectWithTag("Manager");
                if (g == null)
                {
                    g = new GameObject();
                    g.name = $"临时的---单例类{typeof(This).Name}";
                }*/
                return _instance;

            }
            set { if (value != null) { _instance = value; }; }
        }
#if UNITY_EDITOR
        private static int maxDebug = 6;
#endif
        private static This _instance;
        protected virtual void Awake()
        {
            //Debug.Log("awake");
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

        /*  [SerializeField, HideInInspector]
          private SerializationData _serializationData; // Odin 序列化数据容器
          void ISerializationCallbackReceiver.OnBeforeSerialize()
          {
              // Odin 序列化
              UnitySerializationUtility.SerializeUnityObject(this, ref _serializationData);
          }

          void ISerializationCallbackReceiver.OnAfterDeserialize()
          {
              // Odin 反序列化
              UnitySerializationUtility.DeserializeUnityObject(this, ref _serializationData);
              Debug.Log("Trigger");
          }*/

    }
    public abstract class SingletonAsNormalClass<This> where This : new()
    {
        [LabelText("不销毁")] public bool DontDestroy = true;
        public static This Instance
        {
            get
            {
                if (_instance != null) return _instance;
                Debug.LogError($"单例普通类{typeof(This).Name}中不存在");
                return _instance = new This();

            }
            set { if (value != null) { _instance = value; }; }
        }
        private static This _instance;
        // Start is called before the first frame update
    }
    public abstract class SingletonAsSeriMono<This> : SerializedMonoBehaviour where This : MonoBehaviour
    {
        [LabelText("不销毁")] public bool DontDestroy = true;
        public static This Instance
        {
            get
            {
                if (_instance != null) return _instance;
                This t =UnityEngine.Object.FindAnyObjectByType<This>();
                if (t != null)
                {
                    _instance = t;
                    return t;
                }
                if (maxDebug > 0) { Debug.LogError($"单例类{typeof(This).Name}场景中不存在"); maxDebug--; }

                /*GameObject g = GameObject.FindGameObjectWithTag("Manager");
                if (g == null)
                {
                    g = new GameObject();
                    g.name = $"临时的---单例类{typeof(This).Name}";
                }*/
                return _instance;

            }
            set { if (value != null) { _instance = value; }; }
        }
        private static int maxDebug = 6;

        private static This _instance;
        protected virtual void Awake()
        {
            //Debug.Log("awake");
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
    }
}
