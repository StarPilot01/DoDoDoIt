using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoDoDoIt
{
    public class ScoreManager
    {
         private const string HighScoresKey = "HighScores"; // PlayerPrefs 키

        public List<KeyValuePair<string, int>> HighScores { get; private set; } // 이름과 점수 리스트
        public string MyName { get; set; } // 내 이름

        private int _myScore = 0;
        public int MyScore
        {
            get
            {
                return _myScore;
            }
            set
            {
                _myScore = value;
                
                
            }
        }

        public ScoreManager()
        {
            // 점수 데이터 로드
        }

        public void Init()
        {
          
            LoadScores();

        }
        // 점수 저장
        public void SaveScores()
        {
            // 이름과 점수를 JSON 형식으로 변환
            List<string> serializedScores = new List<string>();
            foreach (var entry in HighScores)
            {
                serializedScores.Add($"{entry.Key}:{entry.Value}");
            }
            string highScoresString = string.Join(",", serializedScores);

            PlayerPrefs.SetString(HighScoresKey, highScoresString);
            PlayerPrefs.Save();
        }

        // 점수 로드
        public void LoadScores()
        {
            HighScores = new List<KeyValuePair<string, int>>();

            if (PlayerPrefs.HasKey(HighScoresKey))
            {
                string highScoresString = PlayerPrefs.GetString(HighScoresKey);
                string[] entries = highScoresString.Split(',');

                foreach (string entry in entries)
                {
                    string[] pair = entry.Split(':');
                    if (pair.Length == 2)
                    {
                        string name = pair[0];
                        int score = int.Parse(pair[1]);
                        HighScores.Add(new KeyValuePair<string, int>(name, score));
                    }
                }

                // 점수를 기준으로 내림차순 정렬
                HighScores.Sort((a, b) => b.Value.CompareTo(a.Value));
            }
        }

        // 점수 추가 및 정렬
        public void AddScore(string name, int score)
        {
            HighScores.Add(new KeyValuePair<string, int>(name, score));

            // 점수를 기준으로 내림차순 정렬
            HighScores.Sort((a, b) => b.Value.CompareTo(a.Value));

            SaveScores(); // 변경 사항 저장
        }

        // 내 점수와 이름 설정 (내 점수를 HighScores에 추가 및 갱신)
        public void SetMyScore(string name, int score)
        {
            MyName = name;
            MyScore = score;

            // 내 이름과 점수를 HighScores에 추가
            AddScore(name, score);
        }

        // 특정 등수의 이름과 점수 가져오기
        public KeyValuePair<string, int> GetScore(int rank)
        {
            if (rank <= 0 || rank > HighScores.Count)
            {
                Debug.LogWarning($"Rank {rank} is out of range.");
                return new KeyValuePair<string, int>("Invalid", -1); // 유효하지 않은 순위
            }

            return HighScores[rank - 1]; // 등수는 리스트의 인덱스 + 1
        }

        // 내 점수의 등수 반환
        // 점수랑 이름이 같으면 등수 제대로 안 나오지만 그럴 경우의 수 없다고 가정
        public int GetMyRank()
        {
            for (int i = 0; i < HighScores.Count; i++)
            {
                if (HighScores[i].Key == MyName && HighScores[i].Value == MyScore)
                {
                    return i + 1; // 등수는 인덱스 + 1
                }
            }

            Debug.LogWarning("My score is not in the HighScores list.");
            return -1;
        }
        
        public void ResetScores()
        {
            // HighScores 리스트 초기화
            HighScores.Clear();

            // 내 이름과 점수 초기화
            MyName = string.Empty;
            MyScore = 0;

            // PlayerPrefs에서 데이터 삭제
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();

           
        }
    }
}
