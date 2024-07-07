using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/InteractComponent")]
    [TaskDescription("激活或者取消敌人打击框,enable = false 取消打击框，enable = true && motionType 交换打击框")]
    /// <summary>
    /// 
    /// </summary>
    public class EnableMotionInteract : Action
    {
        public SharedBool enable;
        public SharedMoveMotionType motionType;
        //public SharedMotionType motionType;
        EnemyInteractComponent interactC;
        public override void OnAwake()
        {
            base.OnAwake();
            interactC = gameObject.GetComponentInChildren<EnemyInteractComponent>();
        }
        public override TaskStatus OnUpdate()
        {
            if (interactC == null)
            {
                //Debug.LogError("没找到EnemyInteractComponent");
                return TaskStatus.Failure;
            }
            if(enable.Value == false)
            {
                interactC.EnableCurMotionHitBox(false);
            }
            else
            {
                //Debug.Log(motionType.Value);
                interactC.ChangeMotionHitBox(motionType.Value);
            }
            return TaskStatus.Success;


        }


    }
}