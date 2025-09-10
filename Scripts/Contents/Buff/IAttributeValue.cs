using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoDoDoIt
{
    public interface IAttributeValue<T>
    {
        public T GetValue();
        public void AddModifier(T modifier);
        public void ResetModifier();
    }
}
