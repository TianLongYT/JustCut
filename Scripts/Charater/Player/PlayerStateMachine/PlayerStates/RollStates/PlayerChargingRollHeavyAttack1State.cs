using iysy.GameInput;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 玩家在充能时，使用翻滚状态。
    /// </summary>
    public class PlayerChargingRollHeavyAttack1State : PlayerHeavyAttackState
    {
        public PlayerChargingRollHeavyAttack1State(PlayerStateMachine sM) : base(sM)
        {
        }
        public override void OnEnter()
        {
            base.OnEnter();//获取了frameController组件，chargeController组件，挂载chargeController关键帧回调，设置动画，获取移动组件，初始化位移。
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
            if(lockController.targetTF != null)
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
                curFrameState = obj;
                //FrameDebugMgr.Instance.ChangeState(obj.ToInt(), 0);
            }
        }
        //延长自动释放攻击的时间。
        private void OnRollEnd()
        {
            rollEnd = true;
            if(Erupt == false)
            {
                if (chargeEnd)
                {
                    ChargeErupt();
                }
                else//还能继续充能。结束翻滚动画，播放充能动画。
                {
                    stateMachine.Output.animationController.TriggerHeavyAttack2Enter();
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
            //stateMachine.Output.chargeFrameController.ChangeMotion(MotionType.HeavyAttack2);
        }
        protected override void PlayAttackAnim()
        {
            stateMachine.Output.animationController.TriggerRoll();
        }
        protected override void ChangeAttackInteract(bool just, int chargeCount)
        {
            stateMachine.Output.interactComponent.GenerateAttack(MotionType.HeavyAttack2, just, chargeCount);
        }
        protected override void ChangeFrameMotion()
        {
            stateMachine.Output.frameController.ChangeMotion(MotionType.HeavyAttack2);
        }

        protected override void EruptMove()
        {
            attackMoveHC.OnEnter((int)MotionType.HeavyAttack2, 1);
        }
        protected override void PlayErupAnim()
        {
            stateMachine.Output.animationController.TriggerRollHeavyAttackRelease();
        }
        protected override bool ConditionToErupt()
        {
            return inputBuffer.GetInput(PlayerInputType.HeavyAttackRelease);
        }
        protected override void ChargingRoll2RollState()
        {//前摇无法通过翻滚来转换到其他状态。
        }
        protected override bool CancleTypeGetInput()
        {
            //超重攻击
            if ((curChargeFrameState.Contains(ChargeFrameController.MotionState.ChargeStartUp) || curChargeFrameState.Contains(ChargeFrameController.MotionState.Charge))
                && inputBuffer.GetInput(PlayerInputType.LightAttack))
            {
                var frameCount = stateMachine.LastStateFrameCount;//上一个状态的前摇和打击帧以及部分后摇的帧数。
                frameCount += chargeFrameController.CurMotionFrame;//这个状态的蓄力前摇帧数。
                Debug.Log("jumpToCertainFrame" + frameCount + "last" + stateMachine.LastStateFrameCount + "curCharge" + chargeFrameController.CurMotionFrame);
                stateMachine.LastStateFrameCount = frameCount;//传入，给超重攻击使用，消除一部分前摇。
                stateMachine.ChangeState(nameof(PlayerSuperAttack1State));
                return true;
            }
            if (curFrameState.Contains(FrameController.MotionState.Cancle1) && inputBuffer.GetInput(PlayerInputType.HeavyAttack))
            {
                stateMachine.LastStateFrameCount = frameController.CurMotionFrame;
                stateMachine.ChangeState(nameof(PlayerHeavyAttack3State));
                return true;
            }
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
            float keyFrameTime = stateMachine.Output.animationController.JumpToKeyFrame(0,curAnimTime);
            float deltaTime = keyFrameTime - curAnimTime;
            return frameTime;
        }
        protected override void FreezeWorldTime(int frameFreezeAdder)
        {
            stateMachine.Output.timeFreezeController.FreezeTime(MotionType.HeavyAttack1, frameFreezeAdder);
        }

    }
}