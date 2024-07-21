using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{
    [CreateAssetMenu(fileName ="charaterData",menuName ="PathData/CharaterPathData")]
    /// <summary>
    /// 
    /// </summary>
    public class CharaterPathData :ScriptableObject
    {
        //人物预制体
        public string CharaterPrefabPath;
        //VFX
        public string VFXFolderPath;

        //Sound
        public string SoundFolderPath;
        //Voice
        public string VoiceFolderPath;
    }
}