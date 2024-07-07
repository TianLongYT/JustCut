using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// �ܲ���ֱ���þ�����������ײ����أ��о��С�
    /// </summary>
    public class EnemyCastComponent : MonoBehaviour
    {
        [Header("Ĭ���ú���ľ����⣬��������ײ��⣬����LockController")]
        [SerializeField] bool castByCollider;
        PlayerLockController lockController;

        //����һ���������飬���������ڵ������־����Ρ�
        [System.Serializable]
        struct DistanceRange
        {
            public float min;
            public float max;
        }
        [Header("���0��ʼ���־����С��ǰһ���������ͺ�һ��������ӣ�������Χ�ľ��뷵��-1��ʾ����ǳ�Զ��")]
        [SerializeField]
        DistanceRange[] distanceDegree;
        Transform playerTF 
        { 
            get
            {
                if (lockController.IsLock())
                {
                    return lockController.targetTF;
                }
                else
                    return null;
            }
        }
        private void Start()
        {
            lockController = this.transform.parent.GetComponentInChildren<PlayerLockController>();
        }
        public int DistanceDegree 
        {
            get
            {
                if (playerTF == null)
                    return -1;
                var posDelta = Mathf.Abs(playerTF.position.x - transform.position.x);
                for (int i = 0; i < distanceDegree.Length; i++)
                {
                    if (posDelta >= distanceDegree[i].min && posDelta < distanceDegree[i].max)
                    {
                        return i;
                    }
                }
                return -1;
            }
        }
        private void OnDrawGizmos()
        {
            Color initColor = Random.ColorHSV();
            // initColor = new Color(initColor.r, initColor.g, initColor.b); ȷ����ɫ��͸����
            Vector3 curPos = transform.position;
            Vector3 rightDir = transform.right;
            //��һ�´���������ľ���
            foreach (var item in distanceDegree)
            {
                iysy.DebugDraw.DebugDraw.DrawPoint(curPos + item.max * rightDir, initColor);
                iysy.DebugDraw.DebugDraw.DrawPoint(curPos - item.max * rightDir, initColor);
            }
        }

    }
}