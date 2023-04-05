using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client
{
    public class UI_GameOver : UI_PopUp
    {
        enum Buttons
        {
            TitleBtn,
            RetryBtn,
        }


        public override void Init()
        {
            base.Init();
            Bind<Button>(typeof(Buttons));
            ButtonBind();
        }

        #region Button
        void ButtonBind()
        {
            BindEvent(GetButton((int)Buttons.TitleBtn).gameObject, Btn_GoToTitle);
            BindEvent(GetButton((int)Buttons.RetryBtn).gameObject, Btn_Retry);
        }

        void Btn_GoToTitle(PointerEventData evt)
        {
            SceneManager.LoadScene(Define.Scenes.Title);
        }

        void Btn_Retry(PointerEventData evt)
        {
            SceneManager.LoadScene(Define.Scenes.Game);
        }
        #endregion Button

        public override void ReOpen()
        {

        }
    }
}
