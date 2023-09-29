/******
작성자 : 이우열
작성 일자 : 23.05.08

최근 수정 일자 : 23.09.29
최근 수정 내용 : 게임 종료 버튼 추가
 ******/

using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client
{
    public class UI_TitleScene : UI_Scene
    {
        bool BeforeInit = false;
        const int ExistRoomsButtons = 3;
        /// <summary> 버튼 목록 </summary>
        enum Buttons
        {
            QuickEnter,
            CreateBtn,
            EnterBtn,
            QuitBtn,
            Room1Btn,
            Room2Btn,
            Room3Btn,
        }

        enum InputFields
        {
            RoomInput,
        }
        enum Texts 
        {
            Room1Name,
            Room2Name, 
            Room3Name,
        }


        public void SetExistRoomsName(List<string> roomsName)
        {

            if (!BeforeInit)
            {
                Init();
            }
            for (int i = 0;i < roomsName.Count; i++)
            {
                Debug.Log($"roomsName {i} : {roomsName[i]}");
                GetText(i).text = roomsName[i];
            }
            for (int i = roomsName.Count; i < 3; i++)
            {
                GetText(i).text = "Empty";

            }


        }

        /// <summary> 바인드 </summary>
        public override void Init()
        {
            if (!BeforeInit)
            {
                BeforeInit = true;
                base.Init();
                Bind<Button>(typeof(Buttons));
                Bind<TMP_InputField>(typeof(InputFields));
                Bind<TMP_Text>(typeof(Texts));
                Debug.Log("BindText");

                BindButton();

            }
        }


        #region Btn
        void BindButton()
        {
            BindEvent(GetButton((int)Buttons.QuickEnter).gameObject, QuickEnterBtn);
            BindEvent(GetButton((int)Buttons.CreateBtn).gameObject, CreateRoomBtn);
            BindEvent(GetButton((int)Buttons.EnterBtn).gameObject, EnterRoomBtn);
            BindEvent(GetButton((int)Buttons.QuitBtn).gameObject, QuitBtn);

            BindEvent(GetButton((int)Buttons.Room1Btn).gameObject, ExistRoomEnterBtn);
            BindEvent(GetButton((int)Buttons.Room2Btn).gameObject, ExistRoomEnterBtn);
            BindEvent(GetButton((int)Buttons.Room3Btn).gameObject, ExistRoomEnterBtn);

        }
        /// <summary> 방 생성 버튼 </summary>
        void CreateRoomBtn(PointerEventData evt)
        {
            string roomName = Get<TMP_InputField>((int)InputFields.RoomInput).text;

            if(string.IsNullOrEmpty(roomName))
            {
                GameManager.UI.ShowPopUpUI<UI_ClosableLog>().SetLog("방 이름을 입력하세요.");
                return;
            }

            CTS_CreateRoom pkt = new CTS_CreateRoom();
            pkt.roomName = roomName;
            GameManager.Room.SetRoomName(roomName);
            GameManager.Network.Send(pkt.Write());
            GameManager.UI.ShowPopUpUI<UI_Log>().SetLog("방 생성 중");
        }

        /// <summary> 방 입장 버튼 </summary>
        void EnterRoomBtn(PointerEventData evt)
        {
            string roomName = Get<TMP_InputField>((int)InputFields.RoomInput).text;

            if (string.IsNullOrEmpty(roomName))
            {
                GameManager.UI.ShowPopUpUI<UI_ClosableLog>().SetLog("방 이름을 입력하세요.");
                return;
            }

            CTS_EnterRoom pkt = new CTS_EnterRoom();
            pkt.roomName = roomName;
            GameManager.Room.SetRoomName(roomName);

            GameManager.Network.Send(pkt.Write());
            GameManager.UI.ShowPopUpUI<UI_Log>().SetLog("방 입장 중");


        }
        /// <summary>
        /// 방 빠른 입장
        /// </summary>
        void ExistRoomEnterBtn(PointerEventData evt)
        {
            string roomName = "";
            for (int i=0;i< ExistRoomsButtons; i++)
            {
                if(evt.selectedObject == GetButton((int)Buttons.Room1Btn + i).gameObject)
                {
                    roomName = GetText((int)Texts.Room1Name + i).text;
                    break;
                }
            }
            //string roomName = Get<TMP_InputField>((int)InputFields.RoomInput).text;

            if (string.IsNullOrEmpty(roomName))
            {
                GameManager.UI.ShowPopUpUI<UI_ClosableLog>().SetLog("방 이름을 입력하세요.");
                return;
            }

            CTS_EnterRoom pkt = new CTS_EnterRoom();
            pkt.roomName = roomName;

            GameManager.Network.Send(pkt.Write());
            GameManager.UI.ShowPopUpUI<UI_Log>().SetLog("방 입장 중");
        }
        void QuickEnterBtn(PointerEventData evt)
        {

            CTS_QuickEnterRoom pkt = new CTS_QuickEnterRoom();
            Debug.Log("Enter");
            GameManager.Network.Send(pkt.Write());
        }
        void QuitBtn(PointerEventData evt)
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
#endregion Btn
    }
}