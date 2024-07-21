using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/DistanceCast")]
    [TaskDescription("玩家和敌人处于近距离,距离规模为0")]
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