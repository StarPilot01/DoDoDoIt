using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace DoDoDoIt
{
    public class Managers : MonoBehaviour
    {
        static Managers s_instance; // 유일성이 보장된다

        public static Managers Instance
        {
            get
            {
                Init();
                return s_instance;
            }
        } // 유일한 매니저를 갖고온다

        #region Contents

        private GameManager _game = new GameManager();
        private ObjectManager _object = new ObjectManager();
        private ScoreManager _score = new ScoreManager();


        public static GameManager Game
        {
            get
            {
                return Instance?._game;
            }
        }

        public static ObjectManager Object
        {
            get
            {
                return Instance?._object;
            }
        }

        public static ScoreManager Score
        {
            get
            {
                return Instance?._score;
            }
        }

        #endregion

        #region Core

        DataManager _data = new DataManager();
        PoolManager _pool = new PoolManager();
        ResourceManager _resource = new ResourceManager();
        SceneManagerEx _scene = new SceneManagerEx();
        SoundManager _sound = new SoundManager();
        UIManager _ui = new UIManager();


        public static DataManager Data
        {
            get
            {
                return Instance?._data;
            }
        }

        public static PoolManager Pool
        {
            get
            {
                return Instance?._pool;
            }
        }

        public static ResourceManager Resource
        {
            get
            {
                return Instance?._resource;
            }
        }

        public static SceneManagerEx Scene
        {
            get
            {
                return Instance?._scene;
            }
        }

        public static SoundManager Sound
        {
            get
            {
                return Instance?._sound;
            }
        }

        public static UIManager UI
        {
            get
            {
                return Instance?._ui;
            }
        }

        #endregion


        public static void Init()
        {
            if (s_instance == null)
            {
                GameObject go = GameObject.Find("@Managers");
                if (go == null)
                {
                    DebugLogger.LogWarning("@Manager is not found");
                    go = new GameObject { name = "@Managers" };
                    go.AddComponent<Managers>();
                }

                DontDestroyOnLoad(go);
                s_instance = go.GetComponent<Managers>();
                //s_instance._sound.Init();
                
                s_instance._score.Init();
                
            }
        }

        public static void Clear()
        {
            //Sound.Clear();
            Scene.Clear();
            UI.Clear();
            Pool.Clear();
            Object.Clear();
        }


    }

}