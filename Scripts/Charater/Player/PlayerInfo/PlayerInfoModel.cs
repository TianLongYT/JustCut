
namespace iysy.JustCut
{
    [System.Serializable]
    /// <summary>
    /// PlayerInfo的初始化数据
    /// </summary>
    public class PlayerInfoModel
    {
        public float MaxHP;
        public float MaxPosture;

        public float SqIntensityToPosture;//攻击强度差的平方到架势。
        public float BasePostureAdder;
        public float PostureRecoverSpeed;
        public float PostureRecoverInterval;
        public float PostureToHp;

        public float RollInterval;
        
    }
}