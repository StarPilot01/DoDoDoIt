using System.Collections;
using System.Collections.Generic;
using DoDoDoIt;
using UnityEngine;

public class FootstepEffect : MonoBehaviour
{
    [Header("Footstep Settings")]
    [SerializeField]
    private GameObject _footprintPrefab; // 발자국 프리팹
    [SerializeField]
    private Transform _leftFootTransform; // 왼쪽 발 위치
    [SerializeField]
    private Transform _rightFootTransform; // 오른쪽 발 위치
    [SerializeField]
    private LayerMask _groundLayer; // 발자국이 생성될 레이어 (예: 지면)
    [SerializeField]
    private float _footprintLifetime = 5f; // 발자국 지속 시간
    [SerializeField]
    private float _spawnInterval = 0.5f; // 발자국 생성 간격

    [Header("Object Pool Settings")]
    [SerializeField]
    private int _poolSize = 10; // 풀의 크기

    private Queue<GameObject> _footprintPool; // 발자국을 관리할 큐
    private float _lastSpawnTime; // 마지막으로 발자국이 생성된 시간
    private bool _isLeftFoot = true; // 다음에 생성할 발이 왼쪽 발인지 오른쪽 발인지 확인
    private PlayerController _player;

    void Start()
    {
        _player = GetComponent<PlayerController>();
        // 오브젝트 풀 초기화
        InitializePool();
        
    }

    void Update()
    {
        if (Time.time >= _lastSpawnTime + _spawnInterval && (_player.StateMachine.GetCurrentState() is PlayerRunningState))
        {
            SoundManager.Instance.PlaySFX("event:/SFX/Dodo/Step");
            // 왼쪽 또는 오른쪽 발 위치를 기준으로 발자국 생성
            if (_isLeftFoot)
            {
                CreateFootprint(_leftFootTransform);
            }
            else
            {
                CreateFootprint(_rightFootTransform);
            }

            // 다음 발로 전환
            _isLeftFoot = !_isLeftFoot;
            _lastSpawnTime = Time.time;
        }
    }

    // 오브젝트 풀 초기화
    private void InitializePool()
    {
        _footprintPool = new Queue<GameObject>();

        for (int i = 0; i < _poolSize; i++)
        {
            GameObject obj = Instantiate(_footprintPrefab);
            obj.SetActive(false);
            _footprintPool.Enqueue(obj);
        }
    }

    // 풀에서 발자국 오브젝트를 가져오는 함수
    private GameObject GetFootprintFromPool()
    {
        if (_footprintPool.Count > 0)
        {
            GameObject footprint = _footprintPool.Dequeue();
            footprint.SetActive(true);
            return footprint;
        }
        else
        {
            // 풀에 남은 오브젝트가 없을 경우 새로 생성
            GameObject newFootprint = Instantiate(_footprintPrefab);
            return newFootprint;
        }
    }

    // 발자국을 풀에 반환하는 함수
    private void ReturnFootprintToPool(GameObject footprint)
    {
        footprint.SetActive(false);
        _footprintPool.Enqueue(footprint);
    }

    // 발자국 생성 함수
    void CreateFootprint(Transform footTransform)
    {
        // 발의 위치를 기준으로 발자국 생성
        RaycastHit hit;
        if (Physics.Raycast(footTransform.position, Vector3.down, out hit, 1f, _groundLayer))
        {
            Vector3 footprintPosition = hit.point + hit.normal * 0.01f;
            // 발자국 오브젝트를 풀에서 가져옴
            GameObject footprint = GetFootprintFromPool();
            footprint.transform.position = footprintPosition;

            // 발의 회전 값에 맞춰 발자국 방향 조정
            footprint.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            // 발자국을 캐릭터 이동 방향에 맞추어 회전
            footprint.transform.forward = transform.forward;

            // 발자국이 일정 시간이 지난 후 다시 풀로 반환되도록 설정
            StartCoroutine(ReturnFootprintAfterDelay(footprint, _footprintLifetime));
        }
    }

    // 발자국을 일정 시간이 지난 후 풀로 반환하는 코루틴
    private IEnumerator ReturnFootprintAfterDelay(GameObject footprint, float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnFootprintToPool(footprint);
    }
}
