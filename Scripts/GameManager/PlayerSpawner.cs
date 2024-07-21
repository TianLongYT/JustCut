using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
namespace iysy.JustCut
{

    /// <summary>
    /// 
    /// </summary>
    public class PlayerSpawner : MonoBehaviour
    {
        ResLoader resLoader;
        private void Start()
        {
            var levelData = SceneManager.Instance.levelData;
            if(levelData.GetCharaterPathData() == null)
            {
                levelData.SetPlayerPathData("katana");
            }
            var charaterPathData = levelData.GetCharaterPathData().CharaterPrefabPath;
            resLoader = ResLoader.Allocate();
            resLoader.Add2Load<GameObject>(charaterPathData, (isEnd, prefab) =>
            {
                if (isEnd)
                {
                    prefab.Asset.As<GameObject>().Instantiate(this.transform.position, this.transform.rotation);
                }
            });
            resLoader.LoadAsync(() =>
            {
                // 加载成功 5 秒后回收
                ActionKit.Delay(5.0f, () =>
                {
                    resLoader.Recycle2Cache();

                }).Start(this);
            });
        }


    }
}