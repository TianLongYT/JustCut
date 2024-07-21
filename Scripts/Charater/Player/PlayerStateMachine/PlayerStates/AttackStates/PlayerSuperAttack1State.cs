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
    /// 玩家超重攻击状态，这个状态需要知道上一个状态经过帧数的时间。
    /// </summary>
    public class PlayerSuperAttack1State : PlayerHeavyAttackState
    {
        public PlayerSuperAttack1State(PlayerStateMachine sM) : base(sM)
        {
        }
        private PlayerInteractComponent interactComponent;
        public override void OnEnter()
        {
            base.OnEnter();
            chargeFrameController.JumpToCertainFrame(stateMachine.LastStateFrameCount);
            stateMachine.LastStateFrameCount = 0;
        }
        protected override void ChangeChargeFrameMotion()
        {
            interactComponent = stateMachine.Output.interactComponent;
            chargeFrameController.ChangeMotion(MotionType.SuperAttack1);

        }

        protected override void PlayAttackAnim()
        {
            stateMachine.Output.animationController.TriggerSuperAttackEnter();

        }
        protected override void ChangeAttackInteract(bool just, int chargeCount)
        {
            stateMachine.Output.interactComponent.GenerateAttack(MotionType.SuperAttack1, just, chargeCount);
        }
        protected override void ChangeFrameMotion()
        {
            stateMachine.Output.frameController.ChangeMotion(MotionType.SuperAttack1);
        }
        protected override void EruptMove()
        {
            attackMoveHC.OnEnter((int)MotionType.SuperAttack1, 1);
        }
        protected override void PlayErupAnim()
        {
            stateMachine.Output.animationController.TriggerSuperAttackRelease();
        }
        protected override bool ConditionToErupt()
        {
            return (inputBuffer.GetInput(PlayerInputType.HeavyAttackRelease)||inputBuffer.GetInput(PlayerInputType.LightAttackRelease));
        }
        protected override void ChargingRoll2RollState()
        {
            stateMachine.ChangeState(nameof(PlayerChargingRollSuperAttack1State));
            //改成下面这个，可以获得世界上最抽象的动作转换效果。
            //stateMachine.ChangeState(nameof(PlayerSuperAttack1State));

        }
        protected override bool CancleTypeGetInput()
        {
            //不能取消其他动作。
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
            float keyFrameTime = stateMachine.Output.animationController.JumpToKeyFrame(6,curAnimTime);
            float deltaTime = keyFrameTime - curAnimTime;
            return frameTime;
        }
        protected override void FreezeWorldTime(int frameFreezeAdder)
        {
            stateMachine.Output.timeFreezeController.FreezeTime(MotionType.SuperAttack1, frameFreezeAdder);
        }


    }
}