using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/InteractComponent")]
    [TaskDescription("�������ȡ�����˴����,enable = false ȡ�������enable = true && motionType ���������")]
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
                //Debug.LogError("û�ҵ�EnemyInteractComponent");
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