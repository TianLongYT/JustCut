using iysy.JustCut.Evt;
using MzUtility.FrameWork.EventSystem;
using UnityEngine;
using iysy.FeedBack;

namespace iysy.JustCut
{

    /// <summary>
    /// 负责获取事件，设置fade
    /// </summary>
    public class ViewFocusTrigger : MonoBehaviour
    {
        [SerializeField] float focus2RecoverInterval;
        [SerializeField] float focusTime;
        [SerializeField] float recoverTime;
        [SerializeField] float basefocusValue;
        [SerializeField] float intensity2focusValue;
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
        private void Focus(float focusValue)
        {
            ViewFocusExtension.TriggerFocus(focusValue, focusTime);
            recoverTimer = focus2RecoverInterval;
            needRecover = true;
        }
        private void Recover()
        {
            ViewFocusExtension.TriggerRecover(recoverTime);
            needRecover = false;
        }

        private void OnPlayerGpReduceHp(EvtParamOnGpReduceHp obj)
        {
            var focusValue = basefocusValue + obj.DeltaIntensity * intensity2focusValue;
            Focus(focusValue);
        }
    }
}