using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using iysy.MMath;
using Sirenix.Utilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


namespace iysy.JustCut
{
    public class AudioManager : SingletonBehaviourNoNewGO<AudioManager>
    {
        [SerializeField] AudioSource InteractSoundEffect;//������Ч������GP��Ч��������Ч��
        [SerializeField] AudioSource ActionSoundEffect;//������Ч������ӵ���Ч��
        [HideInInspector]public AudioSource FightingBGMLowpass;//ս������BGM��ͨ��Դ
        [HideInInspector]public AudioSource FightingBGMHighpass;//ս������BGM��ͨ��Դ
        [HideInInspector]public AudioSource EntranceBGM;//������BGM
        [SerializeField] float defaultPitch = 1f;
        [SerializeField] float defaultVolume = 0.5f;
        [SerializeField] float pitchOffsetMax,pitchOffsetMin;//�ɵ����������仯��Χ
        [SerializeField] float volumeOffsetMax,volumeOffsetMin;//�ɵ����������仯��Χ
        [SerializeField] float attackStrengthPitchParam;//����ǿ����������
        [SerializeField] float attackStrengthVolumeParam;//����ǿ�����߲���
        AudioMixerGroup ActionSoundEffectGroup;//����
        AudioMixerGroup InteractSoundEffectGroup;//����
        [SerializeField] AudioMixerGroup FightingBGMLowpassGroup;//ս��BGM��ͨ����
        [SerializeField] AudioMixerGroup FightingBGMHighpassGroup;//ս��BGM��ͨ����
        [SerializeField] AudioMixerGroup EntranceBGMGroup;//������BGM����
        AudioSource[] ActionAudioSource = new AudioSource[10];//������Ч��Դ���飨�о�ʵ���ò�����ô�ࣩ
        AudioSource[] InteractAudioSource = new AudioSource[10];//������Ч��Դ����
        int actionSourceCount = 1;
        int interactSourceCount = 1;
        [SerializeField] AudioMixer audioMixer;

        /// <summary>
        /// 0.GP��Ч
        /// 1.�ջ���Ч
        /// 2.������Ч
        /// </summary>
        //[SerializeField] AudioClip[] SoundEffects;//��Ч�ز����飬�����洢��Ч����
        [SerializeField] SoundEffectsModel SoundEffectsModel;

        //��ť���ò���
        [SerializeField] Button HASSEButton;
        [SerializeField] Button Button2;

        //����
        public override void InitInstanceBeforeAcquire(){
            base.InitInstanceBeforeAcquire();
            InteractAudioSource[0] = InteractSoundEffect;
            ActionAudioSource[0] = ActionSoundEffect;

            //BGM��Դ��ʼ��
            FightingBGMHighpass = this.gameObject.AddComponent<AudioSource>();
            FightingBGMHighpass.loop = true;
            FightingBGMLowpass = this.gameObject.AddComponent<AudioSource>();
            FightingBGMLowpass.loop = true;
            EntranceBGM = this.gameObject.AddComponent<AudioSource>();
            EntranceBGM.loop = true;

            EntranceBGM.clip = SoundEffectsModel.BGMs[0];//��û�ӣ�����������
            FightingBGMHighpass.clip = SoundEffectsModel.BGMs[1];
            FightingBGMLowpass.clip = SoundEffectsModel.BGMs[1];

            FightingBGMHighpass.outputAudioMixerGroup = FightingBGMHighpassGroup;
            FightingBGMLowpass.outputAudioMixerGroup = FightingBGMLowpassGroup;
            EntranceBGM.outputAudioMixerGroup = EntranceBGMGroup;


            ActionSoundEffectGroup = ActionSoundEffect.outputAudioMixerGroup;
            InteractSoundEffectGroup = InteractSoundEffect.outputAudioMixerGroup;
            //���÷�ʽ����ʱ������д
            audioPass = new Dictionary<AudioSource, Tween>();
        }
        //��������������Ϊ�����÷���
        private void OnPlay()
        {
            float pitchOffset = UnityEngine.Random.Range(pitchOffsetMin,pitchOffsetMax);
            float volumeOffset = UnityEngine.Random.Range(volumeOffsetMin,volumeOffsetMax);
            AudioSource audioSource = InteractSourceVecant();
            audioSource.volume = volumeOffset;
            audioSource.pitch = pitchOffset;
            audioSource.PlayOneShot(SoundEffectsModel.soundEffects[1]);
        }//Test
        private void OnPlay2()
        {
            InteractSoundEffect.clip = SoundEffectsModel.soundEffects[1];
            float pitchOffset = UnityEngine.Random.Range(pitchOffsetMin,pitchOffsetMax);
            float volumeOffset = UnityEngine.Random.Range(volumeOffsetMin,volumeOffsetMax);
            audioMixer.SetFloat("InteractPitch",pitchOffset + attackStrengthPitchParam);
            audioMixer.SetFloat("InteractVolume",volumeOffset + attackStrengthVolumeParam);
            InteractSoundEffect.Play();
        }//TestFinish

