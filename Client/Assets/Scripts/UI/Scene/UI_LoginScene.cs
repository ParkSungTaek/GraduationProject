/******
작성자 : 박성택
작성 일자 : 23.09.17

최근 수정 일자 : 23.10.01
최근 수정 내용 : UI_LoginScene 클래스 생성
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
            LoginBtn,
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
            BindEvent(GetButton((int)Buttons.LoginBtn).gameObject, Btn_Login);
        }

        void Btn_Regist(PointerEventData evt)
        {
            string email = Get<TMP_InputField>((int)InputFields.EmailInput).text;
            string password = Get<TMP_InputField>((int)InputFields.PasswordInput).text;

            using (SHA256 sha256 = SHA256.Create())
            {
                string encryptedPW = Encoding.ASCII.GetString(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));

                CTS_RegistUser registPacket = new CTS_RegistUser();
                registPacket.email = email;
                registPacket.password = encryptedPW;

                GameManager.Network.Send(registPacket.Write());
            }
        }

        void Btn_Login(PointerEventData evt)
        {
            string email = Get<TMP_InputField>((int)InputFields.EmailInput).text;
            string password = Get<TMP_InputField>((int)InputFields.PasswordInput).text;

            using (SHA256 sha256 = SHA256.Create())
            {
                string encryptedPW = Encoding.ASCII.GetString(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));

                CTS_Login loginPacket = new CTS_Login();
                loginPacket.email = email;
                loginPacket.password = encryptedPW;

                GameManager.Network.Send(loginPacket.Write());
            }
        }
        #endregion
    }
}
