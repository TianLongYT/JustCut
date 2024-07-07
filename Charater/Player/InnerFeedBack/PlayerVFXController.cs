using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 产生粒子特效，调整刀光粒子特效播放速度，按照位置播放GP特效和打击特效
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
        /// 播放已有的粒子动画。
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
        /// 强制跳到关键帧。然后暂停。
        /// </summary>
        public void JumpToSilhouette()
        {
            var curParticle = AttackParticles[curIndex];
            var main = curParticle.main;
            //curParticle.time = silhouetteTime[curIndex];
            curParticle.Simulate(silhouetteTime[curIndex], true, true);
        }
        /// <summary>
        /// 延迟一段时间后播放当前粒子系统。
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
            if (GUILayout.Button("将当前播放的粒子跳转到关键帧"))
            {
                vfxC.JumpToSilhouette();
            }
            if (GUILayout.Button("暂停当前播放的粒子特效一秒"))
            {
                vfxC.StopSlashParticleForAWhile(1);
            }
            if (GUILayout.Button("播放暂停的粒子特效"))
            {
                vfxC.StopSlashParticleForAWhile(0);
            }
        }
    }
}