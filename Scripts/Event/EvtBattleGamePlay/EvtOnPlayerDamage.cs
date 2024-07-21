using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家受伤事件
/// </summary>
public struct EvtOnPlayerDamage : IEventWithRatioArg
{
    public float Ratio { get; set; }
}
