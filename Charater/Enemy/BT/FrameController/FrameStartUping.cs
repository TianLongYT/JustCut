using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/FrameController")]
    [TaskDescription("�������ǰҡ�� ���� ��ǰҡ�ؼ�֡ǰִ�У��򷵻�running,ǰҡ��������success")]
    /// <summary>
    /// 
    /// </summary>
    public class FrameStartUping : BaseFrameAction
    {
        public override TaskStatus OnUpdate()
        {
            if (frameController.CurFrameState.Contains(EnemyFrameController.MotionState.StartUp)|| frameController.CurMotionFrame == -1)
            {
                //Debug.Log("���ڹ���ǰҡ��֡��" + GameWorld.Instance.FrameCount);
                return TaskStatus.Running;
            }
            return TaskStatus.Success;
        }


    }
}