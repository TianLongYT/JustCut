using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/DistanceCast")]
    [TaskDescription("玩家和敌人处于中距离,距离规模为1")]
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