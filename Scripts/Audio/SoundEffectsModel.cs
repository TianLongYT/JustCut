using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{
    [CreateAssetMenu(fileName ="SoundEffectsModel",menuName ="Data/SoundEffectsModel")]
    /// <summary>
    /// 
    /// </summary>
    public class SoundEffectsModel :ScriptableObject
    {
        public AudioClip[] soundEffects;

        public AudioClip[] BGMs;
    }
}