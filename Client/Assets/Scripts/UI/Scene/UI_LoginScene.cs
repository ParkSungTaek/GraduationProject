/******
작성자 : 이우열
작성 일자 : 23.10.01

최근 수정 일자 : 23.10.02
최근 수정 내용 : 로그인 서버로 전환, 종료 버튼 추가
 ******/

using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client
{
    public class UI_LoginScene : UI_Scene
    {
        enum Buttons
        {
            RegistBtn,
            ForceRegistBtn,
            LoginBtn,
            QuitBtn,
        }

        enum InputFields
        {
            EmailInput,
            PasswordInput,
        }

        public override void Init()
        {
            base.Init();
            Bind<Button>(typeof(Buttons));
            Bind<TMP_InputField>(typeof(InputFields));
            ButtonBind();

        }

        #region Btn
        void ButtonBind()
        {
            BindEvent(GetButton((int)Buttons.RegistBtn).gameObject, Btn_Regist);
            BindEvent(GetButton((int)Buttons.ForceRegistBtn).gameObject, Btn_ForceRegist);
            BindEvent(GetButton((int)Buttons.LoginBtn).gameObject, Btn_Login);
            BindEvent(GetButton((int)Buttons.QuitBtn).gameObject, Btn_Quit);
        }

        void Btn_Regist(PointerEventData evt)
        {
            string email = Get<TMP_InputField>((int)InputFields.EmailInput).text;
            string password = Get<TMP_InputField>((int)InputFields.PasswordInput).text;

            GameManager.Login.resentEmail = email;

            using (SHA256 sha256 = SHA256.Create())
            {
                string encryptedPW = Encoding.ASCII.GetString(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));

                CTL_Regist registPacket = new CTL_Regist();
                registPacket.email = email;
                registPacket.password = encryptedPW;

                GameManager.Login.Post(registPacket);
            }
        }

        void Btn_ForceRegist(PointerEventData evt)
        {
            string email = Get<TMP_InputField>((int)InputFields.EmailInput).text;
            string password = Get<TMP_InputField>((int)InputFields.PasswordInput).text;

            GameManager.Login.resentEmail = email;

            using (SHA256 sha256 = SHA256.Create())
            {
                string encryptedPW = Encoding.ASCII.GetString(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));

                CTL_ForceRegist registPacket = new CTL_ForceRegist();
                registPacket.email = email;
                registPacket.password = encryptedPW;

                GameManager.Login.Post(registPacket);
            }
        }

        void Btn_Login(PointerEventData evt)
        {
            string email = Get<TMP_InputField>((int)InputFields.EmailInput).text;
            string password = Get<TMP_InputField>((int)InputFields.PasswordInput).text;

            using (SHA256 sha256 = SHA256.Create())
            {
                string encryptedPW = Encoding.ASCII.GetString(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));

                CTL_Login registPacket = new CTL_Login();
                registPacket.email = email;
                registPacket.password = encryptedPW;

                GameManager.Network.Email = email;

                GameManager.Login.Post(registPacket);
            }
        }

        void Btn_Quit(PointerEventData evt)
        {
            GameManager.UI.ShowPopUpUI<UI_SimpleSelect>().SetData("정말로 종료하시겠습니까?", AcceptQuitBtn);
        }

        void AcceptQuitBtn()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
        #endregion
    }
}
