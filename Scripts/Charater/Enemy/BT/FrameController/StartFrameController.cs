using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/FrameController")]
    [TaskDescription("����֡�����ű����¶��ű�")]
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