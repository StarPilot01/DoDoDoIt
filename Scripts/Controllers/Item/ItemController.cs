using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoDoDoIt
{
    public abstract class ItemController : BaseController , ICollectible
    {
      
        public Define.EItemType ItemType { get; protected set; }
        public override bool Init()
        {
            base.Init();
            ObjectType = Define.EObjectType.Item;
            return true;
        }
       
       
        public abstract void Collect(PlayerController playerController);
    }
}
