using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoDoDoIt
{
    [CreateAssetMenu(fileName = "StaticModifier", menuName = "Scriptable Object/Modifier/StaticModifier")]
    public class StaticModifier_SO : Modifier_SO
    {
        [SerializeField] 
        private float _modifierValue;
        
        public override void ApplyModifier(ref object attributeValue, Action onComplete = null)
        {
            if (attributeValue is FloatReference floatRef)
            {
                floatRef.Value += _modifierValue;
            }
        }

        public override void RemoveModifier(ref object attributeValue, Action onComplete = null)
        {
            if (attributeValue is FloatReference floatRef)
            {
                floatRef.Value -= _modifierValue;
            }
        }

    }
}
