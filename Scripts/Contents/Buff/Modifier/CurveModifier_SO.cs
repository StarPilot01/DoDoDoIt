using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DoDoDoIt
{
    [CreateAssetMenu(fileName = "CurveModifier", menuName = "Scriptable Object/Modifier/CurveModifier", order = int.MaxValue)]
    public class CurveModifier_SO : Modifier_SO
    {
        [SerializeField]
        private float _increasement;
        //[SerializeField]
        //private float _decreasement;
        
        [SerializeField]
        protected float _increasementDuration; 
        //[SerializeField]
        //protected float _decreasementDuration;
        
        [SerializeField]
        private AnimationCurve _increasementCurve;
        //[SerializeField]
        //private AnimationCurve _decreasementCurve;

        
        
        //protected string _ownerBuffName;


        //TODO : 전역 수정이라 추후 Instance로 올려야됨
        public float Increasement
        {
            get
            {
                return _increasement;
            }
            set
            {
                _increasement = value;
            }
        }
        
        public float IncreasementDuration
        {
            get
            {
                return _increasement;
            }
        }
        //public float DecreasementDuration => _decreasementDuration;
        //public float TotalDuration => _increasementDuration + _decreasementDuration;
        
        void OnEnable()
        {
            ClampCurveKeyAndValue();
        }

        public float EvaluateIncreasement(float elapsedTime)
        {
            float normalizedTime = Mathf.Clamp01(elapsedTime / _increasementDuration);
            return _increasementCurve.Evaluate(normalizedTime);
        }

        //public float EvaluateDecreasement(float elapsedTime)
        //{
        //    float normalizedTime = Mathf.Clamp01(elapsedTime / _decreasementDuration);
        //    return _decreasementCurve.Evaluate(normalizedTime);
        //}

        private void ClampCurveKeyAndValue()
        {
            Keyframe[] incKeys = _increasementCurve.keys;
            for (int i = 0; i < incKeys.Length; i++)
            {
                incKeys[i].time = Mathf.Clamp(incKeys[i].time, 0.0f, 1.0f);
                incKeys[i].value = Mathf.Clamp(incKeys[i].value, 0.0f, 1.0f);
            }
            _increasementCurve.keys = incKeys;

           //Keyframe[] decKeys = _decreasementCurve.keys;
           //for (int i = 0; i < decKeys.Length; i++)
           //{
           //    decKeys[i].time = Mathf.Clamp(decKeys[i].time, 0.0f, 1.0f);
           //    decKeys[i].value = Mathf.Clamp(decKeys[i].value, 0.0f, 1.0f);
           //}
           //_decreasementCurve.keys = decKeys;
        }

        public override void ApplyModifier(ref object attributeValue, Action onComplete = null)
        {
            if (attributeValue is FloatReference floatRef)
            {
                CoroutineManager.StartCoroutine(CoApplyIncreasement(floatRef));
            }
            else
            {
                Debug.LogWarning("attributeValue is not a FloatReference type.");
            }
        }

        public override void RemoveModifier(ref object attributeValue ,Action onComplete = null)
        {
            if (attributeValue is FloatReference floatRef)
            {
                //CoroutineManager.StartCoroutine(CoApplyDecreasement(floatRef));
            }
            else
            {
                Debug.LogWarning("attributeValue is not a FloatReference type.");
            }
            
            onComplete?.Invoke();
        }

        private IEnumerator CoApplyIncreasement(FloatReference attributeValue, Action onComplete = null)
        {
            float initialAttributeValue = attributeValue.Value;
            float elapsedTime = 0f;

            while (elapsedTime < _increasementDuration)
            {
                elapsedTime += Time.deltaTime;
                float curveValue = EvaluateIncreasement(elapsedTime);

                attributeValue.Value = initialAttributeValue + curveValue * _increasement;
                
                yield return null;
            }
            
            //CoroutineManager.StartCoroutine(CoApplyDecreasement(attributeValue , onComplete));
            
            onComplete?.Invoke();
            
        }

       //private IEnumerator CoApplyDecreasement(FloatReference attributeValue, Action onComplete = null)
       //{
       //    float initialAttributeValue = attributeValue.Value;
       //    float elapsedTime = 0f;
       //    while (elapsedTime < _decreasementDuration)
       //    {
       //        elapsedTime += Time.deltaTime;
       //        float curveValue = EvaluateDecreasement(elapsedTime)
       //        attributeValue.Value = initialAttributeValue - curveValue * _decreasement;
       //        
       //        yield return null;
       //    }
       //    
       //    onComplete?.Invoke();
       //}
    }
        
        
    
}
