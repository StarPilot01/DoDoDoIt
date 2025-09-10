using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoDoDoIt
{
    [Serializable]
    public class ModifierSet
    {
        [SerializeField]
        private string _targetAttributeName;

        [SerializeField]
        private Modifier_SO _modifierSO;

        public string TargetAttributeName => _targetAttributeName;
        public Modifier_SO ModifierSO => _modifierSO;
    }
    
    
    
    [CreateAssetMenu(fileName = "Buff", menuName = "Scriptable Object/Buff", order = int.MaxValue)]
    public class BaseBuff_SO : ScriptableObject
    {
        [SerializeField]
        protected string _name;
        
        [SerializeField]
        protected float _duration;

        [SerializeField]
        private List<ModifierSet> _modifierSetList;
        
        
        public string Name => _name;
        public float Duration => _duration;

        public List<ModifierSet> ModifierSets => _modifierSetList;
        
        public void Apply(IBuffable target)
        {
            target.ApplyBuff(this);
        }
        
        ////public abstract void UpdateBuff(IBuffable target);
        //public void Remove(IBuffable target);
        
    }

}