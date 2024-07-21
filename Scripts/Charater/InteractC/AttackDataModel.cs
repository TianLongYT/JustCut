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
        [SerializeField] int attackIntensityJustAdder;//��������ʱ���õĹ������ݡ�
        [SerializeField] int[] attackInetensityChargeAdder;//�������ʱ���õĹ�������
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