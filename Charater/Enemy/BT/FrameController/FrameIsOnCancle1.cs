using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut
{
    [TaskCategory("JustCut/FrameController")]
    [TaskDescription("�������Cancle1����success�����򷵻�Failure,����Ŀǰû��cancle1״̬����Ȼ����failure")]
    /// <summary>
    /// 
    /// </summary>
    public class FrameIsOnCancle1 : Conditional
    {
        EnemyFrameController frameController;
        public override void OnStart()
        {
            base.OnStart();
            frameController = gameObject.GetComponent<EnemyFrameControllerDriver>().FrameController;
        }
        public override TaskStatus OnUpdate()
        {
            //if (frameController.CurFrameState.Contains(FrameController.MotionState.Cancle1))
            //{
            //    return TaskStatus.Success;
            //}
            return TaskStatus.Failure;
        }




    }
}