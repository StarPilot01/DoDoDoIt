using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoDoDoIt
{
    public class BaseBuffInstance
    {
        private BaseBuff_SO _baseBuffSO;

        private float _elapsedTime;

        public Action OnCompleted;
        public BaseBuff_SO BaseBuffSO
        {
            get
            {
                return _baseBuffSO;
            }
        }
        
        public float ElapsedTime
        {
            get
            {
                return _elapsedTime;
            }
            set
            {
                _elapsedTime = value;
            }
        }

        public BaseBuffInstance(BaseBuff_SO buffSO)
        {
            _baseBuffSO = buffSO;
        }

        public void StartBuff()
        {
            CoroutineManager.StartCoroutine(CO_CallOnCompletedAfterDuration());
        }
        
        private IEnumerator CO_CallOnCompletedAfterDuration()
        {
            yield return new WaitForSeconds(_baseBuffSO.Duration);
            OnCompleted?.Invoke(); 
           
        }
        
    }
}
