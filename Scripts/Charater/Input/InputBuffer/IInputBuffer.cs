using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.GameInput
{

    /// <summary>
    /// ��¼Input���У���ȡ��ע��������󡣶�ÿ���������ݽ��л��档
    /// </summary>
    public interface IInputBuffer<InputType> where InputType : struct, System.Enum
    {
        public void AddInput(InputType inputType,float durTime = -1);

        public void UpdateBuffer();

        public bool GetInput(InputType inputType);
    }
}