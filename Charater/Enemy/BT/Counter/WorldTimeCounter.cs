using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut.BT
{
    [TaskCategory("JustCut/Counter")]
    [TaskDescription("计时器，受世界的时间规模影响,可以用加法计时，也可以用减法计时，减法计时最好设置初始时间；加法计时最好设置目标时间；时间到达时，可以继续返回Running，也可以直接返回success")]
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
                Debug.Log("调用Awake的初始化");
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