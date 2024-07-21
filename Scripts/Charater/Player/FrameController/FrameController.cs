using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 关键帧回调
    /// </summary>
    public class FrameController
    {
        //帧数回调
        /// <summary>
        /// 帧状态，不同状态可以同时出现,注意转化成帧数表UI，
        /// </summary>
        [Flags]
        public enum MotionState
        {
            Zero = 0,
            StartUp = 1<<1,
            Active = 1<<2,
            Recover = 1<<3,
            GuardPoint = 1<<4,
            Cancle1 = 1<<5,
            Cancle2 = 1<<6,
            FreeType = 1<<7,
        }
        public struct FrameInfo
        {
            public int MotionStateEnum { get; private set; }
            public FrameInfo(params MotionState[] motionStates)
            {
                MotionStateEnum = 0;
                foreach (var motionState in motionStates)
                {
                    MotionStateEnum = FlagEnumUtility.FlagEnumUtility.And(MotionStateEnum, motionState);
                }
            }
            public void Reset()
            {
                MotionStateEnum = 0;
            }
            public void AddState(MotionState motionState)
            {
                MotionStateEnum = FlagEnumUtility.FlagEnumUtility.And(MotionStateEnum, motionState);
            }
            public void RemoveState(MotionState motionState)
            {
                MotionStateEnum = FlagEnumUtility.FlagEnumUtility.Remove(MotionStateEnum, motionState);
            }
            public bool Contains(MotionState motionState)
            {
                return FlagEnumUtility.FlagEnumUtility.Contains(MotionStateEnum, motionState);
            }
            public bool CheckEqual(FrameInfo frameInfo)
            {
                //Debug.Log("PreInt " + this.MotionStateEnum + "CompareInt " + frameInfo.MotionStateEnum);
                return this.MotionStateEnum == frameInfo.MotionStateEnum;
            }
            public bool CheckEqualInt(FrameInfo frameInfo)
            {
                return this.ToInt() == frameInfo.ToInt();
            }
            /// <summary>
            /// 提供一个专门输出显示的数值
            /// </summary>
            /// <returns></returns>
            public int ToInt()
            {
                if (Contains(MotionState.StartUp))
                    return 0;
                else if (Contains(MotionState.Active))
                    return 1;
                else
                {
                    return 2;
                }
            }
        }
        FrameInfo curFrameState;
        public FrameInfo CurFrameState { get { return curFrameState; } }
        public Action<FrameInfo> OnFrameTick;
        //动画回调
        public Action OnEnter, OnStartUp, OnActive, OnRecover, OnEnd, OnExit;
        //无敌帧回调
        public Action OnGPStart, OnGPOver;
        //取消时机回调
        public Action OnCancle1Start, OnCancle1Over;
        public Action OnCancle2Start, OnCancle2Over;
        //自由态回调
        public Action OnFreeTypeStart;
        //动作数据。
        MotionKeyFrameModel frameModel;
        MotionKeyFrameData curFrameData;
        int curMotionFrame;
        public int CurMotionFrame { get { return curMotionFrame; } }

        public FrameController(MotionKeyFrameModel keyFrameModel)
        {
            frameModel = keyFrameModel;
            GameWorld.Instance.OnFrameTick += OnFrameUpdate;
        }
        /// <summary>
        /// 克隆一个frameController,可以用作并行回调。
        /// </summary>
        /// <returns></returns>
        public FrameController Clone()
        {
            return new FrameController(this.frameModel);
        }
        public void StopFrameController()
        {
            curFrameData = null;
        }
        /// <summary>
        /// 跳跃到关键帧，关键帧一律视为有效帧+1
        /// </summary>
        public int JumpToKeyFrame()
        {
            int frameToJump = curFrameData.startUp + 1 - curMotionFrame;
            int res = frameToJump;
            while (frameToJump > 0)
            {
                frameToJump--;
                OnFrameUpdate(0);
                //JustForDebug
            }
            return res > 0 ? res : 0;
        }
        protected virtual void OnFrameUpdate(float timescale)
        {
            if (curFrameData == null)
                return;
            curMotionFrame++;
            //OnActive,OnRecover
            if(curMotionFrame == 0)
            {
                OnStartUp?.Invoke();
                curFrameState.AddState(MotionState.StartUp);
                //Debug.Log("执行了一次状态转换");
            }
            else if (curMotionFrame == curFrameData.startUp)
            {
                OnActive?.Invoke();
                curFrameState.RemoveState(MotionState.StartUp);
                curFrameState.AddState(MotionState.Active);
            }
            else if(curMotionFrame == curFrameData.startUp + curFrameData.active)
            {
                OnRecover?.Invoke();
                curFrameState.RemoveState(MotionState.Active);
                curFrameState.AddState(MotionState.Recover);
            }
            else if(curMotionFrame == curFrameData.startUp + curFrameData.active + curFrameData.recover)
            {
                OnEnd?.Invoke();
                curFrameState.RemoveState(MotionState.Recover);
                curFrameState.RemoveState(MotionState.FreeType);
                if (curFrameData == null)
                    return;
            }
            //GP
            if(curMotionFrame == curFrameData.gpStart)
            {
                OnGPStart?.Invoke();
                curFrameState.AddState(MotionState.GuardPoint);
            }
            else if(curMotionFrame == curFrameData.gpEnd)
            {
                OnGPOver?.Invoke();
                curFrameState.RemoveState(MotionState.GuardPoint);
            }
            //Cancle
            if(curMotionFrame == curFrameData.cancle1start)
            {
                OnCancle1Start?.Invoke();
                curFrameState.AddState(MotionState.Cancle1);
            }
            else if(curMotionFrame == curFrameData.cancle1end)
            {
                OnCancle1Over?.Invoke();
                curFrameState.RemoveState(MotionState.Cancle1);
            }
            if(curMotionFrame == curFrameData.cancle2start)
            {
                OnCancle2Start?.Invoke();
                curFrameState.AddState(MotionState.Cancle2);
            }
            else if(curMotionFrame == curFrameData.cancle2end)
            {
                OnCancle2Over?.Invoke();
                curFrameState.RemoveState(MotionState.Cancle2);
            }
            //FreeType
            if(curMotionFrame == curFrameData.freetypeStart)
            {
                OnFreeTypeStart?.Invoke();
                curFrameState.AddState(MotionState.FreeType);
                //Debug.Log("进入FreeType");
            }
            OnFrameTick?.Invoke(curFrameState);
        }
        /// <summary>
        /// 不清空帧计时器，继续按帧计时。
        /// </summary>
        /// <param name="newMotionType"></param>
        public void ChangeMotion(MotionType newMotionType)
        {
            OnExit?.Invoke();
            curFrameData = frameModel.GetMotionKeyFrameData(newMotionType);
            curFrameState.Reset();
            curMotionFrame = -1;
            OnEnter?.Invoke();
            //OnStartUp?.Invoke();
        }
        

    }
}