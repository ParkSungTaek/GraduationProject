/******
작성자 : 이우열
작성 일자 : 23.05.08

최근 수정 일자 : 23.05.08
최근 수정 내용 : 알림 표시를 위한 UI_ClosableLog 클래스 생성
 ******/

using TMPro;
using UnityEngine.UI;

namespace Client
{
    public class UI_ClosableLog : UI_PopUp
    {
        enum Texts
        {
            LogTxt,
        }

        enum Buttons
        {
            CloseBtn,
        }


        public override void Init()
        {
            base.Init();
            Bind<TMP_Text>(typeof(Texts));
            Bind<Button>(typeof(Buttons));

            BindEvent(GetButton((int)Buttons.CloseBtn).gameObject, (evt) => ClosePopUpUI());
        }

        /// <summary> 보여줄 텍스트 설정 </summary>
        public void SetLog(string log)
        {
            GetText((int)Texts.LogTxt).text = log;
        }

        public override void ReOpen() { }
    }
}