        /// <summary>
        /// ���ĿǰActionAudioSource�Ƿ��п��У����򲥷ţ������½�
        /// </summary>
        /// <returns></returns>
        public AudioSource ActionSourceVecant()
        {
            for(int i = 0;i<actionSourceCount;i++)
            {
                if(!ActionAudioSource[i].isPlaying)
                {
                    return ActionAudioSource[i];
                }
            }
            ActionAudioSource[actionSourceCount] = this.gameObject.AddComponent<AudioSource>();
            ActionAudioSource[actionSourceCount].outputAudioMixerGroup = ActionSoundEffectGroup;
            actionSourceCount++;
            return ActionAudioSource[actionSourceCount-1];
        }
        /// <summary>
        /// ���ĿǰInteractAudioSource�Ƿ��п��У����򲥷ţ������½�
        /// </summary>
        /// <returns></returns>
        public AudioSource InteractSourceVecant()
        {
            for(int i = 0;i<interactSourceCount;i++)
            {
                if(!InteractAudioSource[i].isPlaying)
                {
                    return InteractAudioSource[i];
                }
            }
            InteractAudioSource[interactSourceCount] = this.gameObject.AddComponent<AudioSource>();
            InteractAudioSource[interactSourceCount].outputAudioMixerGroup = InteractSoundEffectGroup;
            interactSourceCount++;
            return InteractAudioSource[interactSourceCount-1];
        }

