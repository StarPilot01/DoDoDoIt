using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FMODPlus;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DoDoDoIt
{
    public class LoadingManager : MonoBehaviour
    {

        [SerializeField] private GameObject _loadingScene;

        [SerializeField] private Image _progressBar;

        [SerializeField] private Image _fadeImage;

        private static bool _isLoading = false;

        public void LoadSceneWithProgress(string _sceneName)
        {
            if (_isLoading) return;

            _loadingScene.SetActive(true);
            _isLoading = true;
            
            _fadeImage.DOFade(0f, 1f).OnComplete(() =>
            {
                    StartCoroutine(LoadSceneAsync(_sceneName));
            });
        }
        
        private IEnumerator LoadSceneAsync(string sceneName)
        {
            AsyncOperation _operation = SceneManager.LoadSceneAsync(sceneName);
            _operation.allowSceneActivation = false;

            while (!_operation.isDone)
            {
                float progress = Mathf.Clamp01(_operation.progress / 0.9f);
                _progressBar.fillAmount = progress;

                if (_operation.progress >= 0.9f)
                {
                    _fadeImage.DOFade(1f,1f).OnComplete(() =>
                    {
                        
                        Managers.UI.CloseAllPopupUI();
                        _isLoading = false;
                        _operation.allowSceneActivation = true; 
                        
                    });
                }
                
                yield return null;
            }
        }
    }
}
