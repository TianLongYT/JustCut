using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/FrameController")]
    [TaskDescription("�����������̬�򷵻�success�����򷵻�running,����û������̬����Ȼ����failure")]
    /// <summary>
    /// 
    /// </summary>
    public class FrameFreeTyping : BaseFrameAction
    {
        public override TaskStatus OnUpdate()
        {
            //if (frameController.CurFrameState.Contains(EnemyFrameController.MotionState.FreeType))
            //    return TaskStatus.Success;
            return TaskStatus.Failure;
        }


    }
}