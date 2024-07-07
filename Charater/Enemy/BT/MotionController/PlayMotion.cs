using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/MotionController")]
    [TaskDescription("���ɲ���λ������")]
    /// <summary>
    /// 
    /// </summary>
    public class PlayMotion : Action
    {
        [SerializeField]SharedMoveMotionType motionType;
        EnemyAttackMoveHComponent moveHComponent;
        public override void OnAwake()
        {
            base.OnAwake();
            moveHComponent = gameObject.GetComponent<MotionController2D.MotionController2D>().GetMComponent<EnemyAttackMoveHComponent>();
        }
        public override TaskStatus OnUpdate()
        {
            moveHComponent.OnEnter((int)motionType.Value);
            return TaskStatus.Success;
        }


    }
}