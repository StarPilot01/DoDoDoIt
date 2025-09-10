using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


namespace DoDoDoIt
{
    public class PlayerEndingSceneRunningState : IPlayerState
    {
        private PlayerController _player;
        private float _inputX = 0.0f;
        private Transform _targetObject;
        private float _epsilon = 0.1f;
        public void EnterState(PlayerController player)
        {
            _player = player;
            _player.Stats.RemainingJumpCount = _player.Stats.Attributes.JumpCount.GetValue();
            _player.Animator.SetBool("IsGrounded", true);
            
            
            _targetObject = GameObject.Find("Ending_Hamster_Boss").transform;
            //CinemachineCollider collider = GameObject.Find("MainCamera").GetComponent<CinemachineCollider>();
            //collider.enabled = false;
            
            //_player.PlayableDirector.Play(_player._endingTimeLine);

            //Vector3 localTargetPosition = _player.transform.InverseTransformPoint(_targetObject.position);
            
            
            
            _player.transform.position = new Vector3(_targetObject.transform.position.x, _player.transform.position.y, _player.transform.position.z);
            
            
            
        }

        public void Update()
        {
            
            
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

       

        private void ApplyMovement()
        {
            // 목표와 플레이어 간 상대적 위치 계산
            //Vector3 localTargetPosition = _player.transform.InverseTransformPoint(_targetObject.position);
//
            //// x축 이동 속도 계산 (오차 범위 이내라면 정지)
            //float horizontalSpeed = Mathf.Abs(localTargetPosition.x) <= _epsilon 
            //    ? 0 
            //    : Mathf.Sign(localTargetPosition.x) * _player.Stats.Attributes.HorizontalSpeed.GetValue();

            // 이동 벡터 계산 (로컬 기준)
            Vector3 movement =  (_player.transform.forward * 70);

            // Rigidbody 속도 적용
            _player.Rigidbody.velocity = new Vector3(movement.x, _player.Rigidbody.velocity.y, movement.z);
        }

        void ApplyGravity()
        {
            _player.Rigidbody.AddForce(Vector3.up * _player.Stats.Attributes.GravityScale.GetValue(), ForceMode.Acceleration);
        }




    }
}
