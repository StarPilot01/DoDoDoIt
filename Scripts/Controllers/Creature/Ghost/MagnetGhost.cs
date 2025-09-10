using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace DoDoDoIt
{
    public class MagnetGhost : GhostController
    {
        public override bool Init()
        {
            base.Init();
            GhostType = Define.EGhostType.Magnet;
            RuntimeManager.PlayOneShot("event:/SFX/V_Ghost/V_Margnet", transform.position);
            return true;
        }

        public override void Absorb(PlayerController absorber)
        {
            GameObject _magnetEffect = Managers.Resource.Instantiate("MagnetEffect");
            _magnetEffect.transform.position = absorber.EffectPosition.gameObject.transform.position + UnityEngine.Vector3.up * 1.8f;
            _magnetEffect.transform.SetParent(absorber.EffectPosition);
            Destroy(_magnetEffect, 4f);
            absorber.ApplyMagnetBuff();
            
            Destroy(this.gameObject);
        }
    }
}
