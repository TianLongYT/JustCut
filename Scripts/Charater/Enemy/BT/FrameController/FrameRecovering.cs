using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/FrameController")]
    [TaskDescription("当帧数处于后摇时返回running,不是后摇返回Successs")]
    /// <summary>
    /// 
    /// </summary>
    public class FrameRecovering : BaseFrameAction
    {
        public override TaskStatus OnUpdate()
        {
            if (frameController.CurFrameState.Contains(EnemyFrameController.MotionState.Recover))
            {
                //Debug.Log("后摇还在运行"+GameWorld.Instance.FrameCount);
                return TaskStatus.Running;
            }
            //Debug.Log("检测到后摇结束"+ GameWorld.Instance.FrameCount);
            return TaskStatus.Success;
        }



    }
}