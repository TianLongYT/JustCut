using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace iysy.FeedBack
{

    /// <summary>
    /// 依赖后处理特效和 colorGrading
    /// </summary>
    public class ViewFade : MonoBehaviour
    {
        PostProcessVolume volume;
        ColorGrading colorGrading;

        private void Start()
        {
            volume = GetComponent<PostProcessVolume>();
            volume.profile.TryGetSettings<ColorGrading>(out colorGrading);
            ViewFadeExtension.RegistFade(OnFade);
            ViewFadeExtension.RegistRecover(OnRecover);
            if (colorGrading)
            {
                saValue = colorGrading.saturation.value;
                originSaValue = saValue;
            }
            else
            {
                Debug.Log("找不到ColorGrading");
            }
        }
        private void OnDestroy()
        {
            ViewFadeExtension.RegistFade(OnFade, true);
            ViewFadeExtension.RegistRecover(OnRecover, true);
        }
        Tween curTween;
        float saValue;
        float originSaValue;
        private void OnFade(float fadeValue,float fadeTime)
        {
            Debug.Log("进入OnFade函数");
            if (colorGrading == null)
                return;
            if (curTween != null && curTween.IsActive())
                curTween.Kill();
            curTween = DOTween.To(() => saValue, (value) =>
            {
                saValue = value;
                colorGrading.saturation.Override(saValue);
            }, fadeValue, fadeTime);
            Debug.Log("使用DoTween设置Fade");
        }
        private void OnRecover(float fadeTime)
        {
            if (colorGrading == null)
                return;
            if (curTween != null && curTween.IsActive())
                curTween.Kill();
            curTween = DOTween.To(() => saValue, (value) =>
            {
                saValue = value;
                colorGrading.saturation.Override(saValue);
            }, originSaValue, fadeTime);
        }


    }
    public static class ViewFadeExtension
    {
        static GameAction<float,float> ViewFade;
        public static void RegistFade(Action<float, float> action,bool unRegist = false)
        {
            if (unRegist)
            {
                ViewFade.RemoveListener(action);
            }
            else
            {
                ViewFade.AddListener(action);
            }
        }
        public static void TriggerFade(float fadeValue,float fadeTime)
        {
            ViewFade.InvokeAction(fadeValue,fadeTime);
        }
        static GameAction<float> ViewRecover;
        public static void RegistRecover(Action<float> action,bool unRegist = false)
        {
            if (unRegist)
            {
                ViewRecover.RemoveListener(action);
            }
            else
            {
                ViewRecover.AddListener(action);
            }
        }
        public static void TriggerRecover(float fadeTime)
        {
            ViewRecover.InvokeAction(fadeTime);
        }
    }
}