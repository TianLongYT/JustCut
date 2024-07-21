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
    public class PlayerRollLightAttack1State : PlayerLightAttackState
    {
        public PlayerRollLightAttack1State(PlayerStateMachine sM) : base(sM)
        {
        }
        protected override void ChangeAttackInteract()
        {
            stateMachine.Output.interactComponent.GenerateAttack(MotionType.RollLightAttack);
        }
        protected override void ChangeFrameMotion()
        {
            frameController.ChangeMotion(MotionType.RollLightAttack);
            //FrameDebuggerExtension.OnClearBar.InvokeAction(channelData);
        }
        protected override void PlayAttackAnim()
        {
            stateMachine.Output.animationController.TriggerRollLightAttack();
        }
        protected override void PerformAttackMove()
        {
            attackMoveHC.OnEnter((int)MotionType.RollLightAttack, 1);
        }
        protected override bool CancleTypeGetInput()//可以取消到重攻击，可以取消到超重攻击（父类实现）
        {
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
            float keyFrameTime = stateMachine.Output.animationController.JumpToKeyFrame(7,curAnimTime);
            float deltaTime = keyFrameTime - curAnimTime;
            return frameTime;
        }
        protected override void FreezeWorldTime(int frameFreezeAdder)
        {
            stateMachine.Output.timeFreezeController.FreezeTime(MotionType.RollLightAttack, frameFreezeAdder);
        }


    }
}