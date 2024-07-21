using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/FrameController")]
    [TaskDescription("��֡�����ں�ҡʱ����running,���Ǻ�ҡ����Successs")]
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