using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 受伤时，各种效果的执行，伤害计算在PlayerInteractComponnet里面已经执行了。大概。
    /// </summary>
    public class PlayerHurtState : BasePlayerState
    {
        public PlayerHurtState(PlayerStateMachine sM) : base(sM)
        {
        }
        /*被打后的逻辑
         * 播放动画
         * 顿帧。震动。特效。
         * 有一段时间的硬性位移。
         * 一段时间内不会受到二次伤害。
         * 
         * 
         */
        PlayerAttackMoveHComponent attackMoveHComponent;
        FrameController frameController;
        FrameController.FrameInfo curFrameState;
        public override void OnEnter()
        {
            base.OnEnter();

            frameController = stateMachine.Output.frameController;
            curFrameState.Reset();
            frameController.ChangeMotion(MotionType.Hurt);
            frameController.OnFrameTick += OnFrameTick;
            frameController.OnEnd += OnHurtEnd;

            stateMachine.Output.animationController.PlayHurtAnim();
            stateMachine.Output.timeFreezeController.FreezeTime(MotionType.Hurt);
            attackMoveHComponent = stateMachine.Output.motionController.GetMComponent<PlayerAttackMoveHComponent>();
            attackMoveHComponent.OnEnter((int)MotionType.Hurt, 1);
        }
        public override void OnExit()
        {
            base.OnExit();
            FrameDebugMgr.Instance?.ChangeState(-1, 0);
            frameController.OnFrameTick -= OnFrameTick;
            frameController.OnEnd -= OnHurtEnd;
        }
        private void OnFrameTick(FrameController.FrameInfo obj)
        {
            if(curFrameState.CheckEqual(obj) == false)
            {
                curFrameState = obj;
                FrameDebugMgr.Instance?.ChangeState(obj.ToInt(), 0);
            }
        }
        private void OnHurtEnd()
        {
            stateMachine.ChangeState(nameof(PlayerIdleState));
        }
        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            attackMoveHComponent.OnUpdate(deltaTime, 0, lockController.GetFaceDir());

            //跳转
            if (curFrameState.Contains(FrameController.MotionState.Cancle1))
            {
                if (inputBuffer.GetInput(PlayerInputType.HeavyAttack))
                {
                    stateMachine.ChangeState(nameof(PlayerHeavyAttack1State));
                }
                else if (inputBuffer.GetInput(PlayerInputType.LightAttack))
                {
                    stateMachine.ChangeState(nameof(PlayerLightAttack1State));
                }
                else if (inputBuffer.GetInput(PlayerInputType.Roll))
                {
                    stateMachine.ChangeState(nameof(PlayerNormalRollState));
                }
                else if(InputManager.Instance.MoveAxisX != 0)
                {
                    stateMachine.ChangeState(nameof(PlayerIdleState));
                }
            }
        }
        public override bool OnInteract(AttackData attackData)
        {
            if (curFrameState.Contains(FrameController.MotionState.Recover))
            {
                stateMachine.ChangeState(nameof(PlayerHurtState));
            }
            return base.OnInteract(attackData);
        }
    }
}