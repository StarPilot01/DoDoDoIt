using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoDoDoIt
{
    public class PlayerJumpState : IPlayerState , IDamageableState
    {
        private PlayerController _player;
        private bool _bShouldJump;

        public void EnterState(PlayerController player)
        {
            _player = player;
            _bShouldJump = true;
        }

        public void Update()
        {
            CheckFalling();
            HandleInput();
        }

        public void FixedUpdate()
        {
            Jump();
            ApplyMovement();
            ApplyGravity();
        }

        public void ExitState()
        {

        }

        void HandleInput()
        {
            _player.InputX = Input.GetAxis("Horizontal");

            if (Input.GetKeyDown(KeyCode.Space) && _player.Stats.RemainingJumpCount > 0 && !_player.IsSwinging)
            {
                Random.Range(0, 2);
                _bShouldJump = true;
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

        void CheckFalling()
        {
            if (!_bShouldJump && _player.Rigidbody.velocity.y < 0)
            {
                _player.TransitionTo(Define.EPlayerState.Falling);
            }
        }

        private void ApplyMovement()
        {
            Vector3 movement = (_player.transform.right * _player.InputX * _player.Stats.Attributes.HorizontalSpeed.GetValue()) +
                               (_player.transform.forward * _player.Stats.Attributes.ForwardSpeed.GetValue());
            _player.Rigidbody.velocity = new Vector3(movement.x, _player.Rigidbody.velocity.y, movement.z);
        }

        private void Jump()
        {
            if (_bShouldJump)
            {
                if (_player.Stats.RemainingJumpCount > 1)
                {
                    SoundManager.Instance.PlaySFX("event:/SFX/Dodo/Jump", "Type_jump", 0f);
                    _player.Animator.SetTrigger("Jump");
                }
                else
                {
                    SoundManager.Instance.PlaySFX("event:/SFX/Dodo/Jump", "Type_jump", 0f);
                    _player.Animator.SetFloat("JumpIndex", Random.Range(0, 2));
                    _player.Animator.SetTrigger("DoubleJump");
                }
                _player.Stats.RemainingJumpCount--;
                _bShouldJump = false;
                _player.Rigidbody.velocity = new Vector3(_player.Rigidbody.velocity.x, 0, _player.Rigidbody.velocity.z);

                if (_player.Stats.JumpForce < 0)
                {
                    DebugLogger.LogError("JumpForce is lower than 0");
                }
                
                _player.Rigidbody.AddForce(new Vector3(0, _player.Stats.JumpForce, 0), ForceMode.Impulse);

            }
        }

        void ApplyGravity()
        {
            _player.Rigidbody.AddForce(Vector3.up * _player.Stats.Attributes.GravityScale.GetValue(), ForceMode.Acceleration);
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            throw new System.NotImplementedException();
        }
    }
}
