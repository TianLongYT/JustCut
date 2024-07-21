using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/AnimationController")]
    [TaskDescription("��ת��Ϸ����,���û������Ŀ���򲻻ᷭתת����")]
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