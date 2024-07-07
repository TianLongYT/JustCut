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
        public EnemyFrameController FrameController { get; private set; }
        private void Start()
        {
            FrameController = new EnemyFrameController(enemyModel.keyFrameModel);
            FrameController.OnFrameTick += OnFrameTick;
            FrameController.OnEnd += OnEnd;
        }


        private void OnEnd()
        {
            FrameDebugMgr.Instance.ChangeState(-1, 1);
            GameManager.Instance.ForceUpdateBehaviorTree();
        }

        EnemyFrameController.FrameInfo lastFrameState;
        private void OnFrameTick(EnemyFrameController.FrameInfo obj)
        {
            if (lastFrameState.CheckEqualInt(obj) == false)
            {
                FrameDebugMgr.Instance.ChangeState(obj.ToInt(), 1);
                lastFrameState = obj;
                //Debug.Log("×ª»»µ½" + obj.ToInt());
            }
        }

        private void OnDestroy()
        {
            FrameController.OnFrameTick -= OnFrameTick;
            FrameController.OnEnd -= OnEnd;
        }
        public void StopFrameController()
        {
            FrameDebugMgr.Instance.ChangeState(-1, 1);
            FrameController.StopFrameController();
        }



    }
}