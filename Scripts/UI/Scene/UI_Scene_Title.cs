using UnityEngine;
using TMPro;
using DG.Tweening;
using FMODPlus;
using FMODUnity;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DoDoDoIt
{
    public class UI_Scene_Title : UI_Scene_Base
    {
        public float endVal;
        public float duration;
        [SerializeField]
        private Image _titleImage;
        [SerializeField] 
        private GameObject _settingGameObject;

        [SerializeField]
        private Image _fadeOutImage;

        [SerializeField]
        private bool _isStarted;
        enum Texts
        {
            PressToStartText
        }

        enum Buttons
        {
            FullScreenButton, // 전체 화면 클릭을 위한 버튼
            ExitGameButton,
            SettingButton,
            CloseButton
        }
        
        public override bool Init()
        {
            if (base.Init() == false)
                return false;
            // 텍스트 및 버튼 바인딩
            BindText(typeof(Texts));
            BindButton(typeof(Buttons));

            // "Press to Start" 텍스트에 알파 페이드 효과 추가
            TMP_Text pressToStartText = GetText((int)Texts.PressToStartText);
            pressToStartText.DOFade(0, 1f).SetLoops(-1, LoopType.Yoyo);

            _titleImage.DOFade(endVal, duration).SetLoops(-1, LoopType.Yoyo);
            _titleImage.transform.DOScale(7.8f, duration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            // FullScreenButton에 클릭 이벤트 바인딩
            Button fullScreenButton = GetButton((int)Buttons.FullScreenButton);
            BindEvent(fullScreenButton.gameObject, OnPressToStartClicked);
            
            Button exitGameButton = GetButton((int)Buttons.ExitGameButton);
            BindEvent(exitGameButton.gameObject, OnPressToExitClicked);
            
            Button settingButton = GetButton((int)Buttons.SettingButton);
            BindEvent(settingButton.gameObject, OnPressToSettingClicked);
            
            Button closeButton = GetButton((int)Buttons.CloseButton);
            BindEvent(closeButton.gameObject, OnPressToSettingClicked);

            if (_settingGameObject.activeSelf)
            {
                _settingGameObject.SetActive(false);
            }
            return true;
        }

        private void Start()
        {
            Init();
            SoundManager.Instance.PlayBGM("event:/BGM/Main");
            _fadeOutImage.DOFade(0f, 3f);
            _isStarted = false;
        }

        private void OnPressToStartClicked()
        {
            if (!_isStarted)
            {
                _isStarted = true;
                SoundManager.Instance.PlaySFX("event:/SFX/Ui/Button", "UI", 2);
                // 타이틀 화면 페이드 아웃
                Managers.UI.SetCanvas(this.gameObject, false);
                GetComponent<CanvasGroup>().DOFade(0, 2f).OnComplete(() =>
                {
                    // 모든 팝업을 닫고 컷씬을 시작하는 UI 팝업 표시
                    Managers.UI.CloseAllPopupUI();
                    Managers.UI.ShowPopupUI<UI_Popup>("UI_Popup_Cutscene");
                });
            }
        }
        private void OnPressToSettingClicked()
        {
            SoundManager.Instance.PlaySFX("event:/SFX/Ui/Button", "UI", 0f);
            _settingGameObject.SetActive(!_settingGameObject.activeSelf);
        }

        private void OnPressToExitClicked()
        {
            Application.Quit();
            
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }
}