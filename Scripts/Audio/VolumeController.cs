using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace iysy.JustCut{
    public class VolumeController : MonoBehaviour
    {
        [SerializeField] Slider MasterSlider;
        [SerializeField] Slider SFXSlider;
        [SerializeField] Slider MusicSlider;
        [SerializeField] AudioMixer audioMixer;
        float EntranceBGMVolumeOffset;
        float FightingBGMHighpassVolumeOffset;
        float FightingBGMLowpassVolumeOffset;
        float InteractSoundEffectVolumeOffset;
        float ActionSoundEffectVolumeOffset;
        float MasterVolumeOffset;
        
        
        private void Awake() {
            MasterSlider.GetComponent<Slider>();
            SFXSlider.GetComponent<Slider>();
            MusicSlider.GetComponent<Slider>();
            MasterSlider.onValueChanged.AddListener(OnMasterSliderValueChanged);
            SFXSlider.onValueChanged.AddListener(OnSFXSliderValueChanged);
            MusicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
            
            audioMixer.GetFloat("EntranceBGMVolume",out EntranceBGMVolumeOffset);
            audioMixer.GetFloat("FightingBGMHighpassVolume",out FightingBGMLowpassVolumeOffset);
            audioMixer.GetFloat("FightingBGMLowpassVolume",out FightingBGMLowpassVolumeOffset);
            audioMixer.GetFloat("ActionVolume",out ActionSoundEffectVolumeOffset);
            audioMixer.GetFloat("InteractVolume",out InteractSoundEffectVolumeOffset);
            audioMixer.GetFloat("MasterVolume",out MasterVolumeOffset);
        }

        private void OnMusicSliderValueChanged(float arg0)
        {
            audioMixer.SetFloat("EntranceBGMVolume",2.0f * (MusicSlider.value - 10f) + EntranceBGMVolumeOffset);
            audioMixer.SetFloat("FightingBGMHighpassVolume",2.0f * (MusicSlider.value - 10f)+ FightingBGMHighpassVolumeOffset);
            audioMixer.SetFloat("FightingBGMLowpassVolume",2.0f * (MusicSlider.value - 10f) + FightingBGMLowpassVolumeOffset);
            if(MusicSlider.value == 0f)
            {
                audioMixer.SetFloat("FightingBGMLowpassVolume",-80f);
                audioMixer.SetFloat("FightingBGMHighpassVolume",-80f);
                audioMixer.SetFloat("EntranceBGMVolume",-80f);
            }
        }

        private void OnSFXSliderValueChanged(float arg0)
        {
            audioMixer.SetFloat("ActionVolume",2.0f * (SFXSlider.value - 10f) + ActionSoundEffectVolumeOffset);
            audioMixer.SetFloat("InteractVolume",2.0f * (SFXSlider.value - 10f) + InteractSoundEffectVolumeOffset);
            if(SFXSlider.value == 0f)
            {
                audioMixer.SetFloat("InteractVolume",-80f);
                audioMixer.SetFloat("ActionVolume",-80f);
            }
        }

        private void OnMasterSliderValueChanged(float arg0)
        {
            audioMixer.SetFloat("MasterVolume",2.0f * (MasterSlider.value - 10f) + MasterVolumeOffset);
            if(MasterSlider.value == 0f)
            {
                audioMixer.SetFloat("MasterVolume",-80f);
            }
        }
    }
}