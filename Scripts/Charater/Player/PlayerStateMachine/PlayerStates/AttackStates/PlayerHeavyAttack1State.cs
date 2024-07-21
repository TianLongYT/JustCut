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
    /// ��ҵ�һ���ع���״̬��
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
            //Debug.Log("�����ع���1GP");
            //������Ч
            //stateMachine.Output.vfxController.PlaySlashParticle(0);
        }
        private void OnChargeGPOver()
        {
            interactComponent.SetGPEnable(false);
            //Debug.Log("�ر��ع���1GP");
        }
        public override void OnExit()
        {
            base.OnExit();
            chargeFrameController.OnGPStart -= OnChargeGpStart;
            chargeFrameController.OnGPOver -= OnChargeGPOver;
            interactComponent.SetGPEnable(false);
            //Debug.Log("�ر��ع���1GP");

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
            //���ع���
            if ((curChargeFrameState.Contains(ChargeFrameController.MotionState.ChargeStartUp) || curChargeFrameState.Contains(ChargeFrameController.MotionState.Charge))
                && inputBuffer.GetInput(PlayerInputType.LightAttack))
            {
                stateMachine.LastStateFrameCount = 0;
                var frameCount = 0;//��һ��״̬��ǰҡ�ʹ��֡�Լ����ֺ�ҡ��֡����
                frameCount += chargeFrameController.CurMotionFrame;//���״̬������ǰҡ֡����
                stateMachine.LastStateFrameCount = frameCount;//���룬�����ع���ʹ�ã�����һ����ǰҡ��
                stateMachine.ChangeState(nameof(PlayerSuperAttack1State));
                return true;
            }
            if (curFrameState.Contains(FrameController.MotionState.Cancle1) && inputBuffer.GetInput(PlayerInputType.HeavyAttack))//�����֡����Ū�������ٵ�,HA1��ҡ��HA2����δʵ��
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
            //������Ч
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