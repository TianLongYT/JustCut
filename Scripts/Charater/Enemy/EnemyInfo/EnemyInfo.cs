using iysy.JustCut.Evt;
using MzUtility.FrameWork.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 
    /// </summary>
    public class EnemyInfo
    {
        private float curHP;
        public float CurHP
        {
            get => curHP;
            private set
            {
                if(value < 0)
                {
                    curHP = 0;
                    EventManager.Trigger(new EvtOnEnemyDamage { Ratio = curHP / model.MaxHP });//调用敌人UI
                    EventManager.Trigger("OnEnemyDeath");//调用结果Panel
// #if UNITY_EDITOR
//                     UnityEditor.EditorApplication.isPaused = true;
// #endif
                    return;
                }
                var deltaHp = curHP - value;
                if (deltaHp > 0)
                {
                    CurPosture -= deltaHp * model.Hp2Posture;
                }
                curHP = value;//����UI

                EventManager.Trigger(new EvtOnEnemyDamage { Ratio = curHP / model.MaxHP });//调用敌人UI
            }
        }
        private float curPosture;
        public float CurPosture
        {
            get => curPosture;
            private set
            {
                if (value < 0)
                {
                    curPosture = 0;
                    if(inCollapse == false)
                    {
                        EventManager.Trigger(new EvtOnEnemyPostureChanged { Ratio = curPosture / model.MaxPosture });
                        EventCenter.EventCenter.Trigger(new Evt.EvtParamOnEnemyCollapse());
                        inCollapse = true;
                    }
                    return;
                }
                curPosture = value;//����UI
                EventManager.Trigger(new EvtOnEnemyPostureChanged { Ratio = curPosture / model.MaxPosture });
            }
        }
        bool bossAngery;
        bool inCollapse;
        EnemyInfoModel model;
        public EnemyInfo(EnemyInfoModel infoModel)
        {
            model = infoModel;
            curHP = infoModel.MaxHP;
            curPosture = infoModel.MaxPosture;

            EventCenter.EventCenter.Register<Evt.EvtParamOnBossAngery>(SetBossAngery);
            EventCenter.EventCenter.Register<Evt.EvtParamOnBossCoolDown>(SetBossCoolDown);
            EventCenter.EventCenter.Register<Evt.EvtParamOnEnemyCollapseRecover>(SetBossCollapseRecover);
        }
        public void Despose()
        {
            EventCenter.EventCenter.UnRegister<Evt.EvtParamOnBossAngery>(SetBossAngery);
            EventCenter.EventCenter.UnRegister<Evt.EvtParamOnBossCoolDown>(SetBossCoolDown);
            EventCenter.EventCenter.UnRegister<Evt.EvtParamOnEnemyCollapseRecover>(SetBossCollapseRecover);
        }
        public void SetHp(float attackDamage)
        {
            CurHP -= inCollapse ? attackDamage * 1.25f : attackDamage;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaIntensity">��ҵĹ���ǿ��-���˵Ĺ���ǿ��</param>
        public void SetPosture(int deltaIntensity)
        {
            if (deltaIntensity < 0)
                return;
            int deltaIntensitySq = deltaIntensity * deltaIntensity;
            float deltaIntensitySqRate = 50.0f / (deltaIntensitySq + 50.0f);
            var deltaPosture = deltaIntensitySqRate * deltaIntensitySq * model.SqIntensity2Posture + model.BasePostureAdder;
            Debug.Log("deltaIntensityRate" + deltaIntensitySqRate + "deltaIntensitySQ" + deltaIntensitySq);
            deltaPosture = bossAngery ? deltaPosture * 1.5f: deltaPosture;
            var targetPosture = CurPosture - deltaPosture;

            CurPosture = targetPosture;
            
        }
        public void ResetPosture()
        {
            CurPosture = model.MaxPosture;
        }
        private void SetBossAngery(EvtParamOnBossAngery a) => bossAngery = true;
        private void SetBossCoolDown(EvtParamOnBossCoolDown a) => bossAngery = false;
        private void SetBossCollapseRecover(EvtParamOnEnemyCollapseRecover a) => inCollapse = false;

    }

    
}