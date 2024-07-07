using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/AnimationController")]
    [TaskDescription("²¥·ÅIdle¶¯»­")]
    /// <summary>
    /// 
    /// </summary>
    public class PlayIdle : BaseAnimationAction
    {
        public override TaskStatus OnUpdate()
        {
            animatorController.Idle();
            return base.OnUpdate();
        }


    }
}