using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoDoDoIt
{
    public interface IAbsorbable
    {
        public void Absorb(PlayerController absorber);
    }
}
