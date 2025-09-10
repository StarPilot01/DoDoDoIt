using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoDoDoIt
{
    public class ConditionalFieldAttribute : PropertyAttribute
    {
        public string ConditionFieldName { get; }

        public ConditionalFieldAttribute(string conditionFieldName)
        {
            ConditionFieldName = conditionFieldName;
        }
    }
}
