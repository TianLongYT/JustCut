
namespace iysy.JustCut
{
    [System.Serializable]
    /// <summary>
    /// 存储所有动作的基础信息。
    /// </summary>
    public class AttackModel
    {
        public AttackDataModel[] attackDatas;
        public AttackDataModel GetMotionKeyFrameData(MotionType type)
        {
            foreach (var item in attackDatas)
            {
                if (item.Match(type))
                    return item;
            }
            return null;
        }
    }
}