using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/AnimationController")]
    [TaskDescription("����û�ñ𴴽�лл")]
    /// <summary>
    /// ��Ҫ�ǻ�ȡenemyAnimatorController���
    /// </summary>
    public class BaseAnimationAction : Action
    {
        protected EnemyAnimatorController animatorController;
        public override void OnAwake()
        {
            base.OnAwake();
            animatorController = gameObject.GetComponent<EnemyAnimatorController>();
        }
        

    }
}