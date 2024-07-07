using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/LockController")]
    [TaskDescription("����PlayerLockController,������λ�ù�ϵ�ı��ˣ�����success�����򷵻�failure")]
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
            bool relationChanged = false;
            if (lastRelationShip != curRelationShip)
            {
                relationChanged = true;
            }
            lastRelationShip = curRelationShip;

            return relationChanged ? TaskStatus.Success : TaskStatus.Failure;
        }

    }
}