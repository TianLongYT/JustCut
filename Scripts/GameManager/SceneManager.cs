using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using MzUtility.FrameWork.EventSystem;
using MzUtility.FrameWork.SceneSystem;
using System;

namespace iysy.JustCut
{

    /// <summary>
    /// 完成场景状态转换和状态转换时的处理。
    /// </summary>
    public class SceneManager:Singleton<SceneManager>
    {
        public LevelData levelData;
        public bool InFightingScene;
        public override void InitInstance()
        {
            EventManager.Register<LoadingSceneEvent>(OnLoadingScene);
            EventManager.Register<ChangeSceneEvent>(OnChangeScene);
            Debug.Log("注册场景事件");
            //InFightingScene = defaultFightingState;
            levelData = new LevelData();

            //AudioManager.Instance?.AudioSourceFadeIn(AudioManager.Instance.EntranceBGM, 3.0f);
            EventManager.Register("OnGameContinue",OnBattleContinue);
            EventManager.Register("OnGamePause", OnBattlePause);

            int sceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            EventCenter.EventCenter.Trigger(new Evt.EvtParamChangeSceneInfo { sceneIndex = sceneIndex });
            if(sceneIndex == 1)
            {
                InFightingScene = true;
            }

        }

        public override void Dispose()
        {
            base.Dispose();
            EventManager.UnRegister<LoadingSceneEvent>(OnLoadingScene);
            EventManager.UnRegister<ChangeSceneEvent>(OnChangeScene);
            EventManager.UnRegister("OnGameContinue", OnBattleContinue);
            EventManager.UnRegister("OnGamePause", OnBattlePause);

        }

        private void OnLoadingScene(LoadingSceneEvent obj)
        {
            switch (obj.sceneBuildIndex)
            {
                case 0://主场景
                    if(obj.progress == 1)
                    {
                        EventCenter.EventCenter.Trigger(new Evt.EvtParamChangeSceneInfo { sceneIndex = 0, progress = 1 });
                    }
                    break;
                case 1://战斗场景
                    if (obj.progress == 1)
                    {
                        Debug.Log("战斗场景加载完毕，进入战斗场景");
                        EventManager.Trigger("OnGameContinue");//当弹出游戏结算界面时，游戏被暂停，此时点击再次开始，游戏应该继续。
                        EventCenter.EventCenter.Trigger(new Evt.EvtParamChangeSceneInfo { sceneIndex = 1, progress = 1 });
                    }
                    break;

            }
        }
        int curSceneIndex;
        private void OnChangeScene(ChangeSceneEvent obj)
        {
            switch (obj.sceneIndex)
            {
                case 0://主场景
                    InFightingScene = false;

                    Debug.Log("进入主场景Change");

                    EventCenter.EventCenter.Trigger(new Evt.EvtParamChangeSceneInfo { sceneIndex = 0 });
                    break;
                case 1://战斗场景
                    Debug.Log("进入战斗场景Change");
                    InFightingScene = true;

                    EventCenter.EventCenter.Trigger(new Evt.EvtParamChangeSceneInfo { sceneIndex = 1 });
                    break;

            }
            curSceneIndex = obj.sceneIndex;
            Debug.Log("触发场景切换事件");
        }

        private void OnBattleContinue()
        {
            EventCenter.EventCenter.Trigger(new Evt.EvtParamOnBattleContinue());
        }

        private void OnBattlePause()
        {
            EventCenter.EventCenter.Trigger(new Evt.EvtParamOnBattlePause());
        }

    }
}