using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///帧数表条
///</summary>
public class UI_FrameBar : MonoBehaviour
{
    
    [Header("基础设置")]
    public int maxFrameCount=45;//最大显示帧数
    public GameObject frameDotPrefab;
    [SerializeField] GameObject canvansGO;

    private List<UI_FrameDot> _frameDotList=new();
    private int _currentFrame=0;
    
    public void Init(List<Color> stateId2Color)
    {
        _frameDotList.Clear();
        for (int i = 0; i < maxFrameCount; i++)
        {
            GameObject go = Instantiate(frameDotPrefab, transform);
            go.name = "FrameDot" + i;
            go.transform.SetParent(this.transform);
            var comp=go.GetComponent<UI_FrameDot>();
            comp.Init(stateId2Color);
            _frameDotList.Add(comp);
        }
        _currentFrame=0;
    }

    public void PrintFrameDot(int stateId,int frameCount,bool showFrame)
    {
        if (_currentFrame>=maxFrameCount)
        {
            _currentFrame=0;
            OverlayFrameDots();
        }
        _frameDotList[_currentFrame].Show(stateId,frameCount,showFrame);
        _currentFrame++;
    }
    public void ShowLastFrameCount(int frameCount)
    {
        //Debug.Log((_currentFrame - 1 + maxFrameCount) % maxFrameCount);
        _frameDotList[(_currentFrame-1 + maxFrameCount)%maxFrameCount].ShowFrameCountText(frameCount);
    }
    public void AddTagForFrameDot()
    {
        _frameDotList[_currentFrame%maxFrameCount].AddTag();
    }
    //超出最大长度后，覆盖之前的帧数显示
    private void OverlayFrameDots()
    {
        foreach(var dot in _frameDotList)
        {
            dot.Overlay();
        }
    }

    public void ClearFrameDots()
    {
        _currentFrame=0;
        foreach(var dot in _frameDotList)
        {
            dot.Clear();
        }
    }
    public void ShowDebugTable(bool isShow)
    {
        canvansGO.SetActive(isShow);
    }
}
