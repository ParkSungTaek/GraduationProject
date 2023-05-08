/******
작성자 : 이우열
작성 일자 : 23.05.08

최근 수정 일자 : 23.05.08
최근 수정 내용 : 알림 표시를 위한 UI_Log 클래스 생성
 ******/

using TMPro;
using UnityEngine.UI;

namespace Client
{
    public class UI_Log : UI_PopUp
    {
        enum Texts
        {
            LogTxt,
        }


        public override void Init()
        {
            base.Init();
            Bind<TMP_Text>(typeof(Texts));
        }

        /// <summary> 보여줄 텍스트 설정 </summary>
        public void SetLog(string log)
        {
            GetText((int)Texts.LogTxt).text = log;
        }

        public override void ReOpen() { }
    }
}
