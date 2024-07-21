using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/FrameController")]
    [TaskDescription("开启帧记数脚本，孤儿脚本")]
    /// <summary>
    /// 
    /// </summary>
    public class StartFrameController : Action
    {
        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }


    }
}