using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
///帧率表中一帧的UI
///</summary>
public class UI_FrameDot : MonoBehaviour
{
    public static Color DefaultColor=new Color(0.17f, 0.17f, 0.17f, 1f);
    public Image image;
    public Text frameText;
    List<Color> stateId2Color;

    public void Init(List<Color> stateId2Color)
    {
        image.color=DefaultColor;
        this.stateId2Color = stateId2Color;
        frameText.text="";
        frameText.gameObject.SetActive(false);
    }

    public void Show(int stateId,int frame,bool showFrame)
    {
        Color color = DefaultColor;
        if (stateId2Color.Count > stateId&& stateId >=0)
            color = stateId2Color[stateId];
        //Debug.Log(color.ToString());
        image.color = color;//FrameController.Instance[stateId];
        frameText.text=frame.ToString();
        frameText.gameObject.SetActive(showFrame);
    }
    public void AddTag()
    {
        Color tmp = image.color;
        tmp += Color.white;
        image.color = tmp;
    }
    public void ShowFrameCountText(int frame)
    {
        frameText.text = frame.ToString();
        frameText.gameObject.SetActive(true);
    }
    public void Overlay()
    {
        Color tmp=image.color;
        tmp.a=0.2f;
        image.color=tmp;
    }

    public void Clear()
    {
        image.color=DefaultColor;
        frameText.text="";
        frameText.gameObject.SetActive(false);
    }
    //public void SetUnitScale(float rate)
    //{

    //}
}
