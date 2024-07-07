using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// ����������Ч����������������Ч�����ٶȣ�����λ�ò���GP��Ч�ʹ����Ч
    /// </summary>
    public class PlayerVFXController : MonoBehaviour
    {
        [SerializeField] ParticleSystem[] AttackParticles;
        [SerializeField] List<float> silhouetteTime;
        [SerializeField] GameObject HitVFXPrefab;
        [SerializeField] GameObject GPVFXPrefab;
        int curIndex;
        private void Start()
        {
            foreach (var item in AttackParticles)
            {
                var main = item.main;
                main.playOnAwake = false;
            }
        }
        /// <summary>
        /// �������е����Ӷ�����
        /// </summary>
        /// <param name="index">0=>lightAttack1,1=>lightAttack2</param>
        public void PlaySlashParticle(int index)
        {
            var particle = AttackParticles[index];
            particle.Play();

            curIndex = index;
        }
        public void PlayHitParticle(Vector3 pos,Quaternion rot,Vector3 dir,int index = 0)
        {
            rot *= Quaternion.Euler(0, 0, -Vector2.Angle(Vector2.right, dir));
            StartCoroutine(DisableDelayGameObject(1.0f,Lean.LeanPool.Spawn(HitVFXPrefab, pos, rot)));
        }
        public void PlayGPParticle(Vector3 pos,Quaternion rot,Vector3 dir)
        {
            rot *= Quaternion.Euler(0, 0, -Vector2.Angle(Vector2.right, dir));
            StartCoroutine(DisableDelayGameObject(1.0f, Lean.LeanPool.Spawn(GPVFXPrefab, pos, rot)));
        }
        public void PlayHitParticle(Vector3 pos,  Vector3 dir, int index = 0)
        {
            Quaternion rot = this.transform.rotation;
            rot *= Quaternion.Euler(0, 0, -Vector2.Angle(Vector2.right, dir));
            StartCoroutine(DisableDelayGameObject(1.0f, Lean.LeanPool.Spawn(HitVFXPrefab, pos, rot)));
        }
        public void PlayGPParticle(Vector3 pos,  Vector3 dir)
        {
            Quaternion rot = this.transform.rotation;
            rot *= Quaternion.Euler(0, 0, -Vector2.Angle(Vector2.right, dir));
            StartCoroutine(DisableDelayGameObject(1.0f, Lean.LeanPool.Spawn(GPVFXPrefab, pos, rot)));
        }
        /// <summary>
        /// ǿ�������ؼ�֡��Ȼ����ͣ��
        /// </summary>
        public void JumpToSilhouette()
        {
            var curParticle = AttackParticles[curIndex];
            var main = curParticle.main;
            //curParticle.time = silhouetteTime[curIndex];
            curParticle.Simulate(silhouetteTime[curIndex], true, true);
        }
        /// <summary>
        /// �ӳ�һ��ʱ��󲥷ŵ�ǰ����ϵͳ��
        /// </summary>
        /// <param name="time"></param>
        public void StartDelaySlashParticle(float time)
        {
            StartCoroutine(StartDelaySlashParticleCor(time));
        }
        private IEnumerator StartDelaySlashParticleCor(float time)
        {
            yield return new WaitForSeconds(time);
            AttackParticles[curIndex].Play();
        }
        public void StopSlashParticleForAWhile(float time)
        {
            StartCoroutine(StopSlashParticleCor(time));
        }
        private IEnumerator StopSlashParticleCor(float time)
        {
            var curParticle = AttackParticles[curIndex];
            curParticle.Pause(true);
            yield return new WaitForSeconds(time);
            curParticle.Play();
        }
        public IEnumerator DisableDelayGameObject(float time,GameObject gameObject)
        {
            yield return new WaitForSeconds(time);
            Lean.LeanPool.Despawn(gameObject);
        }
    }
    [CustomEditor(typeof(PlayerVFXController))]
    public class PlayerVFXControllerEditor:Editor
    {
        PlayerVFXController vfxC;
        private void OnEnable()
        {
            vfxC = target as PlayerVFXController;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("����ǰ���ŵ�������ת���ؼ�֡"))
            {
                vfxC.JumpToSilhouette();
            }
            if (GUILayout.Button("��ͣ��ǰ���ŵ�������Чһ��"))
            {
                vfxC.StopSlashParticleForAWhile(1);
            }
            if (GUILayout.Button("������ͣ��������Ч"))
            {
                vfxC.StopSlashParticleForAWhile(0);
            }
        }
    }
}