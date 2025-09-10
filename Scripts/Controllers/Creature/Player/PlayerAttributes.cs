using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Serialization;

namespace DoDoDoIt
{
    [Serializable]
    public class PlayerAttributes
    {
        #region field

        private Dictionary<string, AttributeValueFloat> _attributeValueFloatMap =
            new Dictionary<string, AttributeValueFloat>();
        
        private Dictionary<string, AttributeValueInt> _attributeValueIntMap =
            new Dictionary<string, AttributeValueInt>();
        
        [SerializeField]
        private AttributeValueFloat _forwardSpeed;
        
        [SerializeField]
        private AttributeValueFloat _forwardAnimSpeedDivision;
        
        [SerializeField]
        private AttributeValueFloat _horizontalSpeed;
        
        [SerializeField]
        private AttributeValueFloat _jumpHeight;
        
        [SerializeField]
        private AttributeValueInt _jumpCount;
        
        [SerializeField]
        private AttributeValueFloat _gravityScale;


        //Object Pulling
        [SerializeField]
        private AttributeValueFloat _pullRadius; // 오브젝트를 끌어당길 반경

        [SerializeField]
        private AttributeValueFloat _pullForce; // 오브젝트를 끌어당길 힘

        [SerializeField]
        private AttributeValueFloat _absorptionRadius;

        #endregion

        #region Property


        public AttributeValueFloat ForwardSpeed
        {
            get
            {
                return _forwardSpeed;
            }
        }

        public AttributeValueFloat ForwardAnimSpeedDivision
        {
            get
            {
                return _forwardAnimSpeedDivision;
            }
        }

        public AttributeValueFloat HorizontalSpeed
        {
            get
            {
                return _horizontalSpeed;
            }

        }

        public AttributeValueFloat JumpHeight
        {
            get
            {
                return _jumpHeight;
            }
        }

        public AttributeValueInt JumpCount
        {
            get
            {
                return _jumpCount;
            }
        }

        public AttributeValueFloat GravityScale
        {
            get
            {
                return _gravityScale;
            }
        }

        public AttributeValueFloat PullRadius
        {
            get
            {
                return _pullRadius;
            }
        }

        public AttributeValueFloat PullForce
        {
            get
            {
                return _pullForce;
            }
        }

        public AttributeValueFloat AbsorptionRadius
        {
            get
            {
                return _absorptionRadius;
            }
        }

        #endregion


        public void SetInitAttributeValue(PlayerInitStats_SO so)
        {
            _forwardSpeed = new AttributeValueFloat(so.ForwardSpeed);
            _forwardAnimSpeedDivision = new AttributeValueFloat(so.ForwardAnimSpeedDivision);
            _horizontalSpeed = new AttributeValueFloat(so.HorizontalSpeed);
            _jumpHeight = new AttributeValueFloat(so.JumpHeight);
            _jumpCount = new AttributeValueInt(so.JumpCount);
            _gravityScale = new AttributeValueFloat(so.GravityScale);
            
            _pullRadius = new AttributeValueFloat(so.PullRadius); 
            _pullForce = new AttributeValueFloat(so.PullForce); 
            _absorptionRadius = new AttributeValueFloat(so.AbsorptionRadius);
            
            //init
            //ReflectAttributesToMaps();
        }
        
        public void ReflectAttributesToMaps()
        {
            // 리플렉션을 사용하여 PlayerAttributes 클래스의 필드 순회
            var fields = this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            
            foreach (var field in fields)
            {
                // 필드 이름에서 "_"를 제거
                string fieldName = field.Name.StartsWith("_") ? field.Name.Substring(1) : field.Name;
                
                // 필드의 타입에 따라 분기
                if (field.FieldType == typeof(AttributeValueFloat))
                {
                    var fieldValue = (AttributeValueFloat)field.GetValue(this);
                    if (fieldValue != null)
                    {
                        // 타입이 float일 경우 Dictionary에 저장
                        _attributeValueFloatMap.Add(fieldName, fieldValue);
                    }
                }
                else if (field.FieldType == typeof(AttributeValueInt))
                {
                    var fieldValue = (AttributeValueInt)field.GetValue(this);
                    if (fieldValue != null)
                    {
                        // 타입이 int일 경우 Dictionary에 저장
                        _attributeValueIntMap.Add(fieldName, fieldValue);
                    }
                }
            }
        }

        public void ApplyModifier(ModifierSet modifierSet)
        {
            _attributeValueFloatMap[modifierSet.TargetAttributeName].AddModifier(modifierSet.ModifierSO);
        }
    }
}
