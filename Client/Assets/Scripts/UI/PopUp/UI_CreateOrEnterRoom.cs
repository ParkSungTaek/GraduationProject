/******
작성자 : 이우열
작성 일자 : 23.10.22

최근 수정 일자 : 23.10.22
최근 수정 내용 : 방 생성 UI 분리
 ******/

using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client
{
    public class UI_CreateOrEnterRoom : UI_PopUp
    {
        private enum Buttons
        {
            CreateBtn,
            EnterBtn,
            BackBtn,
        }

        private enum InputFields
        {
            RoomNameInput,
        }

        public override void Init()
        {
            base.Init();
            Bind<Button>(typeof(Buttons));
            Bind<TMP_InputField>(typeof(InputFields));

            BindButton();
        }

        #region Buttons
        private void BindButton()
        {
            BindEvent(GetButton((int)Buttons.CreateBtn).gameObject, CreateBtn);
            BindEvent(GetButton((int)Buttons.EnterBtn).gameObject, EnterBtn);
            BindEvent(GetButton((int)Buttons.BackBtn).gameObject, (evt) => ClosePopUpUI());
        }

        private void CreateBtn(PointerEventData evt)
        {
            string roomName = Get<TMP_InputField>((int)InputFields.RoomNameInput).text;

            if (string.IsNullOrEmpty(roomName))
            {
                GameManager.UI.ShowPopUpUI<UI_ClosableLog>().SetLog("방 이름을 입력하세요.");
                return;
            }

            CTS_CreateRoom createPacket = new CTS_CreateRoom();
            createPacket.roomName = roomName;

            GameManager.UI.ShowPopUpUI<UI_Log>().SetLog("방 생성 중");
            GameManager.Network.Send(createPacket.Write());
        }

        private void EnterBtn(PointerEventData evt)
        {
            string roomName = Get<TMP_InputField>((int)InputFields.RoomNameInput).text;

            if (string.IsNullOrEmpty(roomName))
            {
                GameManager.UI.ShowPopUpUI<UI_ClosableLog>().SetLog("방 이름을 입력하세요.");
                return;
            }

            CTS_EnterRoom enterPacket = new CTS_EnterRoom();
            enterPacket.roomName = roomName;

            GameManager.UI.ShowPopUpUI<UI_Log>().SetLog("방 생성 중");
            GameManager.Network.Send(enterPacket.Write());
        }
        #endregion Buttons

        public override void ReOpen() => Get<TMP_InputField>((int)InputFields.RoomNameInput).text = string.Empty;
    }
}