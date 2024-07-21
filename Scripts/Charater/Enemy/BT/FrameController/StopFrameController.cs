using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/FrameController")]
    [TaskDescription("开启帧记数脚本")]
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