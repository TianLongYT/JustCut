using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// �������������˵Ľű���һ���������������ȡ����
    /// </summary>
    public class PlayerLockController : MonoBehaviour
    {
        public Transform targetTF;
        [SerializeField] string targetTag;
        [Header("��ΪĳЩ����Ĭ�ϳ�������ߣ���ѡ��ת���Ժܺ������������")]
        [SerializeField] bool flipFaceDir;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == targetTag)
            {
                Debug.Log("����Ŀ��");
                targetTF = collision.transform;
                this.GetComponent<Collider2D>().enabled = false;
            }
        }
        
        /// <summary>
        /// �Ƚ�boss��player�ľ����ϵ������sign(boss.pos.x - player.pos.x),���û�ҵ�Ŀ�꣬����0��
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
        /// ������ҳ��򡣴����巽���ã����᷵��0���ǺǺǺǡ�
        /// </summary>
        /// <returns></returns>
        public float GetFaceDir()
        {
            //Debug.Log("��ȡ����ұߵķ���" + transform.right);
            var faceDir = flipFaceDir ? -transform.right.x : transform.right.x;
            return Mathf.Sign(faceDir);
        }
        /// <summary>
        /// ���ص�ǰ�Ƿ�������״̬
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
            //��ȡ������Ϣ��֪ͨ�ı䳯��
            float curFaceDir = GetPosRelationship();
            if (lastFaceDir != curFaceDir)
                OnFaceChange?.Invoke(curFaceDir);
            lastFaceDir = curFaceDir;
        }

        /// <summary>
        /// ����Һ͵���λ�ù�ϵ�仯ʱ���ã������µ�λ�ù�ϵ
        /// </summary>
        public event Action<float> OnFaceChange;

    }
}