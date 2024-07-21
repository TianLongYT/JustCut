using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 忽略玩家和敌人的碰撞。让玩家能够穿越敌人。
    /// </summary>
    public class PlayerCrossEnemyController : MonoBehaviour
    {
        Collider2D coll;
        Collider2D m_Coll
        {
            get
            {
                if(coll == null)
                {
                    coll = GetComponent<Collider2D>();
                }
                return coll;
            }
        }
        [SerializeField] Collider2D mainColl;
        public void IgnoreCollision(Collider2D enemyColl,bool ignore)
        {
            //Physics2D.IgnoreCollision(m_Coll, enemyColl, ignore);
            m_Coll.isTrigger = ignore;
            //if(mainColl != null)
            //    mainColl.isTrigger = ignore;
        }


    }
}