using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/FrameController")]
    [TaskDescription("如果正在前摇中 或者 在前摇关键帧前执行，则返回running,前摇结束返回success")]
    /// <summary>
    /// 
    /// </summary>
    public class FrameStartUping : BaseFrameAction
    {
        public override TaskStatus OnUpdate()
        {
            if (frameController.CurFrameState.Contains(EnemyFrameController.MotionState.StartUp)|| frameController.CurMotionFrame == -1)
            {
                //Debug.Log("处于攻击前摇，帧数" + GameWorld.Instance.FrameCount);
                return TaskStatus.Running;
            }
            return TaskStatus.Success;
        }


    }
}