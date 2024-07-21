using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{
    [System.Serializable]
    /// <summary>
    /// 动作关键帧打印
    /// </summary>
    public class EnemyMotionKeyFrameData
    {
        public MoveMotionType motionType;

        public int startUp;//前摇的帧数，打印的绿格子数。
        public int active;//打击帧的帧数，打印的红格子数。
        public int recover;//蓝格子数。

        //public int gpStart, gpEnd;
        //public int cancle1start, cancle1end;
        //public int cancle2start, cancle2end;
        //public int freetypeStart;

        //public int gpFreezeTime;//放在这里也是因为顿帧也会涉及动作的时序性。

        public bool Match(MoveMotionType type) => motionType == type;
    }
}