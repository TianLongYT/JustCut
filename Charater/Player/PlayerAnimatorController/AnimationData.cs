using System;

namespace iysy.JustCut
{
    [Serializable]
    /// <summary>
    /// 动画片段对应参数名。
    /// </summary>
    public class AnimationData
    {
        public string MoveRate;
        public string Move;
        public string LightAttack1;
        public string LightAttack2;
        public string RollLightAttack;
        public string HA1Enter;
        public string HA2Enter;
        public string HA3Enter;
        public string HA1Exit;
        public string HA2Exit;
        public string HA3Exit;
        public string SAEnter;
        public string SAExit;
        public string AttackRelease;
        public string Roll;

        [Serializable]
        public class AttackAnimData
        {
            public string animName;
            public float keyFrameTime;
            public float animLength;
        }
        public AttackAnimData[] attackAnimDatas;

        public float animSlowDownTime;
        public float slowDownRate;
        public float animSpeedUpTime;
        public float speedUpRate;
    }
}