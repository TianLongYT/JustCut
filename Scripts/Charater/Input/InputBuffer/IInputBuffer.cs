using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.GameInput
{

    /// <summary>
    /// 记录Input队列，获取关注的输入对象。对每次输入数据进行缓存。
    /// </summary>
    public interface IInputBuffer<InputType> where InputType : struct, System.Enum
    {
        public void AddInput(InputType inputType,float durTime = -1);

        public void UpdateBuffer();

        public bool GetInput(InputType inputType);
    }
}