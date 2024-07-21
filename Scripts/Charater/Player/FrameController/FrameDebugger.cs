using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 
    /// </summary>
    public class FrameDebugger:MonoBehaviour
    {
        //存储状态对应颜色表
        [SerializeField] List<Color> state2Color;

        //解决帧数计数问题，
        [SerializeField] UI_FrameBar frameBar;
        [SerializeField] bool autoClear;//长时间不进行加块操作时，下次操作会提前清空所有块。
        [SerializeField] float autoClearTime = 2f;
        [SerializeField] ChannelData channelData;
        int curState;
        int stateFrame;

        private void Awake()
        {
            if (frameBar == null)
                frameBar = GetComponent<UI_FrameBar>();
            if (frameBar == null)
            {
                Debug.LogError("给FrameDebugger赋值UI_FrameBar");
                return;
            }
            curState = -2;
            stateFrame = -1;
            frameBar.Init(state2Color);


            FrameDebuggerExtension.OnStateChange.AddListener(ChangeState);
            FrameDebuggerExtension.OnDebugBlock.AddListener(DebugBlock);
            FrameDebuggerExtension.OnClearBar.AddListener(ClearBar);
            //FrameDebuggerExtension.OnShowUIBar.AddListener(ShowUIBar);
        }
        private float autoClearTimer;
        private void Update()
        {
            if(autoClear)
                autoClearTimer += Time.deltaTime;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newState">什么都没有时（debugTable被清除，或者刚start完后初始化），是-2，经过初始化后小状态结束时，是-1，</param>
        /// <param name="channelData"></param>
        public void ChangeState(int newState,ChannelData channelData)
        {
            if (autoClear && autoClearTimer >= autoClearTime)
                ClearMBar(this.channelData);
            autoClearTimer = 0;
            //Debug.Log("帧数表当前状态");
            if (!ChannelDef.Match(this.channelData, channelData))
            {
                return;
            }
            if (curState == newState)
                return;

            if (curState != -2 && curState != -1)
                frameBar.ShowLastFrameCount(stateFrame + 1);
            curState = newState;
            //Debug.Log(" 帧数表转换到状态" + newState);
            stateFrame = -1;
        }
        public void ChangeState(int newState)
        {
            if (curState == newState)
                return;
            if (curState != -2 && curState != -1)
                frameBar.ShowLastFrameCount(stateFrame + 1);
            curState = newState;
            //Debug.Log(" 帧数表转换到状态" + newState);
            stateFrame = -1;
        }
        public void DebugBlock()
        {
            autoClearTimer = 0;
            stateFrame++;
            frameBar.PrintFrameDot(curState, stateFrame, false);
        }
        public void DebugBlock(ChannelData channelData)
        {
            if (!ChannelDef.Match(this.channelData, channelData))
            {
                return;
            }
            autoClearTimer = 0;
            stateFrame++;
            frameBar.PrintFrameDot(curState, stateFrame, false);
            //Debug.Log("帧数表打印状态" + curState);
        }
        public void AddTagForCurBlock()
        {
            frameBar.AddTagForFrameDot();
        }
        private void ClearMBar(ChannelData channelData)
        {
            if (!ChannelDef.Match(this.channelData, channelData))
            {
                return;
            }
            ClearBar(channelData);
        }
        //清空，开关帧数表
        public void ClearBar(ChannelData channelData)
        {
            //if (!ChannelDef.Match(this.channelData, channelData))
            //{
            //    return;
            //}
            frameBar.ClearFrameDots();
            curState = -2;
            stateFrame = -1;
            autoClearTimer = 0;
        }
        public void ClearBar()
        {
            frameBar.ClearFrameDots();
            curState = -2;
            stateFrame = -1;
            autoClearTimer = 0;
        }
        public void ShowUIBar(bool isShow) => frameBar.ShowDebugTable(isShow);


        private void OnDestroy()
        {
            FrameDebuggerExtension.OnStateChange.RemoveListener(ChangeState);
            FrameDebuggerExtension.OnDebugBlock.RemoveListener(DebugBlock);
            FrameDebuggerExtension.OnClearBar.RemoveListener(ClearBar);
            //FrameDebuggerExtension.OnShowUIBar.RemoveListener(ShowUIBar);
        }

    }
    public static class FrameDebuggerExtension
    {
        public static bool showUI;
        public static GameAction<int,ChannelData> OnStateChange = new GameAction<int, ChannelData>();
        public static GameAction<ChannelData> OnDebugBlock = new GameAction<ChannelData>();
        public static GameAction<ChannelData> OnClearBar = new GameAction<ChannelData>();
        public static GameAction<bool> OnShowUIBar = new GameAction<bool>();
    }
}