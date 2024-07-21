using BehaviorDesigner.Runtime;
using MzUtility.FrameWork.EventSystem;
using MzUtility.FrameWork.SceneSystem;
using UnityEngine;
namespace iysy.JustCut
{

    /// <summary>
    /// 主要负责驱动世界吧。沟通场景。还要驱动行为树插件。并且公开一个行为树的刷新方法。
    /// </summary>
    public class GameManager : SingletonBehaviourNoNewGO<GameManager>
    {


        //[SerializeField] bool defaultFightingState;

        public override void InitInstanceWhenAwake()
        {
            base.InitInstanceWhenAwake();


            
            SceneManager.Instance.InitInstance();
            GameWorld.Instance.InitInstance();
        }
        private new void OnDestroy()
        {
            base.OnDestroy();
            SceneManager.Instance.Dispose();
            GameWorld.Instance.Dispose();
        }


        private void Update()
        {
            if (SceneManager.Instance.InFightingScene)
            {

                GameWorld.Instance.Update(Time.deltaTime);
                GameWorld.Instance.RealTimeUpdate(Time.unscaledDeltaTime);
            }
        }


    }
}