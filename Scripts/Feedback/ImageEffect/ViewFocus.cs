using DG.Tweening;
using iysy.JustCut;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace iysy.FeedBack
{

    /// <summary>
    /// 需要PostProcess和Vignette
    /// </summary>
    public class ViewFocus : MonoBehaviour
    {
        PostProcessVolume volume;
        Vignette vignette;

        private void Start()
        {
            volume = GetComponent<PostProcessVolume>();
            volume.profile.TryGetSettings<Vignette>(out vignette);
            ViewFocusExtension.RegistFocus(OnFade);
            ViewFocusExtension.RegistRecover(OnRecover);
            if (vignette)
            {
                intensity = vignette.intensity.value;
                originIntensity = intensity;
            }
        }
        private void OnDestroy()
        {
            ViewFocusExtension.RegistFocus(OnFade, true);
            ViewFocusExtension.RegistRecover(OnRecover, true);
        }
        Tween curTween;
        float intensity;
        float originIntensity;
        private void OnFade(float fadeValue, float fadeTime)
        {
            //Debug.Log("进入OnFade函数");
            if (vignette == null)
                return;
            if (curTween != null && curTween.IsActive())
                curTween.Kill();
            curTween = DOTween.To(() => intensity, (value) =>
            {
                intensity = value;
                vignette.intensity.Override(intensity);
            }, fadeValue, fadeTime);
           //Debug.Log("使用DoTween设置Fade");
        }
        private void OnRecover(float fadeTime)
        {
            if (vignette == null)
                return;
            if (curTween != null && curTween.IsActive())
                curTween.Kill();
            curTween = DOTween.To(() => intensity, (value) =>
            {
                intensity = value;
                vignette.intensity.Override(intensity);
            }, originIntensity, fadeTime);
        }


    }
    public static class ViewFocusExtension
    {
        static GameAction<float, float> ViewFocus;
        public static void RegistFocus(Action<float, float> action, bool unRegist = false)
        {
            if (unRegist)
            {
                ViewFocus.RemoveListener(action);
            }
            else
            {
                ViewFocus.AddListener(action);
            }
        }
        public static void TriggerFocus(float fadeValue, float fadeTime)
        {
            ViewFocus.InvokeAction(fadeValue, fadeTime);
        }
        static GameAction<float> ViewRecover;
        public static void RegistRecover(Action<float> action, bool unRegist = false)
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