using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using DoDoDoIt;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.Timeline;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using STOP_MODE = FMOD.Studio.STOP_MODE;


namespace DoDoDoIt
{
    public class PlayerController : CreatureController, IBuffable
    {
        #region field

        [SerializeField] 
        private PlayerInitStats_SO[] _playerInitStats;

        [Header("자석 버프")]
        [SerializeField]
        private float _magnetBuffDuration;

        [SerializeField]
        private float _magnetBuffPullRadiusMultiplier;
        [SerializeField]
        private float _magnetBuffPullForceMultiplier;
        
        [Header("로켓 버프")]
        
        [SerializeField]
        private float _rocketBuffDuration;

        [SerializeField]
        private float _rocketTargetForwardSpeed;

        [SerializeField]
        private float _rocketTargetForwardSpeedTransitionTime;

        private PlayableDirector _playableDirector;

    

        
        private PlayerStats _stats = new PlayerStats();
        private PlayerStateMachine _stateMachine = new PlayerStateMachine();
        
        private List<BaseBuffInstance> _activeBuffs = new List<BaseBuffInstance>();
        
        
        private GrapplePointDetector _grapplePointDetector;
        private Coroutine _coApplySpeedBuff;

        private int _rotationDirection = 0;
        private bool _isSwinging;
        private float _inputX = 0.0f;
        
        private LayerMask _groundLayer;
        
        
        //public event Action OnHeldBuffUsage;

        /*
         * 하드코딩 ^_^ 나중에 리팩토링 ^-^====@ 물론 학기작 끝나고
         */
        private bool _isAppliedMagnetBuff = false;
        private bool _isAppliedRocketBuff = false;
       
        
        private Coroutine _coApplyMagnetBuff = null;
        private Coroutine _coApplyRocketBuff = null;
        
        
        
        #endregion


        #region Property

        public PlayerStats Stats
        {
            get { return _stats; }
        }
        public PlayerStateMachine StateMachine 
        {
            get { return _stateMachine; }
        }
        public GrapplePointDetector GrapplePointDetector 
        {
            get { return _grapplePointDetector; }
        }
        
        public PlayableDirector PlayableDirector 
        {
            get { return _playableDirector; }
        }

        public LayerMask GroundLayer
        {
            get
            {
                return _groundLayer;
            }
        }

        public int RotationDirection
        {
            get
            {
                return _rotationDirection;
            }
            set
            {
                _rotationDirection = value;
            }
        }
        public float InputX
        {
            get
            {
                return _inputX;
            }
            set
            {
                _inputX = value;
            }
        }

        public bool IsSwinging
        {
            get
            {
                return _isSwinging;
            }
            set
            {
                _isSwinging = value;
            }
        }

        public bool IsAppliedRocketBuff
        {
            get
            {
                return _isAppliedRocketBuff;
            }
        }
        
        
        #endregion


        #region Component_field

        private Transform _groundCheckRayOriginTrs;
        private Transform _lanternTrs;
        private Transform _playerGFX;
        private Transform _effectPosition;
        private Transform _objectGahterPointTrs;
        private LineRenderer _lineRenderer;
        
        private Animator _animator;
        private Rigidbody _rigidbody;

        #endregion

        #region Component_Property

        public GameObject GrapPoint
        {
            get; set;
        }

        public Transform GroundCheckRayOriginTrs 
        {
            get { return _groundCheckRayOriginTrs; }
        }

        public Transform LanternTrs
        {
            get { return _lanternTrs; }
        }

        public Transform PlayerGFX
        {
            get { return _playerGFX; }
            private set { _playerGFX = value; }
        }
        public Transform EffectPosition
        {
            get { return _effectPosition; }
        }
        public Transform ObjectGatherPointTrs
        {
            get { return _objectGahterPointTrs; }
        }

        public LineRenderer LineRenderer
        {
            get { return _lineRenderer; }
        }

        public Rigidbody Rigidbody
        {
            get { return _rigidbody; }
        }

        public Animator Animator
        {
            get { return _animator; }
        }

        #endregion
        
        
        //TODO : 지우기

        [SerializeField]
        private Image _fadeImage;
        
        private void InitComponents()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _animator = Util.FindChild<Animator>(this.gameObject, "DoDoGFX", false);
            _lineRenderer = Util.FindChild<LineRenderer>(this.gameObject, "LineRenderer", true);
            _grapplePointDetector = GetComponent<GrapplePointDetector>();
            _playableDirector = GetComponent<PlayableDirector>();
            
        }

