using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MoonSharp.Interpreter.Debugging.DebuggerAction;

namespace iysy.JustCut
{

    /// <summary>
    /// ∞Ô÷˙stateMachineÕÍ≥…÷°∂≥Ω·
    /// </summary>
    public class PlayerTimeFreezeController
    {
        MotionKeyFrameModel frameModel;
        public PlayerTimeFreezeController(MotionKeyFrameModel frameModel)
        {
            this.frameModel = frameModel;
        }
        public void FreezeTime(MotionType motionType,int frameAdder)
        {
            Debug.Log("Motion" + motionType.ToString() + "gpFreezeTime" + frameModel.GetMotionKeyFrameData(motionType).gpFreezeTime);
            GameWorld.Instance.timeQueue.AddSkipData(new TimeQueue.TimeSkipData(frameAdder * GameWorld.Instance.Frame2Second), TimeQueue.AddMode.addQueue);
            GameWorld.Instance.timeQueue.AddFreezeData(new TimeQueue.TimeFreezeData(GameWorld.Instance.Frame2Second * frameModel.GetMotionKeyFrameData(motionType).gpFreezeTime ),TimeQueue.AddMode.addQueue);  
        }
        public void FreezeTime(int frame)
        {
            GameWorld.Instance.timeQueue.AddFreezeData(new TimeQueue.TimeFreezeData( GameWorld.Instance.Frame2Second * frame), TimeQueue.AddMode.addQueue);
        }

    }
}