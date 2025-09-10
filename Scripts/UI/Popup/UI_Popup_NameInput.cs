using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DoDoDoIt
{
    public class UI_Popup_NameInput : UI_Popup
    {
        
        #region Enum

        enum Images
        {
            BG
        }
        enum GameObjects
        {
            NameInputField
        }

        enum Buttons
        {
            ConfirmButton
        }

        enum Texts
        {
            NameText
        }
        
        #endregion
        // Start is called before the first frame update

        //private TMP_Text _nameInputFieldText;
        
        private void Awake()
        {
            Init();
        }

        public override bool Init()
        {
            if (base.Init() == false)
                return false;
            
            BindImage(typeof(Images));
            BindObject(typeof(GameObjects));
            BindButton(typeof(Buttons));
            BindText(typeof(Texts));
            
            
            //_nameInputFieldText = GetText((int)Texts.NameText);

            //Debug.Log(GetText((int)Texts.NameText).text);
            
            GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);
            
            return true;
        }
        
        void OnClickConfirmButton()
        {
            
            //Debug.Log(GetText((int)Texts.NameText).text + " " +GetText((int)Texts.NameText).text.Length );

            if (IsValidNameInput(GetText((int)Texts.NameText).text))
            {
                Managers.Score.MyName = GetText((int)Texts.NameText).text;
                //Managers.Score.MyScore = 
                
                Managers.Score.SetMyScore(GetText((int)Texts.NameText).text, Managers.Score.MyScore);
                Debug.Log(Managers.Score.MyScore);
                
                Managers.UI.ClosePopupUI(this);
                Managers.UI.ShowPopupUI<UI_Popup_RecordBoard>().gameObject.SetActive(true);
                
            }
            else
            {
                //Debug.Log(GetText((int)Texts.NameText).text);

                Managers.UI.ShowPopupUI<UI_Popup_NameInputAlert>().gameObject.SetActive(true);

            }
        }
        
        bool IsValidNameInput(string input)
        {
            input = input.Replace("\u200B", "");
            //input = input.Trim();
            // 입력이 공백이 아니고, 길이가 6글자 이하인지 확인
            return !string.IsNullOrWhiteSpace(input) && input.Length <= 6;
        }
    }
}