        private void AssignReferences()
        {
            _groundCheckRayOriginTrs = Util.FindChild<Transform>(this.gameObject, "GroundCheckRayOrigin", false);
            _lanternTrs = Util.FindChild<Transform>(this.gameObject, "Lantern 01", true);
            _playerGFX = Util.FindChild<Transform>(this.gameObject, "DoDoGFX", false);
            _effectPosition = Util.FindChild<Transform>(this.gameObject, "Bip001", true);
            _objectGahterPointTrs = Util.FindChild<Transform>(this.gameObject, "ObjectGatherPoint", true);
        }

        public override bool Init()
        {
            InitComponents();
            AssignReferences();


            _groundLayer = LayerMask.GetMask("Ground");
            IsSwinging = false;
            _stats.Init(this , _playerInitStats[0]);
            _stats.RemainingJumpCount = _stats.Attributes.JumpCount.GetValue();
            _stateMachine.Init(this);
            if (SceneManager.GetActiveScene().name == "GameScene")
            {
                _stateMachine.TransitionTo(Define.EPlayerState.Idle);
            }
            else
            {
                _stateMachine.TransitionTo(Define.EPlayerState.Running);
            }
            _stats.PlayerHealth.OnPlayerDead += OnDead;
            //_stats.PlayerHealth.OnTakeDamage += HandleTakeDamage;
    
            
            
            return true;
        }

        public void StartGame()
        {
            _stateMachine.TransitionTo(Define.EPlayerState.Running);
        }

        public void EndTutorial()
        {
            GameObject _healEffect = Managers.Resource.Instantiate("HealEffect");
            _healEffect.transform.position = EffectPosition.gameObject.transform.position + Vector3.up * 2;
            _healEffect.transform.SetParent(EffectPosition);
            
            _stats.Init(this, _playerInitStats[1]);
            Destroy(_healEffect, 2f);
        }

        private void Update()
        {
            _inputX = Input.GetAxis("Horizontal");
            _stateMachine.Update();
        }

        private void LateUpdate()
        {
            _animator.SetFloat("moveSpeed", _stats.Attributes.ForwardSpeed.GetValue() / 56f);
            _animator.SetFloat("moveDirection", InputX);
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
        }

        public void TransitionTo(Define.EPlayerState nextState)
        {
            _stateMachine.TransitionTo(nextState);
        }

        void OnDead()
        {
            Animator.SetTrigger("IsDead");
            TransitionTo(Define.EPlayerState.Dead);
        }

      
        public override void TakeDamage(DamageInfo damageInfo)
        {
            if (_stats.PlayerHealth.IsInvincible)
            {
                return;
            }
            
            _animator.Play("Hit",-1, 0f);
            SoundManager.Instance.PlaySFX("event:/SFX/Dodo/Voice/Hit");
            _stats.PlayerHealth.TakeDamage(damageInfo);

            if (_stateMachine.GetCurrentState() is IDamageableState damageableState)
            {

                _stats.PlayerHealth.IsInvincible = true;
                
                Debug.Log("SlowDown Start");
                //BaseBuffInstance slowDownBuff = new BaseBuffInstance(_slowDownBuff);
                //slowDownBuff.OnCompleted += () =>
                //{
                //    OnSlowDownBuffEnd();
                //};

                //for (int i = 0; i < slowDownBuff.BaseBuffSO.ModifierSets.Count; i++)
                //{
                //    _stats.Attributes.ApplyModifier(slowDownBuff.BaseBuffSO.ModifierSets[i]);
                //}
                
                StartCoroutine(SlowDown());
                
                //_activeBuffs.Add(slowBuff);
                
                
                //if (_coApplySpeedBuff != null)
                //{
                //StopCoroutine(_coApplySpeedBuff);
                //_coApplySpeedBuff = null;
                //_stats.Attributes.ForwardSpeed = _stats.InitAttributes.ForwardSpeed;
                //_stats.Attributes.HorizontalSpeed = _stats.InitAttributes.HorizontalSpeed;
                //StartCoroutine(SlowDown());
                
                //}
                //else
                //{
                //StartCoroutine(SlowDown());
                
                //}
                //Stats.Attributes.ForwardSpeed = Stats.InitAttributes.ForwardSpeed;
                //Stats.Attributes.HorizontalSpeed = Stats.InitAttributes.HorizontalSpeed;
                //damageableState.TakeDamage(damageInfo);
            }
        }

        private void OnSlowDownBuffEnd()
        {
            //BaseBuffInstance recoverySpeedAfterSlownDown = new BaseBuffInstance(_recoverySpeedAfterSlownDownBuff);
            //
            ////TODO : SO라 전역수정임
//
            //CurveModifier_SO horizontalModifier = (CurveModifier_SO)recoverySpeedAfterSlownDown.BaseBuffSO.ModifierSets[0].ModifierSO;
//
            //horizontalModifier.Increasement = _stats.Attributes.ForwardSpeed.b
        }

