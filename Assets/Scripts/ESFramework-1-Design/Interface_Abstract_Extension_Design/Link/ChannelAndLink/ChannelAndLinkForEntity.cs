using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ES
{
    public enum Channel_EntityAttackLink : int
    {
        /// <summary>
        /// 尝试攻击他人
        /// </summary>
        [InspectorName("尝试攻击他人")]TryAttack,
        /// <summary>
        /// 尝试被攻击
        /// </summary>
        [InspectorName("尝试被攻击")] TryBeAttack,
        /// <summary>
        /// 真实攻击他人
        /// </summary>
        [InspectorName("真实攻击他人")] TrulyAttack,
        /// <summary>
        /// 真实被他人攻击
        /// </summary>
        [InspectorName("尝试攻击他人")] TrulyBeAttack
    }

    public struct Link_EntityAttack : ILink
    {

    }
}
