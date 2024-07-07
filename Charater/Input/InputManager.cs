using UnityEngine;
using UnityEngine.InputSystem;
using iysy.GameInput;
using System.Collections.Generic;
using System;

namespace iysy.JustCut
{

    /// <summary>
    /// ���������Ϸ���룬�л���Ч���룬������뻺�壬�ṩ���뻺�塣
    /// </summary>
    public class InputManager:SingletonBehaviourNoNewGO<InputManager>
    {
        [SerializeField] float defaultDurTime = 0.15f;
        [SerializeField] List<float> inputTypeEnum2DurTime;
        public InputBufferTimeVarType<PlayerInputType> PlayerGameInputBuffer { get; private set; }
        PlayerInputAction inputActions;
        public override void InitInstanceBeforeAcquire()
        {
            base.InitInstanceBeforeAcquire();
            PlayerGameInputBuffer = new InputBufferTimeVarType<PlayerInputType>(defaultDurTime);
            inputActions = new PlayerInputAction();

            inputActions.Player.Enable();
            inputActions.Player.LightAttack.performed += OnLightAttack;
            inputActions.Player.HeavyAttack.performed += OnHeavyAttack;
        }


        //private void OnMove(InputAction.CallbackContext obj)
        //{
        //    Debug.Log("MovePerform"+obj);
        //}

        public new void OnDestroy()
        {
            base.OnDestroy();


        }
        public void Update()
        {
            PlayerGameInputBuffer?.UpdateBuffer();
        }
        private void OnLightAttack(InputAction.CallbackContext obj)
        {
            Debug.Log("�����ṥ��");
            PlayerGameInputBuffer.AddInput(PlayerInputType.LightAttack, inputTypeEnum2DurTime[(int)PlayerInputType.LightAttack]);
            PlayerGameInputBuffer.DebugBuffer();
        }
        private void OnHeavyAttack(InputAction.CallbackContext obj)
        {
            PlayerGameInputBuffer.AddInput(PlayerInputType.HeavyAttack, inputTypeEnum2DurTime[(int)PlayerInputType.HeavyAttack]);
            //PlayerGameInputBuffer.DebugBuffer();
        }
        private void OnHeavyAttackRelease(InputAction.CallbackContext obj){
            PlayerGameInputBuffer.AddInput(PlayerInputType.HeavyAttack, inputTypeEnum2DurTime[(int)PlayerInputType.HeavyAttackRelease]);
        }
        public Vector2 MoveAxis => inputActions.Player.Move.ReadValue<Vector2>();
        public float MoveAxisX => MoveAxis.x;

    }
}