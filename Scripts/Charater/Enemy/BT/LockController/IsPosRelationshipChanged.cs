using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/LockController")]
    [TaskDescription("依赖PlayerLockController,如果相关位置关系改变了，返回success，否则返回failure")]
    /// <summary>
    /// 
    /// </summary>
    public class IsPosRelationshipChanged : Conditional
    {
        PlayerLockController lockController;
        public override void OnAwake()
        {
            base.OnAwake();
            lockController = gameObject.GetComponentInChildren<PlayerLockController>();
            lastRelationShip = -1;
        }
        float lastRelationShip;
        public override TaskStatus OnUpdate()
        {
            var curRelationShip = lockController.GetPosRelationship();
            if(curRelationShip != lockController.GetFaceDir())
            {
                return TaskStatus.Success;
            }
            else
            {
                return TaskStatus.Failure;
            }
        }

    }
}