using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoDoDoIt
{
    public class AttributeValueInt : IAttributeValue<int>
    {
        private int BaseValue;
        private int Modifier;

        public AttributeValueInt(int baseValue)
        {
            BaseValue = baseValue;
            Modifier = 0;
        }

        public int GetValue()
        {
            return BaseValue + Modifier;
        }

        public void AddModifier(int modifier)
        {
            Modifier += modifier;
        }

        public void ResetModifier()
        {
            Modifier = 0;
        }
    }
}
