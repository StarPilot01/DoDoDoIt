using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DoDoDoIt
{
    public class HazardObjectController : BaseController
    {
        public Define.EHazardObjectType HazardObjectType { get; protected set; }

        [SerializeField]
        protected int _damage;
        protected DamageInfo _damageInfo = new DamageInfo();
        public override bool Init()
        {
            base.Init();
            ObjectType = Define.EObjectType.Hazard;
            _damageInfo.Amount = _damage;
            _damageInfo.Attacker = this;
            
            return true;
        }
    }

}