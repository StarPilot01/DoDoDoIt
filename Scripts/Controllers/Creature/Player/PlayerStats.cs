using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;


namespace DoDoDoIt
{
    public class PlayerStats
    {
        private PlayerController _player;
        private PlayerInitStats_SO _initStats_SO;
        
        private PlayerHealth _playerHealth = new PlayerHealth(); 
        private PlayerAttributes _attributes = new PlayerAttributes();
        
        
        
        //초기에 지정되지 않는 값들 
        #region Stats_field
        
        private float _remainingJumpCount;
        private float _jumpForce;
        private float _targetForwardSpeed;
        private float _targetHorizontalSpeed;
        
        #endregion
    
        #region Stats_Property

        public PlayerInitStats_SO InitAttributes
        {
            get
            {
                return _initStats_SO;
            }
        }

        public PlayerAttributes Attributes
        {
            get
            {
                return _attributes;
            }
            set
            {
                _attributes = value;
            }
        }

        public PlayerHealth PlayerHealth
        {
            get
            {
                return _playerHealth;
            }
        }

        public float RemainingJumpCount
        {
            get
            {
                return _remainingJumpCount;
            }
            set
            {
                _remainingJumpCount = value;
            }
        }

        public float JumpForce
        {
            get
            {
                return _jumpForce;
            }
            set
            {
                _jumpForce = value;
            }
        }

        public float TargetForwardSpeed
        {
            get
            {
                return _targetForwardSpeed;
            }
            set
            {
                _targetForwardSpeed = value;
            }
        }

        public float TargetHorizontalSpeed
        {
            get
            {
                return _targetHorizontalSpeed;
            }
            set
            {
                _targetHorizontalSpeed = value;
            }
        }
        #endregion



        public void Init(PlayerController playerController , PlayerInitStats_SO initStats_SO)
        {
            _player = playerController;
            _initStats_SO = initStats_SO;
            
            //set playerHealth Init Value
            _playerHealth.MaxHearts = _initStats_SO.MaxHearts;
            _playerHealth.Hearts = _initStats_SO.MaxHearts;
            
            //Init AttributesValue
            _attributes.SetInitAttributeValue(_initStats_SO);
            
            //_attributes = Managers.Resource.Load<PlayerAttributes_SO>("PlayerAttributes");
            _jumpForce = Mathf.Sqrt(_attributes.JumpHeight.GetValue() * -2 * (_attributes.GravityScale.GetValue()));
            
        }
    
    
    
    
    }

}