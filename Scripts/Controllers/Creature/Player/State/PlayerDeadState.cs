using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoDoDoIt
{
    public class PlayerDeadState : IPlayerState
    {

        private PlayerController _player;
        private GameObject _deadEffect;

        public void EnterState(PlayerController player)
        {
            SoundManager.Instance.PlaySFX("event:/SFX/Dodo/Voice/Death");
            SoundManager.Instance.PlaySFX("event:/SFX/Dodo/Gameover");
            SoundManager.Instance.PopupPause(true);
            _player = player;
            _deadEffect = Managers.Resource.Instantiate("DeadEffect");

            _player.Stats.PlayerHealth.SetInvincible();
        }

        public void Update()
        {
            _player.Rigidbody.velocity = Vector3.zero;
            _deadEffect.transform.position = _player.EffectPosition.gameObject.transform.position + (Vector3.up * 1.5f);
        }

        public void FixedUpdate()
        {
            _player.Rigidbody.velocity = Vector3.up * _player.Stats.Attributes.GravityScale.GetValue();
        }

        public void ExitState()
        {
            //throw new NotImplementedException();
        }
    }
}
