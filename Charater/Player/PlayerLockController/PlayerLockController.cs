using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 完成玩家锁定敌人的脚本，一旦完成锁定，不会取消。
    /// </summary>
    public class PlayerLockController : MonoBehaviour
    {
        public Transform targetTF;
        [SerializeField] string targetTag;
        [Header("因为某些敌人默认朝向是左边，勾选翻转可以很好适配这种情况")]
        [SerializeField] bool flipFaceDir;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == targetTag)
            {
                Debug.Log("锁定目标");
                targetTF = collision.transform;
                this.GetComponent<Collider2D>().enabled = false;
            }
        }
        
        /// <summary>
        /// 比较boss和player的距离关系。返回sign(boss.pos.x - player.pos.x),如果没找到目标，返回0；
        /// </summary>
        /// <returns></returns>
        public float GetPosRelationship()
        {
            if(targetTF == null)
            {
                return 0;
            }
            return Mathf.Sign(targetTF.position.x - this.transform.position.x);
        }
        /// <summary>
        /// 返回玩家朝向。从物体方向获得，不会返回0，呵呵呵呵。
        /// </summary>
        /// <returns></returns>
        public float GetFaceDir()
        {
            //Debug.Log("获取玩家右边的方向" + transform.right);
            var faceDir = flipFaceDir ? -transform.right.x : transform.right.x;
            return Mathf.Sign(faceDir);
        }
        /// <summary>
        /// 返回当前是否处于锁定状态
        /// </summary>
        /// <returns></returns>
        public bool IsLock()
        {
            return targetTF != null;
        }
        float lastFaceDir;
        private void Update()
        {
            if (targetTF == null)
                return;
            //获取朝向信息，通知改变朝向。
            float curFaceDir = GetPosRelationship();
            if (lastFaceDir != curFaceDir)
                OnFaceChange?.Invoke(curFaceDir);
            lastFaceDir = curFaceDir;
        }

        /// <summary>
        /// 当玩家和敌人位置关系变化时调用，传入新的位置关系
        /// </summary>
        public event Action<float> OnFaceChange;

    }
}