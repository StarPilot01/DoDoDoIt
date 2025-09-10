using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoDoDoIt
{
    public interface IModifierInt : IModifier
    {
        void ApplyModifier(ref int valueToModify);
        void RemoveModifier(ref int valueToModify);
    }
    
}
