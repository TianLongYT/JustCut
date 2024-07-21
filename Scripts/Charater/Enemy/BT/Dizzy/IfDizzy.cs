using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using iysy.JustCut.Evt;
using MzUtility.FrameWork.EventSystem;
using System;
using UnityEngine;
namespace iysy.JustCut
{
    [TaskCategory("JustCut/Dizzy")]
    [TaskDescription("接收一个事件，有事件就说明自身被击晕。")]
    /// <summary>
    /// 
    /// </summary>
    public class IfDizzy : Conditional
    {
        public override void OnAwake()
        {
            EventCenter.EventCenter.Register<Evt.EvtParamOnEnemyCollapse>(OnEnemyCollapse);
        }

        private void OnEnemyCollapse(EvtParamOnEnemyCollapse obj)
        {
            
        }
        public override void OnBehaviorComplete()
        {
            base.OnBehaviorComplete();
        }

        public override TaskStatus OnUpdate()
        {
            return base.OnUpdate();
        }


    }
}