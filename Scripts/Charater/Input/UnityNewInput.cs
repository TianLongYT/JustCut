using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.GameInput
{

    /// <summary>
    /// 
    /// </summary>
    public class UnityNewInput : IUnityInput
    {
        PlayerInputAction inputActions;
        /// <summary>
        /// ��������ϵͳʱ����Ҫ����Ĭ�ϵ�Map.
        /// </summary>
        public UnityNewInput()
        {
            inputActions = new PlayerInputAction();
            ///
            //inputActions.Player.Enable();
        }
        public Vector2 MoveAxies => throw new System.NotImplementedException();

        public bool JumpPress => throw new System.NotImplementedException();

        public bool JumpStop => throw new System.NotImplementedException();

        public bool JumpHold => throw new System.NotImplementedException();
    }
}