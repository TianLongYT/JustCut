using iysy.GameInput;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// �󲿷ֹ���״̬���ת������ͬ�ġ���˹���һ����������
    /// </summary>
    public abstract class PlayerHeavyAttackState : BasePlayerState
    {
        
        protected FrameController frameController;
        protected ChargeFrameController chargeFrameController;
        protected PlayerAttackMoveHComponent attackMoveHC;
        protected ChargeFrameController.FrameInfo curChargeFrameState;
        protected FrameController.FrameInfo curFrameState;

        protected bool Erupt;//�Ƿ��Ѿ�����������Ĵ��������//����������ǰ��û����ת���ؼ�֡�ġ�

        protected PlayerHeavyAttackState(PlayerStateMachine sM) : base(sM)
        {
        }

        //ʵ��֡����
        public override void OnEnter()
        {
            base.OnEnter();
            //GameWorld.Instance.SetTimeScaleMult(0.3f);
            FlipOnEnter();

            frameController = stateMachine.Output.frameController;
            chargeFrameController = stateMachine.Output.chargeFrameController;
            curFrameState.Reset();
            curChargeFrameState.Reset();
            Erupt = false;

            ChangeChargeFrameMotion();
            chargeFrameController.OnCharge += OnCharge;
            chargeFrameController.OnFrameTick += OnChargeFrameTick;
            chargeFrameController.OnEnd += OnChargeEnd;

            PlayAttackAnim();
            attackMoveHC = stateMachine.Output.motionController.GetMComponent<PlayerAttackMoveHComponent>();
        }

        protected virtual void FlipOnEnter()
        {
            if (lockController.NeedFilp)
                stateMachine.Output.animationController.PlayFilpAnim(lockController.GetPosRelationship());
        }
        protected virtual void OnCharge()
        {
            if (waitForChargeStartUpOver)
            {
                waitForChargeStartUpOver = false;
                ChargeErupt();
            }
        }
        protected virtual void OnChargeEnd()
        {
            if(Erupt == false)
            {
                //�Զ���������
                ChargeErupt();
            }
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
            //Debug.Log("����GP��Ч������ײǰ");
            //GameWorld.Instance.FreezeTime(50);
            //GameWorld.Instance.FreezeUnityTime(50);
            stateMachine.Output.interactComponent.SetGPEnable(true);
            //Debug.Log("����GP��Ч��ײ��");
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
        /// ��ʼ������״̬��
        /// </summary>
        protected abstract void ChangeChargeFrameMotion();
        /// <summary>
        /// ����ÿ��״̬ʱ���ǵ������ж��򣬲��Ҵ����ж���Ϣ��
        /// </summary>
        protected abstract void ChangeAttackInteract(bool just,int chargeCount);
        /// <summary>
        /// ����ÿ��״̬���ŵĶ�������һ����
        /// </summary>
        protected abstract void PlayAttackAnim();
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
            return frameController.JumpToKeyFrame() * GameWorld.Instance.Frame2Second;
        }
        /// <summary>
        /// �ع���һ��ʱ������ʱ����GP����ת���ؼ�����֡��
        /// </summary>
        protected virtual void ChargingGPJumpTokeyFrame()
        {

        }
        public override void OnExit()
        {
            base.OnExit();
            UnregistChargeFrameController();
            UnregistFrameController();
            //GameWorld.Instance.SetTimeScaleMult(1.0f);
        }

        private void UnregistChargeFrameController()
        {
            chargeFrameController.OnCharge -= OnCharge;
            chargeFrameController.OnFrameTick -= OnChargeFrameTick;
            chargeFrameController.OnEnd -= OnChargeEnd;
        }

        private void UnregistFrameController()
        {
            frameController.OnFrameTick -= OnFrameTick;
            frameController.OnEnd -= MotionEndExist;
            frameController.OnActive -= OnActive;
            frameController.OnGPStart -= OnGPStart;
            frameController.OnGPOver -= OnGPOver;
            frameController.OnRecover -= OnRecover;
            frameController.StopFrameController();
        }

        protected virtual void MotionEndExist()
        {
            stateMachine.ChangeState(nameof(PlayerIdleState));
            FrameDebugMgr.Instance?.ChangeState(-1, 0);
        }

        bool waitForChargeStartUpOver;
        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            //ʵ��λ��
            UpdateMotionController(deltaTime);
            //������ǰ����������߼���
            if (Erupt == false)
            {
                if (inputBuffer.GetInput(PlayerInputType.Roll))
                {
                    ChargingRoll2RollState();
                }
                else if (curChargeFrameState.Contains(ChargeFrameController.MotionState.ChargeStartUp) && ConditionToErupt())
                {
                    //�ȵ�ǰҡ�����������ͷ��ع�����
                    //Debug.Log("�ȵ�ǰҡ�����������ͷ��ع���");
                    waitForChargeStartUpOver = true;
                }
                else if (curChargeFrameState.Contains(ChargeFrameController.MotionState.Charge) && ConditionToErupt())
                {
                    //�����ͷź����ع�����
                    //Debug.Log("�����ͷź����ع���");
                    ChargeErupt();
                }
            }
            else if (curFrameState.Contains(FrameController.MotionState.Recover) && inputBuffer.GetInput(PlayerInputType.Roll))
            {
                Debug.Log("��ҡ����");
                stateMachine.ChangeState(nameof(PlayerNormalRollState));
            }

            //��һ��������
            if (CancleTypeGetInput())
            {
                return;//ϸ�ھ����ɰܣ��뵽�Ŀ��ܻ���ֵ�BUG�������Ҫ�㻨����ʱ�����ޡ�
            }
            //����̬�ƶ���
            if (FreeType2OtherState()) return;
            //ʱ�䵽���Զ��˳���

        }

        protected virtual void UpdateMotionController(float deltaTime)
        {
            attackMoveHC.OnUpdate(deltaTime, 0, lockController.GetPosRelationship());

        }
        /// <summary>
        /// Debug.Log("���뷭��״̬���������Լ̳�(��)�ع����ĳ���֡��");
        /// </summary>
        protected abstract void ChargingRoll2RollState();

        //����ǰ����߼�����

        /// <summary>
        /// �������ܵ�֡���󣬿�ʼִ�г��ܺ�Ĺ����߼���
        /// </summary>
        private void StartFrameController()
        {
            frameController.OnActive += OnActive;
            frameController.OnGPStart += OnGPStart;
            frameController.OnGPOver += OnGPOver;
            frameController.OnRecover += OnRecover;
            ChangeFrameMotion();
            frameController.OnFrameTick += OnFrameTick;
            frameController.OnEnd += MotionEndExist;
        }

        protected virtual void ChargeErupt()
        {
            this.Erupt = true;
            if (lockController.NeedFilp)
                stateMachine.Output.animationController.PlayFilpAnim(lockController.GetPosRelationship());

            curChargeFrameState.Reset();
            //ֹͣ��ǰ�ĳ��ܹؼ�֡�ص���
            chargeFrameController.StopFrameController();
            //������ײ����Ϣ��
            if (curChargeFrameState.Contains(ChargeFrameController.MotionState.Just1))
            {
                ChangeAttackInteract(true,1);
            }
            else if (curChargeFrameState.Contains(ChargeFrameController.MotionState.Just2))
            {
                ChangeAttackInteract(true, 2);
            }
            else if (curChargeFrameState.Contains(ChargeFrameController.MotionState.Just3))
            {
                ChangeAttackInteract(true, 3);
            }
            else
            {
                ChangeAttackInteract(false, 0);
            }
            //���ر�������
            StartFrameController();
            //��ʼ����λ�ơ�
            EruptMove();
            //���Ŷ���
            PlayErupAnim();
        }


        /// <summary>
        /// ����λ�ƣ�������ָ��λ�����ߡ�
        /// </summary>
        protected abstract void EruptMove();
        /// <summary>
        /// ���ű���֮��Ķ�����������ָ���������͡�
        /// </summary>
        protected abstract void PlayErupAnim();

        //��ת
        protected virtual bool FreeType2OtherState()
        {
            if (curFrameState.Contains(FrameController.MotionState.FreeType) && Erupt)
            {
                if (inputBuffer.GetInput(PlayerInputType.HeavyAttack))
                {
                    stateMachine.ChangeState(nameof(PlayerHeavyAttack1State));
                    return true;
                }
                else if (inputBuffer.GetInput(PlayerInputType.LightAttack))
                {
                    stateMachine.ChangeState(nameof(PlayerLightAttack1State));
                    return true;
                }
                else if(InputManager.Instance.MoveAxisX != 0)//��ת�������ߣ���Ȼ��ת�����������ֹ�����
                {
                    stateMachine.ChangeState(nameof(PlayerIdleState));
                    FrameDebugMgr.Instance?.ChangeState(-1, 0);
                    return true;
                }

            }
            return false;
        }
        protected abstract bool CancleTypeGetInput();
        /// <summary>
        /// ������ع���״̬���ɿ��ع������ܱ����ع���������ǳ��ع���״̬���ɿ��ṥ�����ع������ܴ������ع�����
        /// </summary>
        /// <returns></returns>
        protected abstract bool ConditionToErupt();

        //ʵ��֡����
        private void OnChargeFrameTick(ChargeFrameController.FrameInfo obj)
        {
            if (curChargeFrameState.CheckEqual(obj) == false)
            {
                FrameDebugMgr.Instance?.ChangeState(obj.ToInt(), 0);
                curChargeFrameState = obj;
                //Debug.Log("ת����" + obj.ToInt());
            }
        }

        private void OnFrameTick(FrameController.FrameInfo obj)
        {
            if (curFrameState.CheckEqual(obj) == false)
            {
                FrameDebugMgr.Instance?.ChangeState(obj.ToInt(),0);
                curFrameState = obj;
                //Debug.Log("ת����" + obj.ToInt());
            }
        }
        //ʵ����罻����

        public override bool OnInteract(AttackData attackData)
        {
            if (curFrameState.Contains(FrameController.MotionState.GuardPoint) || attackData.hitBody == false)
            {
                Debug.Log("gp״̬��ס");
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
                    //attackMoveHC.OnEnter((int)MotionType.Gp,hitIntensity);
                    FreezeWorldTime(0);
                    break;
                case 2://��������ǰƴ������ת���ؼ�֡��������ת֡�Ķ�֡���������ٲ��ţ���Ч���ٲ��š�
                    if (Erupt)
                    {
                        float jumpTime = JumpToKeyFrame();
                        //Debug.Log("GPJump,JumpTime" + jumpTime);
                        //attackMoveHC.OnEnter((int)MotionType.Gp, hitIntensity);
                        FreezeWorldTime(Mathf.FloorToInt(jumpTime * GameWorld.Instance.FramePerSecond));
                    }
                    else
                    {
                        ChargingGPJumpTokeyFrame();
                        //attackMoveHC.OnEnter((int)MotionType.Gp, hitIntensity);
                        FreezeWorldTime(0);
                    }
                    break;
            }
        }
        
    }
}
