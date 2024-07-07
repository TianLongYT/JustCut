using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/FrameController")]
    [TaskDescription("����֡�����ű�")]
    /// <summary>
    /// 
    /// </summary>
    public class StopFrameController : Action
    {
        public override TaskStatus OnUpdate()
        {
            gameObject.GetComponent<EnemyFrameControllerDriver>().StopFrameController();
            return TaskStatus.Success;
        }


    }
}