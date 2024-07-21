using UnityEngine;
using UnityEngine.InputSystem;
using iysy.GameInput;
using System.Collections.Generic;
using System;
using MzUtility.FrameWork.EventSystem;
using Cysharp.Threading.Tasks;
using iysy.JustCut.Evt;

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
        public PlayerInputAction inputActions { get; private set; }
        //public PlayerInput playerInput { get; private set; }
        public override void InitInstanceBeforeAcquire()
        {
            base.InitInstanceBeforeAcquire();
            PlayerGameInputBuffer = new InputBufferTimeVarType<PlayerInputType>(defaultDurTime);
            inputActions = new PlayerInputAction();

            inputActions.Player.Enable();
            Debug.Log("����player");
         
            //playerInput.actions["LightAttack"].performed += OnLightAttack;
            inputActions.Player.LightAttack.performed += OnLightAttack;
            inputActions.Player.LightAttack.canceled += OnLightAttackRelease;
            //playerInput.actions["LightAttack"].canceled += OnLightAttackRelease;

            inputActions.Player.HeavyAttack.performed += OnHeavyAttack;
            inputActions.Player.HeavyAttack.canceled += OnHeavyAttackRelease;
            //playerInput.actions["HeavyAttack"].performed += OnHeavyAttack;
            //playerInput.actions["HeavyAttack"].canceled += OnHeavyAttackRelease;

            inputActions.Player.Roll.performed += OnRoll;
            //playerInput.actions["Roll"].performed += OnRoll;

            EventCenter.EventCenter.Register<Evt.EvtParamOnBattlePause>(OnBattlePause);
            EventCenter.EventCenter.Register<Evt.EvtParamOnBattleContinue>(OnBattleContinue);
            EventCenter.EventCenter.Register<Evt.EvtParamChangeSceneInfo>(OnSceneChange);
            //EventManager.Register<ChangeSceneEvent>(OnSceneChange);

        }

        public new void OnDestroy()
        {
            base.OnDestroy();
            if (inputActions == null)
                return;
            inputActions.Player.LightAttack.performed -= OnLightAttack;
            inputActions.Player.LightAttack.canceled -= OnLightAttackRelease;

            inputActions.Player.HeavyAttack.performed -= OnHeavyAttack;
            inputActions.Player.HeavyAttack.canceled -= OnHeavyAttackRelease;

            inputActions.Player.Roll.performed -= OnRoll;

            EventCenter.EventCenter.UnRegister<Evt.EvtParamOnBattlePause>(OnBattlePause);
            EventCenter.EventCenter.UnRegister<Evt.EvtParamOnBattleContinue>(OnBattleContinue);
            EventCenter.EventCenter.UnRegister<Evt.EvtParamChangeSceneInfo>(OnSceneChange);

        }
        private void OnSceneChange(EvtParamChangeSceneInfo obj)
        {
            switch (obj.sceneIndex)
            {
                case 0:
                    Debug.Log("�л��������棬ȡ������Player");
                    EnableUIDisablePlayer();
                    break;
                case 1:
                    Debug.Log("�л���ս�����棬����Player");
                    DisableUI();
                    if (obj.progress == 1)
                        DelayEnablePlayer();
                    break;
            }
        }
        private void OnBattlePause(EvtParamOnBattlePause obj)
        {
            Debug.Log("��Ϸ��ͣ��ȡ������player");
            EnableUIDisablePlayer();
        }

        private void OnBattleContinue(EvtParamOnBattleContinue obj)
        {
            Debug.Log("��Ϸ����������Player");
            EnablePlayerDisableUI();
        }
        private void DelayEnablePlayer()
        {
            inputActions.Player.Enable();
        }
        private void DisableUI() 
        {
            inputActions.UI.Disable();
        }
        private void EnablePlayerDisableUI()
        {
            inputActions.Player.Enable();
            DisableUI();
        }
        private void EnableUIDisablePlayer()
        {
            inputActions.Player.Disable();
            inputActions.UI.Enable();
        }

        //private void OnMove(InputAction.CallbackContext obj)
        //{
        //    Debug.Log("MovePerform"+obj);
        //}


        public void Update()
        {
            PlayerGameInputBuffer?.UpdateBuffer();
        }
        private void OnLightAttack(InputAction.CallbackContext obj)
        {
            Debug.Log("�����ṥ��");
            LightAttackPressing = true;
            PlayerGameInputBuffer.GetInput(PlayerInputType.LightAttackRelease);
            PlayerGameInputBuffer.AddInput(PlayerInputType.LightAttack, inputTypeEnum2DurTime[(int)PlayerInputType.LightAttack]);
            PlayerGameInputBuffer.DebugBuffer();
        }
        private void OnLightAttackRelease(InputAction.CallbackContext obj)
        {
            Debug.Log("�ɿ��ṥ��");
            LightAttackPressing = false;
            PlayerGameInputBuffer.AddInput(PlayerInputType.LightAttackRelease, inputTypeEnum2DurTime[(int)PlayerInputType.LightAttackRelease]);
        }

        private void OnHeavyAttack(InputAction.CallbackContext obj)
        {
            //Debug.Log("perform heavyattack");
            HeavyAttackPressing = true;
            PlayerGameInputBuffer.GetInput(PlayerInputType.HeavyAttackRelease);
            PlayerGameInputBuffer.AddInput(PlayerInputType.HeavyAttack, inputTypeEnum2DurTime[(int)PlayerInputType.HeavyAttack]);
            PlayerGameInputBuffer.DebugBuffer();
        }
        private void OnHeavyAttackRelease(InputAction.CallbackContext obj){
            //Debug.Log("cancle heavyattack");
            HeavyAttackPressing = false;
            PlayerGameInputBuffer.AddInput(PlayerInputType.HeavyAttackRelease, inputTypeEnum2DurTime[(int)PlayerInputType.HeavyAttackRelease]);
        }

        private void OnRoll(InputAction.CallbackContext obj)
        {
            PlayerGameInputBuffer.AddInput(PlayerInputType.Roll, inputTypeEnum2DurTime[(int)PlayerInputType.Roll]);
        }
        public Vector2 MoveAxis => inputActions.Player.Move.ReadValue<Vector2>();
        public float MoveAxisX => MoveAxis.x;
        public bool LightAttackPressing { get; private set; }
        public bool HeavyAttackPressing { get; private set; }
    }
}