/******
작성자 : 이우열
작성 일자 : 23.05.08

최근 수정 일자 : 23.05.08
최근 수정 내용 : 최초 서버 연결 실패 알림 클래스
 ******/

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class UI_FailConnect : UI_PopUp
    {

        enum Buttons
        { 
            RetryBtn, ExitBtn,
        }


        public override void Init()
        {
            base.Init();
            Bind<Button>(typeof(Buttons));

            BindEvent(GetButton((int)Buttons.RetryBtn).gameObject, (evt) => { ClosePopUpUI(); GameManager.Network.Connect(); });
            BindEvent(GetButton((int)Buttons.ExitBtn).gameObject, (evt) => { Application.Quit(); });
        }

        public override void ReOpen() { }
    }
}
