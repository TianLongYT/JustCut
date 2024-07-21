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
    /// ��ɳ���״̬ת����״̬ת��ʱ�Ĵ���
    /// </summary>
    public class SceneManager:Singleton<SceneManager>
    {
        public LevelData levelData;
        public bool InFightingScene;
        public override void InitInstance()
        {
            EventManager.Register<LoadingSceneEvent>(OnLoadingScene);
            EventManager.Register<ChangeSceneEvent>(OnChangeScene);
            Debug.Log("ע�᳡���¼�");
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
                case 0://������
                    if(obj.progress == 1)
                    {
                        EventCenter.EventCenter.Trigger(new Evt.EvtParamChangeSceneInfo { sceneIndex = 0, progress = 1 });
                    }
                    break;
                case 1://ս������
                    if (obj.progress == 1)
                    {
                        Debug.Log("ս������������ϣ�����ս������");
                        EventManager.Trigger("OnGameContinue");//��������Ϸ�������ʱ����Ϸ����ͣ����ʱ����ٴο�ʼ����ϷӦ�ü�����
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
                case 0://������
                    InFightingScene = false;

                    Debug.Log("����������Change");

                    EventCenter.EventCenter.Trigger(new Evt.EvtParamChangeSceneInfo { sceneIndex = 0 });
                    break;
                case 1://ս������
                    Debug.Log("����ս������Change");
                    InFightingScene = true;

                    EventCenter.EventCenter.Trigger(new Evt.EvtParamChangeSceneInfo { sceneIndex = 1 });
                    break;

            }
            curSceneIndex = obj.sceneIndex;
            Debug.Log("���������л��¼�");
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