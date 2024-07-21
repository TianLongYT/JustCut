using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 等待一段时间，时间受到当前世界规模的影响。
    /// </summary>
    public class WaitForWorldSeconds : CustomYieldInstruction
    {
        float timer;
        public override bool keepWaiting
        {
            get
            {
                timer -= GameWorld.Instance.TimeScale * Time.unscaledDeltaTime;
                return timer > 0;
            }
        }
        public WaitForWorldSeconds(float seconds)
        {
            timer = seconds;
        }
    }
}