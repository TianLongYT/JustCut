using iysy.JustCut.Evt;
using MzUtility.FrameWork.EventSystem;
using UnityEngine;
using iysy.FeedBack;

namespace iysy.JustCut
{

    /// <summary>
    /// 负责获取事件，设置fade
    /// </summary>
    public class ViewFadeTrigger : MonoBehaviour
    {
        [SerializeField] float fade2RecoverInterval;
        [SerializeField] float fadeTime;
        [SerializeField] float recoverTime;
        [SerializeField] float baseFadeValue;
        [SerializeField] float intensity2FadeValue;
        private void Start()
        {
            EventCenter.EventCenter.Register<Evt.EvtParamOnGpReduceHp>(OnPlayerGpReduceHp);
        }

        private void OnDestroy()
        {
            EventCenter.EventCenter.UnRegister<Evt.EvtParamOnGpReduceHp>(OnPlayerGpReduceHp);
        }

        float recoverTimer;
        bool needRecover = true;
        private void Update()
        {
            if (needRecover)
            {
                recoverTimer -= GameWorld.Instance.DeltaTime;
                if (recoverTimer <= 0)
                {
                    Recover();
                }
            }
        }
        private void Fade(float fadeValue)
        {
            Debug.Log("执行拼刀扣血的褪色");
            ViewFadeExtension.TriggerFade(fadeValue, fadeTime);
            recoverTimer = fade2RecoverInterval;
            needRecover = true;
        }
        private void Recover()
        {
            ViewFadeExtension.TriggerRecover(recoverTime);
            needRecover = false;
        }

        private void OnPlayerGpReduceHp(EvtParamOnGpReduceHp obj)
        {
            var fadeValue = (baseFadeValue + intensity2FadeValue * obj.DeltaIntensity);
            Fade(fadeValue);
        }
    }
}