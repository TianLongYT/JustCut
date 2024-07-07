using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/AnimationController")]
    [TaskDescription("����intro����")]
    /// <summary>
    /// 
    /// </summary>
    public class PlayIntro : BaseAnimationAction
    {
        public override TaskStatus OnUpdate()
        {
            animatorController.Intro();


            return TaskStatus.Success;

        }


    }
}