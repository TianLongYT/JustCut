using MzUtility.FrameWork.EventSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace iysy.JustCut
{

    /// <summary>
    /// 
    /// </summary>
    public class FrameDebuggerTest : MonoBehaviour
    {

        Toggle toggle;
        private void OnEnable()
        {
            //FrameDebuggerExtension.OnShowUIBar.InvokeAction(true);
            toggle = GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(OnToggleChanged);
        }

        private void OnToggleChanged(bool arg0)
        {
            Debug.Log("…Ë÷√FrameDebuggerUI" + arg0);
            FrameDebuggerExtension.showUI = arg0;
            FrameDebuggerExtension.OnShowUIBar.InvokeAction(arg0);
        }

        private void OnDestroy()
        {
            toggle.onValueChanged.RemoveListener(OnToggleChanged);
        }

    }
}