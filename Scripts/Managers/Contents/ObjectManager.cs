using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using UnityEngine;


namespace DoDoDoIt
{
    public class ObjectManager
    {
        public PlayerController Player { get; private set; }

        //HashSet<ghostctolroller>
        public ObjectManager()
        {
            Init();
        }

        public void Init()
        {

        }

        public void Clear()
        {

        }


        public T Spawn<T>(Vector3 position, int templateID = 0, string prefabName = "") where T : BaseController
        {
            Type type = typeof(T);

            //if (type == typeof(PlayerController))
            //{
            //    GameObject go = Managers.Resource.Instantiate(Managers.Data.CreatureDic[templateID].PrefabLabel);
            //    go.transform.position = position;
            //    PlayerController pc = go.GetOrAddComponent<PlayerController>();
            //    pc.SetInfo(templateID);
            //    Player = pc;
            //    Managers.Game.Player = pc;

            //    return pc as T;
            //}


            return null;
        }

        public void Despawn<T>(T obj) where T : BaseController
        {
            Type type = typeof(T);

            if (type == typeof(PlayerController))
            {
                // ?
            }


        }




        //public void KillAllMonsters()
        //{
        //    UI_GameScene scene = Managers.UI.SceneUI as UI_GameScene;
//
        //    if(scene != null)
        //        scene.DoWhiteFlash(); 
        //    foreach (MonsterController monster in Monsters.ToList())
        //    {
        //        if (monster.ObjectType == ObjectType.Monster)
        //            monster.OnDead();
        //    }
        //    DespawnAllMonsterProjectiles();
        //}





    }

}