using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/MotionController")]
    [TaskDescription("ͨ��MotionController�е�FreeMoveHComponent����ƶ���Ϸ����,ע�⣬�ƶ�����Ĭ����������Ӱ��,����ҪPlayerLockController�ű���ֻ�᷵��Running���벢�����д������")]
    /// <summary>
    /// 
    /// </summary>
    public class Move : Action
    {
        FreeMoveHComponent moveHComponent;
        PlayerLockController lockController;
        [SerializeField] SharedFloat targetSpeed;
        public SharedBool ignoreFaceDir;
        public override void OnAwake()
        {
            base.OnAwake();
            moveHComponent = gameObject.GetComponent<MotionController2D.MotionController2D>().GetMComponent<FreeMoveHComponent>();
            lockController = gameObject.GetComponentInChildren<PlayerLockController>();
        }
        public override TaskStatus OnUpdate()
        {
            var targetSpeed = ignoreFaceDir.Value ? this.targetSpeed.Value : this.targetSpeed.Value * lockController.GetFaceDir();
            moveHComponent.OnUpdate(GameWorld.Instance.DeltaTime, targetSpeed);
            return TaskStatus.Running;
        }
    }
}