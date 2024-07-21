using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using MzUtility.FrameWork.EventSystem;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/Evnet")]
    [TaskDescription("依赖事件中心。注意！别用，没用")]
    /// <summary>
    /// 
    /// </summary>
    public class SendEventToEventManager : Action
    {
        [SerializeField]SharedString eventName;
        public override TaskStatus OnUpdate()
        {
            EventManager.Trigger(eventName.Value);
            return TaskStatus.Success;

        }


    }
}