using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// ������ɾ���Ľ������������Ϣ����̬�Ͷ�̬��Ϣ���ϲ����ݸ��Է���
    /// </summary>
    public class PlayerAttackHitBox:MonoBehaviour
    {
        const string CastHitLayer = "EnemyHitLayer";
        const string CastHurtLayer = "EnemyHurtLayer";
        AttackDataModel attackData;
        PlayerInteractComponent interactC;

        [SerializeField] Vector3 attackDirection;
        Collider2D coll, enemyHitColl, enemyHurtColl;


        bool isGpActive;
        bool isHitActive;
        bool isGpOver;
        bool isHitOver;

        bool enemyHitBoxIn, enemyHurtBoxIn;

        /*���GP�е���û��Hit,��֡����ǰ����Hit���*/
        /*�����GP����Hit��*/
        /*Hit��GP�ֿ���⣬ֻ��GP�������֡��Hitֻͣ����������*/
        public void Initialize(AttackDataModel attackData,PlayerInteractComponent interactC)
        {
            this.attackData = attackData;
            this.interactC = interactC;
            coll = GetComponent<Collider2D>();
        }
        private void OnEnable()
        {
            isGpActive = false;
            isHitActive = false;
            isGpOver = false;
            isHitOver = false;

            enemyHitColl = null;
            enemyHurtColl = null;
            enemyHurtBoxIn = false;
            enemyHitBoxIn = false;
            //Debug.Log("��ʼ����ײ��+Time"+Time.time);
            //GameWorld.Instance.FreezeTime(30);
        }
        public void SetGPEnable(bool enable)
        {
            if (enable)
            {
                if (isHitActive == false)
                    this.gameObject.SetActive(true);
                isGpActive = enable;
            }
            else
            {
                isGpActive = enable;
                if (isHitActive == false)
                    this.gameObject.SetActive(false);
            }
        }
        public void SetHitEnable(bool enable)
        {
            //Debug.Log(this.gameObject.name+"���ô��״̬" + enable);
            if (enable)
            {
                if (isGpActive == false)
                    this.gameObject.SetActive(true);
                isHitActive = enable;
            }
            else
            {
                isHitActive = enable;
                if (isGpActive == false)
                    this.gameObject.SetActive(false);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D collision)//����֡���߼�֡û�취���룬���³���10֡������
        {
            //Debug.Log("OnTriggerEnter");
            if (interactC == null)
                return;
            if (isGpOver == false)
            {
                //Debug.Log("����GP���"+Time.time);
                if(collision.gameObject.layer == LayerMask.NameToLayer(CastHitLayer))
                {
                    //��⵽Ŀ����ײ�塣
                    //Debug.Log("��⵽�Է�������"+Time.time);
                    //GameWorld.Instance.FreezeTime(30);
                    enemyHitBoxIn = true;
                    enemyHitColl = collision;
                }
            }
            if(isHitOver == false)
            {
                //Debug.Log("����Hit���");
                if(collision.gameObject.layer == LayerMask.NameToLayer(CastHurtLayer))
                {
                    //Debug.Log("��⵽�Է��ܻ���");
                    enemyHurtBoxIn = true;
                    enemyHurtColl = collision;
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (interactC == null)
                return;
            if (isGpOver == false)
            {
                if (collision.gameObject.layer == LayerMask.NameToLayer(CastHitLayer))
                {
                    //Debug.Log("��⵽�Է�GP���뿪");
                    //Ŀ����ײ���뿪��
                    enemyHitBoxIn = false;
                    enemyHitColl = null;
                }
            }
            if (isHitOver == false)
            {
                if (collision.gameObject.layer == LayerMask.NameToLayer(CastHurtLayer))
                {
                    //Debug.Log("��⵽�Է��ܻ����뿪");
                    enemyHurtBoxIn = false;
                    enemyHurtColl = null;
                }
            }
        }
        private void Update()
        {
            if (interactC == null)
                return;
            //Debug.Log("BOOL" + isGpOver + isHitActive + enemyHitBoxIn);

            if (isHitOver == false && isHitActive && enemyHurtBoxIn)
            {
                //Debug.Log("ͨ�����м��");
                OnTriggerHit();
                isHitOver = true;
                if (isGpOver)
                {
                    this.gameObject.SetActive(false);
                    return;
                }
            }
            if(isGpOver == false && isGpActive && enemyHitBoxIn)
            {
                //Debug.Log("ͨ��GP�����������߼�����֡��" + Time.time);
                bool needJump = false;
                if (isHitActive == false)
                {
                    needJump = true;
                    JumpToHitActive();
                }
                OnTriggerGP(needJump);
                isGpOver = true;
                if (isHitOver)
                    this.gameObject.SetActive(false);
            }

        }
        private void JumpToHitActive()//�ô�������ǰ���롣
        {
            //Debug.Log("������ԾGP");
            if (isHitOver == false && enemyHurtBoxIn)
            {
                OnTriggerHit();
                isHitOver = true;
            }
            isHitActive = true;
            //֪ͨstateMachine��ת�����֡���������µ����֡״̬��
            //interactC.JumpToHitActiveFrame();
        }
        private void OnTriggerGP(bool needJump)
        {
            //������֡Ч����
            //���ɴ����Ϣ������������
            var enemyInteractC = enemyHitColl.GetComponentInParent<EnemyInteractComponent>();
            if(enemyInteractC == null)
            {
                Debug.LogError("û�ҵ����˽����ű�");
                return;
            }
            Vector3 pos = (coll.bounds.center + enemyHitColl.bounds.center) * 0.5f;
            Vector3 dir = this.attackDirection;
            enemyInteractC.Hit(new AttackData
            {
                attackModel = this.attackData,
                hitBody = false,
                hitDirection = dir,
                hitPoint = pos
            });
            interactC.PlayGPSound();
            interactC.GenerateGPVFX(pos, dir);
            if (needJump)
                interactC.JumpToHitActiveFrame(dir,attackData.GetFinalIntensity());
            else
                interactC.OnTriggerGP(dir,attackData.GetFinalIntensity());

        }
        private void OnTriggerHit()
        {
            //������������Ч����
            //���ɴ����Ϣ������������
            var enemyInteractC = enemyHurtColl.GetComponentInParent<EnemyInteractComponent>();
            if (enemyInteractC == null)
            {
                Debug.LogError("û�ҵ����˽����ű�");
                return;
            }
            Vector3 pos = enemyHurtColl.bounds.center;
            Vector3 dir = this.attackDirection;
            enemyInteractC.Hit(new AttackData
            {
                attackModel = this.attackData,
                hitBody = true,
                hitDirection = dir,
                hitPoint = pos
            });
            interactC.OnTriggerHit(dir,attackData.GetFinalIntensity());
            interactC.PlayHitSound();
            interactC.GenerateHitVFX(pos,dir,0);
        }
    }
}