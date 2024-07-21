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
    /// 玩家超重攻击状态，这个状态需要知道上一个状态经过帧数的时间。
    /// </summary>
    public class PlayerChargingRollSuperAttack1State : PlayerHeavyAttackState
    {
        public PlayerChargingRollSuperAttack1State(PlayerStateMachine sM) : base(sM)
        {
        }
        private PlayerInteractComponent interactComponent;
        public override void OnEnter()
        {
            base.OnEnter();
            chargeFrameController.JumpToCertainFrame(stateMachine.LastStateFrameCount);
            stateMachine.LastStateFrameCount = 0;

            //蓄能是必须要继续蓄的。
            rollEnd = false;
            chargeEnd = false;
            //挂载翻滚的关键帧回调。
            frameController.ChangeMotion(MotionType.Roll);
            frameController.OnFrameTick += OnRollFrameTick;
            frameController.OnEnd += OnRollEnd;
            //翻滚位移
            attackMoveHC.OnEnter((int)MotionType.Roll, 1);
            initFaceDir = lockController.GetFaceDir(); 
            if (lockController.targetTF != null)
                stateMachine.Output.crossEnemyController.IgnoreCollision(lockController.targetTF.GetComponent<Collider2D>(), true);

        }
        public override void OnExit()
        {
            base.OnExit();
            if(Erupt == false)
            {
                if (lockController.targetTF != null)
                    stateMachine.Output.crossEnemyController.IgnoreCollision(lockController.targetTF.GetComponent<Collider2D>(), false);
                frameController.OnFrameTick -= OnRollFrameTick;
                frameController.OnEnd -= OnRollEnd;
            }
        }
        float initFaceDir;
        protected override void UpdateMotionController(float deltaTime)
        {
            //如果处于翻滚状态，不会改变原来的翻滚方向。当释放时，改变攻击方向。
            if (Erupt == false)
                attackMoveHC.OnUpdate(deltaTime, 0, initFaceDir);
            else
            {
                base.UpdateMotionController(deltaTime);
            }
        }
        bool rollEnd;
        bool chargeEnd;
        //完成状态跟踪。重写受击方法。改为无敌。
        private void OnRollFrameTick(FrameController.FrameInfo obj)
        {
            if (curFrameState.CheckEqual(obj) == false)
            {
                //FrameDebugMgr.Instance.ChangeState(obj.ToInt(), 0);
                curFrameState = obj;
            }
        }
        //延长自动释放攻击的时间。
        private void OnRollEnd()
        {
            rollEnd = true;
            if (Erupt == false)
            {
                if (chargeEnd)
                {
                    ChargeErupt();
                }
                else//还能继续充能。结束翻滚动画，播放充能动画。
                {
                    stateMachine.Output.animationController.TriggerSuperAttackEnter();
                }
            }
        }
        protected override void OnChargeEnd()
        {
            chargeEnd = true;
            if (rollEnd)
            {
                ChargeErupt();
            }

        }
        protected override void ChargeErupt()
        {
            if (lockController.targetTF != null)
                stateMachine.Output.crossEnemyController.IgnoreCollision(lockController.targetTF.GetComponent<Collider2D>(), false);

            frameController.OnFrameTick -= OnRollFrameTick;
            frameController.OnEnd -= OnRollEnd;
            base.ChargeErupt();
        }
        protected override void ChangeChargeFrameMotion()
        {
            interactComponent = stateMachine.Output.interactComponent;
            //chargeFrameController.ChangeMotion(MotionType.SuperAttack1);

        }

        protected override void PlayAttackAnim()
        {
            stateMachine.Output.animationController.TriggerRoll();

        }
        protected override void ChangeAttackInteract(bool just, int chargeCount)
        {
            stateMachine.Output.interactComponent.GenerateAttack(MotionType.SuperAttack1, just, chargeCount);
        }
        protected override void ChangeFrameMotion()
        {
            stateMachine.Output.frameController.ChangeMotion(MotionType.SuperAttack1);
        }
        protected override void EruptMove()
        {
            attackMoveHC.OnEnter((int)MotionType.SuperAttack1, 1);
        }
        protected override void PlayErupAnim()
        {
            stateMachine.Output.animationController.TriggerRollSuperAttackRelease();
        }
        protected override bool ConditionToErupt()
        {
            return (inputBuffer.GetInput(PlayerInputType.HeavyAttackRelease)||inputBuffer.GetInput(PlayerInputType.LightAttackRelease));
        }
        protected override void ChargingRoll2RollState()
        {
        }
        protected override bool CancleTypeGetInput()
        {
            //不能取消其他动作。
            return false;
        }


        protected override void OnActive()
        {
            base.OnActive();
            //播放特效
            stateMachine.Output.vfxController.PlaySlashParticle(0);

        }
        protected override float JumpToKeyFrame()
        {
            float frameTime = base.JumpToKeyFrame();
            float curAnimTime = stateMachine.Output.frameController.CurMotionFrame * GameWorld.Instance.Frame2Second;
            float keyFrameTime = stateMachine.Output.animationController.JumpToKeyFrame(6,curAnimTime);
            float deltaTime = keyFrameTime - curAnimTime;
            return frameTime;
        }
        protected override void FreezeWorldTime(int frameFreezeAdder)
        {
            stateMachine.Output.timeFreezeController.FreezeTime(MotionType.HeavyAttack1, frameFreezeAdder);
        }


    }
}