using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/MotionController")]
    [TaskDescription("通过MotionController中的FreeMoveHComponent组件移动游戏物体,注意，移动方向默认受自身朝向影响,还需要PlayerLockController脚本，只会返回Running，请并行运行此组件。")]
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
        public override void OnStart()
        {
            moveHComponent.OnEnter(0);
        }
        public override TaskStatus OnUpdate()
        {
            var targetSpeed = ignoreFaceDir.Value ? this.targetSpeed.Value : this.targetSpeed.Value * lockController.GetFaceDir();
            moveHComponent.OnUpdate(GameWorld.Instance.DeltaTime, targetSpeed);
            return TaskStatus.Running;
        }
        public override void OnEnd()
        {
            base.OnEnd();
            moveHComponent.OnExit();

        }
    }
}