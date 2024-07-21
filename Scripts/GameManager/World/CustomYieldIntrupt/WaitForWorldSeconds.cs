using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// �ȴ�һ��ʱ�䣬ʱ���ܵ���ǰ�����ģ��Ӱ�졣
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