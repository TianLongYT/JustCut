using iysy.GameInput;
using iysy.MotionController2D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

namespace iysy.JustCut
{

    /// <summary>
    /// 
    /// </summary>
    public class PlayerNormalState : BasePlayerState
    {
        public PlayerNormalState(PlayerStateMachine sM) : base(sM)
        {
        }

        PlayerFTGMoveHComponent moveHComponent;
        //完成部分状态转换
        public override void OnEnter()
        {
            stateMachine.Output.animationController.TriggerMove();
            moveHComponent = stateMachine.Output.motionController.GetMComponent<PlayerFTGMoveHComponent>();
            lastAxis = -2;
            if (lockController.NeedFilp)
                stateMachine.Output.animationController.PlayFilpAnim(lockController.GetPosRelationship());
        }


        public override void OnExit()
        {
            stateMachine.Output.animationController.PlayMoveAnim(0);
            moveHComponent.OnExit();
            lastAxis = 0;
        }
        private void OnFaceDirChange(float obj)
        {
            moveHComponent.OnEnter(obj);
            stateMachine.Output.animationController.PlayFilpAnim(obj);
        }

        bool isMoving;

        float lastAxis;
        public override void OnUpdate(float deltaTime)
        {
            //Debug.Log("deltaTime" + deltaTime);
            //移动
            float curAxis = InputManager.Instance.MoveAxisX;
            curAxis = MMath.Math.DivideValue(curAxis,0.05f);
            if(lastAxis != curAxis)
            {
                if (curAxis == 0)
                {
                    moveHComponent.OnExit();
                    isMoving = false;
                }
                else
                {
                    if (lockController.IsLock())
                    {
                        moveHComponent.OnEnter(lockController.GetPosRelationship());
                    }
                    else
                    {
                        //如果没有进入锁定状态，那么当输入方向变化时，执行转向操作。
                        moveHComponent.OnEnter(curAxis);
                        stateMachine.Output.animationController.PlayFilpAnim(curAxis);
                    }
                    isMoving = true;
                }
            }
            if(isMoving)
                moveHComponent.OnUpdate(deltaTime, curAxis);
            lastAxis = curAxis;
            //播放动画
            float curSpeed = moveHComponent.GetVelocityUtility().GetCurVelocity().x;
            float faceDir = lockController.GetPosRelationship();
            //移动方向和朝向相同。
            float moveRate = Mathf.Sign(curSpeed)==Mathf.Sign(faceDir) || faceDir == 0 ?  curSpeed / moveHComponent.frontMaxSpeed :curSpeed / moveHComponent.baceMaxSpeed;
            moveRate = deltaTime == 0 ? 0 : moveRate;
            //如果没有锁定敌人，则一直使用向前奔跑的动画。
            moveRate = faceDir == 0 ? Mathf.Abs(moveRate) : moveRate * faceDir;
            stateMachine.Output.animationController.PlayMoveAnim(moveRate);


            //状态转换
            if (inputBuffer.GetInput(PlayerInputType.Roll))
            {
                stateMachine.ChangeState(nameof(PlayerNormalRollState));
            }
            else if (inputBuffer.GetInput(PlayerInputType.LightAttack))
            {
                stateMachine.ChangeState(nameof(PlayerLightAttack1State));
            }
            else if (inputBuffer.GetInput(PlayerInputType.HeavyAttack))
            {
                stateMachine.ChangeState(nameof(PlayerHeavyAttack1State));
            }
        }
        public override bool OnInteract(AttackData attackData)
        {
            stateMachine.ChangeState(nameof(PlayerHurtState));
            return base.OnInteract(attackData);
        }
    }
}