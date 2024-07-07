using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/AnimationController")]
    [TaskDescription("播放攻击动画，因为攻击动画太多了，懒得创建那么多类，0报错，1~10 轻攻击StartUp N P P2H，11~20 重攻击StartUp N P N2L，冲撞攻击 21，")]
    /// <summary>
    /// 
    /// </summary>
    public class PlayAttack : BaseAnimationAction
    {
        public SharedInt attackAnimIndex;
        public override TaskStatus OnUpdate()
        {
            switch (attackAnimIndex.Value)
            {
                case 1:
                    animatorController.PlayLightAttackStartUp();
                    break;
                case 2:
                    animatorController.PlayNormalLightAttack();
                    break;
                case 3:
                    animatorController.PlayPowerfulLightAttack();
                    break;
                case 4:
                    animatorController.PlayLightAttackP2H();
                    break;
                case 11:
                    animatorController.PlayHeavyAttackStartUp();
                    break;
                case 12:
                    animatorController.PlayNormalHeavyAttack();
                    break;
                case 13:
                    animatorController.PlayPowerfulHeavyAttack();
                    break;
                case 14:
                    animatorController.PlayHeavyAttackN2L();
                    break;
                case 21:
                    animatorController.PlayHeavyAttackImpact();
                    break;
                default:
                    Debug.Log("请输入正确的攻击动画序号");
                    break;

            }
            return TaskStatus.Success;
        }


    }
}