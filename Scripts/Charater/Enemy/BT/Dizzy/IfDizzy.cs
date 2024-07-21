using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using iysy.JustCut.Evt;
using MzUtility.FrameWork.EventSystem;
using System;
using UnityEngine;
namespace iysy.JustCut
{
    [TaskCategory("JustCut/Dizzy")]
    [TaskDescription("����һ���¼������¼���˵���������Ρ�")]
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