using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// ����һ����MotionController������BT����һ��㸺����
    /// </summary>
    public class EnemyController : MonoBehaviour
    {
        MotionController2D.MotionController2D motionController;
        private void Start()
        {
            motionController = GetComponent<MotionController2D.MotionController2D>();
            motionController.LoadComponents();
            motionController.OnReInitialize();
            GameWorld.Instance.OnUpdate += OnUpdate;
        }

        private void OnUpdate(float obj)
        {
            motionController.OnUpdate(obj);
        }
        private void FixedUpdate()
        {
            var timescale = GameWorld.Instance.TimeScale;
            motionController.UpdateVelocity(timescale);
            motionController.OnFixedUpdate(Time.fixedDeltaTime);
        }
    }
}