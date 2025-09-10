using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DoDoDoIt.Define;

namespace DoDoDoIt
{
    public class PlayerEnterRightCurveState : IPlayerState
    {
        private PlayerController _player;
        private float rotationDuration = 2f;
        private Quaternion targetRotation;  // 목표 회전값
        private Coroutine rotationCoroutine;

        public void EnterState(PlayerController player)
        {
            SoundManager.Instance.PlaySFX("event:/SFX/Dodo/Sliding");
            _player = player;
            _player.Animator.SetTrigger("CorneringRight");
            // 현재 애니메이션의 길이를 가져와서 rotationDuration 설정
            if (_player.Animator != null)
            {
                AnimatorStateInfo currentState = _player.Animator.GetCurrentAnimatorStateInfo(0);
                rotationDuration = 0.7f; // 애니메이션의 길이
            }
            
            // 목표 회전값 계산 (현재 회전에 90도 더함)
            targetRotation = _player.transform.rotation * Quaternion.Euler(0, 90f, 0);

            // 기존 회전 코루틴이 실행 중이라면 정지
            if (rotationCoroutine != null)
                _player.StopCoroutine(rotationCoroutine);

            // 코루틴 시작
            rotationCoroutine = _player.StartCoroutine(PerformRotation());
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
            
        }

        private IEnumerator PerformRotation()
        {
            float elapsedTime = 0f;
            Quaternion initialRotation = _player.transform.rotation;

            while (elapsedTime < rotationDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / rotationDuration;

                // 회전값 점진적 적용
                _player.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);
                yield return null;
            }

            // 최종 회전값 설정 (정확한 목표값 보장)
            _player.transform.rotation = targetRotation;

            // 다음 상태로 전환
            _player.StateMachine.TransitionTo(EPlayerState.Running); // 원하는 상태로 수정
        }
        private void ApplyMovement()
        {
            Vector3 movement = _player.gameObject.transform.forward * (_player.Stats.Attributes.ForwardSpeed.Value);
            _player.Rigidbody.velocity = new Vector3(movement.x, _player.Rigidbody.velocity.y, movement.z);
        }
        void ApplyGravity()
        {
            _player.Rigidbody.AddForce(Vector3.up * _player.Stats.Attributes.GravityScale.GetValue(), ForceMode.Acceleration);
        }
    }
}
