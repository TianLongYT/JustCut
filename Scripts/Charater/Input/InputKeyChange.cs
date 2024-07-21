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
        //Action名
        public Text actionLabel;
        //Binding名
        public Text bindingLabel;
        public Text testLabel;
        //Binding的id
        public string bindingId;
    
        private int index;
        [SerializeField] public InputActionReference inputActionsReference;
        [SerializeField] Label OutputKey;   
        [SerializeField] Button ResetInputActionBindings;
        [SerializeField] Button RebindBingdings;
        [SerializeField] Button TestButton;
        private InputActionRebindingExtensions.RebindingOperation RebindOperation;
    
        /// <summary>
        /// UI刷新binding名
        /// </summary>
        /// <param name="index"></param>
        private void UpdateLabel(int index)
        {
            bindingLabel.text = inputActionsReference.action.GetBindingDisplayString(index,out string diviceLayoutName,out string controllpath);
        }
        public void InputKeyChangeStart()
        {
            //先检查action是否为空
            if(!CheckActionAndBinding(out int bindingIndex))
                return;
            PerformInteractiveRebind(inputActionsReference, bindingIndex);
        }
        //获取bindingIndex，Bingdings数组的下标，从0开始，如果action为空返回false
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
 
            //重新配置绑定
            RebindOperation = action.PerformInteractiveRebinding(bindingIndex)
                //.WithControlsExcluding("Mouse")//剔除鼠标(但其实感觉也可以不剔除)
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
        //恢复默认设置
        public void ResetToDefault()
        {
            if (!CheckActionAndBinding(out var bindingIndex))
                return;
                inputActionsReference.action.RemoveBindingOverride(bindingIndex);
            UpdateLabel(index);
        }
        public void StartReset()
        {
            Debug.Log("调用了重置按钮的方法");
            ResetToDefault();
        }
        public void StartRebinding()
        {
            Debug.Log("调用了按键绑定脚本");
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

    