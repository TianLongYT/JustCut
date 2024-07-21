using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
namespace iysy.JustCut
{
    /// <summary>
    /// 
    /// </summary>
    public class LevelData
    {
        const string LevelRootPath = "Resources/LevelData/";
        public string playerPathData;//Â·¾¶+½ÇÉ«Ãû

        ResLoader resLoader = ResLoader.Allocate();

        public void SetPlayerPathData(string charaterName)
        {
            playerPathData = LevelRootPath + charaterName;
        }
        public CharaterPathData GetCharaterPathData()
        {
            if (playerPathData == default)
                return null;
            return resLoader.LoadSync<CharaterPathData>(playerPathData);
        }

    }
}