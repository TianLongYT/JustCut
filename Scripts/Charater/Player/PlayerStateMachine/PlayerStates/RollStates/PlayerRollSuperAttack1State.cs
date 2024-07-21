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
    public class PlayerRollSuperAttack1State : PlayerHeavyAttackState
    {
        public PlayerRollSuperAttack1State(PlayerStateMachine sM) : base(sM)
        {
        }
        private PlayerInteractComponent interactComponent;
        public override void OnEnter()
        {
            base.OnEnter();
            chargeFrameController.JumpToCertainFrame(stateMachine.LastStateFrameCount);
            stateMachine.LastStateFrameCount = 0;

            //挂载翻滚的关键帧回调。
            frameController.OnFrameTick += OnRollFrameTick;
            frameController.OnEnd += OnRollEnd;
        }
        public override void OnExit()
        {
            base.OnExit();
            if (Erupt == false)
            {
                frameController.OnFrameTick -= OnRollFrameTick;
                frameController.OnEnd -= OnRollEnd;
            }
        }
        private void OnRollEnd()
        {
            if (Erupt == false)
            {
                PlaySuperAttackAnim();
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
            interactComponent = stateMachine.Output.interactComponent;
            chargeFrameController.ChangeMotion(MotionType.SuperAttack1);

        }

        protected override void PlayAttackAnim()
        {
            //stateMachine.Output.animationController.TriggerSuperAttackEnter();

        }
        private void PlaySuperAttackAnim()//在翻滚结束后的第一时间调用。理论上来说翻滚30帧比所有动作的完全蓄力的长度都要短。
        {
            if (Erupt == false)
                stateMachine.Output.animationController.TriggerHeavyAttack2Enter();
        }
        protected override void ChangeAttackInteract(bool just, int chargeCount)
        {
            stateMachine.Output.interactComponent.GenerateAttack(MotionType.SuperAttack1, just, chargeCount);
        }
        protected override void ChangeFrameMotion()
        {
            frameController.OnFrameTick -= OnRollFrameTick;
            frameController.OnEnd -= OnRollEnd;
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
        {//前摇无法通过翻滚来转换到其他状态。
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
            float keyFrameTime = stateMachine.Output.animationController.JumpToKeyFrame(2,curAnimTime);
            float deltaTime = keyFrameTime - curAnimTime;
            return frameTime;
        }
        protected override void FreezeWorldTime(int frameFreezeAdder)
        {
            stateMachine.Output.timeFreezeController.FreezeTime(MotionType.SuperAttack1, frameFreezeAdder);
        }


    }
}