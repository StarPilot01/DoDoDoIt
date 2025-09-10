using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DoDoDoIt
{
    public class PlayerFallingState : IPlayerState , IDamageableState
    {
        private PlayerController _player;

        public void EnterState(PlayerController player)
        {
            _player = player;
            _player.Animator.SetBool("IsGrounded", false);
        }

        public void Update()
        {

            //Debug.DrawRay(_player.GroundCheckRayOriginTrs.transform.position, Vector3.down * 10.1f, Color.red);

           if (_player.Rigidbody.velocity.y < -500)
           {
               SoundManager.Instance.PlaySFX("event:/SFX/Dodo/Voice/Fall");
               //DebugLogger.LogError("y velocity under -400");
               ((Scene_Game)Managers.Scene.CurrentScene).RespawnDoDo();
           }
            
            
            if (IsGrounded())
            {
                GameObject LandingEffect = Managers.Resource.Instantiate("DustEffect");
                LandingEffect.transform.position = _player.gameObject.transform.position + Vector3.up * 1f;
                UnityEngine.GameObject.Destroy(LandingEffect, 2f);
                _player.TransitionTo(Define.EPlayerState.Running);
                return;
            }

            _player.InputX = Input.GetAxis("Horizontal");

            if (Input.GetKeyDown(KeyCode.Space) && _player.Stats.RemainingJumpCount > 0 && !_player.IsSwinging)
            {
                _player.TransitionTo(Define.EPlayerState.Jump);
            }
            
            if (Input.GetKeyDown(KeyCode.LeftShift) && !_player.IsSwinging)
            {
                _player.GrapPoint = _player.GrapplePointDetector.DetectGrapPoint();  // 감지된 오브젝트 할당
                if (_player.GrapPoint != null)
                {
                    _player.IsSwinging = true;
                    _player.TransitionTo(Define.EPlayerState.EnterGrappling);
                }
                else
                {
                    _player.IsSwinging = false;
                }
            }
        }

        public void FixedUpdate()
        {
            ApplyMovement();
            ApplyGravity();
        }

        public void ExitState()
        {
            //_player.Animator.SetBool("Jumping", false);

        }

        public bool IsGrounded()
        {
            return Physics.Raycast (
                _player.GroundCheckRayOriginTrs.transform.position,
                Vector3.down,
                out RaycastHit hit,
                1.1f,
                _player.GroundLayer
            );
        }

        private void ApplyMovement()
        {
            Vector3 movement = (_player.transform.right * _player.InputX * _player.Stats.Attributes.HorizontalSpeed.GetValue()) +
                               (_player.transform.forward * _player.Stats.Attributes.ForwardSpeed.GetValue());
            _player.Rigidbody.velocity = new Vector3(movement.x, _player.Rigidbody.velocity.y, movement.z);
        }

        void ApplyGravity()
        {
            _player.Rigidbody.AddForce ( 
                Vector3.up * _player.Stats.Attributes.GravityScale.GetValue(),
                ForceMode.Acceleration
            );
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            //이속 느려지게
            throw new System.NotImplementedException();
        }
    }
}
