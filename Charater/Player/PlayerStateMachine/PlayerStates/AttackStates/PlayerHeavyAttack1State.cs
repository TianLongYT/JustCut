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
    /// 玩家第一段重攻击状态。
    /// </summary>
    public class PlayerHeavyAttack1State : PlayerHeavyAttackState
    {
        public PlayerHeavyAttack1State(PlayerStateMachine sM) : base(sM)
        {
        }
        protected override void ChangeAttackInteract()
        {
            stateMachine.Output.interactComponent.GenerateAttack(MotionType.HeavyAttack1);
        }
        protected override void ChangeFrameMotion()
        {
            stateMachine.Output.frameController.ChangeMotion(MotionType.HeavyAttack1);
        }
        protected override void PlayAttackAnim()
        {
            //stateMachine.Output.animationController.TriggerBTToHA1S();
            //stateMachine.Output.animationController.TriggerLAToHA1S();
           
        }
        protected override bool CancleTypeGetInput()
        {

            if (curFrameState.Contains(FrameController.MotionState.Cancle1) && inputBuffer.GetInput(PlayerInputType.HeavyAttack))//后面把帧数表弄明白了再调,HA1后摇接HA2功能未实现
            {
                stateMachine.ChangeState(nameof(PlayerHeavyAttack2State));
                return true;
            }
            //if (curFrameState.Contains(FrameController.MotionState.StartUp) && inputBuffer.GetInput(PlayerInputType.HeavyAttackRelease))
            //{
            //    stateMachine.Output.animationController.TriggerHACToHAA();
            //}
            //此处待添加一个延迟时间满然后set上一个if里的触发器的方法，用以实现延迟时间满自动进入攻击前摇的功能，也是等我把帧数表弄明白了就可以弄
            return false;
        }


        protected override void OnActive()
        {
            base.OnActive();
            //播放特效
           // stateMachine.Output.vfxController.PlaySlashParticle(2);

        }
        protected override float JumpToKeyFrame()
        {
            float frameTime = base.JumpToKeyFrame();
            float curAnimTime = stateMachine.Output.frameController.CurMotionFrame * GameWorld.Instance.Frame2Second;
            float keyFrameTime = stateMachine.Output.animationController.JumpToKeyFrame(2,curAnimTime);
            float deltaTime = keyFrameTime - curAnimTime;
            return frameTime;
        }
        protected override void FreezeWorldTime(int frameFreezeAdder)
        {
            stateMachine.Output.timeFreezeController.FreezeTime(MotionType.HeavyAttack1, frameFreezeAdder);
        }

    }
}