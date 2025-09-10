using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoDoDoIt
{
    public interface IDamageableState
    {
        public void TakeDamage(DamageInfo damageInfo);
    }
}
