using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using UnityEngine.ProBuilder;

namespace DoDoDoIt
{
    public class SoundManager : MonoBehaviour
    {
        private Bus _MASTER;
        private Bus _BGM;
        private Bus _SFX;
        private Bus _AMB;
        private static SoundManager _instance;
        public static SoundManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject soundManagerObj = new GameObject("SoundManager");
                    _instance = soundManagerObj.AddComponent<SoundManager>();
                }
                return _instance;
            }
        }

        private EventInstance bgmInstance; // BGM 관리
        private List<EventInstance> sfxInstances = new List<EventInstance>(); // 효과음 관리

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            _MASTER = RuntimeManager.GetBus("bus:/");
            _MASTER.setVolume(0.3f);
            _BGM = RuntimeManager.GetBus("bus:/BGM");
            _SFX = RuntimeManager.GetBus("bus:/SFX");
            _AMB = RuntimeManager.GetBus("bus:/AMB");
            DontDestroyOnLoad(gameObject);
        }

        // --------------------
        // BGM 관련 메서드
        // --------------------

        /// <summary>
        /// BGM 재생 (메인 카메라 위치)
        /// </summary>
        public void PlayBGM(string eventPath)
        {
            if (bgmInstance.isValid())
            {
                bgmInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                bgmInstance.release();
            }

            bgmInstance = RuntimeManager.CreateInstance(eventPath);

            // 메인 카메라 위치를 기준으로 3D 속성 설정
            Set3DAttributes(bgmInstance, Camera.main.transform.position);

            bgmInstance.start();
        }

        public void ChangeBGM(float value)
        {
            if (!bgmInstance.isValid())
            {
                bgmInstance.start();
            }

            bgmInstance.setParameterByName("inst", value);
        }

        public void PopupPause(bool on)
        {
            if (on)
            {
                bgmInstance.setParameterByName("EQ", 1f);
            }
            else
            {
                bgmInstance.setParameterByName("EQ", 0f);
            }
        }

        /// <summary>
        /// BGM 정지
        /// </summary>
        public void StopBGM()
        {
            if (bgmInstance.isValid())
            {
                bgmInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                bgmInstance.release();
            }
        }

        // --------------------
        // 효과음 관련 메서드
        // --------------------

        /// <summary>
        /// FMOD를 사용하여 사운드 효과(SFX)를 재생하며, 필요에 따라 파라미터를 설정할 수 있습니다.
        /// </summary>
        /// <param name="eventPath">재생할 FMOD 이벤트의 경로 (예: "event:/MySFX").</param>
        /// <param name="parameterName">설정할 FMOD 파라미터의 이름 (선택 사항).</param>
        /// <param name="parameterValue">설정할 FMOD 파라미터의 값 (선택 사항).</param>
        public void PlaySFX(string eventPath, string parameterName = null, float parameterValue = 0f, bool isOneShot = false)
        {
            if (isOneShot)
            {
                RuntimeManager.PlayOneShot(eventPath, Camera.main.transform.position);
            }
            else
            {
                EventInstance sfxInstance = RuntimeManager.CreateInstance(eventPath);
                // 메인 카메라 위치를 기준으로 3D 속성 설정
                Set3DAttributes(sfxInstance, Camera.main.transform.position);

                if (!string.IsNullOrEmpty(parameterName))
                {
                    sfxInstance.setParameterByName(parameterName, parameterValue);
                }

                sfxInstance.start();

                // 재생이 끝난 효과음을 자동으로 정리
                StartCoroutine(ReleaseSFXInstance(sfxInstance));
            }
        }

        /// <summary>
        /// 해당 버스의 볼륨을 설정합니다.
        /// </summary>
        public void SetBusVolume(string busName, float volume)
        {
            Bus bus = RuntimeManager.GetBus($"bus:/{busName}");
            if (bus.isValid())
            {
                bus.setVolume(Mathf.Clamp01(volume));
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// 효과음 인스턴스 해제
        /// </summary>
        private System.Collections.IEnumerator ReleaseSFXInstance(EventInstance sfxInstance)
        {
            yield return new WaitForSeconds(1f); // 사운드의 길이에 따라 적절히 조정
            sfxInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            sfxInstance.release();
        }

        // --------------------
        // 유틸리티 메서드
        // --------------------

        /// <summary>
        /// FMOD 이벤트의 3D 속성 설정
        /// </summary>
        private void Set3DAttributes(EventInstance instance, Vector3 position)
        {
            if (instance.isValid())
            {
                var attributes = RuntimeUtils.To3DAttributes(position);
                instance.set3DAttributes(attributes);
            }
            else
            {
                Debug.LogWarning("FMOD EventInstance가 유효하지 않습니다.");
            }
        }
    }
}
