using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 驱动一下下MotionController哈。给BT减少一点点负担。
    /// </summary>
    public class EnemyController : MonoBehaviour
    {
        MotionController2D.MotionController2D motionController;
        EnemyAttackMoveHComponent moveHComponent;
        PlayerLockController lockController;
        [SerializeField] EnemyModel enemyModel;
        EnemyInfo enemyInfo;
        EnemyBehaviorManager behaviorManager;
        private void Start()
        {
            motionController = GetComponent<MotionController2D.MotionController2D>();
            motionController.LoadComponents();
            motionController.OnReInitialize();
            moveHComponent = motionController.GetMComponent<EnemyAttackMoveHComponent>();
            lockController = GetComponentInChildren<PlayerLockController>();
            GameWorld.Instance.OnUpdate += OnUpdate;

            enemyInfo = new EnemyInfo(enemyModel.infoModel);
            behaviorManager = GetComponent<EnemyBehaviorManager>();
            behaviorManager.Initialize(enemyInfo, motionController, GetComponent<EnemyAnimatorController>(),lockController);
        }
        private void OnDestroy()
        {
            enemyInfo.Despose();
        }
        private void OnUpdate(float obj)
        {
            //Debug.Log("执行onUpdate");

                
            motionController.OnUpdate(obj);
            if (lockController && moveHComponent)
                moveHComponent.OnUpdate(GameWorld.Instance.DeltaTime * GameWorld.Instance.TimeScaleWithoutSkip, 0, lockController.GetFaceDir());
        }
        private void FixedUpdate()
        {
            var timescale = GameWorld.Instance.TimeScale;
            motionController.UpdateVelocity(timescale);
            motionController.OnFixedUpdate(Time.fixedDeltaTime);
        }
        public void OnHit(Action<EnemyInfo,AttackData> action,AttackData attackData)
        {
            if(attackData.hitBody)
                moveHComponent.OnEnter((int)MoveMotionType.Hurt, attackData.attackModel.GetFinalIntensity(), -(int)lockController.GetPosRelationship());
            else
                moveHComponent.OnEnter((int)MoveMotionType.Gp, attackData.attackModel.GetFinalIntensity(), -(int)lockController.GetPosRelationship());

            action?.Invoke(enemyInfo,attackData);
        }
    }
}