        /// <summary>
        /// ��������ʱ���ã�����GP������ȣ���ͬʱҲ���Դ��빥��ǿ�Ȳ���(��Ч��ż�SoundEffects)
        /// /����///(�ṥ��ǿ�Ȳ���Ϊ1���ع���2������3)
        /// /ѡ����Ƶ��
        /// </summary>
        /// <param name="index"></param>
        public void InteractOnPlay(int index)
        {
            InteractOnPlay(0, 0, 0, index);
        }
        /// <summary>
        /// ��������ʱ���ã�����GP������ȣ���ͬʱҲ���Դ��빥��ǿ�Ȳ���(��Ч��ż�SoundEffects)
        /// (�ṥ��ǿ�Ȳ���Ϊ1���ع���2������3)
        /// </summary>
        /// <param name="index"></param>
        public void InteractOnPlay(int levelNum,float pitchrate,float volumerate,int index)
        {
            float pitchOffset = UnityEngine.Random.Range(pitchOffsetMin,pitchOffsetMax);
            float volumeOffset = UnityEngine.Random.Range(volumeOffsetMin,volumeOffsetMax);
            AudioSource audioSource = InteractSourceVecant();
            var initVolume = defaultVolume;
            var initPitch = defaultPitch;
            audioSource.pitch = initPitch + pitchOffset + levelNum * pitchrate;
            audioSource.volume = initVolume + volumeOffset + levelNum * volumerate;
            audioSource.PlayOneShot(SoundEffectsModel.soundEffects[index]);
        }
        /// <summary>
        /// ��������ʱ���ã�����GP������ȣ���ͬʱҲ���Դ��빥��ǿ�Ȳ���(��Ч��ż�SoundEffects)
        /// (�ṥ��ǿ�Ȳ���Ϊ1���ع���2������3)
        /// </summary>
        /// <param name="index"></param>
        public void InteractOnPlay(int levelNum, float pitchrate, float volumerate, int index,float initVolume,float initPitch)
        {
            float pitchOffset = UnityEngine.Random.Range(pitchOffsetMin, pitchOffsetMax);
            float volumeOffset = UnityEngine.Random.Range(volumeOffsetMin, volumeOffsetMax);
            AudioSource audioSource = InteractSourceVecant();
            audioSource.pitch = initPitch + pitchOffset + levelNum * pitchrate;
            audioSource.volume = initVolume + volumeOffset + levelNum * volumerate;
            Debug.Log("ƴ��pitch" + audioSource.pitch + "ƴ��Voluem" + audioSource.volume);
            audioSource.PlayOneShot(SoundEffectsModel.soundEffects[index]);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public void ActionOnPlay(int index)
        {
            ActionOnPlay(0, 0, 0, index);
        }
        /// <summary>
        /// ���趯��ʱ����(�����ṥ���ջ�)ͬʱ���빥��ǿ�Ȳ���
        ///�� �ṥ��ǿ�Ȳ���Ϊ1���ع���2������3��
        /// </summary>
        /// <param name="levelNum"></param>
        public void ActionOnPlay(int levelNum,float pitchrate,float volumerate,int index)
        {
            float pitchOffset = UnityEngine.Random.Range(pitchOffsetMin,pitchOffsetMax);
            float volumeOffset = UnityEngine.Random.Range(volumeOffsetMin,volumeOffsetMax);
            //�����������ֵ�����ıȽϱ���
            AudioSource audioSource = ActionSourceVecant();
            var initVolume = defaultVolume;
            var initPitch = defaultPitch;
            audioSource.pitch = initPitch+ pitchOffset + levelNum * pitchrate;
            audioSource.volume = initVolume + volumeOffset + levelNum * volumerate;
            audioSource.PlayOneShot(SoundEffectsModel.soundEffects[index]);
        }
        /// <summary>
        /// ���趯��ʱ����(�����ṥ���ջ�)ͬʱ���빥��ǿ�Ȳ���
        ///�� �ṥ��ǿ�Ȳ���Ϊ1���ع���2������3��
        /// </summary>
        /// <param name="levelNum"></param>
        public void ActionOnPlay(int levelNum, float pitchrate, float volumerate, int index,float initVolume,float initPitch)
        {
            float pitchOffset = UnityEngine.Random.Range(pitchOffsetMin, pitchOffsetMax);
            float volumeOffset = UnityEngine.Random.Range(volumeOffsetMin, volumeOffsetMax);
            //�����������ֵ�����ıȽϱ���
            AudioSource audioSource = ActionSourceVecant();
            audioSource.pitch = initPitch+ pitchOffset + levelNum * pitchrate;
            audioSource.volume = initVolume + volumeOffset + levelNum * volumerate;
            audioSource.PlayOneShot(SoundEffectsModel.soundEffects[index]);
        }
        Dictionary<AudioSource, Tween> audioPass;

        /// <summary>
        /// ��ʼ���ţ�����Audio source�ĵ���Ч����������Դ�͵���ʱ��,���β�����Ƶʱ����������Ƶ��ʼֵΪ0.
        /// </summary>
        /// <param name="audioSource"></param>
        public void AudioSourceFadeIn(AudioSource audioSource,float fadeInTime)
        {
            bool containsSource = audioPass.ContainsKey(audioSource);
            if (containsSource)
            {
                if (audioPass[audioSource].IsActive())
                {
                    audioPass[audioSource].Kill();
                    Debug.Log("ɱ����fadeouttween"+"clip"+audioSource.clip.ToSafeString());
                }
            }
            else
            {
                audioSource.volume = 0f;
            }
            if(audioSource.isPlaying == false)
            {
                audioSource.Play();
            }
            Debug.Log("����");
            float volumeValue = audioSource.volume;
            Tween tween = DOTween.To(
                () => volumeValue,
                (value) =>
                {
                    volumeValue = value;
                    audioSource.volume = volumeValue;
                }
                , 1.0f, fadeInTime);
            if (!containsSource)
            {
                audioPass.Add(audioSource, tween);
            }
            else
            {
                audioPass[audioSource] = tween;
            }
        }
        /// <summary>
        /// ֹͣ���ţ�����Audio source�ĵ���Ч����������Դ�͵���ʱ��
        /// </summary>
        /// <param name="audioSource"></param>
        public void AudioSourceFadeOut(AudioSource audioSource,float fadeOutTime)
        {
            bool containsSource = audioPass.ContainsKey(audioSource);
            if (containsSource)
            {
                if (audioPass[audioSource].IsActive())
                {
                    audioPass[audioSource].Kill();
                    Debug.Log("ɱ����fadeintween" + "clip" + audioSource.clip.ToSafeString());

                }
            }
            Debug.Log("����");
            float volumeValue = audioSource.volume;
            Tween tween = DOTween.To(
                () => volumeValue,
                (value) =>
                {
                    volumeValue = value;
                    audioSource.volume = volumeValue;
                }
                , 0.0f, fadeOutTime);
            if (!containsSource)
            {
                audioPass.Add(audioSource, tween);
            }
            else
            {
                audioPass[audioSource] = tween;
            }
        }
    }
}
