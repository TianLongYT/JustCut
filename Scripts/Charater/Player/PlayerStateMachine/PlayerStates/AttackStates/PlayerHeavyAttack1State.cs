using BehaviorDesigner.Runtime.Tasks.Unity.UnityInput;
using iysy.GameInput;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
namespace iysy.JustCut
{

    /// <summary>
    /// 玩家第一段重攻击状态。
    /// </summary>
    public class PlayerHeavyAttack1State : PlayerHeavyAttackState
    {
        public PlayerHeavyAttack1State(PlayerStateMachine sM) : base(sM)
        {
        }
        private PlayerInteractComponent interactComponent;
        protected override void ChangeChargeFrameMotion()
        {
            interactComponent = stateMachine.Output.interactComponent;
            interactComponent.GenerateAttack(MotionType.Gp);
            chargeFrameController.OnGPStart += OnChargeGpStart;
            chargeFrameController.OnGPOver += OnChargeGPOver;
            chargeFrameController.ChangeMotion(MotionType.HeavyAttack1);

        }


        private void OnChargeGpStart()
        {
            interactComponent.SetGPEnable(true);
            //Debug.Log("开启重攻击1GP");
            //播放特效
            //stateMachine.Output.vfxController.PlaySlashParticle(0);
        }
        private void OnChargeGPOver()
        {
            interactComponent.SetGPEnable(false);
            //Debug.Log("关闭重攻击1GP");
        }
        public override void OnExit()
        {
            base.OnExit();
            chargeFrameController.OnGPStart -= OnChargeGpStart;
            chargeFrameController.OnGPOver -= OnChargeGPOver;
            interactComponent.SetGPEnable(false);
            //Debug.Log("关闭重攻击1GP");

        }

        protected override void PlayAttackAnim()
        {
            stateMachine.Output.animationController.TriggerHeavyAttack1Enter();

        }
        protected override void ChangeAttackInteract(bool just, int chargeCount)
        {
            stateMachine.Output.interactComponent.GenerateAttack(MotionType.HeavyAttack1, just, chargeCount);
        }
        protected override void ChangeFrameMotion()
        {
            stateMachine.Output.frameController.ChangeMotion(MotionType.HeavyAttack1);
        }
        protected override void EruptMove()
        {
            attackMoveHC.OnEnter((int)MotionType.HeavyAttack1, 1);
        }
        protected override void PlayErupAnim()
        {
            stateMachine.Output.animationController.TriggerHeavyAttack1Release();
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
                stateMachine.LastStateFrameCount = 0;
                var frameCount = 0;//上一个状态的前摇和打击帧以及部分后摇的帧数。
                frameCount += chargeFrameController.CurMotionFrame;//这个状态的蓄力前摇帧数。
                stateMachine.LastStateFrameCount = frameCount;//传入，给超重攻击使用，消除一部分前摇。
                stateMachine.ChangeState(nameof(PlayerSuperAttack1State));
                return true;
            }
            if (curFrameState.Contains(FrameController.MotionState.Cancle1) && inputBuffer.GetInput(PlayerInputType.HeavyAttack))//后面把帧数表弄明白了再调,HA1后摇接HA2功能未实现
            {
                stateMachine.LastStateFrameCount = frameController.CurMotionFrame;
                stateMachine.ChangeState(nameof(PlayerHeavyAttack2State));
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
            float keyFrameTime = stateMachine.Output.animationController.JumpToKeyFrame(3,curAnimTime);
            float deltaTime = keyFrameTime - curAnimTime;
            return frameTime;
        }
        protected override void ChargingGPJumpTokeyFrame()
        {
            base.ChargingGPJumpTokeyFrame();
            float curAnimTime = stateMachine.Output.chargeFrameController.CurMotionFrame * GameWorld.Instance.Frame2Second;
            float keyFrameTime = stateMachine.Output.animationController.JumpToKeyFrame(2, curAnimTime);
        }
        protected override void FreezeWorldTime(int frameFreezeAdder)
        {
            stateMachine.Output.timeFreezeController.FreezeTime(MotionType.HeavyAttack1, frameFreezeAdder);
        }


    }
}