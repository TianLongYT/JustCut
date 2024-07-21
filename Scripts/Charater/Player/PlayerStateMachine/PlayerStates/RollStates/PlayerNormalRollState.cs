using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 从idle等状态，动作后摇等状态转换而来。玩家按键松开，一开始没有充能效果。
    /// 翻滚过程中如果按下左键，触发翻滚轻攻击。，翻滚后，翻滚轻攻击。
    /// 翻滚中，按下右键开始蓄力，松开后发动第二段重攻击。翻滚后，按下右键开始触发第一段重攻击。
    /// 翻滚中，按下右键开始蓄力，按下左键，开始转超重攻击，同超重攻击计算方式。
    /// 考虑一种情况，玩家按住左键后翻滚。翻滚中按下右键，此时开始超重攻击蓄力。同超重攻击计算方式。
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
            //关键帧回调
            frameController = stateMachine.Output.frameController;
            frameController.ChangeMotion(MotionType.Roll);
            frameController.OnFrameTick += OnFrameTick;
            frameController.OnEnd += OnRollEnd;
            //动画，位移
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
        //实现帧数表。
        private void OnFrameTick(FrameController.FrameInfo obj)
        {
            if (curFrameState.CheckEqual(obj) == false)
            {
                FrameDebugMgr.Instance?.ChangeState(obj.ToInt(), 0);
                curFrameState = obj;
                //Debug.Log("转换到" + obj.ToInt());
            }
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            //位移实现
            attackMoveHC.OnUpdate(deltaTime, 0, faceDir);
            //状态转化
            //取消到下一段攻击
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
            //取消到下一段位移
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