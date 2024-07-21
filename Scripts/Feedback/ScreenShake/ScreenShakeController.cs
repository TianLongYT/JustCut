using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using Cinemachine;
using System;
using MzUtility.FrameWork.EventSystem;

namespace iysy.JustCut
{
    [RequireComponent(typeof(CinemachineImpulseSource))]
    /// <summary>
    /// 负责屏幕震动，通过将行为注册到载事件中心来被调用
    /// </summary>
    public class ScreenShakeController : MonoBehaviour
    {
        CinemachineImpulseSource impulseSource;
        private void Start()
        {
            impulseSource = GetComponent<CinemachineImpulseSource>();

            EventCenter.EventCenter.Register<Evt.EvtParamGpImpactInfo>(OnGp);
            EventCenter.EventCenter.Register<Evt.EvtParamHitImpactInfo>(OnHitEnemy);

        }
        private void OnDestroy()
        {
            EventCenter.EventCenter.UnRegister<Evt.EvtParamGpImpactInfo>(OnGp);
            EventCenter.EventCenter.UnRegister<Evt.EvtParamHitImpactInfo>(OnHitEnemy);
        }
        private void ShakeScreen(Vector3 dir, float force)
        {
            impulseSource.m_DefaultVelocity = dir;
            impulseSource.GenerateImpulseWithForce(force);
        }
        private void OnGp(Evt.EvtParamGpImpactInfo info)
        {
            info.dir = (info.dir + UnityEngine.Random.onUnitSphere * 0.2f).normalized;
            //Debug.Log("屏幕震动强度" + intensity/5.0f);
            ShakeScreen(info.dir, info.intensity/5.0f);
        }
        private void OnHitEnemy(Evt.EvtParamHitImpactInfo info)
        {
            info.dir = (info.dir + UnityEngine.Random.onUnitSphere * 0.2f).normalized;
            //Debug.Log("屏幕震动强度" + intensity/5.0f);
            ShakeScreen(info.dir, info.intensity / 8.0f);
        }

    }
}