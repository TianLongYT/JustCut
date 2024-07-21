using iysy.GameInput;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 玩家第一段重攻击状态。
    /// </summary>
    public class PlayerHeavyAttack2State : PlayerHeavyAttackState
    {
        public PlayerHeavyAttack2State(PlayerStateMachine sM) : base(sM)
        {
        }

        protected override void ChangeChargeFrameMotion()
        {
            stateMachine.Output.chargeFrameController.ChangeMotion(MotionType.HeavyAttack2);
        }
        protected override void PlayAttackAnim()
        {
            stateMachine.Output.animationController.TriggerHeavyAttack2Enter();
        }
        protected override void ChangeAttackInteract(bool just, int chargeCount)
        {
            stateMachine.Output.interactComponent.GenerateAttack(MotionType.HeavyAttack2, just, chargeCount);
        }
        protected override void ChangeFrameMotion()
        {
            stateMachine.Output.frameController.ChangeMotion(MotionType.HeavyAttack2);
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
        {
            stateMachine.ChangeState(nameof(PlayerChargingRollHeavyAttack1State));
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
                stateMachine.ChangeState(nameof(PlayerSuperAttack1State));
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
            stateMachine.Output.vfxController.PlaySlashParticle(1);

        }
        protected override float JumpToKeyFrame()
        {
            float frameTime = base.JumpToKeyFrame();
            float curAnimTime = stateMachine.Output.frameController.CurMotionFrame * GameWorld.Instance.Frame2Second;
            float keyFrameTime = stateMachine.Output.animationController.JumpToKeyFrame(4,curAnimTime);
            float deltaTime = keyFrameTime - curAnimTime;
            return frameTime;
        }
        protected override void FreezeWorldTime(int frameFreezeAdder)
        {
            stateMachine.Output.timeFreezeController.FreezeTime(MotionType.HeavyAttack2, frameFreezeAdder);
        }

    }
}