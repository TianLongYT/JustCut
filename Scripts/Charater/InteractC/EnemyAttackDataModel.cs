using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace iysy.JustCut
{
    [System.Serializable]
    /// <summary>
    /// 
    /// </summary>
    public class EnemyAttackDataModel
    {
        public MoveMotionType motionType;

        [SerializeField] int baseAttackIntensity;
        public float baseDamage;

        private int finalIntensity;

        public bool Match(MoveMotionType type) => motionType == type;
        public int GetFinalIntensity()
        {
            return finalIntensity == 0 ? baseAttackIntensity : finalIntensity;
        }
        [NonSerialized]
        AttackDataModel m_dataModel;
        public AttackDataModel ToAttackDataModel()
        {
            if(m_dataModel == default)
            {
                m_dataModel = new AttackDataModel() { baseDamage = baseDamage, motionType = MotionType.Roll , baseAttackIntensity = baseAttackIntensity};
                //Debug.Log("初始化敌人攻击参数,敌人攻击强度："+baseAttackIntensity);
            }
            return m_dataModel;
        }
    }
}