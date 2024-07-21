using iysy.GameInput;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// �󲿷ֹ���״̬���ת������ͬ�ġ���˹���һ����������
    /// </summary>
    public abstract class PlayerLightAttackState : BasePlayerState
    {
        public PlayerLightAttackState(PlayerStateMachine sM) : base(sM)
        {
        }
        protected FrameController frameController;
        protected PlayerAttackMoveHComponent attackMoveHC;

        private InputManager inputManager;
        
        public override void OnEnter()
        {
            base.OnEnter();
            if (lockController.NeedFilp)
                stateMachine.Output.animationController.PlayFilpAnim(lockController.GetPosRelationship());
            if(inputManager == null)
                inputManager = InputManager.Instance;

            frameController = stateMachine.Output.frameController;
            curFrameState.Reset();

            ChangeAttackInteract();

            frameController.OnActive += OnActive;
            frameController.OnGPStart += OnGPStart;
            frameController.OnGPOver += OnGPOver;
            frameController.OnRecover += OnRecover;
            ChangeFrameMotion();
            frameController.OnFrameTick += OnFrameTick;
            frameController.OnEnd += MotionEndExist;

            PlayAttackAnim();
            attackMoveHC = stateMachine.Output.motionController.GetMComponent<PlayerAttackMoveHComponent>();
            PerformAttackMove();
        }


        //������Ч����֮���
        protected virtual void OnActive()
        {
            //Debug.Log("���ô����Ч");
            stateMachine.Output.interactComponent.SetHitEnable(true);
        }
        protected virtual void OnRecover()
        {
            //Debug.Log("ȡ�������Ч");
            stateMachine.Output.interactComponent.SetHitEnable(false);
        }
        private void OnGPStart()
        {
            Debug.Log("����GP��Ч������ײǰ");
            //GameWorld.Instance.FreezeTime(50);
            //GameWorld.Instance.FreezeUnityTime(50);
            stateMachine.Output.interactComponent.SetGPEnable(true);
            Debug.Log("����GP��Ч��ײ��");
        }
        protected virtual void OnGPOver()
        {
            stateMachine.Output.interactComponent.SetGPEnable(false);
        }


        /// <summary>
        /// ÿ��״̬����ʱ����FrameController�ĳ�ʼ��ֵ��ͬ��
        /// </summary>
        protected abstract void ChangeFrameMotion();
        /// <summary>
        /// ����ÿ��״̬ʱ���ǵ������ж��򣬲��Ҵ����ж���Ϣ��
        /// </summary>
        protected abstract void ChangeAttackInteract();
        /// <summary>
        /// ����ÿ��״̬���ŵĶ�������һ����
        /// </summary>
        protected abstract void PlayAttackAnim();
        /// <summary>
        /// ��������ָ��λ�����ߡ�
        /// </summary>
        protected abstract void PerformAttackMove();

        /// <summary>
        /// ÿ����������ʱ����֡֡����һ��
        /// </summary>
        /// <param name="frameFreezeAdder">�򶯻���ת����ٵ�֡��</param>
        protected abstract void FreezeWorldTime(int frameFreezeAdder);
        /// <summary>
        /// ��GP�������ʱ������������һ��״̬�Ķ�������ʱ��Ҫָ����ǰ������
        /// </summary>
        /// <returns>�����Ѿ������˵�ʱ�䣬��Ϊ��һ�ζ�֡�Ĳο�</returns>
        protected virtual float JumpToKeyFrame()
        {
            FrameDebugMgr.Instance?.AddTagForCurBlock(0);
            return frameController.JumpToKeyFrame() * GameWorld.Instance.Frame2Second;
        }
        public override void OnExit()
        {
            base.OnExit();


            frameController.OnFrameTick -= OnFrameTick;
            frameController.OnEnd -= MotionEndExist;
            frameController.OnActive -= OnActive;
            frameController.OnGPStart -= OnGPStart;
            frameController.OnGPOver -= OnGPOver;
            frameController.OnRecover -= OnRecover;

        }
        protected virtual void MotionEndExist()
        {
            stateMachine.ChangeState(nameof(PlayerIdleState));
            FrameDebugMgr.Instance?.ChangeState(-1, 0);

        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            //ʵ��λ��
            attackMoveHC.OnUpdate(deltaTime,0,lockController.GetPosRelationship());
            //ʵ�ֶ���
            //��ײ���λ
            //��ײ���
            //��ת�߼�
            //����
            if ((curFrameState.Contains(FrameController.MotionState.StartUp) || curFrameState.Contains(FrameController.MotionState.Recover))
                && inputBuffer.GetInput(PlayerInputType.Roll))
            {
                stateMachine.ChangeState(nameof(PlayerNormalRollState));
                return;
            }
            //��һ��������
            if (inputManager.LightAttackPressing && curFrameState.Contains(FrameController.MotionState.Cancle1) && (inputBuffer.GetInput(PlayerInputType.HeavyAttack)||inputManager.HeavyAttackPressing))
            {
                stateMachine.LastStateFrameCount = frameController.CurMotionFrame;
                stateMachine.ChangeState(nameof(PlayerSuperAttack1State));
                return;
            }

            if (CancleTypeGetInput()) return;//ϸ�ھ����ɰܣ��뵽�Ŀ��ܻ���ֵ�BUG�������Ҫ�㻨����ʱ�����ޡ�
            //����̬�ƶ���
            if (FreeType2MoveState()) return;
            //ʱ�䵽���Զ��˳���

        }
        //��ת
        protected virtual bool FreeType2MoveState()
        {
            if (curFrameState.Contains(FrameController.MotionState.FreeType) && InputManager.Instance.MoveAxisX != 0)
            {
                stateMachine.ChangeState(nameof(PlayerIdleState));
                FrameDebugMgr.Instance?.ChangeState(-1, 0);

                return true;
            }
            return false;
        }
        protected abstract bool CancleTypeGetInput();

        protected FrameController.FrameInfo curFrameState;
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

        public override bool OnInteract(AttackData attackData)
        {
            if (curFrameState.Contains(FrameController.MotionState.GuardPoint))
            {
                attackMoveHC.OnEnter((int)MotionType.Gp, attackData.attackModel.GetFinalIntensity());
                return true;
            }
            stateMachine.ChangeState(nameof(PlayerHurtState));
            return base.OnInteract(attackData);
        }
        public override void OnHit(int hitType,float hitIntensity)
        {
            base.OnHit(hitType,hitIntensity);
            switch (hitType)
            {
                case 0://���е������壬���ж����ļ��ٺ��λ
                    stateMachine.Output.animationController.StuckAnimation();
                    break;
                case 1://ƴ���ɹ������ձ����ж�֡
                    //GameWorld.Instance.FreezeTime()
                    attackMoveHC.OnEnter((int)MotionType.Gp,hitIntensity);
                    FreezeWorldTime(0);
                    break;
                case 2://��������ǰƴ������ת���ؼ�֡��������ת֡�Ķ�֡���������ٲ��ţ���Ч���ٲ��š�
                    float jumpTime = JumpToKeyFrame();
                    Debug.Log("GPJump,JumpTime" + jumpTime);
                    attackMoveHC.OnEnter((int)MotionType.Gp,hitIntensity);
                    FreezeWorldTime(Mathf.FloorToInt( jumpTime * GameWorld.Instance.FramePerSecond));
                    break;
            }
        }
    }
}