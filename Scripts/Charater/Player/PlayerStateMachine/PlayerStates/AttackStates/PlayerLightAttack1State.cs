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
    public class PlayerLightAttack1State : PlayerLightAttackState
    {
        public PlayerLightAttack1State(PlayerStateMachine sM) : base(sM)
        {
        }
        protected override void ChangeAttackInteract()
        {
            stateMachine.Output.interactComponent.GenerateAttack(MotionType.LightAttack1);
        }
        protected override void ChangeFrameMotion()
        {
            frameController.ChangeMotion(MotionType.LightAttack1);
            //FrameDebuggerExtension.OnClearBar.InvokeAction(channelData);
        }
        protected override void PlayAttackAnim()
        {
            stateMachine.Output.animationController.TriggerLightAttack1();
        }
        protected override void PerformAttackMove()
        {
            attackMoveHC.OnEnter((int)MotionType.LightAttack1, 1);
        }
        protected override bool CancleTypeGetInput()
        {
            if (curFrameState.Contains(FrameController.MotionState.Cancle1) && inputBuffer.GetInput(PlayerInputType.LightAttack))
            {
                stateMachine.ChangeState(nameof(PlayerLightAttack2State));
                return true;
            };
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
            stateMachine.Output.timeFreezeController.FreezeTime(MotionType.LightAttack1, frameFreezeAdder);
        }


    }
}