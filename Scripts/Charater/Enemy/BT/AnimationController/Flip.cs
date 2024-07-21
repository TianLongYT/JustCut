using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/AnimationController")]
    [TaskDescription("翻转游戏物体,如果没有锁定目标则不会翻转转物体")]
    /// <summary>
    /// 
    /// </summary>
    public class Flip : BaseAnimationAction
    {
        PlayerLockController lockController;
        public override void OnAwake()
        {
            base.OnAwake();
            lockController = gameObject.GetComponentInChildren<PlayerLockController>();
        }
        public override TaskStatus OnUpdate()
        {
            var posRelationship = lockController.GetPosRelationship();
            if (posRelationship != 0)
            {
                animatorController.Flip(posRelationship);
            }
            return TaskStatus.Success;
        }


    }
}