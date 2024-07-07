
namespace iysy.JustCut
{
    [System.Serializable]
    /// <summary>
    /// 存储所有动作的基础信息。
    /// </summary>
    public class EnemyAttackModel
    {
        public EnemyAttackDataModel[] attackDatas;
        public EnemyAttackDataModel GetMotionKeyFrameData(MoveMotionType type)
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