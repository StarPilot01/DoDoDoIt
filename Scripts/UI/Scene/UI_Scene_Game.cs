using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DoDoDoIt
{
    public class UI_Scene_Game : UI_Scene_Base
    {
        #region Enum

        enum GameObjects
        {
            HeartsLayoutObject
        }

        
        enum Texts
        {
            HeartsValueText,
            SpeedValueText,
            YVelocityValueText,
            CurStateText,
            SoulStackValueText,
            CoinValueText
        }

        enum Buttons
        {
            SettingButton,
            RestartButton,
            ExitButton,
            ResumeButton,
            CloseSettingButton,
            GameOverRestartButton,
            GameOverGoToMainMenuButton
        }
        

        #endregion

        //private Scene_Game _sceneGame;
        private PlayerController _playerController;
        private BuffSystem _buffSystem;
        [SerializeField]
        private GameObject _pauseMenuUI;
        [SerializeField]
        private GameObject _settingMenuUI;
        [SerializeField]
        private GameObject _gameOverUI;
        [SerializeField]
        private Image _fadeImage;
        
        private bool _isWaitingForAnimation = false;

        public override bool Init()
        {
            
            if (base.Init() == false)
                return false;
            
            BindObject(typeof(GameObjects));
            BindText(typeof(Texts));
            BindButton(typeof(Buttons));

            Button settingButton = GetButton((int)Buttons.SettingButton);
            BindEvent(settingButton.gameObject, OnSettingButtonClicked);
            
            Button restartButton = GetButton((int)Buttons.RestartButton);
            BindEvent(restartButton.gameObject, OnRestartButtonClicked);
            
            Button exitButton = GetButton((int)Buttons.ExitButton);
            BindEvent(exitButton.gameObject, OnExitButtonClicked);
            
            Button resumeButton = GetButton((int)Buttons.ResumeButton);
            BindEvent(resumeButton.gameObject, OnResumeButtonClicked);
            
            Button closeSettingButton = GetButton((int)Buttons.CloseSettingButton);
            BindEvent(closeSettingButton.gameObject, OnCloseSettingsButtonClicked);
            
            Button gameOverRestartButton = GetButton((int)Buttons.GameOverRestartButton);
            BindEvent(gameOverRestartButton.gameObject, OnRestartButtonClicked);
            
            Button gameOverGoToMainMenuButton = GetButton((int)Buttons.GameOverGoToMainMenuButton);
            BindEvent(gameOverGoToMainMenuButton.gameObject, OnGotoMainMenuButtonCliked);
            
            if (_pauseMenuUI.activeSelf)
            {
                _pauseMenuUI.SetActive(false);
                _settingMenuUI.SetActive(false);
                _gameOverUI.SetActive(false);
            }
            _playerController = ((Scene_Game)Managers.Scene.CurrentScene).Player;
            _buffSystem = FindAnyObjectByType<BuffSystem>();
            SoundManager.Instance.PlayBGM("event:/BGM/Stage");
            return true;
        }

        private void OnGotoMainMenuButtonCliked()
        {
            //타이틀씬으로 이동
            _fadeImage.DOFade(1f, 2f).OnComplete(() =>
            {
                Time.timeScale = 1f;
                // LoadingManager를 사용하여 GameScene 로드
                LoadingManager loadingManager = FindObjectOfType<LoadingManager>();
                // LoadingManager를 사용하여 GameScene 로드
                if (loadingManager != null)
                {
                    loadingManager.LoadSceneWithProgress("TitleScene");
                }
            });
        }

        private void OnResumeButtonClicked()
        {
            SoundManager.Instance.PlaySFX("event:/SFX/Ui/Button", "UI", 1f);
            Time.timeScale= 1f;
            _pauseMenuUI.SetActive(false);
        }

        private void OnExitButtonClicked()
        {
            SoundManager.Instance.PlaySFX("event:/SFX/Ui/Button", "UI", 1f);
            Application.Quit();

#if UNITY_EDITOR
            SoundManager.Instance.PlaySFX("event:/SFX/Ui/Button", "UI", 1f);
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        private void OnRestartButtonClicked()
        {
            SoundManager.Instance.PlaySFX("event:/SFX/Ui/Button", "UI", 0f);
            SoundManager.Instance.PopupPause(true);
            Time.timeScale = 1f;
            _fadeImage.DOFade(1f, 2f).OnComplete(() =>
            {
                _playerController.TransitionTo(Define.EPlayerState.Idle);
                // LoadingManager를 사용하여 GameScene 로드
                LoadingManager loadingManager = FindObjectOfType<LoadingManager>();
                // LoadingManager를 사용하여 GameScene 로드
                if (loadingManager != null)
                {
                    loadingManager.LoadSceneWithProgress("GameScene");
                }
            });
        }

        private void OnSettingButtonClicked()
        {
            SoundManager.Instance.PlaySFX("event:/SFX/Ui/Button", "UI", 0f);
            _pauseMenuUI.SetActive(false);
            _settingMenuUI.SetActive(true);
        }
        
        public void OnCloseSettingsButtonClicked()
        {
            SoundManager.Instance.PlaySFX("event:/SFX/Ui/Button", "UI", 1f);
            _settingMenuUI.SetActive(false);
            _pauseMenuUI.SetActive(true);
        }

        private void Start()
        {
            Init();
        }

        
        //TODO: 필요할 때만 Refresh하도록 수정?
        public void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape) && !_gameOverUI.activeSelf)
            {
                PauseGame();
            }

            if (_playerController.StateMachine.GetCurrentState() is PlayerDeadState &&  _playerController.Animator.GetCurrentAnimatorStateInfo(0).IsName("Dead") && !_isWaitingForAnimation)
            {
                StartCoroutine(WaitForAnimationToEnd());
            }
            Refresh();
        }

        private IEnumerator WaitForAnimationToEnd()
        {
            
            _isWaitingForAnimation = true;
            while (_playerController.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.3f)
            {
                Debug.Log("is waiting animation end");
                yield return null;
            }
            ShowGameOverUI();
        }

        private void ShowGameOverUI()
        {
            // 이미 Game Over UI가 활성화된 경우 중복 실행 방지
            if (_gameOverUI.activeSelf)
                return;

            // Game Over UI 활성화 후 페이드 인
            _gameOverUI.SetActive(true);
            CanvasGroup canvasGroup = _gameOverUI.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                // CanvasGroup이 없는 경우 추가
                canvasGroup = _gameOverUI.AddComponent<CanvasGroup>();
                canvasGroup.alpha = 0; // 초기 알파 값
            }

            // DOTween으로 페이드 인 효과 추가
            canvasGroup.DOFade(1f, 1.5f).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                
            });
        }

        private void PauseGame()
        {
            if (Time.timeScale <= 0f)
            {
                SoundManager.Instance.PopupPause(false);
                Time.timeScale = 1f;
                _pauseMenuUI.SetActive(false);
                _settingMenuUI.SetActive(false);
                // Time.timeScale = 0 상태에서도 입력을 받을 수 있도록 설정
                EventSystem.current.UpdateModules();
            }
            else
            {
                SoundManager.Instance.PopupPause(true);
                Time.timeScale = 0f;
                _pauseMenuUI.SetActive(true);
            }
        }

        void Refresh()
        {


            //RefreshDebugInfo();
            RefreshHeartsLayout();

            GetText((int)Texts.CoinValueText).text = "x" + Managers.Score.MyScore.ToString();


        }

        void RefreshDebugInfo()
        {
            if (_buffSystem == null)
            {
                Debug.LogError("error");
            }
            
            
            GetText((int)Texts.HeartsValueText).text = $"{_playerController.Stats.PlayerHealth.Hearts}";

            string velocityStr = $"ForwardSpeed : {_playerController.Stats.Attributes.ForwardSpeed.GetValue()} \n" +
                                 $"HorizontalSpeed : {_playerController.Stats.Attributes.HorizontalSpeed.GetValue()}";
            GetText((int)Texts.SpeedValueText).text = velocityStr;
            GetText((int)Texts.YVelocityValueText).text = _playerController.Rigidbody.velocity.y.ToString();
            GetText((int)Texts.CurStateText).text = _playerController.StateMachine.CurrentStateEnum.ToString();
            
            //GetText((int)Texts.SoulStackValueText).text = "SoulStack : " + _buffSystem.SoulStack.ToString();
        }

        void ClearExistingHearts(GameObject layoutObject)
        {
            int childCount = layoutObject.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Destroy(layoutObject.transform.GetChild(i).gameObject);
            }
        }

        

        void RefreshHeartsLayout()
        {
            
            GameObject heartsLayoutObject = GetObject((int)GameObjects.HeartsLayoutObject);

            ClearExistingHearts(heartsLayoutObject); // 기존 하트 제거

            int totalHearts = _playerController.Stats.PlayerHealth.MaxHearts;
            int currentHearts = _playerController.Stats.PlayerHealth.Hearts;
            int fullHearts = currentHearts / 2;  // 완전한 하트 개수
            int halfHearts = currentHearts % 2;  // 반쪽 하트가 필요한지 여부
            int emptyHearts = (totalHearts - currentHearts) / 2;

            
            
            
            for (int i = 0; i < fullHearts; i++)
            {
                Managers.Resource.Instantiate("UI_FullHeartItem", heartsLayoutObject.transform);
                
            }

            
            if (halfHearts == 1)
            {
                Managers.Resource.Instantiate("UI_HalfHeartItem", heartsLayoutObject.transform);

            }
            
            for (int i = 0; i < emptyHearts; i++)
            {
                Managers.Resource.Instantiate("UI_EmptyHeartItem", heartsLayoutObject.transform);
                
            }
        }
            
    }
}
