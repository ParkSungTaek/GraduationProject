/******
작성자 : 이우열
작성일 : 23.04.05

최근 수정 일자 : 23.05.09
최근 수정 사항 : 방 대기 Lobby로 기능 변경
******/

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Client
{
    public class UI_LobbyScene : UI_Scene
    {
        bool AllowQuickEnter { get; set; } = false;
        enum Buttons
        {
            StartBtn,
            LeaveBtn,
            QuickBtn,
        }

        enum Texts
        {
            PlayerTxt1,
            PlayerTxt2,
            PlayerTxt3,
            PlayerTxt4,
            QuickBtnTxt,
        }

        public override void Init()
        {
            base.Init();
            Bind<Button>(typeof(Buttons));
            Bind<TMP_Text>(typeof(Texts));

            GetText((int)Texts.QuickBtnTxt).text = "Allow Quick OFF";
            ButtonBind();

            GameManager.Room.LobbyUpdate = LobbyUpdate;
        }

        #region Button
        void ButtonBind()
        {
            BindEvent(Get<Button>((int)Buttons.StartBtn).gameObject, StartGame);
            BindEvent(Get<Button>((int)Buttons.LeaveBtn).gameObject, LeaveGame);
            BindEvent(Get<Button>((int)Buttons.QuickBtn).gameObject, QuickEnterAllow);

        }

        /// <summary> 게임 시작 버튼 </summary>
        void StartGame(PointerEventData evt)
        {
            for (int i = 0; i < 2; i++)
                GetButton(i).interactable = false;

            CTS_ReadyGame readyPacket = new CTS_ReadyGame();

            GameManager.Network.Send(readyPacket.Write());
        }

        /// <summary> 방 퇴장 버튼 </summary>
        void LeaveGame(PointerEventData evt)
        {
            GameManager.Room.Leave();

            SceneManager.LoadScene(Define.Scenes.Title);
        }

        /// <summary> 사람 들어오거나 나갈 때 정보 업데이트 </summary>
        void LobbyUpdate(RoomInfo roomInfo)
        {
            GetButton((int)Buttons.StartBtn).gameObject.SetActive(roomInfo.IsHost);
            GetButton((int)Buttons.QuickBtn).gameObject.SetActive(roomInfo.IsHost);

            int i = 0;
            foreach(var p in roomInfo.PlayerClasses)
            {
                GetText(i).text = $"{p.Key}, email : {p.Value.email}";
                i++;
            }
            for (; i < 4; i++)
                GetText(i).text = "empty";
        }
        void QuickEnterAllow(PointerEventData evt)
        {
            CTS_SetPublicRoom pkt = new CTS_SetPublicRoom();

            if (AllowQuickEnter)
            {
                AllowQuickEnter = false;
                GetText((int)Texts.QuickBtnTxt).text = "Allow Quick OFF";
            }
            else
            {
                AllowQuickEnter = true;
                GetText((int)Texts.QuickBtnTxt).text = "Allow Quick ON";
            }
            pkt.isPublic = AllowQuickEnter;
            GameManager.Network.Send(pkt.Write());

        }
        #endregion Button

    }
}
