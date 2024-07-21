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
    /// ��ҳ��ع���״̬�����״̬��Ҫ֪����һ��״̬����֡����ʱ�䡣
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
            //�ĳ�������������Ի�������������Ķ���ת��Ч����
            //stateMachine.ChangeState(nameof(PlayerSuperAttack1State));

        }
        protected override bool CancleTypeGetInput()
        {
            //����ȡ������������
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