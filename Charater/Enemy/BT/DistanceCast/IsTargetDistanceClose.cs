using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/DistanceCast")]
    [TaskDescription("��Һ͵��˴��ڽ�����,�����ģΪ0")]
    /// <summary>
    /// 
    /// </summary>
    public class IsTargetDistanceClose : BaseCastDistance
    {
        public override TaskStatus OnUpdate()
        {
            if(castComponent.DistanceDegree == 0)
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }


    }
}