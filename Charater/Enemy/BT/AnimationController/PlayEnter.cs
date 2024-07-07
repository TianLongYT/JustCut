using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/AnimationController")]
    [TaskDescription("播放Intro动画,在发现玩家后调用，自然入场！")]
    /// <summary>
    /// 
    /// </summary>
    public class PlayEnter : BaseAnimationAction
    {
        public override TaskStatus OnUpdate()
        {
            animatorController.Enter();
            return base.OnUpdate();
        }


    }
}