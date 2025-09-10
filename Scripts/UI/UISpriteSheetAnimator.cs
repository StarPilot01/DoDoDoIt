using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DoDoDoIt
{
    public class UISpriteSheetAnimator : MonoBehaviour
    {
        [Header("UI Image Reference")]
        [SerializeField] private Image uiImage; // UI 이미지 컴포넌트

        [Header("Animation Settings")]
        [SerializeField] private Sprite[] spriteFrames; // 스프라이트 시트 프레임
        [SerializeField] private float frameRate = 0.1f; // 프레임 전환 간격 (초)

        private int currentFrame; // 현재 프레임 인덱스
        private Coroutine animationCoroutine;

        private void OnEnable()
        {
            // 애니메이션 시작
            animationCoroutine = StartCoroutine(PlayAnimation());
        }

        private void OnDisable()
        {
            // 애니메이션 중단
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
            }
        }

        private IEnumerator PlayAnimation()
        {
            while (true) // 무한 반복
            {
                // 현재 프레임 스프라이트로 UI 업데이트
                uiImage.sprite = spriteFrames[currentFrame];

                // 다음 프레임으로 이동 (순환)
                currentFrame = (currentFrame + 1) % spriteFrames.Length;

                // 다음 프레임까지 대기
                yield return new WaitForSeconds(frameRate);
            }
        }

        public void SetFrameRate(float newFrameRate)
        {
            // 프레임 속도 동적 변경
            frameRate = newFrameRate;
        }
    }
}