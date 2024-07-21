using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MzUtility.Base;
using MzUtility.FrameWork.EventSystem;
using MzUtility.FrameWork.SceneSystem;
using MzUtility.FrameWork.UI;
using UnityEngine;

//单例防止重复创建
public class SceneChanger : MonoSingleton<SceneChanger>
{
    public override void OnSingletonInit()
    {
        base.OnSingletonInit();
        EventManager.Register<ChangeSceneEvent>(OnChangeScene);
    }

    public async void OnChangeScene(ChangeSceneEvent e)
    {
        switch(e.sceneIndex)
        {
            case 0:
            {
                //主界面
                await ChangeSceneToMainMenu();
                break;
            }
            case 1:
            {
                //游戏界面
                await ChangeSceneToGame();
                break;
            }
        }
    }

    public async UniTask ChangeSceneToMainMenu()
    {
        UIManager.Show<LoadingCanvas>();
        UIManager.Hide<BattleCanvas>();

        await SceneSystem.LoadSceneAsync(0);

        UIManager.Show<MainMenuCanvas>();
        UIManager.Hide<LoadingCanvas>();
    }

    public async UniTask ChangeSceneToGame()
    {
        UIManager.Show<LoadingCanvas>();
        UIManager.Hide<MainMenuCanvas>();

        await SceneSystem.LoadSceneAsync(1);
        EventManager.Trigger("OnGameSceneLoaded");

        UIManager.Show<BattleCanvas>();
        UIManager.Hide<LoadingCanvas>();
    }
}

public struct ChangeSceneEvent
{
    public int sceneIndex;
}
