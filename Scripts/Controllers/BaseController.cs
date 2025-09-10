using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DoDoDoIt
{
    public abstract class BaseController : MonoBehaviour
    {
        public Define.EObjectType ObjectType { get; protected set; }

        bool _init = false;

        void Awake()
        {
            Init();
        }

        public virtual bool Init()
        {
            if (_init)
                return false;

            _init = true;
            return true;
        }
    }
}
