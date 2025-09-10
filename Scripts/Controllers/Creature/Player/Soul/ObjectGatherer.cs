using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DoDoDoIt
{
    public class ObjectGatherer : MonoBehaviour
    {
        #region field
        [SerializeField]
        private LayerMask _objectLayerMask;
        [SerializeField]
        private int _maxColliderCounts;
        [SerializeField]
        private float _checkFrequency = 0.2f; // CheckAndAddObjectsInRadius 주기
       
        
        
        private Collider[] _cachedColliders;
        private PlayerController _playerController;
        private PlayerInitStats_SO _playerInitStats;
        
        #endregion
        
        private WaitForSeconds _checkWait;
        private WaitForSeconds _pullWait;

        private Coroutine _coCheckObjects;
        private Coroutine _coPullObjects;
        
       
        private HashSet<Collider> _trackedObjects = new HashSet<Collider>(); // 추적 중인 객체 목록
        public event Action OnObjectAbsorbed;
        private void Awake()
        {
            _cachedColliders = new Collider[_maxColliderCounts];
            
            
        }

        // Start is called before the first frame update
        void Start()
        {
            _playerController = GetComponent<PlayerController>();
            _checkWait = new WaitForSeconds(_checkFrequency);
            
            _coCheckObjects = StartCoroutine(CoCheckObjects());
            //_coPullObjects = StartCoroutine(CoPullObjects());
            //SoulSystem에서 Event 풀리는 버그로 인해 직접 이벤트 등록
            //TODO: 추후 SoulSystem에서 직접 등록하는 것으로 수정필요
            //SoulSystem system = FindAnyObjectByType<SoulSystem>();
            //OnObjectAbsorbed += system.OnGhostAbsorbed;
            
            
            
        }

        void OnDestroy()
        {
            //SoulSystem system = FindAnyObjectByType<SoulSystem>();

            //if (system != null)
            //{
            //    
            //    OnObjectAbsorbed -= system.OnGhostAbsorbed;
            //}
        }


       /// <summary>
        /// 주변 반경 내 오브젝트를 주기적으로 감지하여 추적 리스트에 추가
        /// </summary>
        private IEnumerator CoCheckObjects()
        {
            while (true)
            {
                CheckAndAddObjectsInRadius();
                yield return _checkWait;
            }
        }

        
        public void Update()
        {
            PullTrackedObjects();
        }

        /// <summary>
        /// 반경 내 새 오브젝트를 감지하여 추적 리스트에 추가
        /// </summary>
        private void CheckAndAddObjectsInRadius()
        {
            int numColliders = Physics.OverlapSphereNonAlloc(
                _playerController.ObjectGatherPointTrs.position,
                _playerController.Stats.Attributes.PullRadius.GetValue(),
                _cachedColliders,
                _objectLayerMask
            );

            for (int i = 0; i < numColliders; i++)
            {
                Collider collider = _cachedColliders[i];

                // 처음 감지된 오브젝트만 추적 리스트에 추가
                if (!_trackedObjects.Contains(collider))
                {
                    _trackedObjects.Add(collider);
                }
            }
        }

        /// <summary>
        /// 추적 중인 모든 오브젝트를 끌어당기고, 흡수 반경 내에 들어온 오브젝트를 처리
        /// </summary>
        private void PullTrackedObjects()
        {
            var objectsToRemove = new List<Collider>();

            foreach (var collider in _trackedObjects)
            {
                if (collider == null) // 오브젝트가 삭제된 경우
                {
                    objectsToRemove.Add(collider);
                    continue;
                }

                // 플레이어 위치에 Y 오프셋 추가
                Vector3 playerPosition = _playerController.ObjectGatherPointTrs.transform.position; // Y 오프셋 1.0f 추가
                Vector3 directionToPlayer = (playerPosition - collider.transform.position).normalized;

                Rigidbody colliderRigidbody = collider.GetComponent<Rigidbody>();

                if (colliderRigidbody != null)
                {
                    float groundY = 0.1f; // 지면의 최소 높이
                    Vector3 currentPosition = colliderRigidbody.position;

                    // Y 위치가 지면 이하로 내려가지 않도록 제한
                    if (currentPosition.y < groundY)
                    {
                        currentPosition.y = groundY;
                        colliderRigidbody.position = currentPosition;
                    }

                    float pullSpeed = _playerController.Stats.Attributes.PullForce.GetValue();
                    Vector3 targetVelocity = directionToPlayer * pullSpeed;

                    colliderRigidbody.velocity = Vector3.Lerp(colliderRigidbody.velocity, targetVelocity, Time.deltaTime * 10f);
                }

                // 흡수 반경 확인
                float distanceSqr = (playerPosition - collider.transform.position).sqrMagnitude;
                float absorptionRadiusSqr = Mathf.Pow(_playerController.Stats.Attributes.AbsorptionRadius.GetValue(), 2);

                if (distanceSqr < absorptionRadiusSqr)
                {
                    GhostController parentGhost = collider.GetComponentInParent<GhostController>();

                    if (parentGhost != null)
                    {
                        parentGhost.Absorb(_playerController);
                    }
                    else if (collider.TryGetComponent(out ItemController item))
                    {
                        item.Collect(_playerController);
                    }

                    // 흡수된 오브젝트는 추적 리스트에서 제거
                    objectsToRemove.Add(collider);
                }
            }

            // 흡수된 오브젝트 또는 삭제된 오브젝트를 리스트에서 제거
            foreach (var obj in objectsToRemove)
            {
                _trackedObjects.Remove(obj);
            }
        }


        //private void PullObjectsInRadius()
        //{
        //   
        //    int numColliders = Physics.OverlapSphereNonAlloc(transform.position, _playerController.Stats.Attributes.PullRadius.GetValue(), _cachedColliders , _objectLayerMask); // 주변 오브젝트 검색
//
        //    Debug.Log(numColliders);
        //    
        //    for (int i = 0; i < numColliders; i++)
        //    {
        //        Collider collider = _cachedColliders[i];
        //        
        //        Vector3 dirColliderToThis = (transform.position - collider.transform.position).normalized;
        //        
        //      
        //        Rigidbody colliderRigidbody = collider.GetComponent<Rigidbody>();
//
        //      
        //        
        //        
        //        
        //        colliderRigidbody.AddForce(dirColliderToThis * _playerController.Stats.Attributes.PullForce.GetValue() , ForceMode.VelocityChange);
//
        //        
        //        //속도 향상을 위하여 제곱된 거리로 비교 
        //        float distanceSqr = (transform.position - collider.transform.position).sqrMagnitude;
        //        
        //        bool isInPullRadius = distanceSqr <_playerController.Stats.Attributes.AbsorptionRadius.GetValue() * _playerController.Stats.Attributes.AbsorptionRadius.GetValue();
//
        //        if (isInPullRadius)
        //        {
        //            Debug.Log(collider.name);
        //                
        //            GhostController parentGhost = collider.GetComponentInParent<GhostController>();
//
        //            if (parentGhost != null)
        //            {
        //                //parentGhost.Absorb(_playerController); // 부모의 Absorb 메서드 호출
        //                //parentGhost.StartTracking();
        //            }
        //           
        //            if (collider.TryGetComponent(out ItemController item))
        //            {
        //                //item.Collect(_playerController);
        //            }
        //        }
        //    }
        //}
        
        //private void OnDrawGizmos()
        //{
        //    if (_playerController == null || _playerController.Stats == null) return;
//
        //    // Absorption Radius (흡수 반경)
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawWireSphere(_playerController.ObjectGatherPointTrs.position, _playerController.Stats.Attributes.AbsorptionRadius.GetValue());
//
        //    // Pull Radius (끌어당기는 반경)
        //    Gizmos.color = Color.blue;
        //    Gizmos.DrawWireSphere(_playerController.ObjectGatherPointTrs.position, _playerController.Stats.Attributes.PullRadius.GetValue());
        //}
    }
}

