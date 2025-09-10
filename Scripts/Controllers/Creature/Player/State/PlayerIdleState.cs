using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoDoDoIt
{
    public class PlayerIdleState : IPlayerState
    {

        private PlayerController _player;

        public void EnterState(PlayerController player)
        {
            _player = player;

        }

        public void Update()
        {

        }

        public void FixedUpdate()
        {

        }

        public void ExitState()
        {

        }


    }
}
