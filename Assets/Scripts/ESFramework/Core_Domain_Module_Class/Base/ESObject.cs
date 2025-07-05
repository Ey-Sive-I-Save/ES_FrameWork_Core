using ES;
using FishNet.Object;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ES
{
    [TypeRegistryItem("Entity,Item的父类")]
 public class  ESObject  : Core
{

        #region 总重要信息
        [FoldoutGroup("【固有】"), LabelText("唯一ID"), ReadOnly] public long ID { get => _id; set => _id=value; }//-1代表未分配状态
        private long _id = -1;
#if UNITY_EDITOR
        [FoldoutGroup("【固有】"), LabelText("是联网的"), SerializeField] private bool Editor_IsNetObject = false;
#endif

        [FoldoutGroup("【固有】"), LabelText("刚体")] public Rigidbody Rigid;
        [FoldoutGroup("【固有】"), LabelText("原始动画器")] public Animator Anim;
        [FoldoutGroup("【固有】"), LabelText("ES超级标签")]
        public ESTagCollection ESTagsC = new ESTagCollection();


#if UNITY_EDITOR
        [BoxGroup("网络支持",VisibleIf = "Editor_IsNetObject")]
        [Required(errorMessage: "如果你制作网络游戏，一般需要配置给他一个FinshnetNetworkObject"), PropertyOrder(-10), PropertySpace(5, 15), InlineButton("DebugNO", "输出NO信息")]
        [LabelText("链接为网络对象")]
#endif

        public NetworkObject NetObject;

        #endregion

        #region 检查器专属
        //输出网络信息
        private void DebugNO()
        {
            Debug.Log("本人的" + NetObject.IsOwner);
            Debug.Log("客户的" + NetObject.IsClientInitialized);
            Debug.Log("服务器的" + NetObject.IsServerInitialized);
        }

        #endregion

        #region 委托事件

        [FoldoutGroup("委托事件(Unity)")] public Action<Collision,Vector3,bool> OnColHappen = (col,where,isEntity) => { };
        [FoldoutGroup("委托事件(Unity)")] public Action<Collider,Vector3,bool> OnTriHappen = (tri, where, isEntity) => { };

        [FoldoutGroup("委托事件(Unity)")] public Action<Entity, Vector3> OnColEntityHappen = (col, where) => { };
        [FoldoutGroup("委托事件(Unity)")] public Action<Entity, Vector3> OnTriEntityHappen = (tri, where) => { };

        [FoldoutGroup("委托事件(Unity)")] public Action<Link_DestroyWhy> OnDestroyHappen=(why)=> { };


        #endregion

        #region 隐藏属性表
        [NonSerialized] public Link_DestroyWhy whyDes;
        [NonSerialized] public Queue<Entity> ignoreEntities = new Queue<Entity>();
        #endregion

        #region 回调
        private void OnTriggerEnter(Collider other)
        {
            Vector3 center = (other.transform.position + transform.position) / 2;
            bool isEntity = false;
            if (other.gameObject.layer == ESEditorRuntimePartMaster.LayerEntity)
            {
              
                var e = other.GetComponent<Entity>();
                if (e != null)
                {
                   
                    if (ignoreEntities.Contains(e)) return;
                    
                    OnTriEntityHappen?.Invoke(e, center);
                    isEntity = true;
                }
                
            }
            OnTriHappen?.Invoke(other, center, isEntity);
        }
        private void OnCollisionEnter(Collision collision)
        {
           
            bool isEntity = false;
            Vector3 pos = collision.contacts[0].point;
            if (collision.gameObject.layer == ESEditorRuntimePartMaster.LayerEntity)
            {
                var e = collision.gameObject.GetComponent<Entity>();
                if (e != null)
                {
                    if (ignoreEntities.Contains(e)) return;
                    OnColEntityHappen?.Invoke(e, pos);
                    isEntity = true;
                }

            }
            OnColHappen?.Invoke(collision, pos,isEntity);
        }
        public virtual void TryDestroyThisESObject()
        {
            OnDestroyHappen?.Invoke(whyDes);
            Destroy(gameObject);
        }
        public void AddIgnoreEntity(Entity e)
        {
            ignoreEntities.Enqueue(e);
            
            if (ignoreEntities.Peek() == null)
            {
                ignoreEntities.Dequeue();
            }
        }
        #endregion
    }
}
   

