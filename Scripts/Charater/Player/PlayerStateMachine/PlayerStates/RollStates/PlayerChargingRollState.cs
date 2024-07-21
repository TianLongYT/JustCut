using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{
    [Obsolete]
    /// <summary>
    /// [废弃代码]
    /// 玩家已经按住右键，开始充能时开始翻滚。这时要考虑翻滚是充能时间的延申。
    /// 分六种情况讨论，
    /// 从重攻击充能而来：
    ///     松开右键：
    ///         在充能前摇松开，等待前摇结束攻击。
    ///         在充能结束前松开。正常攻击。    
    ///         充能结束后松开，但是翻滚没有停止。翻滚结束攻击。
    ///         总结：充能结束的时间是加上翻滚的时间。其他和正常第二段重攻击一致。
    ///     按下左键：
    ///         转成超重攻击，此前的蓄力时间要存储起来，进入新超重攻击时使用跳帧。
    /// 从超重攻击充能而来：
    ///     松开左键或者右键：
    ///         和重攻击充能而来的逻辑相同。
    ///         
    /// 本质上和原版两种动作没区别。除开无敌和自动取消时间。
    /// </summary>
    public class PlayerChargingRollState : BasePlayerState
    {
        public PlayerChargingRollState(PlayerStateMachine sM) : base(sM)
        {
        }
    }
}