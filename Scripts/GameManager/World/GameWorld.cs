using System;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// �Զ���TimeScale
    /// </summary>
    public class GameWorld:Singleton<GameWorld>
    {
        public readonly int FramePerSecond = 60;
        private double frame2Second;
        private double frameTimer;
        public float Frame2Second
        {
            get
            {
                if (frame2Second == 0)
                    frame2Second = 1.0 / FramePerSecond;
                return (float)frame2Second;
            }
        }
        public int FrameCount { get; private set; }
        public float DeltaTime { get; private set; }
        public float UnscaledDeltaTime { get; private set; }
        public float TimeScale { get; private set; }
        public float TimeScaleWithoutFreeze { get; private set; }
        public float TimeScaleWithoutSkip { get; private set; }//玩家切动作时，有一个时间跳跃。（抽帧）这个值不考虑抽帧。
        public float TimeScaleWithoutWitchTime { get; private set; }

        public TimeQueue timeQueue;
        private GameWorldEvtTrigger evtTrigger;
        /// <summary>
        /// 当Unity引擎更新时调用，返回一个受Time.timeScale和GameWorld.Timescale影响的帧间间隔。
        /// </summary>
        public event Action<float> OnUpdate;
        /// <summary>
        /// 顿帧时停止运行，但是跳帧时持续运行。
        /// </summary>
        public event Action<float> OnFrameTick;
        public event Action<float> OnLateFrameTick;
        const float DefaultTimeScale = 1.0f;
        private float timeScaleMult = 1f;

        public override void InitInstance()
        {
            base.InitInstance();
            TimeScale = DefaultTimeScale;
            //timeScaleMult = DefaultTimeScale;
            TimeScaleWithoutFreeze = 1;
            TimeScaleWithoutSkip = 1;
            TimeScaleWithoutWitchTime = 1;
            frame2Second = 1.0 / FramePerSecond;
            FrameCount = 0;
            timeQueue = new TimeQueue();
            evtTrigger = new GameWorldEvtTrigger(this);
        }
        public override void Dispose()
        {
            base.Dispose();
            evtTrigger.Dispose();
            OnFrameTick = null;
            OnLateFrameTick = null;
        }
        public void PauseGame()
        {
            Debug.Log("暂停游戏");
            timeScaleMult = 0f;
        }
        public void ContinueGame()
        {
            Debug.Log("继续游戏");
            timeScaleMult = 1f;
        }
        public void Update(float deltaTime)
        {
            timeQueue.UpdateTimeQueue(Time.unscaledDeltaTime);
            //设置好TimeScale的值。
            TimeScale = DefaultTimeScale * timeQueue.FreezeTimeScale * timeQueue.SkipTimeScale * timeQueue.WitchTimeTimeScale * timeScaleMult;
            TimeScaleWithoutSkip = DefaultTimeScale * timeQueue.WitchTimeTimeScale * timeQueue.FreezeTimeScale * timeScaleMult;

           // Debug.Log("世界更新,timescale" + TimeScale+"freezeTime"+timeQueue.FreezeTimeScale + "defaultTimes"+DefaultTimeScale + "timemult"+timeScaleMult);


            DeltaTime = deltaTime;
            frameTimer += deltaTime * TimeScaleWithoutSkip;
            while(frameTimer >= frame2Second)
            {
                frameTimer -= frame2Second;
                Debug.Log("帧");
                OnFrameTick?.Invoke((float)frame2Second);
                OnLateFrameTick?.Invoke((float)frame2Second);
                FrameCount++;
            }
            //Debug.Log("deltaTime");
            OnUpdate?.Invoke(DeltaTime * TimeScale);
        }
        public void RealTimeUpdate(float unscaledDeltaTime)
        {

            UnscaledDeltaTime = unscaledDeltaTime;

            if(unityFrezzeTimer > 0)
            {
                unityFrezzeTimer -= unscaledDeltaTime;
                Time.timeScale = 0;
                if(unityFrezzeTimer <= 0)
                {
                    Time.timeScale = 1;
                }
            }
        }
        public void FreezeTime(float time,TimeQueue.AddMode addMode)
        {
            timeQueue.AddFreezeData(new TimeQueue.TimeFreezeData(time), addMode);
        }
        public void FreezeTime(int frame,TimeQueue.AddMode addMode)
        {
            FreezeTime((float)(frame * frame2Second),addMode);
            //Debug.Log("����ʱ�䶳��,֡"+frame);
        }
        public void TimeSlowDown(AnimationCurve slowDownCurve)//��Ҫ����һ�����������ߡ�
        {

        }
        public void SetTimeScaleMult(float timeScaleMult)
        {
            this.timeScaleMult = timeScaleMult;
        }
        private float unityFrezzeTimer;
        public void FreezeUnityTime(int frame)
        {
            unityFrezzeTimer = (float)(frame * frame2Second);
        }
    }
}