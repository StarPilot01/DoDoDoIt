using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoDoDoIt
{
    public class Scene_Title : Scene_Base
    {
        
        private UI_Scene_Title _ui;
        private bool _isPreloadCompleted = false;
        
        protected override void Init()
        {
            base.Init();

            SceneType = Define.Scene.TitleScene;
            //TitleUI

            Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
            {
                if (count == totalCount)
                {
                    _isPreloadCompleted = true;
                    _ui = Managers.UI.ShowSceneUI<UI_Scene_Title>();
                }
            });
            
        }


        private void Awake()
        {
            Init();
        }
        public override void Clear()
        {

        }
    }

}