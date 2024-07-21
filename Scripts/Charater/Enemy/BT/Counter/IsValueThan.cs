using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/Counter")]
    [TaskDescription("比较值的大小,如果符合条件，则返回success，否则返回failure")]
    /// <summary>
    /// 
    /// </summary>
    public class IsValueThan : Conditional
    {
        [SerializeField] SharedBool greater;
        [SerializeField] SharedBool containZero;
        [SerializeField] SharedFloat originFloat;
        [SerializeField] SharedFloat targetFloat;

        public override TaskStatus OnUpdate()
        {
            if (greater.Value)
            {
                if (containZero.Value && originFloat.Value >= targetFloat.Value)
                {
                    return TaskStatus.Success;
                }
                else if(containZero.Value == false && originFloat.Value > targetFloat.Value)
                {
                    return TaskStatus.Success;
                }
            }
            else
            {
                if(containZero.Value && originFloat.Value <= targetFloat.Value)
                {
                    return TaskStatus.Success;
                }
                else if(containZero.Value == false && originFloat.Value < targetFloat.Value)
                {
                    return TaskStatus.Success;
                }
            }
            return TaskStatus.Failure;
        }

    }
}