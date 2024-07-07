using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using iysy.JustCut;
using UnityEngine;
namespace iysy.JustCut
{
    [TaskCategory("JustCut/FrameController")]
    [TaskDescription("如果处于FreeStyle返回success，否则返回Failure,敌人没有freeType必然返回failure")]
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