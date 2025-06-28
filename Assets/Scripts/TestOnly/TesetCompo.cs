using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES {

    // 自定义组件需添加 [Serializable] 以支持序列化
    [Serializable]
    public class TesetCompo : Component
    {
        // 序列化字段（在Inspector中显示）
        [SerializeField] private float mass = 1.0f;
        [SerializeField] private Vector3 velocity;

        // 属性封装
        public float Mass => mass;
        public Vector3 Velocity => velocity;

        // 自定义物理更新逻辑
        public void ApplyForce(Vector3 force)
        {
            velocity += force / mass * Time.fixedDeltaTime;
        }
    }
}
