using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/FrameController")]
    [TaskDescription("改变动作状态，重新计数当前帧数进度。")]
    /// <summary>
    /// 
    /// </summary>
    public class InitializeMotionState : BaseFrameAction
    {
        public SharedMoveMotionType targetMotionType;
        public override TaskStatus OnUpdate()
        {
            frameController.ChangeMotion(targetMotionType.Value);
            return TaskStatus.Success;
        }


    }
}