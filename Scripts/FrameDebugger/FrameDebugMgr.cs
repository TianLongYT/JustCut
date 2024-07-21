using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 实现游戏中的帧数打印同步
    /// </summary>
    public class FrameDebugMgr : SingletonBehaviourNoNewGONoDontDestroy<FrameDebugMgr>
    {
        [SerializeField] FrameDebugger playerBar;
        [SerializeField] FrameDebugger bossBar;
        [SerializeField] GameObject canvasGO;
        [SerializeField] bool autoClose = true;
        bool playerInit;
        bool bossInit;
        private void OnEnable()
        {
            playerInit = true;
            bossInit = false;
            GameWorld.Instance.OnLateFrameTick += OnLateFrameTick;


        }
        private void Start()
        {
            FrameDebuggerExtension.OnShowUIBar.AddListener(OnShowUIBar);
            FrameDebuggerExtension.OnShowUIBar.InvokeAction(FrameDebuggerExtension.showUI);
        }

        private void OnDisable()
        {
            GameWorld.Instance.OnLateFrameTick -= OnLateFrameTick;
        }
        private new void OnDestroy()
        {
            base.OnDestroy();
            FrameDebuggerExtension.OnShowUIBar.RemoveListener(OnShowUIBar);

        }

        private void OnShowUIBar(bool obj)
        {
            Debug.Log("设置CanvansGO"+obj);
            canvasGO?.SetActive(obj);
        }
        private void OnLateFrameTick(float frameDeltaTime)
        {
            if (playerInit || bossInit)
            {
                //Debug.Log("打印块块，当前帧数" + GameWorld.Instance.FrameCount);
                playerBar.DebugBlock();
                bossBar.DebugBlock();
            }
        }

        public void ClearBar()
        {
            playerInit = false;
            bossInit = false;
            playerBar.ClearBar();
            bossBar.ClearBar();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="blockIndex">对应的颜色数组,-1表示停止当前条（所有条都停止时才能终止显示）</param>
        /// <param name="barType">0、playerBar\1、bossBar</param>
        public void ChangeState(int blockIndex,int barType)
        {
            if(barType == 0)
            {
                playerBar.ChangeState(blockIndex);
                if(blockIndex == -1)
                {
                    StopBar(barType);//playerInit = false;
                }
                else
                {
                    playerInit = true;
                }
            }
            else if(barType == 1)
            {
                bossBar.ChangeState(blockIndex);
                if(blockIndex == -1)
                {
                    StopBar(barType);//bossInit = false;
                }
                else
                {
                    bossInit = true;
                }
            }
        }
        public void AddTagForCurBlock(int barType)
        {
            if (barType == 0)
            {
                playerBar.AddTagForCurBlock();
            }
            else if (barType == 1)
            {
                bossBar.AddTagForCurBlock();
            }
        }
        /// <summary>
        /// 要注意，只有所有bar都停止时，bar才会停止。
        /// </summary>
        /// <param name="barType">0、playerBar\1、bossBar</param>
        private void StopBar(int barType)
        {
            if (barType == 0)
            {
                playerInit = false;
            }
            else if (barType == 1)
            {
                bossInit = false;
            }
        }


    }
}