using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut.Evt
{

    /// <summary>
    /// 改变场景信息时的事件参数
    /// </summary>
    public struct EvtParamChangeSceneInfo
    {
        public int sceneIndex;
        public float progress;//0刚开始加载、1加载完毕。
    }
}