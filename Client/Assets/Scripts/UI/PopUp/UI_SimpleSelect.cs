/******
작성자 : 이우열
작성일 : 23.09.29

최근 수정 일자 : 23.09.29
최근 수정 사항 : 확인, 취소 버튼 있는 UI 클래스 생성
******/

using System;
using TMPro;
using UnityEngine.UI;

namespace Client
{
    public class UI_SimpleSelect : UI_PopUp
    {
        enum Texts
        {
            ExplainTxt,
        }

        enum Buttons
        {
            AcceptBtn, CloseBtn,
        }

        public override void Init()
        {
            base.Init();
            Bind<TMP_Text>(typeof(Texts));
            Bind<Button>(typeof(Buttons));

            BindEvent(GetButton((int)Buttons.CloseBtn).gameObject, (evt) => ClosePopUpUI());
        }

        public void SetData(string explain, Action acceptCallback)
        {
            GetText((int)Texts.ExplainTxt).text = explain;
            BindEvent(GetButton((int)Buttons.AcceptBtn).gameObject, (evt) => acceptCallback());
        }

        public override void ReOpen()
        {
            GetText((int)Texts.ExplainTxt).text = string.Empty;
            ClearEvent(GetButton((int)Buttons.AcceptBtn).gameObject);
        }
    }
}
