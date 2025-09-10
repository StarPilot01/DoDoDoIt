using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoDoDoIt
{
    [CreateAssetMenu(fileName = "TimedStaticModifier", menuName = "Scriptable Object/Modifier/TimedStaticModifier")]
    public class TimedStaticModifier_SO : Modifier_SO
    {
        [SerializeField]
        private float _duration;
        [SerializeField] 
        private float _modifierValue;
        
        public override void ApplyModifier(ref object attributeValue, Action onComplete = null)
        {
            if (attributeValue is FloatReference floatRef)
            {
                floatRef.Value += _modifierValue;
                CoroutineManager.StartCoroutine(RemoveAfterDuration(floatRef, onComplete));
            }
        }

        public override void RemoveModifier(ref object attributeValue, Action onComplete)
        {
            throw new NotImplementedException();
        }

        private IEnumerator RemoveAfterDuration(FloatReference attributeValue, Action onComplete = null)
        {
            yield return new WaitForSeconds(_duration);
            attributeValue.Value -= _modifierValue;
            onComplete?.Invoke();
        }
    }
}
