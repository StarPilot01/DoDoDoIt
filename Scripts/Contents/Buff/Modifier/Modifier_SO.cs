using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace DoDoDoIt
{
    public abstract class Modifier_SO: ScriptableObject
    {
       
        public UnityEvent OnCompleteCallback;


        public abstract void ApplyModifier(ref object attributeValue , Action onComplete);
        public abstract void RemoveModifier(ref object attributeValue, Action onComplete);
    }

}
