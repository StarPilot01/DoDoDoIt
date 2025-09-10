using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoDoDoIt
{
    public interface ICollectible
    {
        public void Collect(PlayerController playerController);
    }
}
