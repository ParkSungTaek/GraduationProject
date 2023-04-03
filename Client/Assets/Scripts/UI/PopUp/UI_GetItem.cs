using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client
{
    public class UI_GetItem : UI_PopUp
    {
        enum Buttons
        { 
            CloseBtn,
        }


        public override void Init()
        {
            base.Init();
            Bind<Button>(typeof(Buttons));

            BindEvent(GetButton((int)Buttons.CloseBtn).gameObject, Btn_Close);
        }

        #region Buttons
        void Btn_Close(PointerEventData evt)
        {
            GameManager.UI.ClosePopUpUI(this);
        }
        #endregion Buttons
    }
}
