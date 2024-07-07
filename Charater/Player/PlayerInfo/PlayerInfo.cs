
namespace iysy.JustCut
{

    /// <summary>
    /// 
    /// </summary>
    public class PlayerInfo
    {
        private float curHP;
        public float CurHP
        {
            get
            {
                return curHP;
            }
            set
            {
                curHP = value;
            }
        }
        private float curPosture;
        public float CurPosture
        {
            get
            {
                return curPosture;
            }
            set
            {
                curPosture = value;
            }
        }
        public PlayerInfo(PlayerInfoModel model)
        {
            curHP = model.MaxHP;
            curPosture = model.MaxPosture;
        }
    }
}