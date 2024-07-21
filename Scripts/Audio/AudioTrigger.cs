using iysy.JustCut.Evt;
using MzUtility.FrameWork.EventSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// ������Ƶ�����¼���������Ƶ
    /// </summary>
    public class AudioTrigger : MonoBehaviour
    {
        [SerializeField] float hitVolume = 0.5f;
        [SerializeField] float hitPitch = 0.5f;
        [SerializeField] float hitPitchRate = 0.2f;
        [SerializeField] float hitVolumRate = 1f;

        [SerializeField] float gpVolume = 0.9f;
        [SerializeField] float gpPitch = 0.2f;
        [SerializeField] float gpPitchRate = 0.2f;
        [SerializeField] float gpVolumRate = 1f;

        [SerializeField] float waveVolume = 0.3f;
        [SerializeField] float wavePitch = 0.5f;
        [SerializeField] float wavePitchRate = 0.2f;
        [SerializeField] float waveVolumRate = 1f;

        AudioManager audioManager;
        private void OnDestroy()
        {
            if (audioManager == null)
                return;
            EventCenter.EventCenter.UnRegister<Evt.EvtParamChangeSceneInfo>(OnChangeScene);
            EventCenter.EventCenter.UnRegister<Evt.EvtParamOnBattlePause>(OnBattlePause);
            EventCenter.EventCenter.UnRegister<Evt.EvtParamOnBattleContinue>(OnBattleContinue);
            EventCenter.EventCenter.UnRegister<Evt.EvtParamGpImpactInfo>(OnGp);
            EventCenter.EventCenter.UnRegister<Evt.EvtParamHitImpactInfo>(OnHit);
            EventCenter.EventCenter.UnRegister<Evt.EvtParamOnAttackActive>(OnWave);
        }
        private void Awake()
        {
            audioManager = GetComponent<AudioManager>();
            if (audioManager == null)
            {
                Debug.Log("δ��������Ϸ�������ҵ�AudioManager����������õ���ģʽ��ȡ");
                audioManager = AudioManager.Instance;
            }
            if (audioManager == null)
            {
                Debug.LogWarning("�����в�����AudioManager�����AudioTriggerʧЧ");
                return;
            }
            EventCenter.EventCenter.Register<Evt.EvtParamChangeSceneInfo>(OnChangeScene);
            EventCenter.EventCenter.Register<Evt.EvtParamOnBattlePause>(OnBattlePause);
            EventCenter.EventCenter.Register<Evt.EvtParamOnBattleContinue>(OnBattleContinue);
            EventCenter.EventCenter.Register<Evt.EvtParamGpImpactInfo>(OnGp);
            EventCenter.EventCenter.Register<Evt.EvtParamHitImpactInfo>(OnHit);
            EventCenter.EventCenter.Register<Evt.EvtParamOnAttackActive>(OnWave);
        }



        private void OnChangeScene(EvtParamChangeSceneInfo obj)
        {
            switch (obj.sceneIndex)
            {
                case 0:
                    //������,��Ϸ��Ƶ���ɡ�
                    audioManager.AudioSourceFadeIn(audioManager.EntranceBGM, 0.3f);
                    audioManager.AudioSourceFadeOut(audioManager.FightingBGMHighpass, 0.3f);
                    audioManager.AudioSourceFadeOut(audioManager.FightingBGMLowpass, 0.3f);
                    break;
                case 1:
                    //ս�����棬��Ϸ��Ƶ���ɡ�
                    audioManager.AudioSourceFadeOut(audioManager.EntranceBGM, 0.3f);
                    audioManager.AudioSourceFadeIn(audioManager.FightingBGMHighpass, 0.3f);
                    audioManager.AudioSourceFadeIn(audioManager.FightingBGMLowpass, 0.3f);
                    break;


            }
        }
        private void OnBattlePause(EvtParamOnBattlePause obj)
        {
            audioManager.AudioSourceFadeOut(audioManager.FightingBGMHighpass, 0.3f);
        }
        private void OnBattleContinue(EvtParamOnBattleContinue obj)
        {
            audioManager.AudioSourceFadeIn(audioManager.FightingBGMHighpass, 0.3f);
        }

        private void OnHit(EvtParamHitImpactInfo obj)
        {
            audioManager.InteractOnPlay(Mathf.RoundToInt(obj.intensity), hitPitchRate, hitVolumRate, 2,hitVolume,hitPitch);
            //AudioManager.Instance.InteractOnPlay(2);
        }

        private void OnGp(EvtParamGpImpactInfo obj)
        {
            audioManager.InteractOnPlay(Mathf.RoundToInt(obj.intensity), gpPitchRate, gpVolumRate, 0,gpVolume,gpPitch);
           // Debug.Log("����ǿ��"+Mathf.RoundToInt(obj.intensity));
        }

        private void OnWave(EvtParamOnAttackActive obj)
        {
            //AudioManager.Instance.ActionOnPlay(obj.attackIntensity, wavePitchRate, waveVolumRate, 1,waveVolume);
        }
    }
}