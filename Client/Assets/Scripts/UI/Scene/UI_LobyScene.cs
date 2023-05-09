/******
작성자 : 이우열
작성일 : 23.04.05

최근 수정 일자 : 23.05.09
최근 수정 사항 : 방 대기 Loby로 기능 변경
******/

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Client
{
    public class UI_LobyScene : UI_Scene
    {
        enum Buttons
        {
            StartBtn,
            LeaveBtn,
        }

        enum Texts
        {
            PlayerTxt1,
            PlayerTxt2,
            PlayerTxt3,
            PlayerTxt4,
        }

        public override void Init()
        {
            base.Init();
            Bind<Button>(typeof(Buttons));
            Bind<TMP_Text>(typeof(Texts));
            
            ButtonBind();

            GameManager.Room.LobyUpdate = LobyUpdate;
        }

        #region Button
        void ButtonBind()
        {
            BindEvent(Get<Button>((int)Buttons.StartBtn).gameObject, StartGame);
            BindEvent(Get<Button>((int)Buttons.LeaveBtn).gameObject, LeaveGame);
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
        void LobyUpdate(RoomInfo roomInfo)
        {
            GetButton((int)Buttons.StartBtn).interactable = roomInfo.IsHost;

            int i = 0;
            foreach(int p in roomInfo.PlayerClasses.Keys)
            {
                Debug.Log($"{p} : {roomInfo.PlayerClasses[p]}");
                GetText(i).text = $"playerId : {p}";
                i++;
            }
            for (; i < 4; i++)
                GetText(i).text = "empty";
        }
        #endregion Button

    }
}
