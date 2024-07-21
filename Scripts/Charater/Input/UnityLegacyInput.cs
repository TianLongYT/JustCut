using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.GameInput
{

    /// <summary>
    /// 
    /// </summary>
    public class UnityLegacyInput : IUnityInput
    {
        public Vector2 MoveAxies => new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));

        public bool JumpPress => Input.GetButtonDown("Jump");

        public bool JumpStop => Input.GetButtonUp("Jump");

        public bool JumpHold => Input.GetButton("Jump");
    }
}