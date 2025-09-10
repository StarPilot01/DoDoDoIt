using System;
using System.Collections;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace DoDoDoIt
{
    public class PlayerHealth 
    {
        

        private int _hearts; // 1당
        private int _maxHearts; // 1당 반칸
        private bool _isInvincible = false;
        private bool _isDead = false;
        private float _invincibleTime = 1.0f;

        private Coroutine _coApplyInvincible;
        public event Action OnPlayerDead;
        public event Action<int> OnTakeDamage;
        public int Hearts
        {
            get
            {
                return _hearts;
            }
            set
            {
                _hearts = value;

                if (_hearts <= 0 || !_isDead)
                {
                    _isDead = true;
                    OnPlayerDead?.Invoke();
                    
                   
                    // Scene currentScene = SceneManager.GetActiveScene();
                    // SceneManager.LoadScene(currentScene.name);
                    
                }
            }
        }

        public int MaxHearts
        {
            get
            {
                return _maxHearts;
            }
            set
            {
                _maxHearts = value;
            }
        }
        
        public bool IsInvincible
        {
            get
            {
                return _isInvincible;
            }
            set
            {
                _isInvincible = value;
            }
        }

        public float InvincibleTime
        {
            get
            {
                return _invincibleTime;
            }
            set
            {
                _invincibleTime = value;
            }
        }
        

        

        

        private void Start()
        {

            


        }

        public void Heal(int amount)
        {
            _hearts++;
    
            ////Sound
            ////Effect
    
            if (_hearts > _maxHearts)
            {
                _hearts = _maxHearts;
            }

            //UpdateHealthUI();
        }

        public void SetInvincible()
        {
            _isInvincible = true;
        }
        
        public void TakeDamage(DamageInfo damageInfo)
        {
            if (IsInvincible)
            {
                return;
                
            }
            
            _coApplyInvincible = CoroutineManager.StartCoroutine(Co_ApplyInvincible(_invincibleTime));
            
            Hearts -= damageInfo.Amount;
            
            //Sound
            //Effect
            
            //if (_hearts < 0)
            //{
            //    OnPlayerDead?.Invoke();
            //}
            //else
            //{
            //    //OnTakeDamage?.Invoke(damage);
            //}

            
        }

        IEnumerator Co_ApplyInvincible(float invincibleTime)
        {
            _isInvincible = true;
            //Debug.Log("Invincible Start");

            yield return new WaitForSeconds(invincibleTime);

            _isInvincible = false;

            //Debug.Log("Invincible End");
        }


        

    }
}