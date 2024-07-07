using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/FrameController")]
    [TaskDescription("�ı䶯��״̬�����¼�����ǰ֡�����ȡ�")]
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