/******
작성자 : 이우열
작성 일자 : 23.05.08

최근 수정 일자 : 23.05.08
최근 수정 내용 : UI_RobyScene 클래스 생성
 ******/

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client
{
    public class UI_TitleScene : UI_Scene
    {
        /// <summary> 버튼 목록 </summary>
        enum Buttons
        {
            CreateBtn,
            EnterBtn,
        }

        enum Texts
        {
            RoomNameTxt,
        }

        /// <summary> 바인드 </summary>
        public override void Init()
        {
            base.Init();
            Bind<Button>(typeof(Buttons));
            Bind<TMP_Text>(typeof(Texts));

            BindEvent(GetButton((int)Buttons.CreateBtn).gameObject, CreateRoomBtn);
            BindEvent(GetButton((int)Buttons.EnterBtn).gameObject, EnterRoomBtn);
        }

        /// <summary> 방 생성 버튼 </summary>
        void CreateRoomBtn(PointerEventData evt)
        {
            string roomName = GetText((int)Texts.RoomNameTxt).text;

            if(string.IsNullOrEmpty(roomName))
            {
                Debug.Log("빈 방 이름");
                return;
            }

            CTS_CreateRoom pkt = new CTS_CreateRoom();
            pkt.roomName = roomName;

            GameManager.Network.Send(pkt.Write());
            GameManager.UI.ShowPopUpUI<UI_Log>().SetLog("방 생성 중");
        }

        /// <summary> 방 입장 버튼 </summary>
        void EnterRoomBtn(PointerEventData evt)
        {
            string roomName = GetText((int)Texts.RoomNameTxt).text;

            if (string.IsNullOrEmpty(roomName))
            {
                Debug.Log("빈 방 이름");
                return;
            }

            CTS_EnterRoom pkt = new CTS_EnterRoom();
            pkt.roomName = roomName;

            GameManager.Network.Send(pkt.Write());
            GameManager.UI.ShowPopUpUI<UI_Log>().SetLog("방 입장 중");
        }
    }
}