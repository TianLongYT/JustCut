using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// �򲹶���Ϊ�����������ִ��Skipʱ����Ҫִ��֡���´��롣
    /// </summary>
    public class PlayerFrameController : FrameController
    {
        public PlayerFrameController(MotionKeyFrameModel keyFrameModel) : base(keyFrameModel)
        {
        }
        protected override void OnFrameUpdate(float timescale)
        {
            if(GameWorld.Instance.timeQueue.SkipTimeScale == 0)
            {
                return;
            }
            base.OnFrameUpdate(timescale);
        }

    }
}