using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/AnimationController")]
    [TaskDescription("�����ƶ�������0~2 StretchRunSlow CrouchRunFast CrouchRunSlow,��Ϊ���������ֵ��ֻ�᷵��Running���벢��������")]
    /// <summary>
    /// 
    /// </summary>
    public class PlayMove : BaseAnimationAction
    {
        public SharedInt moveAnimType;
        public SharedFloat moveRate;

        private bool Initialized;
        public override void OnStart()
        {
            base.OnStart();
            Initialized = false;
        }

        public override TaskStatus OnUpdate()
        {

            switch (moveAnimType.Value)
            {
                case 0:
                    if(Initialized == false)
                    {
                        animatorController.StretchRunSlow();
                        Initialized = true;
                    }
                    animatorController.SetStretchRunSlowRate(moveRate.Value);
                    break;
                case 1:
                    if (Initialized == false)
                    {
                        animatorController.CrouchRunFast();
                        //Debug.Log("�����˿��ٱ��ܶ���");
                        Initialized = true;
                    }
                    animatorController.CrouchRunFastRate(moveRate.Value);
                    break;
                case 2:
                    if (Initialized == false)
                    {
                        animatorController.CrouchRunSlow();
                        Initialized = true;
                    }
                    animatorController.CrouchRunSlowRate(moveRate.Value);
                    break;
                default:
                    Debug.LogError("�����˴���Ķ���Index");
                    break;
            }
            return TaskStatus.Running;
        }
    }
}