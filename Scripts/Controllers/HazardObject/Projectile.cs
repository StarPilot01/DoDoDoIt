using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DoDoDoIt
{
    public class Projectile : HazardObjectController
    {
        //public int damage = 1; // 함정이 가하는 데미지
        
        public float detectionRadius = 5f; // 플레이어가 감지되는 반경
        public float flySpeed = 10f; // 날아가는 속도
        public float flyDuration = 3f; // 날아가는 시간
        public Vector3 flyDirection = Vector3.forward; // 날아가는 방향 (기본적으로 앞 방향)

        private bool isPlayerInRange = false; // 플레이어가 감지 범위 내에 있는지 확인
        private bool isFlying = false; // 장애물이 현재 날아가는 중인지 확인
        private Vector3 initialPosition; // 장애물의 초기 위치 저장
        private Transform playerTransform; // 플레이어의 Transform 저장
        

        public override bool Init()
        {
            base.Init();
            HazardObjectType = Define.EHazardObjectType.Projectile;

            // 초기 위치 저장
            initialPosition = transform.position;

            // 원형 범위 감지용 트리거 콜라이더 추가
            SphereCollider triggerCollider = gameObject.AddComponent<SphereCollider>();
            triggerCollider.isTrigger = true; // 트리거로 설정
            triggerCollider.radius = detectionRadius; // 트리거 반지름 설정

        
            
            return true;
        }

        private void Update()
        {
            // 플레이어가 감지 범위 내에 있고, 장애물이 날아가는 중이 아니라면
            if (isPlayerInRange && !isFlying)
            {
                // 플레이어가 일정 거리 내로 들어오면 날아가기 시작
                if (Vector3.Distance(transform.position, playerTransform.position) <= detectionRadius)
                {
                    SoundManager.Instance.PlaySFX("event:/SFX/Obj/Flybook", "Book", 0f);
                    StartCoroutine(FlyTowardsDirection());
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // 플레이어가 범위 내로 들어오면
            if (other.CompareTag("Player"))
            {
                isPlayerInRange = true;
                playerTransform = other.transform; // 플레이어의 Transform 저장
                Debug.Log("Player entered detection range!");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // 플레이어가 범위 밖으로 나가면
            if (other.CompareTag("Player"))
            {
                isPlayerInRange = false;
                Debug.Log("Player exited detection range!");
            }
        }

        private IEnumerator FlyTowardsDirection()
        {
            isFlying = true; // 날아가기 시작했음을 표시
            float elapsedTime = 0f;

            // 초기 속도 설정 (플레이어 방향으로 날아가게 설정 가능)
            Vector3 direction = flyDirection.normalized;

            while (elapsedTime < flyDuration)
            {
                // 장애물을 특정 방향으로 이동
                transform.position += direction * flySpeed * Time.deltaTime;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 날아가는 시간이 종료되면 초기 위치로 복귀
            isFlying = false;
            transform.position = initialPosition;
            Debug.Log("Trap returned to initial position.");
        }

        private void OnCollisionEnter(Collision collision)
        {
            // 날아가는 상태에서 Player와 충돌하면 데미지 부여
            if (isFlying && collision.gameObject.CompareTag("Player"))
            {
                PlayerController playerController;
                if (collision.gameObject.TryGetComponent<PlayerController>(out playerController))
                {
                    SoundManager.Instance.PlaySFX("event:/SFX/Obj/Flybook", "Book", 1f);
                    playerController.TakeDamage(_damageInfo);
                    //Debug.Log($"Player took {damage} damage from flying trap!");
                }

                // 충돌 후, 함정을 비활성화 (필요에 따라 이 로직 변경 가능)
                gameObject.SetActive(false);
            }
        }
    }

}