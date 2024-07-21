using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
namespace iysy.JustCut
{
    
    /// <summary>
    /// ªÒ»°FrameController.
    /// </summary>
    public class BaseFrameAction : Action
    {
        protected EnemyFrameController frameController;
        public override void OnStart()
        {
            base.OnStart();
            frameController = gameObject.GetComponent<EnemyFrameControllerDriver>().FrameController;
        }



    }
}