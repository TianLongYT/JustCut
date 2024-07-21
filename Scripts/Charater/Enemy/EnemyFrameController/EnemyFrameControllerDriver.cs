using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 
    /// </summary>
    public class EnemyFrameControllerDriver : MonoBehaviour
    {
        [SerializeField] EnemyModel enemyModel;
        [SerializeField] ChannelData channelData;
        EnemyBehaviorManager ebm;
        public EnemyFrameController FrameController { get; private set; }
        private void Start()
        {
            FrameController = new EnemyFrameController(enemyModel.keyFrameModel);
            FrameController.OnFrameTick += OnFrameTick;
            FrameController.OnEnd += OnEnd;
            if (ebm == null)
                ebm = GetComponent<EnemyBehaviorManager>();
        }


        private void OnEnd()
        {
            FrameDebugMgr.Instance?.ChangeState(-1, 1);
            ebm.ForceUpdateBehaviorTree();
        }

        EnemyFrameController.FrameInfo lastFrameState;
        private void OnFrameTick(EnemyFrameController.FrameInfo obj)
        {
            if (lastFrameState.CheckEqualInt(obj) == false)
            {
                FrameDebugMgr.Instance?.ChangeState(obj.ToInt(), 1);
                lastFrameState = obj;
                //Debug.Log("×ª»»µ½" + obj.ToInt());
            }
        }

        private void OnDestroy()
        {
            if (FrameController == null)
                return;
            FrameController.OnFrameTick -= OnFrameTick;
            FrameController.OnEnd -= OnEnd;
        }
        public void StopFrameController()
        {
            FrameDebugMgr.Instance?.ChangeState(-1, 1);
            FrameController.StopFrameController();
        }



    }
}