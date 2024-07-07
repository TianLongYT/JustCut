using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// ���𶯻��������á��ؼ��¼��ص���
    /// </summary>
    public class PlayerAnimationController:MonoBehaviour
    {
        Animator anim;
        AnimationData animData;
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
        }
        float animSpeed;
        private void Update()
        {
            anim.speed = animSpeed * GameWorld.Instance.TimeScale;
        }
        /// <summary>
        /// �������߻��ܲ�����
        /// </summary>
        /// <param name="curSpeed">�ٶȿ�����-1~1��Χ��</param>
        public void PlayMoveAnim(float moveRate)
        {
            //moveRate = MMath.Math.SmoothStep(-1, 1, moveRate) * 2 - 1f;
            //Ҫ��취���м���ٶȹ��ȵ���һЩ��
            float finalMoveRate = moveRate<0? moveRate * 3.6f: moveRate * 5.0f;
            anim.SetFloat(animData.MoveRate, finalMoveRate);
        }
        /// <summary>
        /// ��ʱ��Animator.Tf.Parent.setRotation��ת�򡣡�������
        /// </summary>
        /// <param name="newfaceDir"></param>
        public void PlayFilpAnim(float newfaceDir)
        {
            if (faceDir == newfaceDir)
                return;
            faceDir = newfaceDir;
            anim.transform.parent.rotation = Quaternion.Euler(0, faceDir > 0 ? 0 : 180, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attackIndex">�밴����������</param>
        // //public void PlayLightAttack(int attackIndex)
        // {
        //     anim.SetTrigger(animData.attackAnimDatas[attackIndex].animName);

        //     curAttackMode = attackIndex;
        // }
        public void TriggerMove()
        {
            anim.SetTrigger(animData.Move);
        }
        public void TriggerLightAttack1(){
            anim.SetTrigger(animData.LightAttack1);
        }
        public void TriggerLightAttack2()
        {
            anim.SetTrigger(animData.LightAttack2);
        }
        public void TriggerHeavyAttack1Enter(){
            anim.SetTrigger(animData.HA1Enter);
        }
        public void TriggerHeavyAttack1Release()
        {
            anim.SetTrigger(animData.HA1Exit);
        }
        public void TriggerHeavyAttack2Enter()
        {
            anim.SetTrigger(animData.HA2Enter);
        }
        public void TriggerHeavyAttack2Release()
        {
            anim.SetTrigger(animData.HA2Exit);
        }
        public void TriggerHeavyAttack3Enter()
        {
            anim.SetTrigger(animData.HA3Enter);
        }
        public void TriggerHeavyAttack3Release()
        {
            anim.SetTrigger(animData.HA3Exit);
        }
        public void TriggerSuperAttackEnter()
        {
            anim.SetTrigger(animData.SAEnter);
        }
        public void TriggerSuperAttackRelease()
        {
            anim.SetTrigger(animData.SAExit);
        }
        public void TriggerRoll()
        {
            anim.SetTrigger(animData.Roll);
        }

        /// <summary>
        /// �Զ������Ž���һ������Ȼ����ٵĹ��̡�ģ�⿨��С�
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
            //Debug.Log("���ò����ٶ�" + GameWorld.Instance.TimeScale * animData.slowDownRate);
            yield return new WaitForWorldSeconds(animData.animSlowDownTime);

        }
        private IEnumerator SpeedUp()
        {
            animSpeed = animData.speedUpRate;
            //Debug.Log("���ò����ٶ�" + GameWorld.Instance.TimeScale * animData.speedUpRate);
            yield return new WaitForWorldSeconds(animData.animSpeedUpTime);
            animSpeed = 1;
            //Debug.Log("���ò����ٶ�" + 1f);
        }
        /// <summary>
        /// ������Ҫ֪����ǰ�������ģ�Ȼ�����animaData,��ת���ؼ�֡�����عؼ�֡������ʱ�䡣���������֡��״̬���м����֡ʱ�䡣
        /// </summary>
        /// <param name="nextAttackIndex">�밴����������</param>
        public float JumpToKeyFrame(int nextAttackIndex,float curAnimTime)
        {
            //��ǰƬ������
            //var currentName = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;ʹ�����������ȡ�������ڲ��ŵ���һ����������ֻ������һ��������
            var animName = animData.attackAnimDatas[nextAttackIndex].animName;
            var animKeyFrameTime = animData.attackAnimDatas[nextAttackIndex].keyFrameTime;
            //var animLength = anim.GetNextAnimatorClipInfo(0)[0].clip.length;//�����ַ�ʽ�ǲ��Եģ���Ϊ��ȷ��ʲôʱ����GP������Ƕ�������һ�����gp����ʱ����ת��������ɣ��Ҳ�����һ������Ƭ�Ρ�
            var animLength = animData.attackAnimDatas[nextAttackIndex].animLength;
            Debug.Log("��һ��������" + animName);//���������ַ�����ȡ�Ķ���Ƭ���ǶԵġ���һ����ֻ�Ƕ����պô���ת��״̬����ʵ��׼��
            StartCoroutine(JumpToKeyFrameCor(animName, 0, curAnimTime / animLength, animKeyFrameTime / animLength, 3));
            return animKeyFrameTime;
        }

        private IEnumerator JumpToKeyFrameCor(string animName,int layer,float normalizedTimeStart,float normalizedTimeEnd,int frameCount)
        {
            //��ȡ�ڼ�֡�ڿ�����궯����
            float deltaNor = (normalizedTimeEnd - normalizedTimeStart) / frameCount;
            for (int i = 0; i < frameCount; i++)
            {
                yield return 0;
                anim.Play(animName, layer, normalizedTimeStart + i * deltaNor);
            }
        }
        private int GetCurAnimFrame(Animator anim)
        {
            //��ǰ����������ʱ��
            var currentTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
            //����Ƭ�γ���
            float length = anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            //��ȡ����Ƭ��֡Ƶ
            float frameRate = anim.GetCurrentAnimatorClipInfo(0)[0].clip.frameRate;
            //���㶯��Ƭ����֡��
            float totalFrame = length / (1 / frameRate);
            //���㵱ǰ���ŵĶ���Ƭ����������һ֡
            int currentFrame = (int)(Mathf.Floor(totalFrame * currentTime) % totalFrame);
            return currentFrame;
        }
    }

}