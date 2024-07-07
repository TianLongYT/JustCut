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

        protected override void ChangeAttackInteract()
        {
            stateMachine.Output.interactComponent.GenerateAttack(MotionType.HeavyAttack2);
        }
        protected override void ChangeFrameMotion()
        {
            stateMachine.Output.frameController.ChangeMotion(MotionType.HeavyAttack2);
        }
        protected override void PlayAttackAnim()
        {
            
        }
        protected override bool CancleTypeGetInput()
        {
            if (curFrameState.Contains(FrameController.MotionState.Cancle1) && inputBuffer.GetInput(PlayerInputType.HeavyAttack))
            {
                stateMachine.ChangeState(nameof(PlayerHeavyAttack3State));
                return true;
            }
            //if (curFrameState.Contains(FrameController.MotionState.StartUp) && Input.GetKeyUp(KeyCode.Mouse1))
            //{
            //    stateMachine.Output.animationController.TriggerHACToHAA();
            //}
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
            stateMachine.Output.timeFreezeController.FreezeTime(MotionType.HeavyAttack1, frameFreezeAdder);
        }

    }
}