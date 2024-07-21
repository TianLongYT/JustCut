using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.Timeline;
using Cysharp.Threading.Tasks;
using iysy.JustCut;
using MzUtility.FrameWork.EventSystem;
using MzUtility.FrameWork.UI;
using Sirenix.OdinInspector;
using UnityEngine;
/// <summary>
/// 测试用脚本
/// </summary>
public class Test : MonoBehaviour
{

    [Button("暂停游戏")]
    public void TestFoo()
    {
        UIManager.Show<PauseCanvas>();
    }

    private float simHealthRatio=1f;
    private void Start()
    {
        InputManager.Instance.inputActions.Player.Pause.performed += OnPausePerformed;

        EventManager.Register("EvtOnPlayerDeath",OnPlayerDead);
        EventManager.Register("OnEnemyDeath",OnEnemyDead);
    }
    private void OnDestroy()
    {
        EventManager.UnRegister("EvtOnPlayerDeath",OnPlayerDead);
        EventManager.UnRegister("OnEnemyDeath",OnEnemyDead);
    }

    private void OnPausePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("escapeP");
        UIManager.Show<PauseCanvas>();
    }
    private float simPostureRatio=0f;
    [Button("测试血条")]
    public void TestHealthBar()
    {
        simHealthRatio-=0.2f;
        EventManager.Trigger(new EvtOnPlayerDamage(){Ratio=simHealthRatio});
    }

    [Button("测试气条")]
    public void TestPosture()
    {
        simPostureRatio+=0.2f;
        EventManager.Trigger(new EvtOnPlayerPostureChanged(){Ratio=simPostureRatio});
    }
    [Button("测试结果UI")]
    public void TestResultCanvas()
    {
       UIManager.Show<ResultCanvas>(new ResultCanvasArgs(){isWin=true});
    }

    //测试结算界面
    public async void OnPlayerDead()
    {
        await UniTask.Delay(500);
        EventManager.Trigger("OnGamePause");
        await UIManager.ShowAsync<ResultCanvas>(new ResultCanvasArgs(){isWin=false});
    }
    public async void OnEnemyDead()
    {
        await UniTask.Delay(500);
        EventManager.Trigger("OnGamePause");
        await UIManager.ShowAsync<ResultCanvas>(new ResultCanvasArgs(){isWin=true});
    }
}
