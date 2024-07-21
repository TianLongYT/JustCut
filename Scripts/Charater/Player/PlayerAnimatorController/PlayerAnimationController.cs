using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 负责动画参数调用。关键事件回调。
    /// </summary>
    public class PlayerAnimationController:MonoBehaviour
    {
        //所有的触发器同一帧只能触发一次。防止有触发器没有被实现。
        Animator anim;
        AnimationData animData;
        AnimationData.AnimTrigger curAnimTrigger;
        Dictionary<string, float> animName2KeyFrameTime;
        float faceDir;
        public void Initialize(Animator animator,AnimationData animationData)
        {
            anim = animator;
            animData = animationData;
            faceDir = 1;
            animSpeed = 1;
            animName2KeyFrameTime = new Dictionary<string, float>(animData.attackAnimDatas.Length);
            foreach (var attackAnimData in animationData.attackAnimDatas)
            {
                animName2KeyFrameTime.Add(attackAnimData.animName, attackAnimData.keyFrameTime);
            }
            curAnimTrigger = AnimationData.AnimTrigger.None;
        }
        float animSpeed;
        private void Update()
        {
            anim.speed = animSpeed * GameWorld.Instance.TimeScale;
            if (curAnimTrigger != AnimationData.AnimTrigger.None)
            {
                anim.SetTrigger(curAnimTrigger.ToString());
                curAnimTrigger = AnimationData.AnimTrigger.None;
            }
        }
        /// <summary>
        /// 播放行走或跑步动画
        /// </summary>
        /// <param name="curSpeed">速度控制在-1~1范围内</param>
        public void PlayMoveAnim(float moveRate)
        {
            //moveRate = MMath.Math.SmoothStep(-1, 1, moveRate) * 2 - 1f;
            //要想办法让中间的速度过度得慢一些。
            float finalMoveRate = moveRate<0? moveRate * 3.6f: moveRate * 5.0f;
            anim.SetFloat(animData.MoveRate, finalMoveRate);
        }
        /// <summary>
        /// 播放受击动画
        /// </summary>
        public void PlayHurtAnim()
        {
            curAnimTrigger = AnimationData.AnimTrigger.Hurt;
            //anim.SetTrigger(animData.Hurt);
        }
        /// <summary>
        /// 暂时用Animator.Tf.Parent.setRotation来转向。。。。。
        /// </summary>
        /// <param name="newfaceDir"></param>
        public void PlayFilpAnim(float newfaceDir)
        {
            if (faceDir == newfaceDir)
                return;
            else if (newfaceDir == 0)
                return;
            faceDir = newfaceDir;
            anim.transform.parent.rotation = Quaternion.Euler(0, faceDir > 0 ? 0 : 180, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attackIndex">请按照数组输入</param>
        // //public void PlayLightAttack(int attackIndex)
        // {
        //     anim.SetTrigger(animData.attackAnimDatas[attackIndex].animName);

        //     curAttackMode = attackIndex;
        // }
        public void TriggerMove()
        {
            curAnimTrigger = AnimationData.AnimTrigger.Move;
            //anim.SetTrigger(animData.Move);
        }
        public void TriggerLightAttack1(){
            curAnimTrigger = AnimationData.AnimTrigger.LightAttack1;
            //anim.SetTrigger(animData.LightAttack1);
        }
        public void TriggerLightAttack2()
        {
            curAnimTrigger = AnimationData.AnimTrigger.LightAttack2;
            //anim.SetTrigger(animData.LightAttack2);
        }
        public void TriggerHeavyAttack1Enter(){
            curAnimTrigger = AnimationData.AnimTrigger.HA1Enter;
           // anim.SetTrigger(animData.HA1Enter);
        }
        public void TriggerHeavyAttack1Release()
        {
            curAnimTrigger = AnimationData.AnimTrigger.HA1Exit;
          //  anim.SetTrigger(animData.HA1Exit);
        }
        public void TriggerHeavyAttack2Enter()
        {
            curAnimTrigger = AnimationData.AnimTrigger.HA2Enter;
          //  anim.SetTrigger(animData.HA2Enter);
        }
        public void TriggerHeavyAttack2Release()
        {
            curAnimTrigger = AnimationData.AnimTrigger.HA2Exit;
          //  anim.SetTrigger(animData.HA2Exit);
        }
        public void TriggerHeavyAttack3Enter()
        {
            curAnimTrigger = AnimationData.AnimTrigger.HA3Enter;
           // anim.SetTrigger(animData.HA3Enter);
        }
        public void TriggerHeavyAttack3Release()
        {
            curAnimTrigger = AnimationData.AnimTrigger.HA3Exit;
           // anim.SetTrigger(animData.HA3Exit);
        }
        public void TriggerSuperAttackEnter()
        {
            curAnimTrigger = AnimationData.AnimTrigger.SAEnter;
            //anim.SetTrigger(animData.SAEnter);
        }
        public void TriggerSuperAttackRelease()
        {
            curAnimTrigger = AnimationData.AnimTrigger.SAExit;
            //anim.SetTrigger(animData.SAExit);
        }
        public void TriggerRoll()
        {
            curAnimTrigger = AnimationData.AnimTrigger.Roll;
            //anim.SetTrigger(animData.Roll);
        }
        public void TriggerRollLightAttack()
        {
            curAnimTrigger = AnimationData.AnimTrigger.RollLightAttack;
            //anim.SetTrigger(animData.RollLightAttack);
        }
        public void TriggerRollHeavyAttackRelease()
        {
            curAnimTrigger = AnimationData.AnimTrigger.HeavyAttackRelease;
            //anim.SetTrigger(animData.HeavyAttackRelease);
        }
        public void TriggerRollSuperAttackRelease()
        {
            curAnimTrigger = AnimationData.AnimTrigger.SuperAttackRelease;
            //anim.SetTrigger(animData.SuperAttackRelease);
        }


        /// <summary>
        /// 对动画播放进行一个减速然后加速的过程。模拟卡肉感。
        /// </summary>
        public void StuckAnimation()
        {
            StartCoroutine(StuckAnimCor());
        }
        private IEnumerator StuckAnimCor()
        {
            yield return SlowDown();
            yield return SpeedUp();
        }
        private IEnumerator SlowDown()
        {
            animSpeed = animData.slowDownRate;
            //Debug.Log("设置播放速度" + GameWorld.Instance.TimeScale * animData.slowDownRate);
            yield return new WaitForWorldSeconds(animData.animSlowDownTime);

        }
        private IEnumerator SpeedUp()
        {
            animSpeed = animData.speedUpRate;
            //Debug.Log("设置播放速度" + GameWorld.Instance.TimeScale * animData.speedUpRate);
            yield return new WaitForWorldSeconds(animData.animSpeedUpTime);
            animSpeed = 1;
            //Debug.Log("设置播放速度" + 1f);
        }
        /// <summary>
        /// 动画需要知道当前动画在哪，然后根据animaData,跳转到关键帧，返回关键帧动画的时间。请根据自身帧数状态自行计算顿帧时间。
        /// </summary>
        /// <param name="nextAttackIndex">请按照数组输入</param>
        public float JumpToKeyFrame(int nextAttackIndex,float curAnimTime)
        {
            //当前片段名称
            //var currentName = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;使用这个方法获取的是正在播放的上一个动画，且只会获得这一个动画。
            var animName = animData.attackAnimDatas[nextAttackIndex].animName;
            var animKeyFrameTime = animData.attackAnimDatas[nextAttackIndex].keyFrameTime;
            //var animLength = anim.GetNextAnimatorClipInfo(0)[0].clip.length;//用这种方式是不对的，因为不确定什么时候发生GP。如果是动画播放一半出现gp，此时动画转换基本完成，找不到下一个动画片段。
            var animLength = animData.attackAnimDatas[nextAttackIndex].animLength;
            //Debug.Log("下一个动画是" + animName);//好像用这种方法获取的动画片段是对的。不一定，只是动画刚好处于转换状态，其实不准。
            if(curAnimTime < animKeyFrameTime)
                StartCoroutine(JumpToKeyFrameCor(animName, 0, curAnimTime / animLength, animKeyFrameTime / animLength, 3));
            return animKeyFrameTime;
        }

        private IEnumerator JumpToKeyFrameCor(string animName,int layer,float normalizedTimeStart,float normalizedTimeEnd,int frameCount)
        {
            //争取在几帧内快进播完动画。
            float deltaNor = (normalizedTimeEnd - normalizedTimeStart) / frameCount;
            for (int i = 0; i < frameCount; i++)
            {
                yield return 0;
                anim.Play(animName, layer, normalizedTimeStart + i * deltaNor);
            }
        }
        private int GetCurAnimFrame(Animator anim)
        {
            //当前动画机播放时长
            var currentTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
            //动画片段长度
            float length = anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            //获取动画片段帧频
            float frameRate = anim.GetCurrentAnimatorClipInfo(0)[0].clip.frameRate;
            //计算动画片段总帧数
            float totalFrame = length / (1 / frameRate);
            //计算当前播放的动画片段运行至哪一帧
            int currentFrame = (int)(Mathf.Floor(totalFrame * currentTime) % totalFrame);
            return currentFrame;
        }
    }

}