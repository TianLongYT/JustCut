using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 敌人受伤事件
/// </summary>
public struct EvtOnEnemyDamage : IEventWithRatioArg
{
    public float Ratio { get; set; }
}
