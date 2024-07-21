using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MzUtility.FrameWork.UI;
/// <summary>
/// 游戏入口，负责进入游戏时的逻辑
/// </summary>
public class GameEntry : MonoBehaviour
{
    void Start()
    {
        UIManager.Show<MainMenuCanvas>();
    }
}
