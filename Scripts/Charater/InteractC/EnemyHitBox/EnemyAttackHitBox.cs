using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// �����ǲ��ᴥ��GP�ģ���ˣ�ֱ�����ù�����Ŀ��ؾ�����
    /// </summary>
    public class EnemyAttackHitBox : MonoBehaviour
    {
        private const string PlayerHurtLayer = "PlayerHurtLayer";
        private const string PlayerHitLayer = "PlayerHitLayer";
        private EnemyAttackDataModel attackData;
        private EnemyInteractComponent interactC;
        Collider2D m_coll;
        [SerializeField] Vector3 hitDirection;

        public void Initialize(EnemyAttackDataModel attackData, EnemyInteractComponent interactComponent)
        {
            this.attackData = attackData;
            interactC = interactComponent;
            m_coll = GetComponent<Collider2D>();
        }
        public void EnableHitBox(bool enable)
        {
            if (enable)
            {
                isActiveOver = false;
                isHitBoxIn = false;
                isHurtBoxIn = false;
                this.gameObject.SetActive(true);
            }
            else
            {
                isActiveOver = true;
                Invoke(nameof(SetSelfDisable), 0.3f);
            }
        }
        private void SetSelfDisable()
        {
            this.gameObject.SetActive(false);

        }
        bool isActiveOver;
        bool isHurtBoxIn;
        Collider2D hurtBoxColl;
        bool isHitBoxIn;
        Collider2D hitBoxColl;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (interactC == null)
                return;
            if(collision.gameObject.layer == LayerMask.NameToLayer(PlayerHurtLayer))
            {
                isHurtBoxIn = true;
                hurtBoxColl = collision;
            }
            if(collision.gameObject.layer == LayerMask.NameToLayer(PlayerHitLayer))
            {
                isHitBoxIn = true;
                hitBoxColl = collision;
            }
        }
        private void Update()
        {
            if (isActiveOver == false && isHitBoxIn)//����GP��
            {
                Debug.Log("���˼�ⴥ��GP");
                isActiveOver = true;
                isHitBoxIn = false;
                PlayerInteractComponent playerInteractC = hitBoxColl.GetComponentInParent<PlayerInteractComponent>();
                playerInteractC.Hit(new AttackData
                {
                    attackModel = attackData.ToAttackDataModel(),
                    chargeCount = 0,
                    hitBody = false,
                    hitDirection = hitDirection,
                    hitPoint = (hitBoxColl.bounds.center + m_coll.bounds.center) *0.5f,
                    perfectAttack = false,
                });
            }
            else if(isActiveOver == false && isHurtBoxIn)//������ҡ�
            {
                Debug.Log("���˼�ⴥ������");
                isActiveOver = true;
                isHurtBoxIn = false;
                PlayerInteractComponent playerInteractC = hurtBoxColl.GetComponentInParent<PlayerInteractComponent>();
                playerInteractC.Hit(new AttackData
                {
                    attackModel = attackData.ToAttackDataModel(),
                    chargeCount = 0,
                    hitBody = true,
                    hitDirection = hitDirection,
                    hitPoint = hurtBoxColl.bounds.center,
                    perfectAttack = false,
                });
                EnableHitBox(false);
            }
        }


    }
}