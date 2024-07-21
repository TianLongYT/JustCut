using iysy.GameInput;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 大部分攻击状态类的转换是相同的。因此公用一个攻击父类
    /// </summary>
    public abstract class PlayerLightAttackState : BasePlayerState
    {
        public PlayerLightAttackState(PlayerStateMachine sM) : base(sM)
        {
        }
        protected FrameController frameController;
        protected PlayerAttackMoveHComponent attackMoveHC;

        private InputManager inputManager;
        
        public override void OnEnter()
        {
            base.OnEnter();
            if (lockController.NeedFilp)
                stateMachine.Output.animationController.PlayFilpAnim(lockController.GetPosRelationship());
            if(inputManager == null)
                inputManager = InputManager.Instance;

            frameController = stateMachine.Output.frameController;
            curFrameState.Reset();

            ChangeAttackInteract();

            frameController.OnActive += OnActive;
            frameController.OnGPStart += OnGPStart;
            frameController.OnGPOver += OnGPOver;
            frameController.OnRecover += OnRecover;
            ChangeFrameMotion();
            frameController.OnFrameTick += OnFrameTick;
            frameController.OnEnd += MotionEndExist;

            PlayAttackAnim();
            attackMoveHC = stateMachine.Output.motionController.GetMComponent<PlayerAttackMoveHComponent>();
            PerformAttackMove();
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
            Debug.Log("设置GP生效产生碰撞前");
            //GameWorld.Instance.FreezeTime(50);
            //GameWorld.Instance.FreezeUnityTime(50);
            stateMachine.Output.interactComponent.SetGPEnable(true);
            Debug.Log("设置GP生效碰撞后");
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
        /// 进入每个状态时，记得生成判定框，并且传入判定信息。
        /// </summary>
        protected abstract void ChangeAttackInteract();
        /// <summary>
        /// 进入每个状态播放的动画都不一样。
        /// </summary>
        protected abstract void PlayAttackAnim();
        /// <summary>
        /// 交给子类指定位移曲线。
        /// </summary>
        protected abstract void PerformAttackMove();

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
            FrameDebugMgr.Instance?.AddTagForCurBlock(0);
            return frameController.JumpToKeyFrame() * GameWorld.Instance.Frame2Second;
        }
        public override void OnExit()
        {
            base.OnExit();


            frameController.OnFrameTick -= OnFrameTick;
            frameController.OnEnd -= MotionEndExist;
            frameController.OnActive -= OnActive;
            frameController.OnGPStart -= OnGPStart;
            frameController.OnGPOver -= OnGPOver;
            frameController.OnRecover -= OnRecover;

        }
        protected virtual void MotionEndExist()
        {
            stateMachine.ChangeState(nameof(PlayerIdleState));
            FrameDebugMgr.Instance?.ChangeState(-1, 0);

        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            //实现位移
            attackMoveHC.OnUpdate(deltaTime,0,lockController.GetPosRelationship());
            //实现动画
            //碰撞框就位
            //碰撞检测
            //跳转逻辑
            //翻滚
            if ((curFrameState.Contains(FrameController.MotionState.StartUp) || curFrameState.Contains(FrameController.MotionState.Recover))
                && inputBuffer.GetInput(PlayerInputType.Roll))
            {
                stateMachine.ChangeState(nameof(PlayerNormalRollState));
                return;
            }
            //下一个攻击。
            if (inputManager.LightAttackPressing && curFrameState.Contains(FrameController.MotionState.Cancle1) && (inputBuffer.GetInput(PlayerInputType.HeavyAttack)||inputManager.HeavyAttackPressing))
            {
                stateMachine.LastStateFrameCount = frameController.CurMotionFrame;
                stateMachine.ChangeState(nameof(PlayerSuperAttack1State));
                return;
            }

            if (CancleTypeGetInput()) return;//细节决定成败，想到的可能会出现的BUG到最后需要你花大量时间来修。
            //自由态移动。
            if (FreeType2MoveState()) return;
            //时间到达自动退出。

        }
        //跳转
        protected virtual bool FreeType2MoveState()
        {
            if (curFrameState.Contains(FrameController.MotionState.FreeType) && InputManager.Instance.MoveAxisX != 0)
            {
                stateMachine.ChangeState(nameof(PlayerIdleState));
                FrameDebugMgr.Instance?.ChangeState(-1, 0);

                return true;
            }
            return false;
        }
        protected abstract bool CancleTypeGetInput();

        protected FrameController.FrameInfo curFrameState;
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

        public override bool OnInteract(AttackData attackData)
        {
            if (curFrameState.Contains(FrameController.MotionState.GuardPoint))
            {
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
                    attackMoveHC.OnEnter((int)MotionType.Gp,hitIntensity);
                    FreezeWorldTime(0);
                    break;
                case 2://产生攻击前拼刀，跳转到关键帧，进行跳转帧的顿帧。动画快速播放，特效快速播放。
                    float jumpTime = JumpToKeyFrame();
                    Debug.Log("GPJump,JumpTime" + jumpTime);
                    attackMoveHC.OnEnter((int)MotionType.Gp,hitIntensity);
                    FreezeWorldTime(Mathf.FloorToInt( jumpTime * GameWorld.Instance.FramePerSecond));
                    break;
            }
        }
    }
}