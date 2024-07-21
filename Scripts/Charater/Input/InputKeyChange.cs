using UnityEngine;
using UnityEngine.InputSystem;
using iysy.GameInput;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using TMPro;
using System.Reflection.Emit;

namespace iysy.JustCut
{  
    public class InputKeyChange :MonoBehaviour
    {
        //Action��
        public Text actionLabel;
        //Binding��
        public Text bindingLabel;
        public Text testLabel;
        //Binding��id
        public string bindingId;
    
        private int index;
        [SerializeField] public InputActionReference inputActionsReference;
        [SerializeField] Label OutputKey;   
        [SerializeField] Button ResetInputActionBindings;
        [SerializeField] Button RebindBingdings;
        [SerializeField] Button TestButton;
        private InputActionRebindingExtensions.RebindingOperation RebindOperation;
    
        /// <summary>
        /// UIˢ��binding��
        /// </summary>
        /// <param name="index"></param>
        private void UpdateLabel(int index)
        {
            bindingLabel.text = inputActionsReference.action.GetBindingDisplayString(index,out string diviceLayoutName,out string controllpath);
        }
        public void InputKeyChangeStart()
        {
            //�ȼ��action�Ƿ�Ϊ��
            if(!CheckActionAndBinding(out int bindingIndex))
                return;
            PerformInteractiveRebind(inputActionsReference, bindingIndex);
        }
        //��ȡbindingIndex��Bingdings������±꣬��0��ʼ�����actionΪ�շ���false
        private bool CheckActionAndBinding(out int bindingIndex)
        {
            bindingIndex = -1;
            if(inputActionsReference == null)
                return false;
            bindingIndex = inputActionsReference.action.bindings.IndexOf(x => x.id == new System.Guid(bindingId));
            return true;
        }
        private void PerformInteractiveRebind(InputAction action, int bindingIndex, bool allCompositeParts = false)
        {
            RebindOperation?.Cancel(); // Will null out m_RebindOperation.
 
            void CleanUp()
            {
                RebindOperation?.Dispose();
                RebindOperation = null;
            }
 
            //�������ð�
            RebindOperation = action.PerformInteractiveRebinding(bindingIndex)
                //.WithControlsExcluding("Mouse")//�޳����(����ʵ�о�Ҳ���Բ��޳�)
                .OnCancel(
                    operation =>
                    {
                        CleanUp();
                    })
                .OnComplete(
                    operation =>
                    {
                        UpdateLabel(index);
                        CleanUp();
                    });
            RebindOperation.Start();
            Debug.Log("111");
        }
        //�ָ�Ĭ������
        public void ResetToDefault()
        {
            if (!CheckActionAndBinding(out var bindingIndex))
                return;
                inputActionsReference.action.RemoveBindingOverride(bindingIndex);
            UpdateLabel(index);
        }
        public void StartReset()
        {
            Debug.Log("���������ð�ť�ķ���");
            ResetToDefault();
        }
        public void StartRebinding()
        {
            Debug.Log("�����˰����󶨽ű�");
            InputKeyChangeStart();
        }
        public void TestFunction1()
        {
            testLabel.text = inputActionsReference.action.GetBindingDisplayString(index,out string diviceLayoutName,out string controllpath);            
        }
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            actionLabel.text = inputActionsReference.name;
            ResetInputActionBindings.GetComponent<Button>().onClick.AddListener(StartReset);
            RebindBingdings.GetComponent<Button>().onClick.AddListener(StartRebinding);
            TestButton.GetComponent<Button>().onClick.AddListener(TestFunction1);
            index = 1;
            bindingId = inputActionsReference.action.bindings[index].id.ToString();
            UpdateLabel(index);
            actionLabel.text = inputActionsReference.name;
            bindingLabel.text = inputActionsReference.action.GetBindingDisplayString(index, out string deviceLayoutName, out string controlPath);
        }
        
    }
}

    