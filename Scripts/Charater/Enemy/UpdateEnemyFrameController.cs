using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut
{
    [TaskCategory("JustCut/FrameController")]
    [TaskDescription("����֡��״̬������Ҫ��״̬������Ϣ")]
    /// <summary>
    /// ����֡��״̬������Ҫ��״̬������Ϣ
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