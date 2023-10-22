/******
작성자 : 이우열
작성일 : 23.04.05

최근 수정 일자 : 23.05.09
최근 수정 사항 : 방 대기 Lobby로 기능 변경
******/

using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

namespace Client
{
    public class UI_LobbyScene : UI_Scene
    {
        bool AllowQuickEnter { get; set; } = false;
        enum Buttons
        {
            StartBtn,
            LeaveBtn,
            SetPublicBtn,
        }

        enum Texts
        {
            PlayerTxt1,
            PlayerTxt2,
            PlayerTxt3,
            PlayerTxt4,
            SetPublicText,
        }

        public override void Init()
        {
            base.Init();
            Bind<Button>(typeof(Buttons));
            Bind<TMP_Text>(typeof(Texts));
            ButtonBind();

            GameManager.Room.LobbyUpdate = LobbyUpdate;
            GameManager.Room.OnSetPublic = PublicRoomSet;

            //나중에 들어온 유저인 경우
            if (GameManager.Room.IsPublic())
                PublicRoomSet();
        }

        #region Button
        void ButtonBind()
        {
            BindEvent(Get<Button>((int)Buttons.StartBtn).gameObject, StartGameBtn);
            BindEvent(Get<Button>((int)Buttons.LeaveBtn).gameObject, LeaveGameBtn);
            Get<Button>((int)Buttons.SetPublicBtn).onClick.AddListener(SetPublicBtn);
        }

        /// <summary> 게임 시작 버튼 </summary>
        void StartGameBtn(PointerEventData evt)
        {
            for (int i = 0; i < 3; i++)
                GetButton(i).gameObject.SetActive(false);

            CTS_ReadyGame readyPacket = new CTS_ReadyGame();

            GameManager.Network.Send(readyPacket.Write());
        }

        /// <summary> 방 퇴장 버튼 </summary>
        void LeaveGameBtn(PointerEventData evt)
        {
            GameManager.Room.Leave();
            SceneManager.LoadScene(Define.Scenes.Title);
        }

        /// <summary> 사람 들어오거나 나갈 때 정보 업데이트 </summary>
        void LobbyUpdate(RoomInfo roomInfo)
        {
            GetButton((int)Buttons.StartBtn).gameObject.SetActive(roomInfo.IsHost);
            GetButton((int)Buttons.SetPublicBtn).gameObject.SetActive(roomInfo.IsHost);

            int i = 0;
            foreach(var p in roomInfo.PlayerClasses)
            {
                GetText(i).text = $"email : {p.Value.email}";
                i++;
            }
            for (; i < 4; i++)
                GetText(i).text = "empty";
        }

        /// <summary> 공개방으로 전환 버튼 </summary>
        void SetPublicBtn()
        {
            GameManager.UI.ShowPopUpUI<UI_SimpleSelect>().SetData("공개방으로 전환하시겠습니까?\n한 번 전환 시 되돌릴 수 없습니다.", AcceptSetPublicBtn);
        }

        private void AcceptSetPublicBtn()
        {
            PublicRoomSet();

            CTS_SetPublicRoom pkt = new CTS_SetPublicRoom();
            AllowQuickEnter = true;
            pkt.isPublic = AllowQuickEnter;
            GameManager.Network.Send(pkt.Write());

            GameManager.UI.ClosePopUpUI(typeof(UI_SimpleSelect));
        }

        private void PublicRoomSet()
        {
            var button = GetButton((int)Buttons.SetPublicBtn);
            button.interactable = false;
            GetText((int)Texts.SetPublicText).text = "공개방으로 전환됨";
        }
        #endregion Button

    }
}
