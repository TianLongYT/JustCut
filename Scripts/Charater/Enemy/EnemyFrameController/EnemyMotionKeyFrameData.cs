using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{
    [System.Serializable]
    /// <summary>
    /// �����ؼ�֡��ӡ
    /// </summary>
    public class EnemyMotionKeyFrameData
    {
        public MoveMotionType motionType;

        public int startUp;//ǰҡ��֡������ӡ���̸�������
        public int active;//���֡��֡������ӡ�ĺ��������
        public int recover;//����������

        //public int gpStart, gpEnd;
        //public int cancle1start, cancle1end;
        //public int cancle2start, cancle2end;
        //public int freetypeStart;

        //public int gpFreezeTime;//��������Ҳ����Ϊ��֡Ҳ���漰������ʱ���ԡ�

        public bool Match(MoveMotionType type) => motionType == type;
    }
}