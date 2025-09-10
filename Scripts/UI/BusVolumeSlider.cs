using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DoDoDoIt
{
    public class BusVolumeSlider : MonoBehaviour
    {
        [SerializeField] private Slider masterSlider;
        [SerializeField] private Slider bgmSlider;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Slider ambSlider;

        private void Start()
        {
            // 초기값 설정
            masterSlider.value = 0.2f;
            bgmSlider.value = 0.8f;
            sfxSlider.value = 0.8f;
            ambSlider.value = 0.8f;

            // 슬라이더 변경 시 호출될 메서드 연결
            masterSlider.onValueChanged.AddListener(value => SoundManager.Instance.SetBusVolume("", value));
            bgmSlider.onValueChanged.AddListener(value => SoundManager.Instance.SetBusVolume("BGM", value));
            sfxSlider.onValueChanged.AddListener(value => SoundManager.Instance.SetBusVolume("SFX", value));
            ambSlider.onValueChanged.AddListener(value => SoundManager.Instance.SetBusVolume("AMB", value));
        }
    }

}