        private void HandleTakeDamage(int damage)
        {
            //StartCoroutine(FlashOnDamage());  // 깜빡이는 효과
            //StartCoroutine(SlowDown()); // 느려지는 효과
        }
        
        private IEnumerator SlowDown()
        {
            //Debug.Log("SlowDown Start");

            float originalSpeed = _stats.InitAttributes.ForwardSpeed;
            
            _stats.Attributes.ForwardSpeed.Value *= 0.5f;

            // Material 가져오기
            Material material = Util.FindChild(this.gameObject, "character_maincat", true).GetComponent<SkinnedMeshRenderer>().material;
    
            // 원래 색상 가져오기
            Color baseColor = material.GetColor("_BaseColor");

            // 깜빡임 설정
            float blinkDuration = _stats.PlayerHealth.InvincibleTime;  // 깜빡일 총 시간
            float blinkSpeed = 0.1f;  // 깜빡임 속도 (알파값이 변경되는 주기)

            float elapsedTime = 0f;

            // 깜빡임 코루틴
            while (elapsedTime < blinkDuration)
            {
                elapsedTime += blinkSpeed;

                // 알파 값 깜빡임 (0.5f ~ 0.75f 사이를 부드럽게 전환)
                baseColor.a = Mathf.Lerp(0.5f, 0.85f, Mathf.PingPong(Time.time * 5, 1));

                // 변경된 색상 적용
                material.SetColor("_BaseColor", baseColor);
                
                // blinkSpeed만큼 대기
                yield return new WaitForSeconds(blinkSpeed);
            }

            // 원래 속도 복원
            _stats.Attributes.ForwardSpeed.Value = _stats.InitAttributes.ForwardSpeed;
            _stats.Attributes.HorizontalSpeed.Value = _stats.InitAttributes.HorizontalSpeed;

            // 알파 값을 1.0f로 복원
            baseColor.a = 1.0f;
            material.SetColor("_BaseColor", baseColor);

            //Debug.Log("SlowDown End");

        }

        public void ApplyMagnetBuff()
        {
            if (_coApplyMagnetBuff != null)
            {
                StopCoroutine(_coApplyMagnetBuff);
            }

            _coApplyMagnetBuff = StartCoroutine(CO_ApplyMagnetBuff(_magnetBuffDuration));

        }

        private IEnumerator CO_ApplyMagnetBuff(float duration)
        {
            EventInstance instance = RuntimeManager.CreateInstance("event:/SFX/Item/Margnet");
            instance.start();
            _isAppliedMagnetBuff = true;

            _stats.Attributes.PullForce.Value *= _magnetBuffPullForceMultiplier;
            _stats.Attributes.PullRadius.Value *= _magnetBuffPullRadiusMultiplier;

            yield return new WaitForSeconds(duration);

            instance.stop(STOP_MODE.IMMEDIATE);
            instance.release();
            _stats.Attributes.PullForce.Value = _stats.InitAttributes.PullForce;
            _stats.Attributes.PullRadius.Value = _stats.InitAttributes.PullRadius;
            _isAppliedMagnetBuff = false;
            
            SoundManager.Instance.PlaySFX("event:/SFX/Item/Margnet", "-", 1f);
        }

        public void ApplyRocketBuff()
        {
            if (_coApplyRocketBuff != null)
            {
                StopCoroutine(_coApplyRocketBuff);
            }

            _coApplyRocketBuff = StartCoroutine(CO_ApplyRocketBuff(_rocketTargetForwardSpeed, _rocketBuffDuration, _rocketTargetForwardSpeedTransitionTime));
            
            if (_coApplyMagnetBuff != null)
            {
                StopCoroutine(_coApplyMagnetBuff);
            }

            _coApplyMagnetBuff = StartCoroutine(CO_ApplyMagnetBuff(_rocketBuffDuration));

            
        }

        IEnumerator CO_ApplyRocketBuff(float targetSpeed, float duration, float transitionTime)
        {
            _stats.PlayerHealth.IsInvincible = true;
            
            yield return StartCoroutine(ChangeSpeed(_stats.Attributes.ForwardSpeed.Value, targetSpeed, transitionTime));

            // 2. 목표 속도를 유지
            yield return new WaitForSeconds(duration);

            // 3. 점진적으로 원래 속도로 복귀
            yield return StartCoroutine(ChangeSpeed(_stats.Attributes.ForwardSpeed.Value, _stats.InitAttributes.ForwardSpeed, transitionTime));

            _stats.PlayerHealth.IsInvincible = false;
            _coApplyRocketBuff = null; // 코루틴 종료
        }
        private IEnumerator ChangeSpeed(float startSpeed, float endSpeed, float duration)
        {
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                _stats.Attributes.ForwardSpeed.Value = Mathf.Lerp(startSpeed, endSpeed, elapsedTime / duration); // 선형 보간
                yield return null;
            }

