using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 不同按键类型，（可以有不同的缓冲时间）
    /// </summary>
    public enum PlayerInputType
    {
        Jump,
        LightAttack,
        LightAttackRelease,
        HeavyAttack,
        HeavyAttackRelease,
        Roll,

    }
}