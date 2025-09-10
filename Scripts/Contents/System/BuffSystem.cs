using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DoDoDoIt
{
    
    public class BuffSystem : MonoBehaviour
    {
        
        [SerializeField]
        private List<BaseBuff_SO> _baseBuffSOList;

        private ObjectGatherer _objectsGatherer;
        private PlayerController _player;

     

        private void Awake()
        {
            //원래 여기서 이벤트 등록하는 것이 맞으나 자꾸 풀리는 버그로 인해 SoulGatherer에서 직접 등록
            //_soulGatherer = FindAnyObjectByType<SoulGatherer>();
            //_soulGatherer.OnSpeedGhostAbsorbed += OnGhostAbsorbed;
        }

       
        void Start()
        {
            //_maxSoulStack = _buffSetList.Count;
            _player = ((Scene_Game)(Managers.Scene.CurrentScene)).Player;
           //_player.OnHeldBuffUsage += () =>
           //{
           //    //_soulStack = 0;
           //    Debug.Log("Heldbuffer");
           //};
        }

        public void OnGhostAbsorbed()
        {
            //if (_soulStack < _maxSoulStack)
            //{
            //    _soulStack++;
            //    //Debug.Log(_soulStack);
//
            //    QueueBuffBasedOnSoul();
//
            //}
            
            
            
        }

        void QueueBuffBasedOnSoul()
        {
            
            
            
           //Debug.Assert(_buffSetList[_soulStack - 1].buffList.Count != 0 , $"{_soulStack} buff List Count is 0");

           //int curSetBuffListCount = _buffSetList[_soulStack - 1].buffList.Count;
           //if (curSetBuffListCount > 0)
           //{
           //    player.ClearHeldBuffs();
           //}
           //
           //for (int i = 0; i < curSetBuffListCount; i++)
           //{
           //    BaseBuff_SO buff = _buffSetList[_soulStack - 1].buffList[i];
           //    player.QueueBuff(buff);
           //}
        }
        
    }
}