using System.Collections;
using System.Collections.Generic;
using QFramework;
using Unity.VisualScripting;
using UnityEngine;

///<summary>
///负责帧数表的显示，以及接受输入信息
///</summary>
public class FrameController //: MonoSingleton<FrameController>, IController
{
    public IArchitecture GetArchitecture()
    {
        throw new System.NotImplementedException();
    }
    public static Dictionary<int,Color> FrameColorMap = new()
    {
        {-1,new Color(0.17f, 0.17f, 0.17f, 1f)},//默认
        {0,Color.white},//输入
        {1,Color.green},//前摇
        {2,Color.red},//打击帧
        {3,Color.blue},//后摇
        {4,Color.yellow},//受击
    };

    public Color this[int stateId]
    {
        get
        {
            if(FrameColorMap.TryGetValue(stateId,out var color))
            {
                return color;
            }
            else
            {
                return new Color(0.17f, 0.17f, 0.17f, 1f);
            }
        }
    }

    //玩家的帧数表
    public FrameDebugger frameDebugger;

}
