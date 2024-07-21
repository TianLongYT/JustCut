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
        [SerializeField] AudioSource InteractSoundEffect;//交互音效，例如GP音效、打肉音效等
        [SerializeField] AudioSource ActionSoundEffect;//动作音效，例如挥刀音效等
        [HideInInspector]public AudioSource FightingBGMLowpass;//战斗场景BGM低通音源
        [HideInInspector]public AudioSource FightingBGMHighpass;//战斗场景BGM高通音源
        [HideInInspector]public AudioSource EntranceBGM;//主界面BGM
        [SerializeField] float defaultPitch = 1f;
        [SerializeField] float defaultVolume = 0.5f;
        [SerializeField] float pitchOffsetMax,pitchOffsetMin;//可调整的音调变化范围
        [SerializeField] float volumeOffsetMax,volumeOffsetMin;//可调整的音量变化范围
        [SerializeField] float attackStrengthPitchParam;//攻击强度音调参数
        [SerializeField] float attackStrengthVolumeParam;//攻击强度音高参数
        AudioMixerGroup ActionSoundEffectGroup;//音道
        AudioMixerGroup InteractSoundEffectGroup;//音道
        [SerializeField] AudioMixerGroup FightingBGMLowpassGroup;//战斗BGM低通音道
        [SerializeField] AudioMixerGroup FightingBGMHighpassGroup;//战斗BGM高通音道
        [SerializeField] AudioMixerGroup EntranceBGMGroup;//主界面BGM音道
        AudioSource[] ActionAudioSource = new AudioSource[10];//动作音效音源数组（感觉实际用不到这么多）
        AudioSource[] InteractAudioSource = new AudioSource[10];//交互音效音源数组
        int actionSourceCount = 1;
        int interactSourceCount = 1;
        [SerializeField] AudioMixer audioMixer;

        /// <summary>
        /// 0.GP音效
        /// 1.空挥音效
        /// 2.打肉音效
        /// </summary>
        //[SerializeField] AudioClip[] SoundEffects;//音效素材数组，用来存储音效引用
        [SerializeField] SoundEffectsModel SoundEffectsModel;

        //按钮调用测试
        [SerializeField] Button HASSEButton;
        [SerializeField] Button Button2;

        //唤醒
        public override void InitInstanceBeforeAcquire(){
            base.InitInstanceBeforeAcquire();
            InteractAudioSource[0] = InteractSoundEffect;
            ActionAudioSource[0] = ActionSoundEffect;

            //BGM音源初始化
            FightingBGMHighpass = this.gameObject.AddComponent<AudioSource>();
            FightingBGMHighpass.loop = true;
            FightingBGMLowpass = this.gameObject.AddComponent<AudioSource>();
            FightingBGMLowpass.loop = true;
            EntranceBGM = this.gameObject.AddComponent<AudioSource>();
            EntranceBGM.loop = true;

            EntranceBGM.clip = SoundEffectsModel.BGMs[0];//还没加，加了再启用
            FightingBGMHighpass.clip = SoundEffectsModel.BGMs[1];
            FightingBGMLowpass.clip = SoundEffectsModel.BGMs[1];

            FightingBGMHighpass.outputAudioMixerGroup = FightingBGMHighpassGroup;
            FightingBGMLowpass.outputAudioMixerGroup = FightingBGMLowpassGroup;
            EntranceBGM.outputAudioMixerGroup = EntranceBGMGroup;


            ActionSoundEffectGroup = ActionSoundEffect.outputAudioMixerGroup;
            InteractSoundEffectGroup = InteractSoundEffect.outputAudioMixerGroup;
            //调用方式，到时候重新写
            audioPass = new Dictionary<AudioSource, Tween>();
        }
        //以下两个方法均为测试用方法
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
        /// 检查目前ActionAudioSource是否有空闲，有则播放，无则新建
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
        /// 检查目前InteractAudioSource是否有空闲，有则播放，无则新建
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
        /// 触发交互时调用（比如GP、打肉等），同时也可以传入攻击强度参数(音效序号见SoundEffects)
        /// /错误///(轻攻击强度参数为1，重攻击2，超重3)
        /// /选择音频。
        /// </summary>
        /// <param name="index"></param>
        public void InteractOnPlay(int index)
        {
            InteractOnPlay(0, 0, 0, index);
        }
        /// <summary>
        /// 触发交互时调用（比如GP、打肉等），同时也可以传入攻击强度参数(音效序号见SoundEffects)
        /// (轻攻击强度参数为1，重攻击2，超重3)
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
        /// 触发交互时调用（比如GP、打肉等），同时也可以传入攻击强度参数(音效序号见SoundEffects)
        /// (轻攻击强度参数为1，重攻击2，超重3)
        /// </summary>
        /// <param name="index"></param>
        public void InteractOnPlay(int levelNum, float pitchrate, float volumerate, int index,float initVolume,float initPitch)
        {
            float pitchOffset = UnityEngine.Random.Range(pitchOffsetMin, pitchOffsetMax);
            float volumeOffset = UnityEngine.Random.Range(volumeOffsetMin, volumeOffsetMax);
            AudioSource audioSource = InteractSourceVecant();
            audioSource.pitch = initPitch + pitchOffset + levelNum * pitchrate;
            audioSource.volume = initVolume + volumeOffset + levelNum * volumerate;
            Debug.Log("拼刀pitch" + audioSource.pitch + "拼刀Voluem" + audioSource.volume);
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
        /// 挥舞动作时调用(比如轻攻击空挥)同时传入攻击强度参数
        ///（ 轻攻击强度参数为1，重攻击2，超重3）
        /// </summary>
        /// <param name="levelNum"></param>
        public void ActionOnPlay(int levelNum,float pitchrate,float volumerate,int index)
        {
            float pitchOffset = UnityEngine.Random.Range(pitchOffsetMin,pitchOffsetMax);
            float volumeOffset = UnityEngine.Random.Range(volumeOffsetMin,volumeOffsetMax);
            //具体的增幅数值，调的比较保守
            AudioSource audioSource = ActionSourceVecant();
            var initVolume = defaultVolume;
            var initPitch = defaultPitch;
            audioSource.pitch = initPitch+ pitchOffset + levelNum * pitchrate;
            audioSource.volume = initVolume + volumeOffset + levelNum * volumerate;
            audioSource.PlayOneShot(SoundEffectsModel.soundEffects[index]);
        }
        /// <summary>
        /// 挥舞动作时调用(比如轻攻击空挥)同时传入攻击强度参数
        ///（ 轻攻击强度参数为1，重攻击2，超重3）
        /// </summary>
        /// <param name="levelNum"></param>
        public void ActionOnPlay(int levelNum, float pitchrate, float volumerate, int index,float initVolume,float initPitch)
        {
            float pitchOffset = UnityEngine.Random.Range(pitchOffsetMin, pitchOffsetMax);
            float volumeOffset = UnityEngine.Random.Range(volumeOffsetMin, volumeOffsetMax);
            //具体的增幅数值，调的比较保守
            AudioSource audioSource = ActionSourceVecant();
            audioSource.pitch = initPitch+ pitchOffset + levelNum * pitchrate;
            audioSource.volume = initVolume + volumeOffset + levelNum * volumerate;
            audioSource.PlayOneShot(SoundEffectsModel.soundEffects[index]);
        }
        Dictionary<AudioSource, Tween> audioPass;

        /// <summary>
        /// 开始播放，调用Audio source的淡入效果，传入音源和淡入时间,初次播放音频时，会设置音频初始值为0.
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
                    Debug.Log("杀死了fadeouttween"+"clip"+audioSource.clip.ToSafeString());
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
            Debug.Log("淡入");
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
        /// 停止播放，调用Audio source的淡出效果，传入音源和淡出时间
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
                    Debug.Log("杀死了fadeintween" + "clip" + audioSource.clip.ToSafeString());

                }
            }
            Debug.Log("淡出");
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
