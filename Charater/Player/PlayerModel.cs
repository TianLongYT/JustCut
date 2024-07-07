
using UnityEngine;
namespace iysy.JustCut
{
    [CreateAssetMenu(fileName ="NewPlayerModel",menuName = "Data/PlayerModel")]
    /// <summary>
    /// 包含所有非动态变化的数据。
    /// </summary>
    public class PlayerModel:ScriptableObject
    {
        public PlayerInfoModel infoModel;
        public AnimationData animationModel;
        public MotionKeyFrameModel frameModel;
        public AttackModel attackModel;//主要是打击的伤害等信息。
    }
}