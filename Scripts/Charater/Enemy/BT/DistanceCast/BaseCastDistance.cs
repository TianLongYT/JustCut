using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/DistanceCast")]
    [TaskDescription("检测距离的基类，负责获取EnemyCastComponent")]
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseCastDistance : Conditional
    {
        protected EnemyCastComponent castComponent;
        public override void OnAwake()
        {
            base.OnAwake();
            castComponent = gameObject.GetComponentInChildren<EnemyCastComponent>();

        }


    }
}