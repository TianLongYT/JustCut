using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/AnimationController")]
    [TaskDescription("²¥·Åºó¹ö¶¯»­")]
    /// <summary>
    /// 
    /// </summary>
    public class PlayDodge : BaseAnimationAction
    {
        public override TaskStatus OnUpdate()
        {
            animatorController.Dodge();
            return base.OnUpdate();
        }


    }
}