using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.GameInput
{

    /// <summary>
    /// 所有输入具有相同持续时间。
    /// </summary>
    public class InputBufferFixedDurTime<InputType>:IInputBuffer<InputType>where InputType : struct, System.Enum
    {
        float durTime;
        public InputBufferFixedDurTime(float durTime)
        {
            this.durTime = durTime;
            buffer = new LinkedList<InputData>();
        }
        struct InputData
        {
            public InputType inputType;
            public float enterTime;

            public override string ToString()
            {
                return base.ToString();
            }
        }
        LinkedList<InputData> buffer;


        public void AddInput(InputType inputType,float durTime = -1)
        {
            var newNode = new LinkedListNode<InputData>(new InputData() { inputType = inputType, enterTime = Time.time });
            buffer.AddFirst(newNode);
            //最后输入的数据在链表的最前。更新时间时，最好从后往前更新数据。读取输入时最好从前往后读取。
        }

        public void UpdateBuffer()
        {
            //更新时间时，最好从后往前更新数据,因为最先输入的数据在最后。
            var lastP = buffer.Last;
            float time = Time.time;
            float nodeTime = 0;
            while (lastP != null)
            {
                nodeTime = lastP.Value.enterTime + durTime;
                if (time > nodeTime)//过时间了，去掉最后的节点。
                {
                    lastP = lastP.Previous;
                    buffer.RemoveLast();
                }
                else//最后的节点时间没过。说明前面所有的节点时间都没过。
                {
                    break;
                }
            }

        }
        public bool CheckInput(InputType inputType)
        {
            foreach (var inputData in buffer)
            {
                if(inputData.inputType.Equals(inputType))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取输入并且把输入从buffer中取出。
        /// </summary>
        /// <returns>buffer中是否有这个输入</returns>
        public bool GetInput(InputType inputType)
        {
            var node = buffer.First;
            while(node!= null)
            {
                if(node.Value.inputType.Equals(inputType))
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
        public string DebugBuffer()
        {
            string format = string.Empty;
            foreach (var inputData in buffer)
            {
                format += inputData.ToString() + " ";
            }
            Debug.Log(format);
            return format;
        }

    }
}