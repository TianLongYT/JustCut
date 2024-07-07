using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{
    [CreateAssetMenu(fileName = "EnemyModel",menuName = "Data/EnemyModel")]
    /// <summary>
    /// 
    /// </summary>
    public class EnemyModel : ScriptableObject
    {
        public EnemyMotionKeyFrameModel keyFrameModel;
        public EnemyAttackModel attackModel;
        public EnemyAnimationData animationData;
    }
}