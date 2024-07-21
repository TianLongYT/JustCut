using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/LockController")]
    [TaskDescription("返回Running，设置是否需要翻转。")]
    /// <summary>
    /// 
    /// </summary>
    public class CalNeedFlip : Action
    {
        [SerializeField] SharedBool needFlip;
        PlayerLockController lockController;
        public override void OnAwake()
        {
            base.OnAwake();
            lockController = gameObject.GetComponentInChildren<PlayerLockController>();
        }
        public override TaskStatus OnUpdate()
        {
            needFlip.Value = lockController.GetPosRelationship() != lockController.GetFaceDir();
            return TaskStatus.Running;
        }


    }
}