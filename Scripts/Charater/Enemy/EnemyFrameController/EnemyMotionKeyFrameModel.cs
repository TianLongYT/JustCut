using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{
    [System.Serializable]
    /// <summary>
    /// �洢�������еĶ����ؼ�֡����
    /// </summary>
    public class EnemyMotionKeyFrameModel
    {
        //public int FramePerSecond = 60;
        [SerializeField] EnemyMotionKeyFrameData[] frameDatas;
        /// <summary>
        /// ��ö�Ӧ�����Ķ�Ӧ�ؼ�֡��Ϣ��
        /// </summary>
        /// <param name="type"></param>
        /// <returns>���û�ж�Ӧ�����Ĺؼ�֡��Ϣ������null</returns>
        public EnemyMotionKeyFrameData GetMotionKeyFrameData(MoveMotionType type)
        {
            foreach (var item in frameDatas)
            {
                if (item.Match(type))
                    return item;
            }
            return null;
        }


    }
}