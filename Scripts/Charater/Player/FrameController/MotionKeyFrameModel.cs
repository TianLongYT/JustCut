using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{
    [System.Serializable]
    /// <summary>
    /// �洢�������еĶ����ؼ�֡����
    /// </summary>
    public class MotionKeyFrameModel
    {
        //public int FramePerSecond = 60;
        [SerializeField] ChargeMotionKeyFrameData[] chargeFrameDatas;
        [SerializeField] MotionKeyFrameData[] frameDatas;
        /// <summary>
        /// ��ö�Ӧ�����Ķ�Ӧ�ؼ�֡��Ϣ��
        /// </summary>
        /// <param name="type"></param>
        /// <returns>���û�ж�Ӧ�����Ĺؼ�֡��Ϣ������null</returns>
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