using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoDoDoIt
{
    public interface IPlayerState
    {
        //public Define.EPlayerState CurState { get; protected set; }
        public void EnterState(PlayerController player);
        public void Update();
        public void FixedUpdate();
        public void ExitState();
    }

}