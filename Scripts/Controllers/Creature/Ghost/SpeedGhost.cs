using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

namespace DoDoDoIt
{
    public class SpeedGhost : GhostController
    {
        
        public override bool Init()
        {
            base.Init();
            GhostType = Define.EGhostType.Speed;
            RuntimeManager.PlayOneShot("event:/SFX/V_Ghost/V_Rocket", transform.position);
            return true;
        }

        public override void Absorb(PlayerController absorber)
        {
            SoundManager.Instance.PlaySFX("event:/SFX/Item/Rocket");
            GameObject _rocketEffect = Managers.Resource.Instantiate("SpeedEffect");
            _rocketEffect.transform.position = absorber.EffectPosition.gameObject.transform.position + (absorber.gameObject.transform.TransformVector(Vector3.up * 1.5f + Vector3.back * 30f));
            _rocketEffect.transform.rotation = (absorber.gameObject.transform.rotation);
            _rocketEffect.transform.SetParent(absorber.gameObject.transform);
            
            Destroy(_rocketEffect, 2.5f);
            absorber.ApplyRocketBuff();
            Destroy(this.gameObject);
           
        }
    }
}
