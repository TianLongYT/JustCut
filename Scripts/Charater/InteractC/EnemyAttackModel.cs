
namespace iysy.JustCut
{
    [System.Serializable]
    /// <summary>
    /// �洢���ж����Ļ�����Ϣ��
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