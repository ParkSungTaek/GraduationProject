/******
작성자 : 이우열
작성 일자 : 23.10.03

최근 수정 일자 : 23.10.03
최근 수정 내용 : 인증번호 입력 UI 생성
 ******/

using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client
{
    public class UI_RegistAuth : UI_PopUp
    {
        public enum Buttons
        {
            EnterBtn,
            CloseBtn,
        }

        public enum InputFields
        {
            AuthInput,
        }

        public override void Init()
        {
            base.Init();

            Bind<Button>(typeof(Buttons));
            Bind<TMP_InputField>(typeof(InputFields));

            ButtonBind();
        }

        private void ButtonBind()
        {
            BindEvent(GetButton((int)Buttons.EnterBtn).gameObject, Btn_Enter);
            BindEvent(GetButton((int)Buttons.CloseBtn).gameObject, evt => ClosePopUpUI());
        }

        private void Btn_Enter(PointerEventData evt)
        {
            string txt = Get<TMP_InputField>((int)InputFields.AuthInput).text;
            int authNo;
            if (int.TryParse(txt, out authNo))
            {
                CTL_RegistAuth authPacket = new CTL_RegistAuth();
                authPacket.email = GameManager.Login.resentEmail;
                authPacket.authNo = authNo;
                GameManager.Login.Post(authPacket);
            }
            else
            {
                GameManager.UI.ShowPopUpUI<UI_ClosableLog>().SetLog("숫자를 입력하세요");
            }
        }

        public override void ReOpen() { }
    }
}