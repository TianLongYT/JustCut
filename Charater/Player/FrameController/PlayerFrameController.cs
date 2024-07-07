using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 打补丁，为的是让玩家在执行Skip时，不要执行帧更新代码。
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