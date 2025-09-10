using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

namespace DoDoDoIt.Tests
{
    public class ScoreManagerTests : MonoBehaviour
    {
        //private ScoreManager _scoreManager;

        
        public void Awake()
        {
            // ScoreManager 초기화
            //_scoreManager = Managers.Score;
            //_scoreManager.HighScores.Clear(); // 기존 점수 초기화
//
//          
            //ResetScoreManager();
            //AddScore_SortsScoresDescending();
            //
            //ResetScoreManager();
            //GetScore_ReturnsCorrectRank();
            //
            //ResetScoreManager();
            //GetMyRank_ReturnsCorrectRank();
            //
            //ResetScoreManager();
            //NoDuplicateEntriesForSamePlayer();
            
        }

        private void Start()
        {
            //ResetScoreManager();
        }

        private void ResetScoreManager()
        {
            // 기존 데이터를 초기화
            Managers.Score.HighScores?.Clear();
            Managers.Score.MyName = string.Empty;
            Managers.Score.MyScore = 0;

            // 저장된 PlayerPrefs 초기화 (테스트 환경에 영향을 주지 않도록)
            PlayerPrefs.DeleteAll();
        }
        public void AddScore_SortsScoresDescending()
        {
            // 점수 추가
            Managers.Score.AddScore("Alice", 100);
            Managers.Score.AddScore("Bob", 200);
            Managers.Score.AddScore("Charlie", 50);

            // 확인: 점수는 내림차순으로 정렬되어야 함
            Assert.AreEqual("Bob", Managers.Score.HighScores[0].Key);
            Assert.AreEqual(200, Managers.Score.HighScores[0].Value);

            Assert.AreEqual("Alice", Managers.Score.HighScores[1].Key);
            Assert.AreEqual(100, Managers.Score.HighScores[1].Value);

            Assert.AreEqual("Charlie", Managers.Score.HighScores[2].Key);
            Assert.AreEqual(50, Managers.Score.HighScores[2].Value);
        }

        
        public void GetScore_ReturnsCorrectRank()
        {
            // 점수 추가
            Managers.Score.AddScore("Alice", 300);
            Managers.Score.AddScore("Bob", 500);
            Managers.Score.AddScore("Charlie", 400);

            // 확인: 각 등수에 맞는 이름과 점수를 반환해야 함
            var firstPlace = Managers.Score.GetScore(1);
            Assert.AreEqual("Bob", firstPlace.Key);
            Assert.AreEqual(500, firstPlace.Value);

            var secondPlace = Managers.Score.GetScore(2);
            Assert.AreEqual("Charlie", secondPlace.Key);
            Assert.AreEqual(400, secondPlace.Value);

            var thirdPlace = Managers.Score.GetScore(3);
            Assert.AreEqual("Alice", thirdPlace.Key);
            Assert.AreEqual(300, thirdPlace.Value);
        }

        
        public void GetMyRank_ReturnsCorrectRank()
        {
            // 점수와 이름 설정
            Managers.Score.SetMyScore("MyPlayer", 450);

            // 다른 점수 추가
            Managers.Score.AddScore("Alice", 300);
            Managers.Score.AddScore("Bob", 500);
            Managers.Score.AddScore("Charlie", 400);

            // 확인: 내 점수의 랭킹 확인
            int myRank = Managers.Score.GetMyRank();
            Assert.AreEqual(2, myRank); // 내 점수(450)는 2등이어야 함
        }

        
        public void NoDuplicateEntriesForSamePlayer()
        {
            // 동일 플레이어 이름으로 점수 갱신
            Managers.Score.SetMyScore("MyPlayer", 400);
            Managers.Score.SetMyScore("MyPlayer", 450);

            // 확인: 동일 이름은 중복되지 않음
            int myRank = Managers.Score.GetMyRank();
            Assert.AreEqual(1, myRank); // 450은 1등이어야 함

            //Assert.AreEqual(1, _scoreManager.HighScores.FindAll(x => x.Key == "MyPlayer").Count); // 중복 항목 없음
        }
    }
}
