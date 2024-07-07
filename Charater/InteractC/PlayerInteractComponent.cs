using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// �������Ե��˵Ĵ����Ϣ������󣬵���PlayerInfo(��Ѫ����������),��StateMachine(���ˡ�����);
    /// �����Լ��Ĵ����Ϣ������Ҫ���������Ϣ�����ǿ�ȣ�������������ʵʱ�����Ϣ�����λ�ã�������򣬻���״̬��
    /// </summary>
    public class PlayerInteractComponent : MonoBehaviour
    {
        AttackModel attackModel;
        PlayerInfo playerInfo;
        PlayerVFXController vfxController;
        PlayerStateMachine stateMachine;
        [System.Serializable]
        public struct PlayerAttackHitPass
        {
            public MotionType attackMotionType;
            public PlayerAttackHitBox hitBox;
        }
        [SerializeField] List<PlayerAttackHitPass> attackPasses;
        Dictionary<MotionType, PlayerAttackHitBox> type2HitBox;
        MotionType curMotionType;
        public void Initalize(PlayerVFXController vfxController,AttackModel playerAttackModel,PlayerInfo playerInfo)
        {
            this.vfxController = vfxController;
            this.attackModel = playerAttackModel;
            this.playerInfo = playerInfo;
            //�Ѵ����Ϣ�����Ӧ�Ĵ���ű��С�
            type2HitBox = new Dictionary<MotionType, PlayerAttackHitBox>(attackPasses.Count);
            foreach (var item in attackPasses)
            {
                if (type2HitBox.ContainsKey(item.attackMotionType))
                {
                    Debug.LogError("PlayerAttackHitPass��������������ͬ��MotionType");
                    return;
                }
                item.hitBox.Initialize(attackModel.GetMotionKeyFrameData(item.attackMotionType),this);
                type2HitBox.Add(item.attackMotionType, item.hitBox);
            }
        }
        public void SetStateMachine(PlayerStateMachine stateMachine) => this.stateMachine = stateMachine;

        public void Hit(AttackData attackData)//�п�����Ҫ֪ͨStateMachine�����ܻ�״̬��
        {
            //�������Ե��˵Ĺ�����
            //GP����
            if(attackData.hitBody == false)
            {
                if (isGpActive == false)
                    return;
                int intensityMinus = (attackData.attackModel.GetFinalIntensity() - attackModel.GetMotionKeyFrameData(curMotionType).GetFinalIntensity());
                playerInfo.CurPosture += intensityMinus > 0 ? intensityMinus : 0;
                return;
            }
            //���˻�����ҵ��ܻ���
            if (isGpActive)
                return;
            playerInfo.CurHP -= attackData.attackModel.baseDamage;
        }
        bool isGpActive;
        public void GenerateAttack(MotionType motionType)
        {
            curMotionType = motionType;
        }
        public void SetGPEnable(bool enable)
        {
            isGpActive = enable;
            type2HitBox[curMotionType].SetGPEnable(enable);
        }
        public void SetHitEnable(bool enable)
        {
            type2HitBox[curMotionType].SetHitEnable(enable);
        }
        public void JumpToHitActiveFrame(Vector3 dir,float intensity)//��stateMachine���ж�����֡��Ȼ����ж�֡���㡣������ִ���ˣ���ôGP�ж��Ͳ���ִ�С�
        {
            stateMachine.OnHit(2);
            EasyEvents.Get<Evt.EvtOnGp>().Trigger(dir, intensity);
        }
        public void OnTriggerGP(Vector3 dir,float intensity)//��StateMachine ����һ����֡��������Ч���š�
        {
            stateMachine.OnHit(1);
            EasyEvents.Get<Evt.EvtOnGp>().Trigger(dir, intensity);
        }
        public void OnTriggerHit(Vector3 dir,float intensity)//��stateMachin ����һ���������١�������Ч���š�
        {
            stateMachine.OnHit(0);
            EasyEvents.Get<Evt.EvtOnHitEnemy>().Trigger(dir, intensity);
        }
        public void PlayGPSound()//���Ͳ�����Ƶ�¼���
        {

        }
        public void PlayHitSound()//���Ͳ�����Ƶ�¼�������Ϊ��Ƶ�ļ�����player���ϡ�����Ч�ļ���player���ϡ�
        {

        }
        public void GenerateGPVFX(Vector3 pos,Vector3 dir)
        {
            vfxController.PlayGPParticle(pos, dir);
        }
        public void GenerateHitVFX(Vector3 pos,Vector3 dir,int intensity)
        {
            vfxController.PlayHitParticle(pos, dir, 0);
        }
    }
}