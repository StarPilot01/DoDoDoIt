using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DoDoDoIt;
using FMOD.Studio;
using FMODPlus;
using FMODUnity;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Video;
using Image = UnityEngine.UI.Image;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class UI_Popup_Cutscene : UI_Popup
{
    
    [SerializeField]
    private VideoPlayer _cutsceneVideoClipPlayer;
    [SerializeField]
    private RawImage _videoImage;
    [SerializeField] 
    private Image _fadeImage;
    [SerializeField]
    private Image _skipButton;
    [SerializeField] 
    private GameObject _loadingScene;

    private EventInstance _amb;
    private VideoClip[] cutsceneVideoClips;
    private int currentFrame = 0;
    private bool _isFadingOut = false;

    
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        if (!_loadingScene.gameObject.activeSelf)
        {
            if (_videoImage.enabled)
            {
                Addressables.LoadAssetsAsync<VideoClip>("Cutscene", null).Completed += OnLoadCutsceneFramesComplete;
            }
        
            _amb = RuntimeManager.CreateInstance("event:/AMB/Main");
            BindEvent(_skipButton.gameObject, OnSkipButtonClicked);
            BindEvent(_videoImage.gameObject, OnNextFrameClicked);

            _fadeImage.DOFade(0f, 2f).OnComplete(() =>
            {
                _cutsceneVideoClipPlayer.loopPointReached += OnVideoEnd;
            });
        }
        return true;
    }

    private void OnLoadCutsceneFramesComplete(AsyncOperationHandle<IList<VideoClip>> op)
    {
        if (op.Status == AsyncOperationStatus.Succeeded && !_isFadingOut)
        {
            cutsceneVideoClips = op.Result.OrderBy(video => video.name).ToArray();
            if (cutsceneVideoClips.Length > 0)
            {
                ShowFrame(currentFrame);
            }
        }
    }

    private void ShowFrame(int frameIndex)
    {
        if (frameIndex < cutsceneVideoClips.Length && !_loadingScene.gameObject.activeSelf)
        {
            _cutsceneVideoClipPlayer.clip = cutsceneVideoClips[frameIndex];
            
            string eventPath = $"event:/Cutscene/init/init_cutscene_{_cutsceneVideoClipPlayer.clip.name}";
            
            gameObject.GetComponent<FMODAudioSource>().clip = RuntimeManager.PathToEventReference(eventPath);
            gameObject.GetComponent<FMODAudioSource>().Play();
            if (frameIndex == 1)
            {
                _amb.start();
            }
            else if(frameIndex == 2)
            {
                _amb.setParameterByName("EQ", 1f);
            }
            else
            {
                _amb.stop(STOP_MODE.ALLOWFADEOUT);
                _amb.release();
            }
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        OnNextFrameClicked();
    }

    private void OnSkipButtonClicked()
    {
        if(_isFadingOut && _loadingScene.gameObject.activeSelf) return;
        
        SoundManager.Instance.PlaySFX("event:/SFX/Ui/Button", "UI", 3f);
        _isFadingOut = true;
        _amb.stop(STOP_MODE.ALLOWFADEOUT);
        _amb.release();
        SoundManager.Instance.StopBGM();
        _fadeImage.color = new Color(255f, 255f, 255f, 0f);
        _fadeImage.DOFade(1f, 2f).OnComplete(() =>
        {
            _videoImage.gameObject.SetActive(false);
            _loadingScene.gameObject.SetActive(true);
            // LoadingManager를 사용하여 GameScene 로드
            LoadingManager loadingManager = FindObjectOfType<LoadingManager>();
            _fadeImage.DOFade(0, 1f).OnComplete(() =>
            {
                // LoadingManager를 사용하여 GameScene 로드
                if (loadingManager != null && _loadingScene.gameObject.activeSelf)
                {
                    loadingManager.LoadSceneWithProgress("GameScene");
                }
            });
        });
    }

    private void OnNextFrameClicked()
    {
        if(_isFadingOut && _loadingScene.gameObject.activeSelf) return;
        currentFrame++;
        if (currentFrame < cutsceneVideoClips.Length)
        {
            ShowFrame(currentFrame);
        }
        else
        {
            _fadeImage.color = new Color(255f, 255f, 255f, 0f);
            _isFadingOut = true;
            _amb.stop(STOP_MODE.ALLOWFADEOUT);
            _amb.release();
            SoundManager.Instance.StopBGM();
            LoadingManager loadingManager = FindObjectOfType<LoadingManager>();
            _fadeImage.DOFade(1f, 2f).OnComplete(() =>
            {
                _videoImage.gameObject.SetActive(false);
                _loadingScene.gameObject.SetActive(true);
                _fadeImage.DOFade(0, 1f).OnComplete(() =>
                {
                    // LoadingManager를 사용하여 GameScene 로드
                    if (loadingManager != null && _loadingScene.gameObject.activeSelf)
                    {
                        loadingManager.LoadSceneWithProgress("GameScene");
                    }
                });
            });
        }
    }
}