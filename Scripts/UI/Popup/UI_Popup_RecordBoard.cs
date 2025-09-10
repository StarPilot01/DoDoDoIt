using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DoDoDoIt
{
    public class UI_Popup_RecordBoard : UI_Popup
    {
        #region Enum

        enum Images
        {
            
        }
        
        enum Buttons
        {
           ReturnButton
        }

        enum GameObjects
        {
            RecordListItem1,
            RecordListItem2,
            RecordListItem3,
            RecordListItem4,
            RecordListItem5,
            MyRecordItem
        }
    
        #endregion

     
        
        public override bool Init()
        {
            if (base.Init() == false)
                return false;
            
            
            
            BindObject(typeof(GameObjects));
            BindButton(typeof(Buttons));
            
            GetButton((int)Buttons.ReturnButton).gameObject.BindEvent(OnClickReturnButton);


           

            
            SetInfo();
            
            return true;
        }

        void SetInfo()
        {
            for (int i = 0; i < 5; i++)
            {
                var recordItem = GetObject((int)GameObjects.RecordListItem1 + i);
                recordItem.SetActive(false);
            }

            for (int i = 0; i < 5; i++)
            {
                var recordItem = GetObject((int)GameObjects.RecordListItem1 + i);
                if (recordItem == null)
                {
                    Debug.LogWarning($"RecordListItem{i + 1} not found.");
                    continue;
                }

                // ScoreManager에서 해당 등수의 이름과 점수 가져오기
                var scoreEntry = Managers.Score.GetScore(i + 1); // 1위부터 시작
                if (scoreEntry.Key == "Invalid")
                {
                    Debug.LogWarning($"Invalid score entry for rank {i + 1}.");
                    continue;
                }
                recordItem.SetActive(true);
                // NameText와 ScoreText에 값 설정
                var nameText = recordItem.transform.Find("NameText").GetComponent<TMP_Text>();
                var scoreText = recordItem.transform.Find("ScoreText").GetComponent<TMP_Text>();

                
                if (nameText != null) nameText.text = (i + 1).ToString() + "." + scoreEntry.Key; // 이름
                if (scoreText != null) scoreText.text = "x " + scoreEntry.Value.ToString(); // 점수
            }

            // MyRecordItem에 내 이름과 점수 설정
            var myRecordItem = GetObject((int)GameObjects.MyRecordItem);
            if (myRecordItem != null)
            {
                var myNameText = myRecordItem.transform.Find("NameText").GetComponent<TMP_Text>();
                var myScoreText = myRecordItem.transform.Find("ScoreText").GetComponent<TMP_Text>();

                string prefix = Managers.Score.GetMyRank().ToString();
                if (myNameText != null) myNameText.text = prefix +  "." + Managers.Score.MyName; // 내 이름
                if (myScoreText != null) myScoreText.text = "x " + Managers.Score.MyScore.ToString(); // 내 점수
            }
        }

        void OnClickReturnButton()
        {
            //Destroy(Managers.Instance);

            Managers.UI.CloseAllPopupUI();

            SceneManager.LoadScene("TitleScene");


        }


    }
}
