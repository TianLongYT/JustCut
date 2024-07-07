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
    /// ��ҵ�һ���ع���״̬��
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

            if (curFrameState.Contains(FrameController.MotionState.Cancle1) && inputBuffer.GetInput(PlayerInputType.HeavyAttack))//�����֡����Ū�������ٵ�,HA1��ҡ��HA2����δʵ��
            {
                stateMachine.ChangeState(nameof(PlayerHeavyAttack2State));
                return true;
            }
            //if (curFrameState.Contains(FrameController.MotionState.StartUp) && inputBuffer.GetInput(PlayerInputType.HeavyAttackRelease))
            //{
            //    stateMachine.Output.animationController.TriggerHACToHAA();
            //}
            //�˴������һ���ӳ�ʱ����Ȼ��set��һ��if��Ĵ������ķ���������ʵ���ӳ�ʱ�����Զ����빥��ǰҡ�Ĺ��ܣ�Ҳ�ǵ��Ұ�֡����Ū�����˾Ϳ���Ū
            return false;
        }


        protected override void OnActive()
        {
            base.OnActive();
            //������Ч
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