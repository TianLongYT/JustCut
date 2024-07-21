using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

///<summary>
///
///</summary>
public class FrameDebugger:MonoBehaviour
{
    private int _startFrame=0;
    private int _frameCounter=0;
    private int _frameCounterCache=0;//状态改变时，缓存上一个状态的帧数
    private int _currentStateId=-1;
    private int _prevStateId=-1;
    private bool _isPrinting=false;//是否正在打印

    private int _inputStartFrame=0;
    private int _inputCounter=0;
    private int _inputInterval=3000;//输入间隔，大于该值时清空帧数表
    private bool _isStateChanged=false;

    //UI相关
    public UI_FrameBar frameBar;

    public void Init()
    {
        if(frameBar is null)
        {
            Debug.LogError("FrameDebugger.Init() frameBar is null");
            return;
        }
        frameBar.Init(new List<Color>());
    }

    /// <summary>
    /// 玩家输入时调用
    /// </summary>
    /// <param name="stateId">
    public void OnReceiveInput(int stateId)
    {
        _inputStartFrame=Time.frameCount;
        _inputCounter=0;
        if(_isPrinting)
        {
            OnStateChanged(stateId);
        }
        else
        {
            StartPrint(stateId);
        }
        
    }

    public void StartPrint(int stateId)
    {
        _isPrinting=true;
        _startFrame=Time.frameCount;
        _frameCounter=0;
        _frameCounterCache=0;
        _currentStateId=stateId;
        _prevStateId=-1;
    }
    /// <summary>
    /// 玩家状态改变时调用，如在Animator中调用
    /// </summary>
    /// <param name="stateId"></param>
    /// <returns></returns>
    public bool OnStateChanged(int stateId)
    {
        if(_currentStateId!=stateId)
        {
            _frameCounterCache=_frameCounter;
            _frameCounter=0;
            _startFrame=Time.frameCount;
            _prevStateId=_currentStateId;
            _currentStateId=stateId;
            _isStateChanged=true;
        }
        return _isStateChanged;
    }
    //清空
    public void ClearFrames()
    {
        _frameCounter=0;
        _frameCounterCache=0;
        _isStateChanged=false;
        frameBar.ClearFrameDots();
    }


    //更新输入计时器
    void Update()
    {
        _inputCounter=Time.frameCount-_inputStartFrame;
        if(_inputCounter>_inputInterval)
        {
            _inputCounter=0;
            ClearFrames();
        }
    }

    void LateUpdate()
    {
        if(!_isPrinting) return;
        _frameCounter=Time.frameCount-_startFrame+1;
        if(_isStateChanged)
        {
            _isStateChanged=false;
            frameBar.PrintFrameDot(_prevStateId, _frameCounterCache,true);
        }
        else
        {
            frameBar.PrintFrameDot(_currentStateId, _frameCounter,false);
        }

        if(_currentStateId==-1) _isPrinting=false;
    }
}
