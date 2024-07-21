using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using iysy.JustCut;
using UnityEngine;
namespace iysy.JustCut
{
    [TaskCategory("JustCut/FrameController")]
    [TaskDescription("�������FreeStyle����success�����򷵻�Failure,����û��freeType��Ȼ����failure")]
    /// <summary>
    /// 
    /// </summary>
    public class FrameIsOnFreeStyle : Conditional
    {
        EnemyFrameController frameController;
        public override void OnStart()
        {
            base.OnStart();
            frameController = gameObject.GetComponent<EnemyFrameControllerDriver>().FrameController;
        }
        public override TaskStatus OnUpdate()
        {
            //if (frameController.CurFrameState.Contains(FrameController.MotionState.FreeType))
            //{
            //    return TaskStatus.Success;
            //}
            return TaskStatus.Failure;
        }


    }
}