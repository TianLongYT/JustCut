using MzUtility.FrameWork.EventSystem;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 接收来自敌人的打击信息，处理后，调用PlayerInfo(扣血，增长架势),和StateMachine(受伤、死亡);
    /// 发送自己的打击信息，（需要基础打击信息，打击强度，攻击力，计算实时打击信息，打击位置，打击方向，击中状态）
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
            //把打击信息传入对应的打击脚本中。
            type2HitBox = new Dictionary<MotionType, PlayerAttackHitBox>(attackPasses.Count);
            foreach (var item in attackPasses)
            {
                if (type2HitBox.ContainsKey(item.attackMotionType))
                {
                    Debug.LogError("PlayerAttackHitPass不允许含有两个相同的MotionType");
                    return;
                }
                item.hitBox.Initialize(attackModel.GetMotionKeyFrameData(item.attackMotionType),this);
                type2HitBox.Add(item.attackMotionType, item.hitBox);
            }
            init = true;
        }
        public void SetStateMachine(PlayerStateMachine stateMachine) => this.stateMachine = stateMachine;

        public void Hit(AttackData attackData)//有可能需要通知StateMachine进入受击状态。
        {
            //处理来自敌人的攻击。
            Debug.Log("处理来自敌人的攻击" +" 攻击强度"+ attackData.attackModel.GetFinalIntensity());
            var res = stateMachine.OnInteract(attackData);
            //GP攻击
            if (res == true)//玩家处于GP无敌状态。
            {
                if (attackData.hitBody)
                    return;//击中翻滚的玩家。
                int intensityMinus = (attackData.attackModel.GetFinalIntensity() - attackModel.GetMotionKeyFrameData(curMotionType).GetFinalIntensity());
                playerInfo.SetPostrue(intensityMinus);
                Debug.Log("敌人击中玩家GP框");
                return;
            }
            //敌人击中玩家的受击框
            else
            {
                playerInfo.CurHP -= attackData.attackModel.baseDamage;
                Debug.Log("玩家被击中了！扣血" + attackData.attackModel.baseDamage);
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
        public void JumpToHitActiveFrame(Vector3 dir,float intensity)//让stateMachine进行动画跳帧，然后进行顿帧计算。如果这个执行了，那么GP判定就不会执行。
        {
            stateMachine.OnHit(2,intensity);
            Debug.Log("Jump!");
            EventCenter.EventCenter.Trigger(new Evt.EvtParamGpImpactInfo { dir = dir, intensity = intensity });
        }
        public void OnTriggerGP(Vector3 dir,float intensity)//让StateMachine 进行一个顿帧。进行特效播放。
        {
            stateMachine.OnHit(1,intensity);
            EventCenter.EventCenter.Trigger(new Evt.EvtParamGpImpactInfo { dir = dir, intensity = intensity });

        }
        public void OnTriggerHit(Vector3 dir,float intensity)//让stateMachin 进行一个动画减速。进行特效播放。
        {
            stateMachine.OnHit(0,intensity);
            EventCenter.EventCenter.Trigger(new Evt.EvtParamHitImpactInfo { dir = dir, intensity = intensity });

        }
        public void PlayGPSound()//发送播放音频事件。
        {

        }
        public void PlayHitSound()//发送播放音频事件。（因为音频文件不在player身上。而特效文件在player身上。
        {

        }
        public void PlayWaveSound(int intensity)//播放挥舞的音频。
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