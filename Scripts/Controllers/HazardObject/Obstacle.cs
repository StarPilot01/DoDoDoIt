using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace DoDoDoIt
{
    public class Obstacle : HazardObjectController
    {
        
        //private DamageInfo _damageInfo;

        public override bool Init()
        {
            base.Init();
            HazardObjectType = Define.EHazardObjectType.Obstacle;
            return true;
        }

        public void OnCollisionEnter(Collision collision)
        {
            PlayerController playerController;
            if (collision.gameObject.TryGetComponent<PlayerController>(out playerController))
            {
                playerController.TakeDamage(_damageInfo);
                if (this.gameObject.CompareTag("Playroom"))
                {
                    SoundManager.Instance.PlaySFX("event:/SFX/Obj/Hit","Obj", 0f);   
                }
                else if (this.gameObject.CompareTag("Library"))
                {
                    SoundManager.Instance.PlaySFX("event:/SFX/Obj/Hit","Obj", 1f);
                }
                else if (this.gameObject.CompareTag("Cloth"))
                {
                    SoundManager.Instance.PlaySFX("event:/SFX/Obj/Hit","Obj", 2f);
                }
                else if (this.gameObject.CompareTag("Plant"))
                {
                    SoundManager.Instance.PlaySFX("event:/SFX/Obj/Hit","Obj", 3f);
                }
                GameObject breakEffect = Managers.Resource.Instantiate("DustEffect");
                breakEffect.transform.position = gameObject.transform.position + Vector3.up * 4f;
                GameObject breakEffect2 = Managers.Resource.Instantiate("AttackEffect");
                breakEffect2.transform.position = gameObject.transform.position + Vector3.up * 2f;
                
                Destroy(breakEffect, 2f);
                Destroy(breakEffect2, 2f);
                Destroy(this.gameObject);
            }

        }
    }

}