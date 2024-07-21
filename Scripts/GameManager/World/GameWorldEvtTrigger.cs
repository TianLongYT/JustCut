using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 负责接收处理所有相关事件，调用GameWorld相关方法。
    /// </summary>
    public class GameWorldEvtTrigger
    {
        GameWorld gameWorld;
        public GameWorldEvtTrigger(GameWorld gameWorld)
        {
            this.gameWorld = gameWorld;

            EventCenter.EventCenter.Register<Evt.EvtParamOnBattlePause>(OnBattlePause);
            EventCenter.EventCenter.Register<Evt.EvtParamOnBattleContinue>(OnBattleContinue);

        }
        public void Dispose()
        {
            EventCenter.EventCenter.UnRegister<Evt.EvtParamOnBattlePause>(OnBattlePause);
            EventCenter.EventCenter.UnRegister<Evt.EvtParamOnBattleContinue>(OnBattleContinue);
        }
        private void OnBattleContinue(Evt.EvtParamOnBattleContinue obj)
        {
            gameWorld?.ContinueGame();
        }

        private void OnBattlePause(Evt.EvtParamOnBattlePause obj)
        {
            gameWorld?.PauseGame();
        }
    }
}