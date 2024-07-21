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
        //�洢״̬��Ӧ��ɫ��
        [SerializeField] List<Color> state2Color;

        //���֡���������⣬
        [SerializeField] UI_FrameBar frameBar;
        [SerializeField] bool autoClear;//��ʱ�䲻���мӿ����ʱ���´β�������ǰ������п顣
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
                Debug.LogError("��FrameDebugger��ֵUI_FrameBar");
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
        /// <param name="newState">ʲô��û��ʱ��debugTable����������߸�start����ʼ��������-2��������ʼ����С״̬����ʱ����-1��</param>
        /// <param name="channelData"></param>
        public void ChangeState(int newState,ChannelData channelData)
        {
            if (autoClear && autoClearTimer >= autoClearTime)
                ClearMBar(this.channelData);
            autoClearTimer = 0;
            //Debug.Log("֡����ǰ״̬");
            if (!ChannelDef.Match(this.channelData, channelData))
            {
                return;
            }
            if (curState == newState)
                return;

            if (curState != -2 && curState != -1)
                frameBar.ShowLastFrameCount(stateFrame + 1);
            curState = newState;
            //Debug.Log(" ֡����ת����״̬" + newState);
            stateFrame = -1;
        }
        public void ChangeState(int newState)
        {
            if (curState == newState)
                return;
            if (curState != -2 && curState != -1)
                frameBar.ShowLastFrameCount(stateFrame + 1);
            curState = newState;
            //Debug.Log(" ֡����ת����״̬" + newState);
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
            //Debug.Log("֡�����ӡ״̬" + curState);
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
        //��գ�����֡����
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