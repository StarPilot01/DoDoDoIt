using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoDoDoIt
{
    public class SavePoint : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            // 플레이어와 충돌했을 때만 처리
            if (other.CompareTag("Player"))
            {
                Scene_Game sceneGame = FindObjectOfType<Scene_Game>();
                if (sceneGame != null)
                {
                    // 현재 SavePoint를 업데이트
                    sceneGame.UpdateSavePoint(transform);
                }
            }
        }
    }
}
