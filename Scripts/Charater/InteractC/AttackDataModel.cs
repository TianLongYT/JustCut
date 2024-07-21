using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{
    [System.Serializable]
    /// <summary>
    /// 
    /// </summary>
    public class AttackDataModel
    {
        public MotionType motionType;

        [SerializeField]public int baseAttackIntensity;
        [SerializeField] int attackIntensityJustAdder;//完美攻击时采用的攻击数据。
        [SerializeField] int[] attackInetensityChargeAdder;//多段蓄力时采用的攻击数据
        public float baseDamage;

        private int finalIntensity;

        public bool Match(MotionType type) => motionType == type;
        public int SetFinalIntensity(bool just = false,int chargeCount = -1)
        {
            int finalIntensity = baseAttackIntensity;
            if (just)
                finalIntensity += attackIntensityJustAdder;
            if (chargeCount != -1)
                finalIntensity += attackInetensityChargeAdder[chargeCount];
            return finalIntensity;
        }
        public int GetFinalIntensity()
        {
            return finalIntensity == 0 ? baseAttackIntensity : finalIntensity;
        }
    }
}