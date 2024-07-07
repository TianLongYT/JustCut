using iysy.GameInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 玩家状态管理类，除了完成基本的状态转换之外，还要限定每个状态的类的获取范围，以及接受处理消息队列。
    /// </summary>
    public class PlayerStateMachine : iysy.StateMachine.StateMachine<BasePlayerState>
    {
        //处理输入。输入来自InputBuffer,InteractC,MotionController2DTrigger后者可以通过挂载事件解决。
        public struct StateMachineInput
        {
            public IInputBuffer<PlayerInputType> inputBuffer;
            public PlayerLockController lockController;
        }
        public StateMachineInput Input { get; private set; }
        //private 
        //处理输出。输出有FrameController,MotionController,AnimationController.等
        public struct StateMachineOutput
        {
            public MotionController2D.MotionController2D motionController;
            public PlayerAnimationController animationController;
            public PlayerVFXController vfxController;
            public PlayerTimeFreezeController timeFreezeController;
            public PlayerInteractComponent interactComponent;
            public FrameController frameController;
            
        }
        public StateMachineOutput Output { get; private set; }//具体状态可以通过这个来输出。



        public PlayerStateMachine(int dicCapacity,StateMachineInput input,StateMachineOutput output) : base(dicCapacity)
        {

            this.Input = input;
            this.Output = output;
            //挂载OnTrigger事件。

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
            //Debug.Log("状态转换，当前状态" + stateTypeName);
        }

        /// <summary>
        /// 击中敌人时需要处理不同情况
        /// </summary>
        /// <param name="hitType">0-击中敌人肉体，1-拼刀成功，2-击中前出现拼刀需要跳转动画进度</param>
        public void OnHit(int hitType)
        {
            curState.OnHit(hitType);
        }
        /// <summary>
        /// 被敌人击中，需要进行状态跳转
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