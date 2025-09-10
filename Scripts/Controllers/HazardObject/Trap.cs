using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace DoDoDoIt
{
    public class Trap : HazardObjectController
    {
        //public int damage = 1; // 함정이 가하는 데미지
        private float _triggerRadius = 20f; // 원형 범위의 반지름
        private float _trapActivationSpeed = 120f; // 함정이 튀어나오는 속도
        private float _activateLength;

        private bool isPlayerInRange = false; // 플레이어가 범위에 있는지 확인
        private bool isTrapActivated = false; // 함정이 활성화되었는지 확인

        private Vector3 initialPosition; // 함정의 초기 위치
        private Vector3 activatedPosition; // 함정이 튀어나올 위치

        public override bool Init()
        {
            base.Init();
            HazardObjectType = Define.EHazardObjectType.Trap;
            _activateLength = GetComponent<Collider>().bounds.size.y * 0.8f;
            // 초기 위치와 활성화 위치 설정
            initialPosition = transform.position;
            activatedPosition = initialPosition + gameObject.transform.forward * _activateLength; // 위로 2만큼 이동 (필요에 따라 조정)

            // 원형 범위 감지용 콜라이더 설정
            SphereCollider triggerCollider = gameObject.AddComponent<SphereCollider>();
            triggerCollider.isTrigger = true; // 트리거로 설정
            triggerCollider.radius = _triggerRadius; // 트리거 반지름 설정

          
            
            return true;
        }

        private void Update()
        {
            // 플레이어가 범위에 있고, 함정이 아직 활성화되지 않은 경우 함정을 튀어나오게 함
            if (isPlayerInRange && !isTrapActivated)
            {
                // 함정이 활성화 위치로 이동 (상승 애니메이션)
                transform.position = Vector3.MoveTowards(transform.position, activatedPosition,
                    _trapActivationSpeed * Time.deltaTime);

                
                
                // 함정이 목표 위치에 도달하면 활성화 완료
                
                
                if (Vector3.Distance(transform.position, activatedPosition) < 0.01f)
                {
                    isTrapActivated = true;
                    Debug.Log("Trap activated!");
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // 원형 범위 내에 Player 태그를 가진 오브젝트가 들어오면 함정을 활성화
            if (other.CompareTag("Player"))
            {
                isPlayerInRange = true;
                Debug.Log("Player entered trap range!");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // Player가 범위를 벗어나면 함정을 비활성화하고 초기 위치로 복귀
            if (other.CompareTag("Player"))
            {
                isPlayerInRange = false;
                isTrapActivated = false;
                Debug.Log("Player exited trap range!");

                // 함정을 초기 위치로 복귀
                transform.position = initialPosition;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            // 함정이 활성화된 상태에서 Player와 충돌하면 데미지 부여
            if (isTrapActivated && collision.gameObject.CompareTag("Player"))
            {
                PlayerController playerController;
                if (collision.gameObject.TryGetComponent<PlayerController>(out playerController))
                {
                    playerController.TakeDamage(_damageInfo);
                    //Debug.Log($"Player took {damage} damage from trap!");
                }

                // 함정을 비활성화 (원한다면, 일정 시간 후 다시 활성화 가능)
                gameObject.SetActive(false);
            }
        }
    }
}
