using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DoDoDoIt
{
    public class Scene_Game : Scene_Base
    {

        private bool isGameEnd = false;

        private PlayerController _player;

        private bool _isPreloadCompleted = false;

        //[SerializeField]
        //private Transform _playerTrs;

        private UI_Scene_Game _ui;
        
        private Transform _currentSavePoint;
        //private Transform[] _savePointsTrs;

        public PlayerController Player
        {
            get
            {
                return _player;
            }
        }
        
        private void Awake()
        {
            Init();
            
            
            
            
          
            //SceneChangeAnimation_Out anim = Managers.Resource.Instantiate("SceneChangeAnimation_Out").GetOrAddComponent<SceneChangeAnimation_Out>();
            //anim.transform.SetParent(parents);
            //anim.SetInfo(SceneType, () => { });
        }


        protected override void Init()
        {
            //Debug.Log("@>> GameScene Init()");
            base.Init();
            SceneType = Define.Scene.GameScene;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            _player = FindAnyObjectByType<PlayerController>();

            Managers.Score.MyScore = 0;
            
            Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
            {
                if (count == totalCount)
                {
                    _isPreloadCompleted = true;
                    //Managers.Data.Init();
                    //Managers.Game.Init();   
//
                    //Debug.Log("Load Completed");

//                GameObject player = Managers.Resource.Instantiate("PlayerPrefab");
//                  
                    //player.transform.position = _playerTrs.transform.position;
                    
                    
                    //_player
                    
                    _ui = Managers.UI.ShowSceneUI<UI_Scene_Game>();

                }
            });

        }

       
        
        public override void Clear()
        {
            
        }

        public void UpdateSavePoint(Transform newSavePoint)
        {
            _currentSavePoint = newSavePoint;
        }
        public void RespawnDoDo()
        {
            _player.Stats.PlayerHealth.Hearts--;
            
            
            _player.transform.position = _currentSavePoint.position;
            
            _player.Rigidbody.velocity = Vector3.zero;
            
        }

    }

}