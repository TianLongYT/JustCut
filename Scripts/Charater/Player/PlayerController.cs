using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] PlayerModel playerModel;

        PlayerStateMachine stateMachine;

        PlayerInfo playerInfo;

        PlayerLockController lockController;

        MotionController2D.MotionController2D motionController;
        PlayerAnimationController animationController;
        PlayerVFXController vfxController;
        PlayerTimeFreezeController timeFreezeController;
        PlayerInteractComponent interactComponent;
        FrameController frameController;
        ChargeFrameController chargeFrameController;
        PlayerCrossEnemyController crossEnemyController;

        private void Awake()
        {
            if(playerModel == null)
            {
                Debug.LogError("请放入PlayerModel作为数据支撑");
                return;
            }

            if (playerInfo == null)
                playerInfo = new PlayerInfo(playerModel.infoModel);
            if (lockController == null)
                lockController = GetComponentInChildren<PlayerLockController>();
            if (motionController == null)
                motionController = GetComponentInChildren<MotionController2D.MotionController2D>();
            motionController.LoadComponents();
            motionController.OnReInitialize();
            if (animationController == null)
                animationController = GetComponentInChildren<PlayerAnimationController>();
            animationController.Initialize(GetComponentInChildren<Animator>(), playerModel.animationModel);
            if (vfxController == null)
                vfxController = GetComponentInChildren<PlayerVFXController>();
            if (timeFreezeController == null)
                timeFreezeController = new PlayerTimeFreezeController(playerModel.frameModel);
            if (interactComponent == null)
                interactComponent = GetComponentInChildren<PlayerInteractComponent>();
            interactComponent.Initalize(vfxController,playerModel.attackModel, playerInfo);
            if (frameController == null)
                frameController = new PlayerFrameController(playerModel.frameModel);
            if (chargeFrameController == null)
                chargeFrameController = new ChargeFrameController(playerModel.frameModel);
            if (crossEnemyController == null)
                crossEnemyController = GetComponentInChildren<PlayerCrossEnemyController>();

            if (stateMachine == null)
                stateMachine = new PlayerStateMachine(16,
                new PlayerStateMachine.StateMachineInput
                {
                    inputBuffer = InputManager.Instance.PlayerGameInputBuffer,
                    lockController = lockController,
                },
                new PlayerStateMachine.StateMachineOutput
                {
                    animationController = animationController,
                    motionController = motionController,
                    frameController = frameController,
                    chargeFrameController = chargeFrameController,
                    vfxController = vfxController,
                    interactComponent = interactComponent,
                    timeFreezeController = timeFreezeController,
                    crossEnemyController = crossEnemyController,
                }
                
                );
            stateMachine.InitStateMachine(nameof(PlayerIdleState));

            interactComponent.SetStateMachine(stateMachine);//交互模块还需要通知stateMachine受伤状态。顿帧效果，动画减速等。
        }
        [SerializeField] Vector2 debugVelocity;
        private void Update()
        {
            float worldDeltaTime = GameWorld.Instance.TimeScale * GameWorld.Instance.UnscaledDeltaTime;
            //Debug.Log("TimeScale" + GameWorld.Instance.TimeScale + "UnscaledDeltaTime" + GameWorld.Instance.UnscaledDeltaTime);
           
            motionController.OnUpdate(worldDeltaTime);
            stateMachine.UpdateStateMachine(worldDeltaTime);
            debugVelocity = motionController.Velocity * GameWorld.Instance.TimeScale;

            playerInfo.OnUpdate(worldDeltaTime);
        }
        private void FixedUpdate()
        {
            motionController.OnFixedUpdate(Time.fixedDeltaTime);
            motionController.UpdateVelocity(GameWorld.Instance.TimeScale);
        }

    }
}