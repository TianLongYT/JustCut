using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut
{
    [TaskCategory("JustCut/FrameController")]
    [TaskDescription("更新帧数状态，给需要的状态机发消息")]
    /// <summary>
    /// 更新帧数状态，给需要的状态机发消息
    /// </summary>
    public class UpdateEnemyFrameController : Action
    {
        EnemyFrameControllerDriver frameController;
        public override void OnStart()
        {
            base.OnStart();

        }
        public override TaskStatus OnUpdate()
        {
            return base.OnUpdate();
        }

    }
}