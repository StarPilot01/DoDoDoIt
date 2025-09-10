using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoDoDoIt
{
    public abstract class GhostController : BaseController , IAbsorbable
    {
       
        protected Rigidbody _rigidbody;

        public Rigidbody Rigidbody => _rigidbody;
        public Define.EGhostType GhostType { get; protected set; }

        public override bool Init()
        {
            base.Init();

            _rigidbody = Util.FindChild<Rigidbody>(this.gameObject, null,true);

            return true;
        }

        
       
        
        //public abstract void OverlappedWithPlayer(PlayerController pc);

        public abstract void Absorb(PlayerController absorber);

    }
}

