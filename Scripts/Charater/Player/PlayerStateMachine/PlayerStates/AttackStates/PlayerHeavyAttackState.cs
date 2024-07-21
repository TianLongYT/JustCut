using iysy.GameInput;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 大部分攻击状态类的转换是相同的。因此公用一个攻击父类
    /// </summary>
    public abstract class PlayerHeavyAttackState : BasePlayerState
    {
        
        protected FrameController frameController;
        protected ChargeFrameController chargeFrameController;
        protected PlayerAttackMoveHComponent attackMoveHC;
        protected ChargeFrameController.FrameInfo curChargeFrameState;
        protected FrameController.FrameInfo curFrameState;

        protected bool Erupt;//是否已经进入蓄力后的打击动作。//进入打击动作前是没有跳转到关键帧的。

        protected PlayerHeavyAttackState(PlayerStateMachine sM) : base(sM)
        {
        }

        //实现帧数表。
        public override void OnEnter()
        {
            base.OnEnter();
            //GameWorld.Instance.SetTimeScaleMult(0.3f);
            FlipOnEnter();

            frameController = stateMachine.Output.frameController;
            chargeFrameController = stateMachine.Output.chargeFrameController;
            curFrameState.Reset();
            curChargeFrameState.Reset();
            Erupt = false;

            ChangeChargeFrameMotion();
            chargeFrameController.OnCharge += OnCharge;
            chargeFrameController.OnFrameTick += OnChargeFrameTick;
            chargeFrameController.OnEnd += OnChargeEnd;

            PlayAttackAnim();
            attackMoveHC = stateMachine.Output.motionController.GetMComponent<PlayerAttackMoveHComponent>();
        }

        protected virtual void FlipOnEnter()
        {
            if (lockController.NeedFilp)
                stateMachine.Output.animationController.PlayFilpAnim(lockController.GetPosRelationship());
        }
        protected virtual void OnCharge()
        {
            if (waitForChargeStartUpOver)
            {
                waitForChargeStartUpOver = false;
                ChargeErupt();
            }
        }
        protected virtual void OnChargeEnd()
        {
            if(Erupt == false)
            {
                //自动触发爆发
                ChargeErupt();
            }
        }


        //进行特效播放之类的
        protected virtual void OnActive()
        {
            //Debug.Log("设置打击有效");
            stateMachine.Output.interactComponent.SetHitEnable(true);
        }
        protected virtual void OnRecover()
        {
            //Debug.Log("取消打击有效");
            stateMachine.Output.interactComponent.SetHitEnable(false);
        }
        private void OnGPStart()
        {
            //Debug.Log("设置GP生效产生碰撞前");
            //GameWorld.Instance.FreezeTime(50);
            //GameWorld.Instance.FreezeUnityTime(50);
            stateMachine.Output.interactComponent.SetGPEnable(true);
            //Debug.Log("设置GP生效碰撞后");
        }
        protected virtual void OnGPOver()
        {
            stateMachine.Output.interactComponent.SetGPEnable(false);
        }


        /// <summary>
        /// 每个状态进入时，对FrameController的初始化值不同。
        /// </summary>
        protected abstract void ChangeFrameMotion();
        /// <summary>
        /// 初始化充能状态。
        /// </summary>
        protected abstract void ChangeChargeFrameMotion();
        /// <summary>
        /// 进入每个状态时，记得生成判定框，并且传入判定信息。
        /// </summary>
        protected abstract void ChangeAttackInteract(bool just,int chargeCount);
        /// <summary>
        /// 进入每个状态播放的动画都不一样。
        /// </summary>
        protected abstract void PlayAttackAnim();
        /// <summary>
        /// 每个动作击中时，顿帧帧数不一样
        /// </summary>
        /// <param name="frameFreezeAdder">因动画跳转而多顿的帧数</param>
        protected abstract void FreezeWorldTime(int frameFreezeAdder);
        /// <summary>
        /// 当GP过早出现时，动画还处上一个状态的动画。此时需要指定当前动画。
        /// </summary>
        /// <returns>返回已经跳过了的时间，作为下一次顿帧的参考</returns>
        protected virtual float JumpToKeyFrame()
        {
            return frameController.JumpToKeyFrame() * GameWorld.Instance.Frame2Second;
        }
        /// <summary>
        /// 重攻击一段时，充能时产生GP，跳转到关键动画帧。
        /// </summary>
        protected virtual void ChargingGPJumpTokeyFrame()
        {

        }
        public override void OnExit()
        {
            base.OnExit();
            UnregistChargeFrameController();
            UnregistFrameController();
            //GameWorld.Instance.SetTimeScaleMult(1.0f);
        }

        private void UnregistChargeFrameController()
        {
            chargeFrameController.OnCharge -= OnCharge;
            chargeFrameController.OnFrameTick -= OnChargeFrameTick;
            chargeFrameController.OnEnd -= OnChargeEnd;
        }

        private void UnregistFrameController()
        {
            frameController.OnFrameTick -= OnFrameTick;
            frameController.OnEnd -= MotionEndExist;
            frameController.OnActive -= OnActive;
            frameController.OnGPStart -= OnGPStart;
            frameController.OnGPOver -= OnGPOver;
            frameController.OnRecover -= OnRecover;
            frameController.StopFrameController();
        }

        protected virtual void MotionEndExist()
        {
            stateMachine.ChangeState(nameof(PlayerIdleState));
            FrameDebugMgr.Instance?.ChangeState(-1, 0);
        }

        bool waitForChargeStartUpOver;
        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            //实现位移
            UpdateMotionController(deltaTime);
            //开启当前动作打击的逻辑。
            if (Erupt == false)
            {
                if (inputBuffer.GetInput(PlayerInputType.Roll))
                {
                    ChargingRoll2RollState();
                }
                else if (curChargeFrameState.Contains(ChargeFrameController.MotionState.ChargeStartUp) && ConditionToErupt())
                {
                    //等到前摇结束后立刻释放重攻击。
                    //Debug.Log("等到前摇结束后立刻释放重攻击");
                    waitForChargeStartUpOver = true;
                }
                else if (curChargeFrameState.Contains(ChargeFrameController.MotionState.Charge) && ConditionToErupt())
                {
                    //立刻释放后续重攻击。
                    //Debug.Log("立刻释放后续重攻击");
                    ChargeErupt();
                }
            }
            else if (curFrameState.Contains(FrameController.MotionState.Recover) && inputBuffer.GetInput(PlayerInputType.Roll))
            {
                Debug.Log("后摇翻滚");
                stateMachine.ChangeState(nameof(PlayerNormalRollState));
            }

            //下一个攻击。
            if (CancleTypeGetInput())
            {
                return;//细节决定成败，想到的可能会出现的BUG到最后需要你花大量时间来修。
            }
            //自由态移动。
            if (FreeType2OtherState()) return;
            //时间到达自动退出。

        }

        protected virtual void UpdateMotionController(float deltaTime)
        {
            attackMoveHC.OnUpdate(deltaTime, 0, lockController.GetPosRelationship());

        }
        /// <summary>
        /// Debug.Log("进入翻滚状态，翻滚可以继承(超)重攻击的充能帧数");
        /// </summary>
        protected abstract void ChargingRoll2RollState();

        //蓄力前后的逻辑处理。

        /// <summary>
        /// 经过充能的帧数后，开始执行充能后的攻击逻辑。
        /// </summary>
        private void StartFrameController()
        {
            frameController.OnActive += OnActive;
            frameController.OnGPStart += OnGPStart;
            frameController.OnGPOver += OnGPOver;
            frameController.OnRecover += OnRecover;
            ChangeFrameMotion();
            frameController.OnFrameTick += OnFrameTick;
            frameController.OnEnd += MotionEndExist;
        }

        protected virtual void ChargeErupt()
        {
            this.Erupt = true;
            if (lockController.NeedFilp)
                stateMachine.Output.animationController.PlayFilpAnim(lockController.GetPosRelationship());

            curChargeFrameState.Reset();
            //停止当前的充能关键帧回调。
            chargeFrameController.StopFrameController();
            //传入碰撞框信息，
            if (curChargeFrameState.Contains(ChargeFrameController.MotionState.Just1))
            {
                ChangeAttackInteract(true,1);
            }
            else if (curChargeFrameState.Contains(ChargeFrameController.MotionState.Just2))
            {
                ChangeAttackInteract(true, 2);
            }
            else if (curChargeFrameState.Contains(ChargeFrameController.MotionState.Just3))
            {
                ChangeAttackInteract(true, 3);
            }
            else
            {
                ChangeAttackInteract(false, 0);
            }
            //挂载爆发函数
            StartFrameController();
            //开始爆发位移。
            EruptMove();
            //播放动画
            PlayErupAnim();
        }


        /// <summary>
        /// 爆发位移，让子类指定位移曲线。
        /// </summary>
        protected abstract void EruptMove();
        /// <summary>
        /// 播放爆发之后的动画，让子类指定动画类型。
        /// </summary>
        protected abstract void PlayErupAnim();

        //跳转
        protected virtual bool FreeType2OtherState()
        {
            if (curFrameState.Contains(FrameController.MotionState.FreeType) && Erupt)
            {
                if (inputBuffer.GetInput(PlayerInputType.HeavyAttack))
                {
                    stateMachine.ChangeState(nameof(PlayerHeavyAttack1State));
                    return true;
                }
                else if (inputBuffer.GetInput(PlayerInputType.LightAttack))
                {
                    stateMachine.ChangeState(nameof(PlayerLightAttack1State));
                    return true;
                }
                else if(InputManager.Instance.MoveAxisX != 0)//能转化成行走，自然能转化成其他各种攻击。
                {
                    stateMachine.ChangeState(nameof(PlayerIdleState));
                    FrameDebugMgr.Instance?.ChangeState(-1, 0);
                    return true;
                }

            }
            return false;
        }
        protected abstract bool CancleTypeGetInput();
        /// <summary>
        /// 如果是重攻击状态，松开重攻击就能爆发重攻击，如果是超重攻击状态，松开轻攻击或重攻击都能触发超重攻击。
        /// </summary>
        /// <returns></returns>
        protected abstract bool ConditionToErupt();

        //实现帧数表。
        private void OnChargeFrameTick(ChargeFrameController.FrameInfo obj)
        {
            if (curChargeFrameState.CheckEqual(obj) == false)
            {
                FrameDebugMgr.Instance?.ChangeState(obj.ToInt(), 0);
                curChargeFrameState = obj;
                //Debug.Log("转换到" + obj.ToInt());
            }
        }

        private void OnFrameTick(FrameController.FrameInfo obj)
        {
            if (curFrameState.CheckEqual(obj) == false)
            {
                FrameDebugMgr.Instance?.ChangeState(obj.ToInt(),0);
                curFrameState = obj;
                //Debug.Log("转换到" + obj.ToInt());
            }
        }
        //实现外界交互。

        public override bool OnInteract(AttackData attackData)
        {
            if (curFrameState.Contains(FrameController.MotionState.GuardPoint) || attackData.hitBody == false)
            {
                Debug.Log("gp状态防住");
                attackMoveHC.OnEnter((int)MotionType.Gp, attackData.attackModel.GetFinalIntensity());
                return true;
            }
            stateMachine.ChangeState(nameof(PlayerHurtState));
            return base.OnInteract(attackData);
        }
        public override void OnHit(int hitType,float hitIntensity)
        {
            base.OnHit(hitType,hitIntensity);
            switch (hitType)
            {
                case 0://击中敌人肉体，进行动画的减速后归位
                    stateMachine.Output.animationController.StuckAnimation();
                    break;
                case 1://拼刀成功，按照表格进行顿帧
                    //GameWorld.Instance.FreezeTime()
                    //attackMoveHC.OnEnter((int)MotionType.Gp,hitIntensity);
                    FreezeWorldTime(0);
                    break;
                case 2://产生攻击前拼刀，跳转到关键帧，进行跳转帧的顿帧。动画快速播放，特效快速播放。
                    if (Erupt)
                    {
                        float jumpTime = JumpToKeyFrame();
                        //Debug.Log("GPJump,JumpTime" + jumpTime);
                        //attackMoveHC.OnEnter((int)MotionType.Gp, hitIntensity);
                        FreezeWorldTime(Mathf.FloorToInt(jumpTime * GameWorld.Instance.FramePerSecond));
                    }
                    else
                    {
                        ChargingGPJumpTokeyFrame();
                        //attackMoveHC.OnEnter((int)MotionType.Gp, hitIntensity);
                        FreezeWorldTime(0);
                    }
                    break;
            }
        }
        
    }
}
