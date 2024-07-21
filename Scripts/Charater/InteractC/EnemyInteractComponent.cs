using iysy.JustCut.Evt;
using MzUtility.FrameWork.EventSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 敌人的交互模块
    /// </summary>
    public class EnemyInteractComponent : MonoBehaviour
    {
        [SerializeField] EnemyModel enemyModel;
        EnemyAttackModel attackModel;
        [SerializeField] bool disableHitBoxOnStart = true;
        [System.Serializable]
        public struct EnemyAttackHitPass
        {
            public MoveMotionType attackMotionType;
            public EnemyAttackHitBox hitBox;
        }
        [SerializeField] List<EnemyAttackHitPass> attackPasses;
        Dictionary<MoveMotionType, EnemyAttackHitBox> type2HitBox;

        EnemyController enemyController;
        private void Start()
        {
            type2HitBox = new Dictionary<MoveMotionType, EnemyAttackHitBox>(attackPasses.Count);
            attackModel = enemyModel.attackModel;
            foreach (var pass in attackPasses)
            {
                var motionType = pass.attackMotionType;
                var hitBox = pass.hitBox;
                hitBox.Initialize(attackModel.GetMotionKeyFrameData(motionType),this);
                if (disableHitBoxOnStart)
                    hitBox.EnableHitBox(false);
                type2HitBox.Add(motionType, hitBox);
            }

            if (enemyController == null)
                enemyController = GetComponentInParent<EnemyController>();

            EventCenter.EventCenter.Register<Evt.EvtParamOnEnemyCollapse>(OnEnemyCollapse);
            
        }
        private void OnDestroy()
        {
            EventCenter.EventCenter.UnRegister<Evt.EvtParamOnEnemyCollapse>(OnEnemyCollapse);
        }
        private void OnEnemyCollapse(EvtParamOnEnemyCollapse obj)
        {
            DisableCurHitBox();
        }

        public void Hit(AttackData attackData)
        {
            if (attackData.hitBody)
            {
                enemyController.OnHit(OnHitBody,attackData);
            }
            else
            {
                enemyController.OnHit(OnGPHit, attackData);
                //type2HitBox[curMotion].EnableHitBox(false);
            }
        }



        private void OnHitBody( EnemyInfo enemyInfo,AttackData attackData)
        {
            Debug.Log("击中敌人" + "造成伤害" + attackData.attackModel.baseDamage);
            enemyInfo.SetHp(attackData.attackModel.baseDamage);
            //enemyInfo.CurHP -= attackData.attackModel.baseDamage;
        }
        private void OnGPHit(EnemyInfo enemyInfo, AttackData attackData)
        {
            Debug.Log("拼刀" + "攻击强度" + attackData.attackModel.GetFinalIntensity());
            enemyInfo.SetPosture(attackData.attackModel.GetFinalIntensity() - attackModel.GetMotionKeyFrameData(curMotion).GetFinalIntensity());
        }

        MoveMotionType curMotion;
        /// <summary>
        /// 单纯设置当前是否激活碰撞框。
        /// </summary>
        /// <param name="motionType"></param>
        public void EnableCurMotionHitBox(bool enable)
        {
            type2HitBox[curMotion].EnableHitBox(enable);
        }
        public void ChangeMotionHitBox(MoveMotionType motionType)
        {
            curMotion = motionType;
            type2HitBox[curMotion].EnableHitBox(true);
        }
        public void SetAllHitBoxDisable()
        {
            foreach (var item in type2HitBox)
            {
                item.Value.EnableHitBox(false);
            }
        }
        public void DisableCurHitBox()
        {
            if(type2HitBox.ContainsKey(curMotion))
                type2HitBox[curMotion].EnableHitBox(false);
        }
        public void OnHitPlayer()
        {

        }
        public void OnPlayerGp()
        {

        }
    }
}