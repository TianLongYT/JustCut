using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut
{
    [TaskCategory("JustCut/FrameController")]
    [TaskDescription("如果处于Cancle1返回success，否则返回Failure,敌人目前没有cancle1状态，必然返回failure")]
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