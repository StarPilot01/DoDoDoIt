using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DoDoDoIt
{
    public class SceneManagerEx
    {
        public Scene_Base CurrentScene
        {
            get
            {
                return GameObject.FindObjectOfType<Scene_Base>();
            }
        }


        public void LoadScene(Define.Scene type, Transform parents = null)
        {
            switch (CurrentScene.SceneType)
            {
                case Define.Scene.TitleScene:
                    Managers.Clear();
                    SceneManager.LoadScene(GetSceneName(type));


                    break;
                case Define.Scene.GameScene:
                    SceneManager.LoadScene(GetSceneName(type));
                    Time.timeScale = 1;


                    break;
                case Define.Scene.EndingScene:
                    Managers.Clear();
                    SceneManager.LoadScene(GetSceneName(type));
                    break;

            }

        }

        string GetSceneName(Define.Scene type)
        {
            string name = Enum.GetName(typeof(Define.Scene), type);
            return name;
        }

        public void Clear()
        {
            CurrentScene.Clear();
        }
    }

}