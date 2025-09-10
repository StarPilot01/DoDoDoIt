using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoDoDoIt
{
    public class ModifierInstance
    {
        private Modifier_SO _modifierSO;

        private float _elapsedTime;
        
        public Modifier_SO ModifierSO
        {
            get
            {
                return _modifierSO;
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
        
        public ModifierInstance(Modifier_SO modifierSO)
        {
            _modifierSO = modifierSO;
        }

        public void ApplyModifier()
        {
            
        }
    }
}
