using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using iysy.JustCut.Evt;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/Event")]
    [TaskDescription("�����¼����ġ�")]
    /// <summary>
    /// 
    /// </summary>
    public class SendBossAngeryEventToEventManagerByParam : Action
    {
        public override TaskStatus OnUpdate()
        {
            EventCenter.EventCenter.Trigger(new EvtParamOnBossAngery { bossPosition = this.gameObject.transform.position });
            return TaskStatus.Success;
        }


    }
}