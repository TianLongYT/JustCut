using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/AnimationController")]
    [TaskDescription("基类没用别创建谢谢")]
    /// <summary>
    /// 主要是获取enemyAnimatorController组件
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