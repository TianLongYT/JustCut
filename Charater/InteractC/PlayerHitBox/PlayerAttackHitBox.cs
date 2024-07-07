using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 负责完成具体的交互，将打击信息（静态和动态信息）合并传递给对方。
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

        /*如果GP有但是没有Hit,跳帧并提前进行Hit检测*/
        /*如果有GP，和Hit，*/
        /*Hit和GP分开检测，只有GP能整体顿帧。Hit只停顿自身动画。*/
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
            //Debug.Log("初始化碰撞框+Time"+Time.time);
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
            //Debug.Log(this.gameObject.name+"设置打击状态" + enable);
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
        
        private void OnTriggerEnter2D(Collider2D collision)//物理帧和逻辑帧没办法对齐，导致超过10帧的误差？！
        {
            //Debug.Log("OnTriggerEnter");
            if (interactC == null)
                return;
            if (isGpOver == false)
            {
                //Debug.Log("进行GP检测"+Time.time);
                if(collision.gameObject.layer == LayerMask.NameToLayer(CastHitLayer))
                {
                    //检测到目标碰撞体。
                    //Debug.Log("检测到对方攻击框"+Time.time);
                    //GameWorld.Instance.FreezeTime(30);
                    enemyHitBoxIn = true;
                    enemyHitColl = collision;
                }
            }
            if(isHitOver == false)
            {
                //Debug.Log("进行Hit检测");
                if(collision.gameObject.layer == LayerMask.NameToLayer(CastHurtLayer))
                {
                    //Debug.Log("检测到对方受击框");
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
                    //Debug.Log("检测到对方GP框离开");
                    //目标碰撞体离开。
                    enemyHitBoxIn = false;
                    enemyHitColl = null;
                }
            }
            if (isHitOver == false)
            {
                if (collision.gameObject.layer == LayerMask.NameToLayer(CastHurtLayer))
                {
                    //Debug.Log("检测到对方受击框离开");
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
                //Debug.Log("通过击中检测");
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
                //Debug.Log("通过GP检测进入其他逻辑（顿帧）" + Time.time);
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
        private void JumpToHitActive()//让打击检测提前进入。
        {
            //Debug.Log("触发跳跃GP");
            if (isHitOver == false && enemyHurtBoxIn)
            {
                OnTriggerHit();
                isHitOver = true;
            }
            isHitActive = true;
            //通知stateMachine跳转到打击帧动画，更新到打击帧状态。
            //interactC.JumpToHitActiveFrame();
        }
        private void OnTriggerGP(bool needJump)
        {
            //触发顿帧效果。
            //生成打击信息，赋予打击对象。
            var enemyInteractC = enemyHitColl.GetComponentInParent<EnemyInteractComponent>();
            if(enemyInteractC == null)
            {
                Debug.LogError("没找到敌人交互脚本");
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
            //触发动画减速效果。
            //生成打击信息，赋予打击对象。
            var enemyInteractC = enemyHurtColl.GetComponentInParent<EnemyInteractComponent>();
            if (enemyInteractC == null)
            {
                Debug.LogError("没找到敌人交互脚本");
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