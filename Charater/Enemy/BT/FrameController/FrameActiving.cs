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
    public class FrameActiving : BaseFrameAction
    {
        public override TaskStatus OnUpdate()
        {
            if (frameController.CurFrameState.Contains(EnemyFrameController.MotionState.Active))
            {
                return TaskStatus.Running;
            }
            //Debug.Log("EnemyRecover");
            return TaskStatus.Success;
        }


    }
}