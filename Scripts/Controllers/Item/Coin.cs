using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DoDoDoIt
{
    public class Coin : ItemController 
    {
        public float PointAmount = 1f;

        public override bool Init()
        {
            base.Init();
            ItemType = Define.EItemType.Coin;
            return true;
        }

        public override void Collect(PlayerController playerController)
        {
            SoundManager.Instance.PlaySFX("event:/SFX/Item/Soul");
            Managers.Score.MyScore++;
            this.gameObject.SetActive(false);
        }

       

        //private void OnTriggerEnter(Collider other)
        //{
        //    PlayerController playerController = other.GetComponent<PlayerController>();
        //    if (playerController != null)
        //    {
        //        Managers.Game.Coin++;
        //        //OverlappedWithPlayer(playerController);
        //    }
//
        //    this.gameObject.SetActive(false);
        //}
    }
}