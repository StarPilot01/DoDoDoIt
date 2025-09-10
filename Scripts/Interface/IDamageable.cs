using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoDoDoIt
{
    public interface IDamageable
    {
        public void TakeDamage(DamageInfo damageInfo);
    }
}
