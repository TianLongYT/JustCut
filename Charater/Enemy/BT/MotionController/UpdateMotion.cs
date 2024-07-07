using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/MotionController")]
    [TaskDescription("�������¶���λ�ƽű�.�벢�����д˽ű�������success")]
    /// <summary>
    /// 
    /// </summary>
    public class UpdateMotion : Action
    {
        EnemyAttackMoveHComponent moveHC;
        PlayerLockController lockController;
        public override void OnAwake()
        {
            base.OnAwake();
            moveHC = gameObject.GetComponent<MotionController2D.MotionController2D>().GetMComponent<EnemyAttackMoveHComponent>();
            lockController = gameObject.GetComponentInChildren<PlayerLockController>();
        }
        public override TaskStatus OnUpdate()
        {
            moveHC.OnUpdate(GameWorld.Instance.DeltaTime,lockController.GetFaceDir());
            return TaskStatus.Running;
        }


    }
}