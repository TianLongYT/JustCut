using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 没有寒气buff时，重击第一段会有特殊动作（或特殊特效）第一段会有GP判定。
    /// 重攻击第一段GP时进入此状态，状态进入时，获得寒气BUFF。
    /// 按轻攻击可以取消“强化轻攻击”，消除寒气buff。
    /// 寒气buff在2时间规模后自动消除，消除后可再使用特殊重攻击格挡
    /// 格挡成功后再按右键可以
    /// 可以延续重攻击蓄力时间，
    /// 
    /// 
    /// </summary>
    public class PlayerBlockState : BasePlayerState
    {
        public PlayerBlockState(PlayerStateMachine sM) : base(sM)
        {
        }

    }
}