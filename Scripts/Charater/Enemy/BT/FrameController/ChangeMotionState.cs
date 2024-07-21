using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/FrameController")]
    [TaskDescription("改变动作状态，但是不会改变当前帧数计数进度。")]
    /// <summary>
    /// 
    /// </summary>
    public class ChangeMotionState : BaseFrameAction
    {
        public SharedMoveMotionType targetMotionType; 
        public override TaskStatus OnUpdate()
        {
            //Debug.Log("动作状态改变，当前帧数" + GameWorld.Instance.FrameCount);
            frameController.ChangeMotion(targetMotionType.Value);
            return TaskStatus.Success;
        }


    }
}