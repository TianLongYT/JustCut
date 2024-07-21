using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 能不能直接用距离来代替碰撞检测呢？感觉行。
    /// </summary>
    public class EnemyCastComponent : MonoBehaviour
    {
        [Header("默认用横向的距离检测，而不是碰撞检测，依赖LockController")]
        [SerializeField] bool castByCollider;
        PlayerLockController lockController;

        //包含一个距离数组，按照数组内的数划分距离层次。
        [System.Serializable]
        struct DistanceRange
        {
            public float min;
            public float max;
        }
        [Header("请从0开始划分距离大小，前一个距离必须和后一个距离相接，超出范围的距离返回-1表示距离非常远。")]
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
            // initColor = new Color(initColor.r, initColor.g, initColor.b); 确保颜色不透明。
            Vector3 curPos = transform.position;
            Vector3 rightDir = transform.right;
            //画一下创建的数组的距离
            foreach (var item in distanceDegree)
            {
                iysy.DebugDraw.DebugDraw.DrawPoint(curPos + item.max * rightDir, initColor);
                iysy.DebugDraw.DebugDraw.DrawPoint(curPos - item.max * rightDir, initColor);
            }
        }

    }
}