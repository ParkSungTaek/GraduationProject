/******
작성자 : 박성택
작성 일자 : 23.04.08

최근 수정 일자 : 23.04.10
최근 수정 내용 : image 불러오기 변경
 ******/

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
        enum Images
        {
            WinImg,
            DefeatImg,
        }

        public override void Init()
        {
            base.Init();
            Bind<Button>(typeof(Buttons));
            Bind<Image>(typeof(Images));

            ButtonBind();
            ImageBind();
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

        #region Image
        /// <summary>
        /// 나중에 Win Defeat 이미지 생기면 그때 이미지 갈아 끼우는 것으로 변경 예정
        /// 지금은 그냥 Set True False로 간략하게 나타냄
        /// </summary>
        void ImageBind()
        {
            if(GameManager.InGameData.CurrState == Define.State.Win)
            {
                GetImage((int)Images.WinImg).gameObject.SetActive(true);
                GetImage((int)Images.DefeatImg).gameObject.SetActive(false);

            }
            else
            {

                GetImage((int)Images.WinImg).gameObject.SetActive(false);
                GetImage((int)Images.DefeatImg).gameObject.SetActive(true);
            }
        }

        #endregion

        public override void ReOpen()
        {

        }
    }
}
