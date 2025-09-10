using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoDoDoIt
{
    public class Scene_Ending : Scene_Base
    {

        private UI_Scene_Ending _ui;

        private void Awake()
        {
            Init();
        }

        
        protected override void Init()
        {
            //Debug.Log("@>> GameScene Init()");
            base.Init();
            SceneType = Define.Scene.GameScene;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;


            Managers.Resource.Load<GameObject>("hi");
            _ui = Managers.UI.ShowSceneUI<UI_Scene_Ending>();

            //Managers.Resource.LoadAllAsync<Object>("End_Cutscene", (key, count, totalCount) =>
            //{
            //    if (count == totalCount)
            //    {
            //        
            //        
            //            
            //    }
            //});

        }
        public override void Clear()
        {
           
        }
    }
}
