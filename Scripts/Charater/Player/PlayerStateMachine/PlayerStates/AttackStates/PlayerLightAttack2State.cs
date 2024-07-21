using iysy.GameInput;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 玩家第一段轻攻击状态。
    /// </summary>
    public class PlayerLightAttack2State : PlayerLightAttackState
    {
        public PlayerLightAttack2State(PlayerStateMachine sM) : base(sM)
        {
        }

        protected override void ChangeAttackInteract()
        {
            stateMachine.Output.interactComponent.GenerateAttack(MotionType.LightAttack2);
        }
        protected override void ChangeFrameMotion()
        {
            frameController.ChangeMotion(MotionType.LightAttack2);
        }
        protected override void PlayAttackAnim()
        {
            stateMachine.Output.animationController.TriggerLightAttack2();
        }
        protected override void PerformAttackMove()
        {
            attackMoveHC.OnEnter((int)MotionType.LightAttack2, 1);
        }
        protected override bool CancleTypeGetInput()
        {
            if (curFrameState.Contains(FrameController.MotionState.Cancle1) && inputBuffer.GetInput(PlayerInputType.LightAttack))
            {
                stateMachine.ChangeState(nameof(PlayerLightAttack3State));
                return true;
            }
            if (curFrameState.Contains(FrameController.MotionState.Cancle1) && inputBuffer.GetInput(PlayerInputType.HeavyAttack))
            {
                stateMachine.ChangeState(nameof(PlayerHeavyAttack1State));
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
            float keyFrameTime = stateMachine.Output.animationController.JumpToKeyFrame(1, curAnimTime);
            float deltaTime = keyFrameTime - curAnimTime;
            return frameTime;
        }
        protected override void FreezeWorldTime(int frameFreezeAdder)
        {
            stateMachine.Output.timeFreezeController.FreezeTime(MotionType.LightAttack2, frameFreezeAdder);
        }


    }
}