using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/AnimationController")]
    [TaskDescription("����Intro����,�ڷ�����Һ���ã���Ȼ�볡��")]
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