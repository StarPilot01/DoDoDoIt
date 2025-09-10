using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace DoDoDoIt
{
    public class PlayerRunningState : IPlayerState , IDamageableState
    {
        private PlayerController _player;
        private float _inputX = 0.0f;


        private Coroutine _coStartSlow;
       

        public void EnterState(PlayerController player)
        {
            _player = player;
            _player.Stats.RemainingJumpCount = _player.Stats.Attributes.JumpCount.GetValue();
            _player.Animator.SetBool("IsGrounded", true);
        }

        public void Update()
        {
            if (_player.Rigidbody.velocity.y < 0 && !IsGrounded())
            {
                //DebugLogger.Log("go to falling" , Define.DebugLogColor.blue);
                _player.TransitionTo(Define.EPlayerState.Falling);
            }
            
            HandleInput();
        }

        public void FixedUpdate()
        {
            ApplyMovement();
            ApplyGravity();
        }

        public void ExitState()
        {
            _player.Rigidbody.velocity = new Vector3(0, _player.Rigidbody.velocity.y, 0);
        }

        void HandleInput()
        {
            _player.InputX = Input.GetAxis("Horizontal");

            if (Input.GetKeyDown(KeyCode.Space) && !_player.IsSwinging)
            {
                _player.TransitionTo(Define.EPlayerState.Jump);
                _player.Animator.SetBool("IsGrounded", false);
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
            _player.Rigidbody.AddForce(Vector3.up * _player.Stats.Attributes.GravityScale.GetValue(), ForceMode.Acceleration);
        }

        void IDamageableState.TakeDamage(DamageInfo damageInfo)
        {
            if (_coStartSlow == null)
            {
                //_coStartSlow = CoroutineManager.StartCoroutine(SlowDown());
                Debug.Log("Slow");
            }
            
            
        }

        //private IEnumerator SlowDown()
        //{
        //    float originalSpeed = _player.Stats.Attributes.ForwardSpeed.GetValue();
        //    //_player.Stats.Attributes.ForwardSpeed *= 0.5f;  
        //    //yield return new WaitForSeconds(1.2f);  
        //    //_player.Stats.Attributes.ForwardSpeed = _player.Stats.InitAttributes.ForwardSpeed;  // 원래 속도로 복원
        //    //_coStartSlow = null;
//
//
        //    //if (_player.HasActiveSpeedBuff())
        //    //{
        //    //    SpeedUpBuff_SO speedUpBuff = _player.GetSpeedBuffInActive();
        //    //    _player.ApplyBuff(speedUpBuff);
        //    //}
        //    //else
        //    //{
        //    //    _player.Stats.Attributes.ForwardSpeed = originalSpeed;
        //    //}
        //}
    }
}
