/******
작성자 : 이우열
작성 일자 : 23.11.05

최근 수정 일자 : 23.11.05
최근 수정 내용 : 계정 탈퇴 UI 클래스 생성
 ******/

using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client
{
    public class UI_Unregist : UI_PopUp
    {
        enum InputFields
        {
            EmailInput,
            PasswordInput,
        }

        enum Buttons
        {
            DeleteBtn,
            CancelBtn,
        }

        public override void Init()
        {
            base.Init();

            Bind<Button>(typeof(Buttons));
            Bind<TMP_InputField>(typeof(InputFields));
            BindButtons();
        }

        private void BindButtons()
        {
            BindEvent(GetButton((int)Buttons.DeleteBtn).gameObject, OnDeleteBtn);
            BindEvent(GetButton((int)Buttons.CancelBtn).gameObject, (evt) => ClosePopUpUI());
        }

        private void OnDeleteBtn(PointerEventData evt)
        {
            string email = Get<TMP_InputField>((int)InputFields.EmailInput).text;
            string password = Get<TMP_InputField>((int)InputFields.PasswordInput).text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                GameManager.UI.ShowPopUpUI<UI_ClosableLog>().SetLog("공백은 입력할 수 없습니다.");
                return;
            }

            using (SHA256 sha256 = SHA256.Create())
            {
                string encryptedPW = Encoding.ASCII.GetString(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));

                CTL_Unregist unregistPacket = new CTL_Unregist();
                unregistPacket.email = email;
                unregistPacket.password = encryptedPW;

                GameManager.Login.Post(unregistPacket);
            }
        }

        public override void ReOpen()
        {
            Get<TMP_InputField>((int)InputFields.EmailInput).text = string.Empty;
            Get<TMP_InputField>((int)InputFields.PasswordInput).text = string.Empty;
        }
    }
}
