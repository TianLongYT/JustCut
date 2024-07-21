
using iysy.GameInput;
using MzUtility.FrameWork.EventSystem;
using MzUtility.FrameWork.UI;
using System;
using UnityEngine;

namespace iysy.JustCut
{

    /// <summary>
    /// 
    /// </summary>
    public class PlayerInfo
    {
        PlayerInfoModel model;
        private float curHP;
        public float CurHP
        {
            get
            {
                return curHP;
            }
            set
            {
                curHP = value;
                if(curHP <= 0)
                {
                    curHP = 0;
                    EventManager.Trigger(new EvtOnPlayerDamage { Ratio = curHP / model.MaxHP });
                    EventManager.Trigger("EvtOnPlayerDeath");
// #if UNITY_EDITOR
//                     UnityEditor.EditorApplication.isPaused = true;
// #endif

                    return;
                }
                EventManager.Trigger(new EvtOnPlayerDamage { Ratio = curHP/model.MaxHP});
                
            }
        }
        private float curPosture;
        public float CurPosture
        {
            get
            {
                return curPosture;
            }
            private set
            {
                curPosture = value;
                //Debug.Log("��ǰ������"+curPosture);
                EventManager.Trigger(new EvtOnPlayerPostureChanged { Ratio = curPosture / model.MaxPosture });
            }
        }
        InputBufferTimeVarType<PlayerInputType> inputBuffer;
        private float RollTimer;
        public PlayerInfo(PlayerInfoModel model)
        {
            this.model = model;
            curHP = model.MaxHP;
            curPosture = 0;
            RollTimer = model.RollInterval;
            inputBuffer = InputManager.Instance.PlayerGameInputBuffer;
            inputBuffer.UnLockInput(PlayerInputType.Roll, OnPlayerRoll);
        }

        private void OnPlayerRoll()
        {
            RollTimer = 0;
            inputBuffer.LockInput(PlayerInputType.Roll, null);
        }

        float postureTimer;
        /// <summary>
        /// ��һ��ʱ����Զ����ټ�ʻ�����Զ����������ܡ�
        /// </summary>
        public void OnUpdate(float deltaTime)
        {
            //����������
            RollTimer += deltaTime;
            if (RollTimer >= model.RollInterval)
            {
                inputBuffer.UnLockInput(PlayerInputType.Roll, OnPlayerRoll);
            }
            var radio = Mathf.Clamp01(RollTimer / model.RollInterval);
            //Debug.Log("��ǰ������" + radio);
            EventManager.Trigger(new EvtOnRollCD { Ratio = radio, isDone = radio == 1 });

            //�Զ�����Posture
            postureTimer += deltaTime;
            if (postureTimer >= model.PostureRecoverInterval)
            {
                if (CurPosture > 0)
                {
                    CurPosture -= deltaTime * model.PostureRecoverSpeed;
                }
                else
                {
                    CurPosture = 0;
                }
            }
        }
        public void SetPostrue(int deltaIntensity)
        {
            postureTimer = 0;
            if (deltaIntensity < 0)
                return;
            var targetPosture = CurPosture + deltaIntensity * deltaIntensity * model.SqIntensityToPosture + model.BasePostureAdder;
            var postureDelta = targetPosture - model.MaxPosture;
            
            if(postureDelta >= 0)
            {
                var reduceHP = (postureDelta - model.BasePostureAdder) * model.PostureToHp;
                if(reduceHP > 0)
                {
                    //����ƴ����Ѫ�¼���
                    EventCenter.EventCenter.Trigger(new Evt.EvtParamOnGpReduceHp { DeltaIntensity = deltaIntensity });
                    CurHP -= reduceHP;
                }

                CurPosture = model.MaxPosture;
            }
            else
            {
                CurPosture = targetPosture;
            }
        }
    }
}