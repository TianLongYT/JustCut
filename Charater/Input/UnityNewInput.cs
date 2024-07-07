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
        /// 构建输入系统时，需要传入默认的Map.
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