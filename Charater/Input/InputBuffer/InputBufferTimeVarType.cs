using System;
using System.Collections.Generic;
using UnityEngine;

namespace iysy.GameInput
{

    /// <summary>
    /// 不同种输入的持续时间不同。
    /// </summary>
    public class InputBufferTimeVarType<InputType> : IInputBuffer<InputType> where InputType : struct, System.Enum
    {
        Dictionary<InputType, float> type2DurTime;
        float defaultDurTime;
        struct InputData
        {
            public InputType inputType;
            public float enterTime;
            public float endTime;

            public override string ToString()
            {
                return inputType.ToString();
            }
        }
        LinkedList<InputData> buffer;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultDurTime">没有遇到匹配的类型时的默认时间</param>
        public InputBufferTimeVarType(float defaultDurTime)
        {
            type2DurTime = new Dictionary<InputType, float>();
            buffer = new LinkedList<InputData>();
            this.defaultDurTime = defaultDurTime;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type2DurTime"></param>
        /// <param name="defalultDurTime">没有遇到字典匹配的类型时的默认时间</param>
        public InputBufferTimeVarType(Dictionary<InputType,float> type2DurTime,float defaultDurTime)
        {
            this.type2DurTime = type2DurTime;
            if(this.type2DurTime == null)
            {
                type2DurTime = new Dictionary<InputType, float>();
            }
            buffer = new LinkedList<InputData>();
            this.defaultDurTime = defaultDurTime;
        }
        /// <summary>
        /// 加入输入进buffer中，顺便可以改变对应类型输入的持续时间。
        /// </summary>
        /// <param name="inputType">输入类型</param>
        /// <param name="durTime">当赋值大于等于0时，会刷新该输入类型对应的持续时间，但是已经在Buffer中的时间不变。</param>
        public void AddInput(InputType inputType, float durTime = -1)
        {
            if(durTime>= 0)
            {
                if (type2DurTime.ContainsKey(inputType))
                {
                    type2DurTime[inputType] = durTime;
                }
                else
                {
                    type2DurTime.Add(inputType, durTime);
                }
            }

            var newNode = new LinkedListNode<InputData>(new InputData
            {
                inputType = inputType,
                enterTime = Time.time,
                endTime = Time.time +( type2DurTime.ContainsKey(inputType) ? type2DurTime[inputType] : defaultDurTime)
            });
            buffer.AddFirst(newNode);
        }
        /// <summary>
        /// 从buffer中取出对应的输入。
        /// </summary>
        /// <param name="inputType"></param>
        /// <returns>有没有对应的输入</returns>
        public bool GetInput(InputType inputType)
        {
            //从前往后遍历，获取第一个有效node。
            var node = buffer.First;
            while(node != null)
            {
                if (node.Value.inputType.Equals(inputType))
                {
                    buffer.Remove(node);
                    return true;
                }
                else
                {
                    node = node.Next;
                }
            }
            return false;
        }

        public void UpdateBuffer()
        {
            //从后往前遍历，去掉每个超时的node;
            var lastP = buffer.Last;
            var PToRemove = lastP;
            float curTime = Time.time;
            while(lastP != null)
            {
                if(curTime >= lastP.Value.endTime)
                {
                    PToRemove = lastP;
                    lastP = lastP.Previous;
                    buffer.Remove(PToRemove);
                }
                else
                {
                    lastP = lastP.Previous;
                }
            }
        }
        public string DebugBuffer()
        {
            string format = string.Empty;
            foreach (var inputData in buffer)
            {
                format += (inputData.ToString() + " ");
            }
            Debug.Log(format);

            return format;
        }
    }
}