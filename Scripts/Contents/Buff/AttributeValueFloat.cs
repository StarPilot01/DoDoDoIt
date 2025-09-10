using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoDoDoIt
{
    [System.Serializable]
    public class AttributeValueFloat : IAttributeValue<float>
    {
        [SerializeField]
        private float _baseValue;
        private FloatReference _modifierValueRef;
        
        private List<Modifier_SO> _modifiers = new List<Modifier_SO>();

        //TODO : 은닉화
        public float Value
        {
            get
            {
                return _baseValue;
            }
            set
            {
                _baseValue = value;
            }
        }
        
        public AttributeValueFloat(float baseValue)
        {
            _baseValue = baseValue;
            _modifierValueRef = new FloatReference(0f);
        }
        
        
        public float GetValue() 
        {
            return _baseValue + _modifierValueRef.Value;
        }
        
        public void AddModifier(float modifier)
        {
            _modifierValueRef.Value += modifier;
        }
        
        public void ResetModifier()
        {
            _modifierValueRef.Value = 0f;
        }

        public void AddModifier(Modifier_SO modifierSO)
        {
            _modifiers.Add(modifierSO);
            
            
            object floatToObject = _modifierValueRef;
            
            modifierSO.ApplyModifier(ref floatToObject, ()=>
            {
                RemoveModifier(modifierSO);
            });
        }

        public void RemoveModifier(Modifier_SO modifierSO)
        {
            _modifiers.Remove(modifierSO);
        }
    }
}
