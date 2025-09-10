using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoDoDoIt
{
    public interface IBuffable
    {
        public void ApplyBuff(BaseBuff_SO buff);
        public void RemoveBuff(BaseBuff_SO buff);
    }
}
