using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 游戏中具体的打击数据，包括静态Model和动态Info部分
    /// </summary>
    public class AttackData
    {
        public AttackDataModel attackModel;
        public int chargeCount;
        public bool perfectAttack;
        //打击发生的点位。影响特效生成。
        public Vector3 hitPoint;
        //public Vector3 sourcePoint;//打击起始点位
        public Vector3 hitDirection;//打击方向，影响特效生成和屏幕震动。

        public bool hitBody;//如果true,则表示攻击击中受击框。
    }
}