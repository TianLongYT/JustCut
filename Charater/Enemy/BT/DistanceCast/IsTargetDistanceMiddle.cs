using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/DistanceCast")]
    [TaskDescription("��Һ͵��˴����о���,�����ģΪ1")]
    /// <summary>
    /// 
    /// </summary>
    public class IsTargetDistanceMiddle : BaseCastDistance
    {
        public override TaskStatus OnUpdate()
        {
            if (castComponent.DistanceDegree == 1)
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }


    }
}