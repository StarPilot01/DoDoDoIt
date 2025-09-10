using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DoDoDoIt
{
    [CreateAssetMenu(fileName = "Player Stats", menuName = "Scriptable Object/Player Stats", order = int.MaxValue)]

    public class PlayerInitStats_SO : ScriptableObject
    {
        
        [SerializeField]
        private int _maxHearts = 10;
        [SerializeField]
        private float _forwardSpeed = 2f;
        [SerializeField]
        private float _forwardAnimSpeedDivision = 20;
        [SerializeField]
        private float _horizontalSpeed = 3f;
        
        [SerializeField]
        private float _jumpHeight = 7f;
        [SerializeField]
        private int _jumpCount;
        [SerializeField]
        private float _gravityScale;
        
        [SerializeField]
        private float _pullRadius = 5f; 
        [SerializeField]
        private float _pullForce = 10f;
        [SerializeField]
        private float _absorptionRadius = 5f;
        
        
        
        
        public int MaxHearts
        {
            get
            {
                return _maxHearts;
            }
        }

        public float ForwardSpeed
        {
            get
            {
                return _forwardSpeed;
            }
        }

        public float ForwardAnimSpeedDivision
        {
            get
            {
                return _forwardAnimSpeedDivision;
            }
        }

        public float HorizontalSpeed
        {
            get
            {
                return _horizontalSpeed;
            }
        }

        public float JumpHeight
        {
            get
            {
                return _jumpHeight;
            }
        }

        public int JumpCount
        {
            get
            {
                return _jumpCount;
            }
        }

        public float GravityScale
        {
            get
            {
                return _gravityScale;
            }
        }

        public float PullRadius
        {
            get
            {
                return _pullRadius;
            }
        }

        public float PullForce
        {
            get
            {
                return _pullForce;
            }
        }

        public float AbsorptionRadius
        {
            get
            {
                return _absorptionRadius;
            }
        }


    }
}