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
        [SerializeField] bool disableHitBoxOnStart = true;
        [System.Serializable]
        public struct EnemyAttackHitPass
        {
            public MoveMotionType attackMotionType;
            public EnemyAttackHitBox hitBox;
        }
        [SerializeField] List<EnemyAttackHitPass> attackPasses;
        Dictionary<MoveMotionType, EnemyAttackHitBox> type2HitBox;
        private void Start()
        {
            type2HitBox = new Dictionary<MoveMotionType, EnemyAttackHitBox>(attackPasses.Count);
            foreach (var pass in attackPasses)
            {
                var motionType = pass.attackMotionType;
                var hitBox = pass.hitBox;
                hitBox.Initialize(enemyModel.attackModel.GetMotionKeyFrameData(motionType),this);
                if (disableHitBoxOnStart)
                    hitBox.EnableHitBox(false);
                type2HitBox.Add(motionType, hitBox);
            }
        }
        public void Hit(AttackData attackData)
        {
            if (attackData.hitBody)
                Debug.Log("击中敌人" + "造成伤害" + attackData.attackModel.baseDamage);
            else
            {
                Debug.Log("拼刀" + "攻击强度" + attackData.attackModel.GetFinalIntensity());
                //type2HitBox[curMotion].EnableHitBox(false);
            }
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
        public void OnHitPlayer()
        {

        }
        public void OnPlayerGp()
        {

        }
    }
}