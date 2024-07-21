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
    public class PlayerChargingRollSuperAttack1State : PlayerHeavyAttackState
    {
        public PlayerChargingRollSuperAttack1State(PlayerStateMachine sM) : base(sM)
        {
        }
        private PlayerInteractComponent interactComponent;
        public override void OnEnter()
        {
            base.OnEnter();
            chargeFrameController.JumpToCertainFrame(stateMachine.LastStateFrameCount);
            stateMachine.LastStateFrameCount = 0;

            //�����Ǳ���Ҫ������ġ�
            rollEnd = false;
            chargeEnd = false;
            //���ط����Ĺؼ�֡�ص���
            frameController.ChangeMotion(MotionType.Roll);
            frameController.OnFrameTick += OnRollFrameTick;
            frameController.OnEnd += OnRollEnd;
            //����λ��
            attackMoveHC.OnEnter((int)MotionType.Roll, 1);
            initFaceDir = lockController.GetFaceDir(); 
            if (lockController.targetTF != null)
                stateMachine.Output.crossEnemyController.IgnoreCollision(lockController.targetTF.GetComponent<Collider2D>(), true);

        }
        public override void OnExit()
        {
            base.OnExit();
            if(Erupt == false)
            {
                if (lockController.targetTF != null)
                    stateMachine.Output.crossEnemyController.IgnoreCollision(lockController.targetTF.GetComponent<Collider2D>(), false);
                frameController.OnFrameTick -= OnRollFrameTick;
                frameController.OnEnd -= OnRollEnd;
            }
        }
        float initFaceDir;
        protected override void UpdateMotionController(float deltaTime)
        {
            //������ڷ���״̬������ı�ԭ���ķ������򡣵��ͷ�ʱ���ı乥������
            if (Erupt == false)
                attackMoveHC.OnUpdate(deltaTime, 0, initFaceDir);
            else
            {
                base.UpdateMotionController(deltaTime);
            }
        }
        bool rollEnd;
        bool chargeEnd;
        //���״̬���١���д�ܻ���������Ϊ�޵С�
        private void OnRollFrameTick(FrameController.FrameInfo obj)
        {
            if (curFrameState.CheckEqual(obj) == false)
            {
                //FrameDebugMgr.Instance.ChangeState(obj.ToInt(), 0);
                curFrameState = obj;
            }
        }
        //�ӳ��Զ��ͷŹ�����ʱ�䡣
        private void OnRollEnd()
        {
            rollEnd = true;
            if (Erupt == false)
            {
                if (chargeEnd)
                {
                    ChargeErupt();
                }
                else//���ܼ������ܡ������������������ų��ܶ�����
                {
                    stateMachine.Output.animationController.TriggerSuperAttackEnter();
                }
            }
        }
        protected override void OnChargeEnd()
        {
            chargeEnd = true;
            if (rollEnd)
            {
                ChargeErupt();
            }

        }
        protected override void ChargeErupt()
        {
            if (lockController.targetTF != null)
                stateMachine.Output.crossEnemyController.IgnoreCollision(lockController.targetTF.GetComponent<Collider2D>(), false);

            frameController.OnFrameTick -= OnRollFrameTick;
            frameController.OnEnd -= OnRollEnd;
            base.ChargeErupt();
        }
        protected override void ChangeChargeFrameMotion()
        {
            interactComponent = stateMachine.Output.interactComponent;
            //chargeFrameController.ChangeMotion(MotionType.SuperAttack1);

        }

        protected override void PlayAttackAnim()
        {
            stateMachine.Output.animationController.TriggerRoll();

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
            stateMachine.Output.animationController.TriggerRollSuperAttackRelease();
        }
        protected override bool ConditionToErupt()
        {
            return (inputBuffer.GetInput(PlayerInputType.HeavyAttackRelease)||inputBuffer.GetInput(PlayerInputType.LightAttackRelease));
        }
        protected override void ChargingRoll2RollState()
        {
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
            stateMachine.Output.timeFreezeController.FreezeTime(MotionType.HeavyAttack1, frameFreezeAdder);
        }


    }
}