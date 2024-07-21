using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{
    [RequireComponent(typeof(BoxCollider2D))]
    /// <summary>
    /// 检测物体是否在碰撞体内
    /// </summary>
    public class CastCollider2DComponent : MonoBehaviour
    {
        public bool IsIn { get; set; }
        [SerializeField] bool castByTag;
        [SerializeField] string tagName;
        [SerializeField] bool castByLayer;
        [SerializeField] LayerMask layerMask;
        Collider2D targetColl;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (castByLayer && collision.gameObject.layer != layerMask.value) return;
            if (castByTag && collision.gameObject.tag != tagName) return;
            IsIn = true;
            targetColl = collision;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (castByLayer && collision.gameObject.layer != layerMask.value) return;
            if (castByTag && collision.gameObject.tag != tagName) return;
            IsIn = false;
            targetColl = null;
        }



    }
}