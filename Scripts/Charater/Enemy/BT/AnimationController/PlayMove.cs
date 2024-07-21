using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/AnimationController")]
    [TaskDescription("播放移动动画，0~2 StretchRunSlow CrouchRunFast CrouchRunSlow,因为会持续设置值，只会返回Running，请并行运行他")]
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
                        //Debug.Log("启动了快速奔跑动画");
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
                    Debug.LogError("启用了错误的动画Index");
                    break;
            }
            return TaskStatus.Running;
        }
    }
}