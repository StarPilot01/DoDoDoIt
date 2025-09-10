using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace DoDoDoIt
{
    public class DebugTools : MonoBehaviour
    {
        // Start is called before the first frame update
        
        public PlayerController Player; // 플레이어 오브젝트 참조
        private PlayableDirector _timelineDirector;


        public Image fadeImage;
        public Transform[] DebugPoints; // 특정 위치로 이동할 디버그 포인트들
        //private int _currentPointIndex = 0; // 현재 선택된 포인트 인덱스

        void Awake()
        {
            //DontDestroyOnLoad(this);

            Player = FindAnyObjectByType<PlayerController>();
            //_timelineDirector = Player.GetComponent<PlayableDirector>();
            
           // InitializeDebugPoints();
        }

        void Update()
        {
            if (Player == null)
            {
                Debug.LogWarning("Player is not assigned!");
                return;
            }

            HandleDebugInputs();
        }

        private void HandleDebugInputs()
        {
            // 체력 조정
            if (Input.GetKeyDown(KeyCode.Alpha1)) // H 키로 체력 회복
            {
                AdjustPlayerHealth(1); // 체력 1 회복
            }

            //if (Input.GetKeyDown(KeyCode.Alpha2)) // D 키로 체력 감소
            //{
            //    AdjustPlayerHealth(-1); // 체력 1 감소
            //}
//
            //// 특정 위치로 이동
            //if (Input.GetKeyDown(KeyCode.UpArrow)) // P 키로 포인트 이동
            //{
            //    MoveToNextDebugPoint();
            //}
//
            //if (Input.GetKeyDown(KeyCode.DownArrow))
            //{
            //    MoveToPreviousDebugPoint();
            //}
//
            //// 가장 끝 디버그 포인트로 이동 (End 키)
            //if (Input.GetKeyDown(KeyCode.End) || Input.GetKeyDown(KeyCode.O))
            //{
            //    MoveToLastDebugPoint();
            //}
//
//
            //if (Input.GetKeyDown(KeyCode.Q))
            //{
            //    ResetAndStopTimeline();
            //}
          
        }

        private void AdjustPlayerHealth(int amount)
        {
            if (Player == null || Player.Stats == null || Player.Stats.PlayerHealth == null)
            {
                Debug.LogWarning("PlayerHealth is not available.");
                return;
            }

            Player.Stats.PlayerHealth.Hearts += amount;

            // 체력 상태 출력
            //Debug.Log($"Player health adjusted by {amount}. Current health: {Player.Stats.PlayerHealth.Hearts}/{Player.Stats.PlayerHealth.MaxHearts}");
        }

        //private void MoveToNextDebugPoint()
        //{
        //    if (DebugPoints == null || DebugPoints.Length == 0)
        //    {
        //        //Debug.LogWarning("No Debug Points assigned!");
        //        return;
        //    }
//
        //    // 다음 디버그 포인트로 이동
//
        //    if (_currentPointIndex < DebugPoints.Length)
        //    {
        //        Transform targetPoint = DebugPoints[_currentPointIndex++];
        //        Player.transform.position = targetPoint.position;
        //        Player.transform.rotation = targetPoint.rotation;
        //    }
        //    //_currentPointIndex = (_currentPointIndex + 1) % DebugPoints.Length;
        //    
//
        //    //Debug.Log($"Player moved to next Debug Point {_currentPointIndex} at {targetPoint.position}");
        //}
//
        //private void MoveToPreviousDebugPoint()
        //{
        //    if (DebugPoints == null || DebugPoints.Length == 0)
        //    {
        //        //Debug.LogWarning("No Debug Points assigned!");
        //        return;
        //    }
//
        //    // 이전 디버그 포인트로 이동
        //    
        //    if (_currentPointIndex > 0 && DebugPoints.Length > 0)
        //    {
        //        Transform targetPoint = DebugPoints[_currentPointIndex--];
        //        Player.transform.position = targetPoint.position;
        //        Player.transform.rotation = targetPoint.rotation;
        //    }
        //    
        //    //_currentPointIndex = (_currentPointIndex - 1 + DebugPoints.Length) % DebugPoints.Length;
        //    //Transform targetPoint = DebugPoints[_currentPointIndex];
        //    //Player.transform.position = targetPoint.position;
//
        //    //Debug.Log($"Player moved to previous Debug Point {_currentPointIndex} at {targetPoint.position}");
        //    
        //    Debug.Log(_currentPointIndex);
        //}
//
        //private void MoveToLastDebugPoint()
        //{
        //    if (DebugPoints == null || DebugPoints.Length == 0)
        //    {
        //        //Debug.LogWarning("No Debug Points assigned!");
        //        return;
        //    }
//
        //    
        //    // 가장 마지막 디버그 포인트로 이동
        //    _currentPointIndex = DebugPoints.Length - 1;
        //    Transform targetPoint = DebugPoints[_currentPointIndex];
        //    Player.transform.position = targetPoint.position;
        //    Player.transform.rotation = targetPoint.rotation;
//
        //    //Debug.Log($"Player moved to last Debug Point {_currentPointIndex} at {targetPoint.position}");
        //    
        //    //Debug.Log(_currentPointIndex);
        //}
        //
        //public void ResetAndStopTimeline()
        //{
        //    if (_timelineDirector == null)
        //    {
        //        Debug.LogWarning("PlayableDirector is not assigned.");
        //        return;
        //    }
//
        //    // 타임라인 정지
//
        //    //// 초기 상태로 되돌림 (타임라인의 시작 상태로)
        //    //_timelineDirector.time = 0; 
        //    //_timelineDirector.Evaluate(); // Evaluate를 호출해 즉시 상태를 초기화
        //    _timelineDirector.Stop();
//
        //    Player.EndTutorial();
        //    Player.StartGame();
        //    fadeImage.gameObject.SetActive(false);
        //}
        //
        //private void InitializeDebugPoints()
        //{
        //    
        //    Transform parentDebugPoints = GameObject.Find("@DebugPoints")?.transform;
//
        //    
        //   
//
        //    int childCount = parentDebugPoints.childCount;
        //    DebugPoints = new Transform[childCount];
//
        //    for (int i = 0; i < childCount; i++)
        //    {
        //        DebugPoints[i] = parentDebugPoints.GetChild(i);
        //    }
//
        //}
    }
    
    
}
