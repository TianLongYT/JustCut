using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/AnimationController")]
    [TaskDescription("���Ź�����������Ϊ��������̫���ˣ����ô�����ô���࣬0����1~10 �ṥ��StartUp N P P2H��11~20 �ع���StartUp N P N2L����ײ���� 21��")]
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
                    Debug.Log("��������ȷ�Ĺ����������");
                    break;

            }
            return TaskStatus.Success;
        }


    }
}