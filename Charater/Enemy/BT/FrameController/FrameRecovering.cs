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
    public class FrameRecovering : BaseFrameAction
    {
        public override TaskStatus OnUpdate()
        {
            if (frameController.CurFrameState.Contains(EnemyFrameController.MotionState.Recover))
            {
                //Debug.Log("��ҡ��������"+GameWorld.Instance.FrameCount);
                return TaskStatus.Running;
            }
            //Debug.Log("��⵽��ҡ����"+ GameWorld.Instance.FrameCount);
            return TaskStatus.Success;
        }



    }
}