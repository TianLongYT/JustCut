using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{
    [System.Serializable]
    /// <summary>
    /// �����ؼ�֡��ӡ
    /// </summary>
    public class ChargeMotionKeyFrameData
    {
        public MotionType motionType;

        public int chargeFront;
        public int chargeMaxFrame;
        public bool needGP;
        public int gpStart, gpEnd;

        public bool needJust2, needJust3;
        public int just1Start, just1End;
        public int just2Start, just2End;
        public int just3Start, just3End;

        public bool Match(MotionType type) => motionType == type;
    }
}