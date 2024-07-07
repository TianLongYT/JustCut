using System.Collections;
using System.Collections.Generic;
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
        AttackDataModel m_dataModel;
        public AttackDataModel ToAttackDataModel()
        {
            if(m_dataModel == null)
            {
                m_dataModel = new AttackDataModel() { baseDamage = baseDamage, motionType = MotionType.Roll , baseAttackIntensity = baseAttackIntensity};
            }
            return m_dataModel;
        }
    }
}