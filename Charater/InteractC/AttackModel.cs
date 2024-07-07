
namespace iysy.JustCut
{
    [System.Serializable]
    /// <summary>
    /// �洢���ж����Ļ�����Ϣ��
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