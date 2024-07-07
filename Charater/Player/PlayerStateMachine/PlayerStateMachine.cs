using iysy.GameInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// ���״̬�����࣬������ɻ�����״̬ת��֮�⣬��Ҫ�޶�ÿ��״̬����Ļ�ȡ��Χ���Լ����ܴ�����Ϣ���С�
    /// </summary>
    public class PlayerStateMachine : iysy.StateMachine.StateMachine<BasePlayerState>
    {
        //�������롣��������InputBuffer,InteractC,MotionController2DTrigger���߿���ͨ�������¼������
        public struct StateMachineInput
        {
            public IInputBuffer<PlayerInputType> inputBuffer;
            public PlayerLockController lockController;
        }
        public StateMachineInput Input { get; private set; }
        //private 
        //��������������FrameController,MotionController,AnimationController.��
        public struct StateMachineOutput
        {
            public MotionController2D.MotionController2D motionController;
            public PlayerAnimationController animationController;
            public PlayerVFXController vfxController;
            public PlayerTimeFreezeController timeFreezeController;
            public PlayerInteractComponent interactComponent;
            public FrameController frameController;
            
        }
        public StateMachineOutput Output { get; private set; }//����״̬����ͨ������������



        public PlayerStateMachine(int dicCapacity,StateMachineInput input,StateMachineOutput output) : base(dicCapacity)
        {

            this.Input = input;
            this.Output = output;
            //����OnTrigger�¼���

            base.AddNewState(nameof(PlayerIdleState), new PlayerIdleState(this));
            base.AddNewState(nameof(PlayerRunState), new PlayerRunState(this));
            base.AddNewState(nameof(PlayerLightAttack1State), new PlayerLightAttack1State(this));
            base.AddNewState(nameof(PlayerLightAttack2State), new PlayerLightAttack2State(this));
            base.AddNewState(nameof(PlayerLightAttack3State), new PlayerLightAttack3State(this));
            base.AddNewState(nameof(PlayerHeavyAttack1State), new PlayerHeavyAttack1State(this));
            base.AddNewState(nameof(PlayerHeavyAttack2State), new PlayerHeavyAttack2State(this));
            base.AddNewState(nameof(PlayerHeavyAttack3State), new PlayerHeavyAttack3State(this));
        }
        public override void ChangeState(string stateTypeName)
        {
            base.ChangeState(stateTypeName);
            //Debug.Log("״̬ת������ǰ״̬" + stateTypeName);
        }

        /// <summary>
        /// ���е���ʱ��Ҫ����ͬ���
        /// </summary>
        /// <param name="hitType">0-���е������壬1-ƴ���ɹ���2-����ǰ����ƴ����Ҫ��ת��������</param>
        public void OnHit(int hitType)
        {
            curState.OnHit(hitType);
        }
        /// <summary>
        /// �����˻��У���Ҫ����״̬��ת
        /// </summary>
        /// <param name="hurtType"></param>
        public void OnHurt(int hurtType)
        {

        }
        private void OnTrigger()
        {

        }
    }
}