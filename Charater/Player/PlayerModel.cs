
using UnityEngine;
namespace iysy.JustCut
{
    [CreateAssetMenu(fileName ="NewPlayerModel",menuName = "Data/PlayerModel")]
    /// <summary>
    /// �������зǶ�̬�仯�����ݡ�
    /// </summary>
    public class PlayerModel:ScriptableObject
    {
        public PlayerInfoModel infoModel;
        public AnimationData animationModel;
        public MotionKeyFrameModel frameModel;
        public AttackModel attackModel;//��Ҫ�Ǵ�����˺�����Ϣ��
    }
}