using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoDoDoIt
{
    public class UI_Popup_NameInputAlert : UI_Popup
    {
        #region Enum

        enum Images
        {
            BG
        }
        
        enum Buttons
        {
            ConfirmButton
        }

       
        
        #endregion
        public override bool Init()
        {
            if (base.Init() == false)
                return false;
            
            BindImage(typeof(Images));
          
            BindButton(typeof(Buttons));
          
            
            
           

            GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);
            
            return true;
        }

        void OnClickConfirmButton()
        {
            Managers.UI.ClosePopupUI(this);
        }
    }
}
