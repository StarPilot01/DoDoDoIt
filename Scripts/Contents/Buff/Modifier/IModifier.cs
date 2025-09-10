using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoDoDoIt
{
    public interface IModifier
    {
        bool IsComplete { get; }
    }
}
