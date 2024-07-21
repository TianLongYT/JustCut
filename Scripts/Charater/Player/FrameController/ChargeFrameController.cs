using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 充能时的关键帧回调
    /// </summary>
    public class ChargeFrameController
    {
        //帧数回调
        /// <summary>
        /// 帧状态，不同状态可以同时出现,注意转化成帧数表UI，
        /// </summary>
        [Flags]
        public enum MotionState
        {
            Zero = 0,
            ChargeStartUp = 1<<0,
            Charge = 1<<1,
            GuardPoint = 1<<2,
            Just1 = 1<<3,
            Just2 = 1<<4,
            Just3 = 1<<5,
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
                if (Contains(MotionState.ChargeStartUp))
                    return 3;
                else
                    return 4;
            }
        }
        FrameInfo curFrameState;
        public FrameInfo CurFrameState { get { return curFrameState; } }
        public Action<FrameInfo> OnFrameTick;
        //动画回调
        public Action OnEnter, OnChargeStart, OnCharge, OnEnd, OnExit;
        //无敌帧回调
        public Action OnGPStart, OnGPOver;
        //Just时机回调
        public Action OnJust1Start, OnJust1Over;
        public Action OnJust2Start, OnJust2Over;
        public Action OnJust3Start, OnJust3Over;
        //动作数据。
        MotionKeyFrameModel frameModel;
        ChargeMotionKeyFrameData curFrameData;
        int curMotionFrame;
        public int CurMotionFrame { get { return curMotionFrame; } }

        public ChargeFrameController(MotionKeyFrameModel keyFrameModel)
        {
            frameModel = keyFrameModel;
            GameWorld.Instance.OnFrameTick += OnFrameUpdate;
        }
        public void StopFrameController()
        {
            curFrameData = null;
        }
        ///// <summary>
        ///// 跳跃到关键帧，关键帧一律视为有效帧+1
        ///// </summary>
        //public int JumpToKeyFrame()
        //{
        //    int frameToJump = curFrameData.startUp + 1 - curMotionFrame;
        //    int res = frameToJump;
        //    while (frameToJump > 0)
        //    {
        //        frameToJump--;
        //        OnFrameUpdate(0);
        //        //JustForDebug
        //    }
        //    return res > 0 ? res : 0;
        //}
        /// <summary>
        /// 跳转到指定帧数。触发帧数前的所有回调
        /// </summary>
        /// <param name="certainFrame"></param>
        /// <returns>已经跳过了的帧数certainFrame - curMotionFrame</returns>
        public int JumpToCertainFrame(int certainFrame)
        {
            int frameToJump = certainFrame - curMotionFrame;
            int res = frameToJump;
            while(frameToJump > 0)
            {
                frameToJump--;
                OnFrameUpdate(0);
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
                OnChargeStart?.Invoke();
                curFrameState.AddState(MotionState.ChargeStartUp);
                //Debug.Log("执行了一次状态转换");
            }
            else if (curMotionFrame == curFrameData.chargeFront)
            {
                OnCharge?.Invoke();
                curFrameState.RemoveState(MotionState.ChargeStartUp);
                curFrameState.AddState(MotionState.Charge);
            }
            else if(curMotionFrame == curFrameData.chargeFront + curFrameData.chargeMaxFrame)
            {
                OnEnd?.Invoke();
                curFrameState.RemoveState(MotionState.Charge);
            }
            if (curFrameData == null)
                return;
            //GP
            if (curFrameData.needGP)
            {
                if (curMotionFrame == curFrameData.gpStart)
                {
                    OnGPStart?.Invoke();
                    curFrameState.AddState(MotionState.GuardPoint);
                }
                else if (curMotionFrame == curFrameData.gpEnd)
                {
                    OnGPOver?.Invoke();
                    curFrameState.RemoveState(MotionState.GuardPoint);
                }
            }

            //Cancle
            if(curMotionFrame == curFrameData.just1Start)
            {
                OnJust1Start?.Invoke();
                curFrameState.AddState(MotionState.Just1);
            }
            else if(curMotionFrame == curFrameData.just1End)
            {
                OnJust1Over?.Invoke();
                curFrameState.RemoveState(MotionState.Just1);
            }
            if (curFrameData.needJust2)
            {
                if (curMotionFrame == curFrameData.just2Start)
                {
                    OnJust2Start?.Invoke();
                    curFrameState.AddState(MotionState.Just2);
                }
                else if (curMotionFrame == curFrameData.just2End)
                {
                    OnJust2Over?.Invoke();
                    curFrameState.RemoveState(MotionState.Just2);
                }
            }
            if (curFrameData.needJust3)
            {
                if (curMotionFrame == curFrameData.just3Start)
                {
                    OnJust3Start?.Invoke();
                    curFrameState.AddState(MotionState.Just3);
                }
                else if (curMotionFrame == curFrameData.just3End)
                {
                    OnJust3Over?.Invoke();
                    curFrameState.RemoveState(MotionState.Just3);
                }
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
            curFrameData = frameModel.GetChargeMotionKeyFrameData(newMotionType);
            curFrameState.Reset();
            curMotionFrame = -1;
            OnEnter?.Invoke();
            //OnStartUp?.Invoke();
        }
        

    }
}