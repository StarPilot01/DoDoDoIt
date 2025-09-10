using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;


namespace DoDoDoIt
{
    public class HealGhost : GhostController
    {
        public float healAmount;

        public override bool Init()
        {
            base.Init();
            GhostType = Define.EGhostType.Heal;
            RuntimeManager.PlayOneShot("event:/SFX/V_Ghost/V_Heal", transform.position);
            return true;
        }

        public override void Absorb(PlayerController absorber)
        {
            SoundManager.Instance.PlaySFX("event:/SFX/Item/Heal");
            GameObject _healEffect = Managers.Resource.Instantiate("HealEffect");
            _healEffect.transform.position = absorber.EffectPosition.gameObject.transform.position + Vector3.up * 2;
            _healEffect.transform.SetParent(absorber.EffectPosition);
            
            absorber.Stats.PlayerHealth.Heal(1);
            
            Destroy(_healEffect, 2f);
            Destroy(this.gameObject);
        }
    }
}