using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// ��idle��״̬��������ҡ��״̬ת����������Ұ����ɿ���һ��ʼû�г���Ч����
    /// �������������������������������ṥ�����������󣬷����ṥ����
    /// �����У������Ҽ���ʼ�������ɿ��󷢶��ڶ����ع����������󣬰����Ҽ���ʼ������һ���ع�����
    /// �����У������Ҽ���ʼ�����������������ʼת���ع�����ͬ���ع������㷽ʽ��
    /// ����һ���������Ұ�ס����󷭹��������а����Ҽ�����ʱ��ʼ���ع���������ͬ���ع������㷽ʽ��
    /// </summary>
    public class PlayerNormalRollState : BasePlayerState
    {
        public PlayerNormalRollState(PlayerStateMachine sM) : base(sM)
        {
        }
        private FrameController.FrameInfo curFrameState;
        private FrameController frameController;

        private PlayerAttackMoveHComponent attackMoveHC;
        public override void OnEnter()
        {
            base.OnEnter();
            curFrameState.Reset();
            //�ؼ�֡�ص�
            frameController = stateMachine.Output.frameController;
            frameController.ChangeMotion(MotionType.Roll);
            frameController.OnFrameTick += OnFrameTick;
            frameController.OnEnd += OnRollEnd;
            //������λ��
            stateMachine.Output.animationController.TriggerRoll();

            attackMoveHC = stateMachine.Output.motionController.GetMComponent<PlayerAttackMoveHComponent>();
            attackMoveHC.OnEnter((int)MotionType.Roll, 1);
            faceDir = lockController.GetFaceDir();
            if(lockController.targetTF != null)
                stateMachine.Output.crossEnemyController.IgnoreCollision(lockController.targetTF.GetComponent<Collider2D>(), true);
        }
        float faceDir;


        public override void OnExit()
        {
            base.OnExit();

            frameController.OnFrameTick -= OnFrameTick;
            frameController.OnEnd -= OnRollEnd;
            if (lockController.targetTF != null)
                stateMachine.Output.crossEnemyController.IgnoreCollision(lockController.targetTF.GetComponent<Collider2D>(), false);


        }
        private void OnRollEnd()
        {
            stateMachine.ChangeState(nameof(PlayerIdleState));
            FrameDebugMgr.Instance?.ChangeState(-1, 0);
        }
        //ʵ��֡����
        private void OnFrameTick(FrameController.FrameInfo obj)
        {
            if (curFrameState.CheckEqual(obj) == false)
            {
                FrameDebugMgr.Instance?.ChangeState(obj.ToInt(), 0);
                curFrameState = obj;
                //Debug.Log("ת����" + obj.ToInt());
            }
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            //λ��ʵ��
            attackMoveHC.OnUpdate(deltaTime, 0, faceDir);
            //״̬ת��
            //ȡ������һ�ι���
            if (curFrameState.Contains(FrameController.MotionState.Cancle1))
            {
                if (inputBuffer.GetInput(PlayerInputType.LightAttack))
                {
                    stateMachine.ChangeState(nameof(PlayerRollLightAttack1State));
                    return;
                }
                if (inputBuffer.GetInput(PlayerInputType.HeavyAttack))
                {
                    stateMachine.ChangeState(nameof(PlayerRollHeavyAttack1State));
                    return;
                }
            }
            //ȡ������һ��λ��
            if (curFrameState.Contains(FrameController.MotionState.FreeType) && InputManager.Instance.MoveAxisX != lockController.GetFaceDir())
            {
                stateMachine.ChangeState(nameof(PlayerIdleState));
                FrameDebugMgr.Instance?.ChangeState(-1, 0);
                return;
            }

        }
        public override bool OnInteract(AttackData attackData)
        {
            if (curFrameState.Contains(FrameController.MotionState.GuardPoint))
            {
                //attackMoveHC.OnEnter((int)MotionType.Gp, attackData.attackModel.GetFinalIntensity());
                return true;
            }
            stateMachine.ChangeState(nameof(PlayerHurtState));
            return base.OnInteract(attackData);
        }
    }
}