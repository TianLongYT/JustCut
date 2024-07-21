using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

namespace iysy.JustCut
{

    /// <summary>
    /// 负责播放动画，转向。
    /// </summary>
    public class EnemyAnimatorController : MonoBehaviour
    {
        Animator anim;
        [SerializeField] EnemyModel enemyModel;
        EnemyAnimationData animData;

        private void Start()
        {
            anim = GetComponentInChildren<Animator>();
            animData = enemyModel.animationData;
        }
        private void Update()
        {
            anim.speed = GameWorld.Instance.TimeScaleWithoutSkip;
        }
        public void Flip(float newFaceDir)
        {
            if (newFaceDir > 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        /*动画列表
         *移动动画，CrouchRun,StretchRun
         *一般动画，Idle，Dodge，Intro
         *攻击动画
         *LightStartUp LightN,LightP,LightP2H
         *HeavyStartUp HeavyN,HeavyP,HeavyN2L
         *HeavyImpact
         *
         
         */
        /// <summary>
        /// 开启动画，自然入场！
        /// </summary>
        public void Enter()
        {
            Intro();
            anim.SetFloat(animData.Enter, 1);
        }
        public void StretchRunSlow()
        {
            anim.SetTrigger(animData.StretchRunSlow);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rate">传入一个-1~1的值我会帮你设置成正确的动画</param>
        public void SetStretchRunSlowRate(float rate)
        {
            float finalMoveRate = rate < 0 ? rate * 3.6f : rate * 5.0f;
            anim.SetFloat(animData.StretchRunSlowRate, finalMoveRate);
        }
        public void CrouchRunFast()
        {
            anim.SetTrigger(animData.CrouchRunFast);
        }
        public void CrouchRunFastRate(float rate)
        {
            float finalMoveRate = rate < 0 ? rate * 3.6f : rate * 5.0f;
            anim.SetFloat(animData.CrouchRunFastRate, finalMoveRate);
        }
        public void CrouchRunSlow()
        {
            anim.SetTrigger(animData.CrouchRunSlow);
        }
        public void CrouchRunSlowRate(float rate)
        {
            float finalMoveRate = rate < 0 ? rate * 3.6f : rate * 5.0f;
            anim.SetFloat(animData.CrouchRunSlowRate, finalMoveRate);
        }
        public void Idle()
        {
            anim.SetTrigger(animData.Idle);
        }
        public void Dodge()
        {
            anim.SetTrigger(animData.Dodge);
        }
        public void Intro()
        {
            anim.SetTrigger(animData.Intro);
        }
        public void PlayLightAttackStartUp()
        {
            anim.SetTrigger(animData.LightAttackStartUp);
        }
        public void PlayNormalLightAttack()
        {
            anim.SetTrigger(animData.LightAttackNormal);
        }
        public void PlayPowerfulLightAttack()
        {
            anim.SetTrigger(animData.LightAttackPowerful);
        }
        public void PlayLightAttackP2H()
        {
            anim.SetTrigger(animData.LigthAttackP2H);
        }

        public void PlayHeavyAttackStartUp()
        {
            anim.SetTrigger(animData.HeavyAttackStartUp);
        }
        /// <summary>
        /// 值得注意的是，普通重三连击可以转轻攻击追加
        /// </summary>
        public void PlayNormalHeavyAttack()
        {
            anim.SetTrigger(animData.HeavyAttackNormal);
        }
        public void PlayPowerfulHeavyAttack()
        {
            anim.SetTrigger(animData.HeavyAttackPowerful);
        }
        public void PlayHeavyAttackN2L()
        {
            anim.SetTrigger(animData.HeavyAttackN2L);
        }
        public void PlayHeavyAttackImpact()
        {
            anim.SetTrigger(animData.StartUpAttackInpact);
        }

        public void Dizzily()
        {
            anim.SetTrigger(animData.Dizzily);
        }
    }
}