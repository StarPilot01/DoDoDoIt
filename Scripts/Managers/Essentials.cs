using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoDoDoIt
{
    public class Essentials : MonoBehaviour
    {
        
        void Awake()
        {
            DontDestroyOnLoad(this);
        }

        
    }
}
