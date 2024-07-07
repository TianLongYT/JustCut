using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.GameInput
{

    /// <summary>
    /// 
    /// </summary>
    public interface IUnityInput
    {
        Vector2 MoveAxies { get; }
        bool JumpPress { get; }
        bool JumpStop { get; }
        bool JumpHold { get; }


    }
}