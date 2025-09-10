using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace DoDoDoIt
{
    public class UI_Scene_Ending : UI_Scene_Base
    {
        
        //Button
        
        public override bool Init()
        {
            if (base.Init() == false)
                return false;

            //None Scene UI
            Debug.Log(Managers.Score.MyScore);
            
            return true;
        }

        private void Start()
        {
            Managers.Resource.LoadAllAsync<VideoClip>("End_Cutscene", (key, count, totalCount) =>
            {
                if (count == totalCount)
                {
                    Debug.Log("Load On");
                    Managers.UI.ShowPopupUI<UI_Popup_Ending_Cutscene>().gameObject.SetActive(true);

                }
                
            });
            //Managers.UI.ShowPopupUI<UI_Popup_NameInput>().gameObject.SetActive(true);
            
        }
    }
}
