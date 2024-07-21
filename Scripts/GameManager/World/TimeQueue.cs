using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 
    /// </summary>
    public class TimeQueue
    {
        public class TimeData
        {
            public float time;
            public TimeChangeType type;
            public virtual float GetValue()
            {
                return 0;
            }
            public virtual float GetPrimaryValue()
            {
                return 1;
            }
            public TimeData(float time)
            {
                this.time = time;
            }
        }
        public class TimeFreezeData:TimeData
        {
            public TimeFreezeData(float time):base(time)
            {
                type = TimeChangeType.Freeze;
            }
        }
        public class TimeSkipData : TimeData
        {
            public TimeSkipData(float time) : base(time)
            {
                type = TimeChangeType.Skip;
            }
        }
        public class WitchTimeData:TimeData
        {
            public AnimationCurve curve;

            public WitchTimeData(float time) : base(time)
            {
                type = TimeChangeType.WitchTime;
            }

            public override float GetValue()
            {
                return curve.Evaluate(time);
            }
        }
        public float FreezeTimeScale { get; private set; }
        public float SkipTimeScale { get; private set; }
        public float WitchTimeTimeScale { get; private set; }
        Queue<TimeData> timeDataQueue;
        TimeFreezeData timeFreezeData = new TimeFreezeData(0);
        TimeSkipData timeSkipData = new TimeSkipData(0);
        public TimeQueue()
        {
            FreezeTimeScale = 1;
            SkipTimeScale = 1;
            WitchTimeTimeScale = 1;
            timeDataQueue = new Queue<TimeData>();
        }
        public enum AddMode
        {
            addQueue,
            mergeQueue,
            overrideQueue,
        }
        public void AddFreezeData(TimeFreezeData freezeData,AddMode addMode)
        {
            switch (addMode)
            {
                case AddMode.addQueue:
                    timeDataQueue.Enqueue(freezeData);
                    break;
                default:
                    Debug.Log("其他的入队选项还没做");
                    break;
            }
        }
        public void AddSkipData(TimeSkipData skipData,AddMode addMode)
        {
            switch (addMode)
            {
                case AddMode.addQueue:
                    timeDataQueue.Enqueue(skipData);
                    break;
                default:
                    Debug.Log("其他的入队选项还没做");
                    break;
            }
        }
        public void UpdateTimeQueue(float deltaTime)
        {
            if(timeDataQueue.Count == 0)
            {
                return;
            }
            var top = timeDataQueue.Peek();
            top.time -= deltaTime;
            if (top.time <= 0)
            {
                switch (top.type)
                {
                    case TimeChangeType.Freeze:
                        FreezeTimeScale = top.GetPrimaryValue();
                        //Debug.Log("冻结帧还原");
                        break;
                    case TimeChangeType.Skip:
                        SkipTimeScale = top.GetPrimaryValue();
                        //Debug.Log("跳帧还原");

                        break;
                    case TimeChangeType.WitchTime:
                        WitchTimeTimeScale = top.GetPrimaryValue();
                        break;
                }
                timeDataQueue.Dequeue();
                if (timeDataQueue.Count == 0)
                    return;
                top = timeDataQueue.Peek();
            }
            switch (top.type)
            {
                case TimeChangeType.Freeze:
                    FreezeTimeScale = top.GetValue();
                    //Debug.Log("冻结帧");

                    break;
                case TimeChangeType.Skip:
                    SkipTimeScale = top.GetValue();
                    //Debug.Log("跳帧");

                    break;
                case TimeChangeType.WitchTime:
                    WitchTimeTimeScale = top.GetValue();
                    break;
            }

        }
    }
}