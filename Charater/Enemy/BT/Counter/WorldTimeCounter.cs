using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/Counter")]
    [TaskDescription("��ʱ�����������ʱ���ģӰ��,�����üӷ���ʱ��Ҳ�����ü�����ʱ��������ʱ������ó�ʼʱ�䣻�ӷ���ʱ�������Ŀ��ʱ�䣻ʱ�䵽��ʱ�����Լ�������Running��Ҳ����ֱ�ӷ���success")]
    /// <summary>
    /// 
    /// </summary>
    public class WorldTimeCounter : Action
    {
        [SerializeField] SharedFloat timerValue;

        [SerializeField] SharedBool setInitTimeAwake;
        [SerializeField] SharedBool setInitTimeStart;
        [SerializeField] SharedFloat initTime;

        [SerializeField] SharedBool addTime;
        [SerializeField] SharedFloat targetTime;

        [SerializeField] SharedBool timerOverContinueRunning;

        float frameDeltaTime = 0;
        public override void OnAwake()
        {
            base.OnAwake();
            if (setInitTimeAwake.Value)
            {
                timerValue.Value = initTime.Value;
                Debug.Log("����Awake�ĳ�ʼ��");
            }
        }
        public override void OnStart()
        {
            base.OnStart();
            if (setInitTimeStart.Value)
            {
                timerValue.Value = initTime.Value;
            }
            frameDeltaTime = GameWorld.Instance.Frame2Second;
        }
        public override TaskStatus OnUpdate()
        {
            if (addTime.Value)
            {
                timerValue.Value += frameDeltaTime;
                if(!timerOverContinueRunning.Value && timerValue.Value >= targetTime.Value)
                {
                    return TaskStatus.Success;
                }
            }
            else
            {
                timerValue.Value -= frameDeltaTime;
                if(!timerOverContinueRunning.Value && timerValue.Value <= 0)
                {
                    return TaskStatus.Success;
                }
            }
            return TaskStatus.Running;
        }


    }
}