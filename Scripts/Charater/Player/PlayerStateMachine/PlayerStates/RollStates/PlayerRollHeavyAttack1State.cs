using iysy.GameInput;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 从普通的Roll状态转换到这个状态，和一般的重攻击二段状态，只有在前期有蓄力上的不同。蓄力在翻滚有效帧内有无敌。
    /// 一旦松手，无敌就被替换成GP。
    /// </summary>
    public class PlayerRollHeavyAttack1State : PlayerHeavyAttackState
    {
        public PlayerRollHeavyAttack1State(PlayerStateMachine sM) : base(sM)
        {
        }
        public override void OnEnter()
        {
            base.OnEnter();//获取了frameController组件，chargeController组件，挂载chargeController关键帧回调，设置动画，获取移动组件，初始化位移。
            //挂载翻滚的关键帧回调。
            frameController.OnFrameTick += OnRollFrameTick;
            frameController.OnEnd += OnRollEnd;
        }
        public override void OnExit()
        {
            base.OnExit();
            if(Erupt == false)
            {
                frameController.OnFrameTick -= OnRollFrameTick;
                frameController.OnEnd -= OnRollEnd;
            }
        }
        private void OnRollEnd()
        {
            if(Erupt == false)
            {
                PlayHeavyAttackAnim();
            }
        }

        //完成状态跟踪。重写受击方法。改为无敌。
        private void OnRollFrameTick(FrameController.FrameInfo obj)
        {
            if (curFrameState.CheckEqual(obj) == false)
                curFrameState = obj;
        }

        protected override void ChangeChargeFrameMotion()
        {
            stateMachine.Output.chargeFrameController.ChangeMotion(MotionType.HeavyAttack2);
        }
        protected override void PlayAttackAnim()
        {
            //stateMachine.Output.animationController.TriggerHeavyAttack2Enter();
        }
        private void PlayHeavyAttackAnim()//在翻滚结束后的第一时间调用。理论上来说翻滚30帧比所有动作的完全蓄力的长度都要短。
        {
            if(Erupt == false)
                stateMachine.Output.animationController.TriggerHeavyAttack2Enter();
        }
        protected override void ChangeAttackInteract(bool just, int chargeCount)
        {
            stateMachine.Output.interactComponent.GenerateAttack(MotionType.HeavyAttack2, just, chargeCount);
        }
        protected override void ChangeFrameMotion()
        {
            frameController.OnFrameTick -= OnRollFrameTick;
            frameController.OnEnd -= OnRollEnd;
            frameController.ChangeMotion(MotionType.HeavyAttack2);
        }

        protected override void EruptMove()
        {
            attackMoveHC.OnEnter((int)MotionType.HeavyAttack2, 1);
        }
        protected override void PlayErupAnim()
        {
            stateMachine.Output.animationController.TriggerHeavyAttack2Release();
        }
        protected override bool ConditionToErupt()
        {
            return inputBuffer.GetInput(PlayerInputType.HeavyAttackRelease);
        }
        protected override void ChargingRoll2RollState()
        {//前摇无法通过翻滚来转换到其他状态。
        }
        protected override bool CancleTypeGetInput()
        {
            //超重攻击
            if ((curChargeFrameState.Contains(ChargeFrameController.MotionState.ChargeStartUp) || curChargeFrameState.Contains(ChargeFrameController.MotionState.Charge))
                && inputBuffer.GetInput(PlayerInputType.LightAttack))
            {
                var frameCount = stateMachine.LastStateFrameCount;//上一个状态的前摇和打击帧以及部分后摇的帧数。
                frameCount += chargeFrameController.CurMotionFrame;//这个状态的蓄力前摇帧数。
                Debug.Log("jumpToCertainFrame" + frameCount + "last" + stateMachine.LastStateFrameCount + "curCharge" + chargeFrameController.CurMotionFrame);
                stateMachine.LastStateFrameCount = frameCount;//传入，给超重攻击使用，消除一部分前摇。
                stateMachine.ChangeState(nameof(PlayerRollSuperAttack1State));
                return true;
            }
            if (curFrameState.Contains(FrameController.MotionState.Cancle1) && inputBuffer.GetInput(PlayerInputType.HeavyAttack))
            {
                stateMachine.LastStateFrameCount = frameController.CurMotionFrame;
                stateMachine.ChangeState(nameof(PlayerHeavyAttack3State));
                return true;
            }
            return false;
        }


        protected override void OnActive()
        {
            base.OnActive();
            //播放特效
            stateMachine.Output.vfxController.PlaySlashParticle(0);

        }
        protected override float JumpToKeyFrame()
        {
            float frameTime = base.JumpToKeyFrame();
            float curAnimTime = stateMachine.Output.frameController.CurMotionFrame * GameWorld.Instance.Frame2Second;
            float keyFrameTime = stateMachine.Output.animationController.JumpToKeyFrame(0,curAnimTime);
            float deltaTime = keyFrameTime - curAnimTime;
            return frameTime;
        }
        protected override void FreezeWorldTime(int frameFreezeAdder)
        {
            stateMachine.Output.timeFreezeController.FreezeTime(MotionType.HeavyAttack2, frameFreezeAdder);
        }
    }
}