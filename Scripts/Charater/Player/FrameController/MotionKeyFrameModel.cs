using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{
    [System.Serializable]
    /// <summary>
    /// 存储人物所有的动作关键帧数据
    /// </summary>
    public class MotionKeyFrameModel
    {
        //public int FramePerSecond = 60;
        [SerializeField] ChargeMotionKeyFrameData[] chargeFrameDatas;
        [SerializeField] MotionKeyFrameData[] frameDatas;
        /// <summary>
        /// 获得对应动作的对应关键帧信息。
        /// </summary>
        /// <param name="type"></param>
        /// <returns>如果没有对应动作的关键帧信息，返回null</returns>
        public MotionKeyFrameData GetMotionKeyFrameData(MotionType type)
        {
            foreach (var item in frameDatas)
            {
                if (item.Match(type))
                    return item;
            }
            return null;
        }
        public ChargeMotionKeyFrameData GetChargeMotionKeyFrameData(MotionType type)
        {
            foreach (var item in chargeFrameDatas)
            {
                if (item.Match(type))
                    return item;
            }
            return null;
        }


    }
}