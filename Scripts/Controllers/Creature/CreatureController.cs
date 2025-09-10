using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DoDoDoIt
{
    public abstract class CreatureController : BaseController , IDamageable
    {
        public abstract void TakeDamage(DamageInfo damageInfo);


    }
}
