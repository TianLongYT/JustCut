using iysy.GameInput;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// ����ڳ���ʱ��ʹ�÷���״̬��
    /// </summary>
    public class PlayerChargingRollHeavyAttack1State : PlayerHeavyAttackState
    {
        public PlayerChargingRollHeavyAttack1State(PlayerStateMachine sM) : base(sM)
        {
        }
        public override void OnEnter()
        {
            base.OnEnter();//��ȡ��frameController�����chargeController���������chargeController�ؼ�֡�ص������ö�������ȡ�ƶ��������ʼ��λ�ơ�
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
            if(lockController.targetTF != null)
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
                curFrameState = obj;
                //FrameDebugMgr.Instance.ChangeState(obj.ToInt(), 0);
            }
        }
        //�ӳ��Զ��ͷŹ�����ʱ�䡣
        private void OnRollEnd()
        {
            rollEnd = true;
            if(Erupt == false)
            {
                if (chargeEnd)
                {
                    ChargeErupt();
                }
                else//���ܼ������ܡ������������������ų��ܶ�����
                {
                    stateMachine.Output.animationController.TriggerHeavyAttack2Enter();
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
            //stateMachine.Output.chargeFrameController.ChangeMotion(MotionType.HeavyAttack2);
        }
        protected override void PlayAttackAnim()
        {
            stateMachine.Output.animationController.TriggerRoll();
        }
        protected override void ChangeAttackInteract(bool just, int chargeCount)
        {
            stateMachine.Output.interactComponent.GenerateAttack(MotionType.HeavyAttack2, just, chargeCount);
        }
        protected override void ChangeFrameMotion()
        {
            stateMachine.Output.frameController.ChangeMotion(MotionType.HeavyAttack2);
        }

        protected override void EruptMove()
        {
            attackMoveHC.OnEnter((int)MotionType.HeavyAttack2, 1);
        }
        protected override void PlayErupAnim()
        {
            stateMachine.Output.animationController.TriggerRollHeavyAttackRelease();
        }
        protected override bool ConditionToErupt()
        {
            return inputBuffer.GetInput(PlayerInputType.HeavyAttackRelease);
        }
        protected override void ChargingRoll2RollState()
        {//ǰҡ�޷�ͨ��������ת��������״̬��
        }
        protected override bool CancleTypeGetInput()
        {
            //���ع���
            if ((curChargeFrameState.Contains(ChargeFrameController.MotionState.ChargeStartUp) || curChargeFrameState.Contains(ChargeFrameController.MotionState.Charge))
                && inputBuffer.GetInput(PlayerInputType.LightAttack))
            {
                var frameCount = stateMachine.LastStateFrameCount;//��һ��״̬��ǰҡ�ʹ��֡�Լ����ֺ�ҡ��֡����
                frameCount += chargeFrameController.CurMotionFrame;//���״̬������ǰҡ֡����
                Debug.Log("jumpToCertainFrame" + frameCount + "last" + stateMachine.LastStateFrameCount + "curCharge" + chargeFrameController.CurMotionFrame);
                stateMachine.LastStateFrameCount = frameCount;//���룬�����ع���ʹ�ã�����һ����ǰҡ��
                stateMachine.ChangeState(nameof(PlayerSuperAttack1State));
                return true;
            }
            if (curFrameState.Contains(FrameController.MotionState.Cancle1) && inputBuffer.GetInput(PlayerInputType.HeavyAttack))
            {
                stateMachine.LastStateFrameCount = frameController.CurMotionFrame;
                stateMachine.ChangeState(nameof(PlayerHeavyAttack3State));
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