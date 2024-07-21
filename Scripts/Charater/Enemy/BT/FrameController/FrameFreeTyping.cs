using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/FrameController")]
    [TaskDescription("如果处于自由态则返回success，否则返回running,敌人没有自由态，必然返回failure")]
    /// <summary>
    /// 
    /// </summary>
    public class FrameFreeTyping : BaseFrameAction
    {
        public override TaskStatus OnUpdate()
        {
            //if (frameController.CurFrameState.Contains(EnemyFrameController.MotionState.FreeType))
            //    return TaskStatus.Success;
            return TaskStatus.Failure;
        }


    }
}