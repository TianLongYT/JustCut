using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// ��Ϸ�о���Ĵ�����ݣ�������̬Model�Ͷ�̬Info����
    /// </summary>
    public class AttackData
    {
        public AttackDataModel attackModel;
        public int chargeCount;
        public bool perfectAttack;
        //��������ĵ�λ��Ӱ����Ч���ɡ�
        public Vector3 hitPoint;
        //public Vector3 sourcePoint;//�����ʼ��λ
        public Vector3 hitDirection;//�������Ӱ����Ч���ɺ���Ļ�𶯡�

        public bool hitBody;//���true,���ʾ���������ܻ���
    }
}