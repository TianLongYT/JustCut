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
            public ChargeFrameController chargeFrameController;
            public PlayerCrossEnemyController crossEnemyController;
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

            base.AddNewState(nameof(PlayerSuperAttack1State), new PlayerSuperAttack1State(this));

            base.AddNewState(nameof(PlayerNormalRollState), new PlayerNormalRollState(this));
            //base.AddNewState(nameof(PlayerChargingRollState), new PlayerChargingRollState(this));
            base.AddNewState(nameof(PlayerRollLightAttack1State), new PlayerRollLightAttack1State(this));
            base.AddNewState(nameof(PlayerRollHeavyAttack1State), new PlayerRollHeavyAttack1State(this));
            base.AddNewState(nameof(PlayerRollSuperAttack1State), new PlayerRollSuperAttack1State(this));

            base.AddNewState(nameof(PlayerChargingRollHeavyAttack1State), new PlayerChargingRollHeavyAttack1State(this));
            base.AddNewState(nameof(PlayerChargingRollSuperAttack1State), new PlayerChargingRollSuperAttack1State(this));
            base.AddNewState(nameof(PlayerHurtState), new PlayerHurtState(this));
        }
        public string lastStateTypeName;//����ʱ�����ܣ���Ҫ֪����һ��״̬�����ع��������ܣ���Ҫ֪����һ��״̬
        public BasePlayerState lastState;
        public int LastStateFrameCount { get; set; }//ת��Ϊ���ع���ʱ��Ҫʹ�á�
        public override void ChangeState(string stateTypeName)
        {
            lastState = curState;
            lastStateTypeName = nameof(curState);
            base.ChangeState(stateTypeName);
            Debug.Log("״̬ת������ǰ״̬" + stateTypeName);
        }

        /// <summary>
        /// ���е���ʱ��Ҫ����ͬ���
        /// </summary>
        /// <param name="hitType">0-���е������壬1-ƴ���ɹ���2-����ǰ����ƴ����Ҫ��ת��������</param>
        public void OnHit(int hitType,float hitIntensity)
        {
            curState.OnHit(hitType,hitIntensity);
        }
        /// <summary>
        /// �������й����ж�ʱ���á�������״̬����������״̬��,true��ʾ��Ҵ����޵�״̬��false��ʾ��Ҵ���һ����Ҫ��Ѫ��״̬��
        /// </summary>
        public bool OnInteract(AttackData attackData)
        {
            return curState.OnInteract(attackData);
        }
        private void OnTrigger()
        {

        }
    }
}