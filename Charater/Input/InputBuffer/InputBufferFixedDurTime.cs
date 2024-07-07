using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace iysy.GameInput
{

    /// <summary>
    /// �������������ͬ����ʱ�䡣
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
            //���������������������ǰ������ʱ��ʱ����ôӺ���ǰ�������ݡ���ȡ����ʱ��ô�ǰ�����ȡ��
        }

        public void UpdateBuffer()
        {
            //����ʱ��ʱ����ôӺ���ǰ��������,��Ϊ������������������
            var lastP = buffer.Last;
            float time = Time.time;
            float nodeTime = 0;
            while (lastP != null)
            {
                nodeTime = lastP.Value.enterTime + durTime;
                if (time > nodeTime)//��ʱ���ˣ�ȥ�����Ľڵ㡣
                {
                    lastP = lastP.Previous;
                    buffer.RemoveLast();
                }
                else//���Ľڵ�ʱ��û����˵��ǰ�����еĽڵ�ʱ�䶼û����
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
        /// ��ȡ���벢�Ұ������buffer��ȡ����
        /// </summary>
        /// <returns>buffer���Ƿ����������</returns>
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