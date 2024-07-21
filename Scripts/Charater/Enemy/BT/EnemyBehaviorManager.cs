using BehaviorDesigner.Runtime;
using Cysharp.Threading.Tasks;
using iysy.JustCut.Evt;
using iysy.MotionController2D;
using MzUtility.FrameWork.EventSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 管理bt,需要行为树
    /// </summary>
    public class EnemyBehaviorManager : MonoBehaviour
    {
        BehaviorTree bt;
        [SerializeField] EnemyModel enemyModel;
        EnemyAnimatorController animatorController;
        EnemyInfo enemyInfo;
        MotionController2D.MotionController2D motionController;
        EnemyAttackMoveHComponent moveHComponent;
        PlayerLockController lockController;
        BehaviorManager bm;
        BehaviorManager BM
        {
            get
            {
                bm = BehaviorManager.instance;
                if (bm == null)
                {
                    bm = FindObjectOfType<BehaviorManager>();
                }
                return bm;
            }
        }
        bool bTNeedManualUpdate;
        [SerializeField] bool DriveBTByFrameUpdate = true;

        private void Awake()
        {
            bt = GetComponent<BehaviorTree>();
            
            //bt.enabled = false;
            //bt.EnableBehavior();
            EventCenter.EventCenter.Register<Evt.EvtParamOnEnemyCollapse>(OnEnemyCollapse);
        }
        private void Start()
        {
            //bt.enabled = true;
            bt.EnableBehavior();
            //EnableTree();
            BM  .UpdateInterval = UpdateIntervalType.Manual;
            if(DriveBTByFrameUpdate)
                GameWorld.Instance.OnFrameTick += UpdateBehaviorTree;

        }
        
        private void UpdateBehaviorTree(float obj)
        {
            ForceUpdateBehaviorTree();
        }

        private async void EnableTree()
        {
            await UniTask.Delay(100);
            bt.enabled = false;
            await UniTask.Delay(100);
            bt.enabled = true;
            bt.EnableBehavior();
        }
        private void OnDestroy()
        {
            EventCenter.EventCenter.UnRegister<Evt.EvtParamOnEnemyCollapse>(OnEnemyCollapse);
        }
        public void Initialize(EnemyInfo enemyInfo,MotionController2D.MotionController2D motionController, EnemyAnimatorController animatorController,PlayerLockController lockController)
        {
            this.enemyInfo = enemyInfo;
            this.motionController = motionController;
            this.animatorController = animatorController;
            this.lockController = lockController;
            moveHComponent = motionController.GetMComponent<EnemyAttackMoveHComponent>();

        }
        private void OnEnemyCollapse(EvtParamOnEnemyCollapse obj)
        {
            animatorController.Dizzily();
            
            bt.DisableBehavior();
            animatorController.Flip(lockController.GetPosRelationship());
            motionController.GetMHandler<PlayerDecelerateHandler>()?.Enable();
            EventCenter.EventCenter.Trigger(new Evt.EvtParamOnEnemyCollapseWithPos { enemyPosition = this.transform.position });
            Invoke(nameof(EnableBT), enemyModel.infoModel.CollapseTime);
        }
        private void EnableBT()
        {
            bt.EnableBehavior();
            enemyInfo.ResetPosture();
            EventCenter.EventCenter.Trigger(new Evt.EvtParamOnEnemyCollapseRecover());
        }
        private void Update()
        {
            float a= (float)bt?.GetVariable("AngeryTimer").GetValue();
            //Debug.Log(a);
        }
        public void ForceUpdateBehaviorTree()
        {
            BM.Tick();
            //Debug.Log("TickBT");
        }
    }
}