using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// �ؼ�֡�ص�,ֻ���������˵Ĳ��֣����ֻ����ǰҡ�����кͺ�ҡ��
    /// </summary>
    public class EnemyFrameController
    {
        //֡���ص�
        /// <summary>
        /// ֡״̬����ͬ״̬����ͬʱ����,ע��ת����֡����UI��
        /// </summary>
        [Flags]
        public enum MotionState
        {
            Zero = 0,
            StartUp = 1 << 0,
            Active = 1 << 1,
            Recover = 1 << 2,
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
            /// �ṩһ��ר�������ʾ����ֵ
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
        //�����ص�
        public Action OnEnter, OnStartUp, OnActive, OnRecover, OnEnd, OnExit;
        //�޵�֡�ص�
        //public Action OnGPStart, OnGPOver;
        //ȡ��ʱ���ص�
        //public Action OnCancle1Start, OnCancle1Over;
        //public Action OnCancle2Start, OnCancle2Over;
        //����̬�ص�
        //public Action OnFreeTypeStart;
        //�������ݡ�
        EnemyMotionKeyFrameModel frameModel;
        EnemyMotionKeyFrameData curFrameData;
        int curMotionFrame;
        public int CurMotionFrame { get { return curMotionFrame; } }

        public EnemyFrameController(EnemyMotionKeyFrameModel keyFrameModel)
        {
            frameModel = keyFrameModel;
            GameWorld.Instance.OnFrameTick += OnFrameUpdate;
        }
        public void StopFrameController()
        {
            curFrameData = null;
        }
        /// <summary>
        /// ��Ծ���ؼ�֡���ؼ�֡һ����Ϊ��Ч֡+1
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
            if (curMotionFrame == 0)
            {
                OnStartUp?.Invoke();
                curFrameState.AddState(MotionState.StartUp);
                //Debug.Log("ִ����һ��״̬ת��");
            }
            else if (curMotionFrame == curFrameData.startUp)
            {
                OnActive?.Invoke();
                curFrameState.RemoveState(MotionState.StartUp);
                curFrameState.AddState(MotionState.Active);
            }
            else if (curMotionFrame == curFrameData.startUp + curFrameData.active)
            {
                OnRecover?.Invoke();
                curFrameState.RemoveState(MotionState.Active);
                curFrameState.AddState(MotionState.Recover);
            }
            else if (curMotionFrame == curFrameData.startUp + curFrameData.active + curFrameData.recover)
            {
                //Debug.Log("���֡��"+GameWorld.Instance.FrameCount);
                curFrameState.RemoveState(MotionState.Recover);
                OnEnd?.Invoke();
            }
            ////GP
            //if (curMotionFrame == curFrameData.gpStart)
            //{
            //    OnGPStart?.Invoke();
            //    curFrameState.AddState(MotionState.GuardPoint);
            //}
            //else if (curMotionFrame == curFrameData.gpEnd)
            //{
            //    OnGPOver?.Invoke();
            //    curFrameState.RemoveState(MotionState.GuardPoint);
            //}
            ////Cancle
            //if (curMotionFrame == curFrameData.cancle1start)
            //{
            //    OnCancle1Start?.Invoke();
            //    curFrameState.AddState(MotionState.Cancle1);
            //}
            //else if (curMotionFrame == curFrameData.cancle1end)
            //{
            //    OnCancle1Over?.Invoke();
            //    curFrameState.RemoveState(MotionState.Cancle1);
            //}
            //if (curMotionFrame == curFrameData.cancle2start)
            //{
            //    OnCancle2Start?.Invoke();
            //    curFrameState.AddState(MotionState.Cancle2);
            //}
            //else if (curMotionFrame == curFrameData.cancle2end)
            //{
            //    OnCancle2Over?.Invoke();
            //    curFrameState.RemoveState(MotionState.Cancle2);
            //}
            ////FreeType
            //if (curMotionFrame == curFrameData.freetypeStart)
            //{
            //    OnFreeTypeStart?.Invoke();
            //    curFrameState.AddState(MotionState.FreeType);
            //    //Debug.Log("����FreeType");
            //}
            OnFrameTick?.Invoke(curFrameState);
        }
        /// <summary>
        /// �����֡��ʱ����������֡��ʱ��
        /// </summary>
        /// <param name="newMotionType"></param>
        public void ChangeMotion(MoveMotionType newMotionType)
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