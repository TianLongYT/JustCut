using System;
using System.Collections.Generic;
using UnityEngine;

namespace iysy.GameInput
{

    /// <summary>
    /// ��ͬ������ĳ���ʱ�䲻ͬ��
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
        /// <param name="defaultDurTime">û������ƥ�������ʱ��Ĭ��ʱ��</param>
        public InputBufferTimeVarType(float defaultDurTime)
        {
            type2DurTime = new Dictionary<InputType, float>();
            buffer = new LinkedList<InputData>();
            this.defaultDurTime = defaultDurTime;

            inputType2LockData = new Dictionary<InputType, lockInputData>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type2DurTime"></param>
        /// <param name="defalultDurTime">û�������ֵ�ƥ�������ʱ��Ĭ��ʱ��</param>
        public InputBufferTimeVarType(Dictionary<InputType,float> type2DurTime,float defaultDurTime)
        {
            this.type2DurTime = type2DurTime;
            if(this.type2DurTime == null)
            {
                type2DurTime = new Dictionary<InputType, float>();
            }
            buffer = new LinkedList<InputData>();
            this.defaultDurTime = defaultDurTime;

            inputType2LockData = new Dictionary<InputType, lockInputData>();
        }
        /// <summary>
        /// ���������buffer�У�˳����Ըı��Ӧ��������ĳ���ʱ�䡣
        /// </summary>
        /// <param name="inputType">��������</param>
        /// <param name="durTime">����ֵ���ڵ���0ʱ����ˢ�¸��������Ͷ�Ӧ�ĳ���ʱ�䣬�����Ѿ���Buffer�е�ʱ�䲻�䡣</param>
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
        struct lockInputData
        {
            public bool locked;
            public Action involveAction;//�ж��Ƿ���������á����ڻ�ȡ����֮����ã������������źŵ����жϲ��ܻ�ȡ�������á�
        }
        Dictionary<InputType, lockInputData> inputType2LockData;
        public void LockInput(InputType inputType,Action InvolveInputAction = null)
        {
            var newLockInputData = new lockInputData { locked = true, involveAction = InvolveInputAction };
            if (inputType2LockData.ContainsKey(inputType))
            {
                inputType2LockData[inputType] = newLockInputData;
            }
            else
            {
                inputType2LockData.Add(inputType,newLockInputData);
            }
        }
        public void UnLockInput(InputType inputType, Action InvolveInputAction = null)
        {
            var newLockInputData = new lockInputData { locked = false, involveAction = InvolveInputAction };
            if (inputType2LockData.ContainsKey(inputType))
            {
                inputType2LockData[inputType] = newLockInputData;
            }
            else
            {
                inputType2LockData.Add(inputType, newLockInputData);
            }
        }
        /// <summary>
        /// ��buffer��ȡ����Ӧ�����롣
        /// </summary>
        /// <param name="inputType"></param>
        /// <returns>��û�ж�Ӧ������</returns>
        public bool GetInput(InputType inputType)
        {

            //��ǰ�����������ȡ��һ����Чnode��
            var node = buffer.First;
            while(node != null)
            {
                if (node.Value.inputType.Equals(inputType))
                {

                    if (inputType2LockData != null && inputType2LockData.ContainsKey(inputType))
                    {
                        if (inputType2LockData[inputType].locked)
                        {
                            Debug.Log("�����Ѿ�����");
                            inputType2LockData[inputType].involveAction?.Invoke();
                            return false;

                        }
                        else
                        {
                            inputType2LockData[inputType].involveAction?.Invoke();
                            buffer.Remove(node);
                            return true;
                        }
                    }
                    else
                    {
                        buffer.Remove(node);
                        return true;
                    }

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
            //�Ӻ���ǰ������ȥ��ÿ����ʱ��node;
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