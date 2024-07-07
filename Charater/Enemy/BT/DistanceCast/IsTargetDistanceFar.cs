using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/DistanceCast")]
    [TaskDescription("玩家和敌人的距离远,距离规模为-1")]
    /// <summary>
    /// 
    /// </summary>
    public class IsTargetDistanceFar : BaseCastDistance
    {
        public override TaskStatus OnUpdate()
        {
            if (castComponent.DistanceDegree == -1)
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}