using MzUtility.FrameWork.EventSystem;
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
            init = true;
        }
        public void SetStateMachine(PlayerStateMachine stateMachine) => this.stateMachine = stateMachine;

        public void Hit(AttackData attackData)//�п�����Ҫ֪ͨStateMachine�����ܻ�״̬��
        {
            //�������Ե��˵Ĺ�����
            Debug.Log("�������Ե��˵Ĺ���" +" ����ǿ��"+ attackData.attackModel.GetFinalIntensity());
            var res = stateMachine.OnInteract(attackData);
            //GP����
            if (res == true)//��Ҵ���GP�޵�״̬��
            {
                if (attackData.hitBody)
                    return;//���з�������ҡ�
                int intensityMinus = (attackData.attackModel.GetFinalIntensity() - attackModel.GetMotionKeyFrameData(curMotionType).GetFinalIntensity());
                playerInfo.SetPostrue(intensityMinus);
                Debug.Log("���˻������GP��");
                return;
            }
            //���˻�����ҵ��ܻ���
            else
            {
                playerInfo.CurHP -= attackData.attackModel.baseDamage;
                Debug.Log("��ұ������ˣ���Ѫ" + attackData.attackModel.baseDamage);
            }
        }
        MotionType lastMotion;
        bool init;
        public void GenerateAttack(MotionType motionType)
        {

            lastMotion = curMotionType;
            if (init)
            {
                lastMotion = motionType;
                init = false;
            }
            curMotionType = motionType;
            type2HitBox[lastMotion].SetGPEnable(false);
            type2HitBox[lastMotion].SetHitEnable(false);

        }
        public void GenerateAttack(MotionType motionType,bool just,int chargeCount)
        {
            GenerateAttack(motionType);
            type2HitBox[curMotionType].SetAttackDataModel(just, chargeCount);
        }
        public void SetGPEnable(bool enable)
        {
            type2HitBox[curMotionType].SetGPEnable(enable);

        }
        public void SetHitEnable(bool enable)
        {
            type2HitBox[curMotionType].SetHitEnable(enable);

        }
        public struct ImpactInfo
        {
            public Vector3 dir;
            public float intensity;
        }
        public void JumpToHitActiveFrame(Vector3 dir,float intensity)//��stateMachine���ж�����֡��Ȼ����ж�֡���㡣������ִ���ˣ���ôGP�ж��Ͳ���ִ�С�
        {
            stateMachine.OnHit(2,intensity);
            Debug.Log("Jump!");
            EventCenter.EventCenter.Trigger(new Evt.EvtParamGpImpactInfo { dir = dir, intensity = intensity });
        }
        public void OnTriggerGP(Vector3 dir,float intensity)//��StateMachine ����һ����֡��������Ч���š�
        {
            stateMachine.OnHit(1,intensity);
            EventCenter.EventCenter.Trigger(new Evt.EvtParamGpImpactInfo { dir = dir, intensity = intensity });

        }
        public void OnTriggerHit(Vector3 dir,float intensity)//��stateMachin ����һ���������١�������Ч���š�
        {
            stateMachine.OnHit(0,intensity);
            EventCenter.EventCenter.Trigger(new Evt.EvtParamHitImpactInfo { dir = dir, intensity = intensity });

        }
        public void PlayGPSound()//���Ͳ�����Ƶ�¼���
        {

        }
        public void PlayHitSound()//���Ͳ�����Ƶ�¼�������Ϊ��Ƶ�ļ�����player���ϡ�����Ч�ļ���player���ϡ�
        {

        }
        public void PlayWaveSound(int intensity)//���Ż������Ƶ��
        {
            EventCenter.EventCenter.Trigger(new Evt.EvtParamOnAttackActive { attackIntensity = intensity });
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