            _stats.Attributes.ForwardSpeed.Value = endSpeed; // 최종 속도 설정
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("EndingFlag"))
            {
                _stateMachine.TransitionTo(Define.EPlayerState.EndingSceneRunning);
            }
            else if (other.CompareTag("EndingSceneChangeFlag"))
            {
                
                _fadeImage.gameObject.SetActive(true);
                
                _fadeImage.DOFade(1f, 1.5f).OnComplete(() =>
                {
                    SceneManager.LoadScene("EndingScene");
                });
                
                Debug.Log(Managers.Score.MyScore);
            }
        }

        #region Buff Function



        public void ApplyBuff(BaseBuff_SO buff)
        {
            BaseBuffInstance baseBuffInstance = new BaseBuffInstance(buff);
            
            List<ModifierSet> modifierSets = baseBuffInstance.BaseBuffSO.ModifierSets;
            
            for (int i = 0; i < modifierSets.Count; i++)
            {
                _stats.Attributes.ApplyModifier(modifierSets[i]);
                
            }
            
        }
        public void RemoveBuff(BaseBuff_SO buff)
        {
            throw new NotImplementedException();
        }
       


        
        
        

        
        
        
        //private void ApplySpeedBuff(float forwardSpeedIncrease, float horizontalSpeedIncrease, AnimationCurve forwardCurve, AnimationCurve horizontalCurve, float duration)
        //{
        //    Debug.Log("Speed buff On");
        //    _coApplySpeedBuff = StartCoroutine(ApplySpeedBuffCoroutine(forwardSpeedIncrease, horizontalSpeedIncrease, forwardCurve, horizontalCurve, duration));
        //}
//
        //private IEnumerator ApplySpeedBuffCoroutine(float forwardSpeedIncrease, float horizontalSpeedIncrease, AnimationCurve forwardCurve, AnimationCurve horizontalCurve, float duration)
        //{
        //    float elapsedTime = 0f;
        //    float originalForwardSpeed = Stats.Attributes.ForwardSpeed;
        //    float originalHorizontalSpeed = Stats.Attributes.HorizontalSpeed;
        //    
        //    while (elapsedTime < duration)
        //    {
        //        elapsedTime += Time.deltaTime;
        //        float normalizedTime = Mathf.Clamp01(elapsedTime / duration);
//
        //        float forwardSpeedMultiplier = forwardCurve.Evaluate(normalizedTime);
        //        Stats.Attributes.ForwardSpeed = originalForwardSpeed + forwardSpeedIncrease * forwardSpeedMultiplier;
//
        //        float horizontalSpeedMultiplier = horizontalCurve.Evaluate(normalizedTime);
        //        Stats.Attributes.HorizontalSpeed = originalHorizontalSpeed + horizontalSpeedIncrease * horizontalSpeedMultiplier;
//
        //        yield return null;
        //    }
//
        //    // 버프가 종료되면 속도를 원래대로 복원
        //    Debug.Log("SpeedBuff End");
        //    Stats.Attributes.ForwardSpeed = Stats.InitAttributes.ForwardSpeed;
        //    Stats.Attributes.HorizontalSpeed = Stats.InitAttributes.HorizontalSpeed;
        //    
        //    _activeBuffs.Clear();
        //}
//
        //public void ResetSpeedGradually(float duration)
        //{
        //    StartCoroutine(ResetSpeedGraduallyCoroutine(duration));
        //}
//
        //private IEnumerator ResetSpeedGraduallyCoroutine(float duration)
        //{
        //    float elapsedTime = 0f;
        //    float currentForwardSpeed = Stats.Attributes.ForwardSpeed;
        //    float currentHorizontalSpeed = Stats.Attributes.HorizontalSpeed;
        //    float originalForwardSpeed = playerInitStats.ForwardSpeed;
        //    float originalHorizontalSpeed = playerInitStats.HorizontalSpeed;
//
        //    while (elapsedTime < duration)
        //    {
        //        elapsedTime += Time.deltaTime;
        //        float t = Mathf.Clamp01(elapsedTime / duration);
//
        //        Stats.Attributes.ForwardSpeed = Mathf.Lerp(currentForwardSpeed, originalForwardSpeed, t);
        //        Stats.Attributes.HorizontalSpeed = Mathf.Lerp(currentHorizontalSpeed, originalHorizontalSpeed, t);
//
        //        yield return null;
        //    }
//
        //    Stats.Attributes.ForwardSpeed = originalForwardSpeed;
        //    Stats.Attributes.HorizontalSpeed = originalHorizontalSpeed;
        //    
        //}
        #endregion
        
    }

}