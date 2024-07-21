using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/Counter")]
    [TaskDescription("�Ƚ�ֵ�Ĵ�С,��������������򷵻�success�����򷵻�failure")]
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