using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/FrameController")]
    [TaskDescription("�ı䶯��״̬�����ǲ���ı䵱ǰ֡���������ȡ�")]
    /// <summary>
    /// 
    /// </summary>
    public class ChangeMotionState : BaseFrameAction
    {
        public SharedMoveMotionType targetMotionType; 
        public override TaskStatus OnUpdate()
        {
            //Debug.Log("����״̬�ı䣬��ǰ֡��" + GameWorld.Instance.FrameCount);
            frameController.ChangeMotion(targetMotionType.Value);
            return TaskStatus.Success;
        }


    }
}