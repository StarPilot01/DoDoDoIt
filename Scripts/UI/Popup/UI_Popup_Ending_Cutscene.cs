using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using FMOD.Studio;
using FMODPlus;
using FMODUnity;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using UnityEngine.Video;
using Image = UnityEngine.UI.Image;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace DoDoDoIt
{
    
    public class UI_Popup_Ending_Cutscene : UI_Popup
    {

        [SerializeField]
        private VideoPlayer _cutsceneVideoClipPlayer;

        [SerializeField]
        private RawImage _videoImage;

        [SerializeField]
        private Image _fadeImage;

        [SerializeField]
        private Image _skipButton;

        private EventInstance _amb;
        private VideoClip[] cutsceneVideoClips;
        private int currentFrame = 0;

        private bool _isFadingOut = false;


        public override bool Init()
        { 
            if (base.Init() == false)
                return false;
            if (_videoImage.enabled)
            {

                
                
                
                //Managers.Resource.LoadAllAsync<VideoClip>("End_Cutscene", (key, count, totalCount) =>
                //{
                //    if (count == totalCount)
                //    {
                //        cutsceneVideoClips = new VideoClip[4];
                //        cutsceneVideoClips[0] = Managers.Resource.Load<VideoClip>("Ending_Cutscene_1");
                //        cutsceneVideoClips[1] = Managers.Resource.Load<VideoClip>("Ending_Cutscene_2");
                //        cutsceneVideoClips[2] = Managers.Resource.Load<VideoClip>("Ending_Cutscene_3");
                //        cutsceneVideoClips[3] = Managers.Resource.Load<VideoClip>("Ending_Cutscene_4");
                //        
                //        
                //        ShowFrame(currentFrame);
//
                //    }
                //});
            }


            cutsceneVideoClips = new VideoClip[4];
            cutsceneVideoClips[0] = Managers.Resource.Load<VideoClip>("Ending_Cutscene_1");
            cutsceneVideoClips[1] = Managers.Resource.Load<VideoClip>("Ending_Cutscene_2");
            cutsceneVideoClips[2] = Managers.Resource.Load<VideoClip>("Ending_Cutscene_3");
            cutsceneVideoClips[3] = Managers.Resource.Load<VideoClip>("Ending_Cutscene_4");
            SoundManager.Instance.PopupPause(true);
            //
//
            //
            //
            //
            //
            
            BindEvent(_skipButton.gameObject, OnSkipButtonClicked);
            BindEvent(_videoImage.gameObject, OnNextFrameClicked);

            _fadeImage.DOFade(0f, 2f).OnComplete(() => { _cutsceneVideoClipPlayer.loopPointReached += OnVideoEnd; });
            
            ShowFrame(currentFrame);
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
            if (frameIndex < cutsceneVideoClips.Length)
            {
                _cutsceneVideoClipPlayer.clip = cutsceneVideoClips[frameIndex];
                
                string eventPath = $"event:/Cutscene/end/end_Cutscene_{frameIndex + 1}";
                
                Debug.Log(_cutsceneVideoClipPlayer.clip.name);
                gameObject.GetComponent<FMODAudioSource>().clip = RuntimeManager.PathToEventReference(eventPath);
                gameObject.GetComponent<FMODAudioSource>().Play();
            }
        }

        void OnVideoEnd(VideoPlayer vp)
        {
            OnNextFrameClicked();
        }

        private void OnSkipButtonClicked()
        {
            SoundManager.Instance.PlaySFX("event:/SFX/Ui/Button", "UI", 3f);
            _isFadingOut = true;
            SoundManager.Instance.StopBGM();
            _fadeImage.color = new Color(255f, 255f, 255f, 0f);
            _fadeImage.DOFade(1f, 2f).OnComplete(() =>
            {
                _videoImage.gameObject.SetActive(false);

                Managers.UI.ClosePopupUI(this);
                Managers.UI.ShowPopupUI<UI_Popup_NameInput>().gameObject.SetActive(true);
                
            });
        }

        private void OnNextFrameClicked()
        {
            if (_isFadingOut) return;

            currentFrame++;
            if (currentFrame < cutsceneVideoClips.Length)
            {
                ShowFrame(currentFrame);
            }
            else
            {
                _fadeImage.color = new Color(255f, 255f, 255f, 0f);
                _isFadingOut = true;
                SoundManager.Instance.StopBGM();
                _fadeImage.DOFade(1f, 2f).OnComplete(() =>
                {
                    _videoImage.gameObject.SetActive(false);
                    Managers.UI.ClosePopupUI(this);
                    Managers.UI.ShowPopupUI<UI_Popup_NameInput>().gameObject.SetActive(true);
                });
            }
        }
    }
}