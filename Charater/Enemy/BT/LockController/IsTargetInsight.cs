using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/LockController")]
    [TaskDescription("依赖:PlayerLockController判断目标是否进入视线内")]
    /// <summary>
    /// 
    /// </summary>
    public class IsTargetInsight : Conditional
    {
        PlayerLockController lockController;
        public override void OnStart()
        {
            base.OnStart();
            lockController = gameObject.GetComponentInChildren<PlayerLockController>();
        }
        public override TaskStatus OnUpdate()
        {
            if (lockController.IsLock())
                return TaskStatus.Success;
            else
                return TaskStatus.Failure;
        }

    }
}